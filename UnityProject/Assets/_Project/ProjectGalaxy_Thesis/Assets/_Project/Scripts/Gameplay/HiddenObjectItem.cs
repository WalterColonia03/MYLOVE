using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// ═══════════════════════════════════════════════════════════════════════════
/// HIDDEN OBJECT ITEM - Componente de Objeto Oculto en Escena
/// ═══════════════════════════════════════════════════════════════════════════
/// Attach esto a cada objeto oculto. Usa PolygonCollider2D para clicks precisos.
/// ═══════════════════════════════════════════════════════════════════════════
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(PolygonCollider2D))]
public class HiddenObjectItem : MonoBehaviour
{
    // ═══════════════════════════════════════════════════════════════════════
    // DATOS
    // ═══════════════════════════════════════════════════════════════════════
    
    [Header("═══ DATOS ═══")]
    [SerializeField] private HiddenObjectData objectData;
    
    public HiddenObjectData Data => objectData;
    public string ObjectID => objectData != null ? objectData.objectID : "";
    
    // ═══════════════════════════════════════════════════════════════════════
    // ESTADO
    // ═══════════════════════════════════════════════════════════════════════
    
    [Header("═══ ESTADO ═══")]
    [SerializeField] private bool isFound = false;
    [SerializeField] private bool isHinted = false;
    
    public bool IsFound => isFound;
    public bool IsHinted => isHinted;
    
    // ═══════════════════════════════════════════════════════════════════════
    // VISUAL
    // ═══════════════════════════════════════════════════════════════════════
    
    [Header("═══ COLORES ═══")]
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color hintGlowColor = new Color(1f, 0.92f, 0.6f, 1f);
    [SerializeField] private Color foundColor = new Color(1f, 1f, 1f, 0.4f);
    
    [Header("═══ ANIMACIÓN ═══")]
    [SerializeField] private float glowPulseSpeed = 2.5f;
    [SerializeField] private float foundFadeDuration = 0.5f;
    
    // ═══════════════════════════════════════════════════════════════════════
    // COMPONENTES
    // ═══════════════════════════════════════════════════════════════════════
    
    private SpriteRenderer spriteRenderer;
    private PolygonCollider2D polygonCollider;
    private Coroutine glowCoroutine;
    
    // ═══════════════════════════════════════════════════════════════════════
    // EVENTOS
    // ═══════════════════════════════════════════════════════════════════════
    
    [Header("═══ EVENTOS ═══")]
    public UnityEvent<HiddenObjectItem> OnFound;
    public UnityEvent<HiddenObjectItem> OnHintActivated;
    
    // ═══════════════════════════════════════════════════════════════════════
    // UNITY LIFECYCLE
    // ═══════════════════════════════════════════════════════════════════════
    
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        polygonCollider = GetComponent<PolygonCollider2D>();
    }
    
    private void Start()
    {
        InitializeFromData();
    }
    
    // ═══════════════════════════════════════════════════════════════════════
    // INICIALIZACIÓN
    // ═══════════════════════════════════════════════════════════════════════
    
    public void InitializeFromData()
    {
        if (objectData == null) return;
        
        // Sprite
        if (objectData.objectSprite != null)
        {
            spriteRenderer.sprite = objectData.objectSprite;
            spriteRenderer.sortingOrder = objectData.sortingOrder;
        }
        
        // Regenerar collider
        RegenerateCollider();
        
        // Color inicial
        spriteRenderer.color = normalColor;
        isFound = false;
        isHinted = false;
    }
    
    /// <summary>
    /// Regenera el PolygonCollider2D desde el sprite para clicks precisos.
    /// </summary>
    [ContextMenu("Regenerar Collider")]
    public void RegenerateCollider()
    {
        if (spriteRenderer == null || spriteRenderer.sprite == null) return;
        if (polygonCollider == null) return;
        
        Sprite sprite = spriteRenderer.sprite;
        int shapeCount = sprite.GetPhysicsShapeCount();
        
        if (shapeCount == 0)
        {
            Debug.LogWarning($"[HiddenObjectItem] {name}: Sprite sin physics shapes!");
            return;
        }
        
        polygonCollider.pathCount = shapeCount;
        List<Vector2> path = new List<Vector2>();
        
        for (int i = 0; i < shapeCount; i++)
        {
            path.Clear();
            sprite.GetPhysicsShape(i, path);
            polygonCollider.SetPath(i, path);
        }
    }
    
    // ═══════════════════════════════════════════════════════════════════════
    // DESCUBRIMIENTO
    // ═══════════════════════════════════════════════════════════════════════
    
    /// <summary>
    /// Llamado cuando el jugador encuentra este objeto.
    /// </summary>
    public void Discover()
    {
        if (isFound) return;
        isFound = true;
        
        StopGlow();
        
        // Efecto visual
        StartCoroutine(FoundAnimation());
        
        // Audio
        if (objectData != null && objectData.foundSound != null)
        {
            AudioSource.PlayClipAtPoint(objectData.foundSound, transform.position);
        }
        
        // Partículas
        if (objectData != null && objectData.effectPrefab != null)
        {
            Instantiate(objectData.effectPrefab, transform.position, Quaternion.identity);
        }
        
        // Evento
        OnFound?.Invoke(this);
        
        // Desactivar collider
        if (polygonCollider != null)
            polygonCollider.enabled = false;
    }
    
    private IEnumerator FoundAnimation()
    {
        float elapsed = 0f;
        Color startColor = spriteRenderer.color;
        
        while (elapsed < foundFadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / foundFadeDuration;
            spriteRenderer.color = Color.Lerp(startColor, foundColor, t);
            yield return null;
        }
        
        spriteRenderer.color = foundColor;
    }
    
    // ═══════════════════════════════════════════════════════════════════════
    // PISTAS (HINT)
    // ═══════════════════════════════════════════════════════════════════════
    
    public void ShowHint()
    {
        if (isFound || isHinted) return;
        isHinted = true;
        
        glowCoroutine = StartCoroutine(GlowAnimation());
        OnHintActivated?.Invoke(this);
    }
    
    public void HideHint()
    {
        isHinted = false;
        StopGlow();
        spriteRenderer.color = normalColor;
    }
    
    private void StopGlow()
    {
        if (glowCoroutine != null)
        {
            StopCoroutine(glowCoroutine);
            glowCoroutine = null;
        }
    }
    
    private IEnumerator GlowAnimation()
    {
        while (isHinted && !isFound)
        {
            float t = (Mathf.Sin(Time.time * glowPulseSpeed) + 1f) * 0.5f;
            spriteRenderer.color = Color.Lerp(normalColor, hintGlowColor, t);
            yield return null;
        }
    }
    
    // ═══════════════════════════════════════════════════════════════════════
    // RESET
    // ═══════════════════════════════════════════════════════════════════════
    
    public void ResetState()
    {
        StopGlow();
        isFound = false;
        isHinted = false;
        spriteRenderer.color = normalColor;
        
        if (polygonCollider != null)
            polygonCollider.enabled = true;
    }
}
