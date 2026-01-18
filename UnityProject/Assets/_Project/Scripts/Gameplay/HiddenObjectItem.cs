using UnityEngine;
using UnityEngine.Events;
using System.Collections;

/// <summary>
/// HIDDEN OBJECT ITEM - Componente para objetos ocultos en escena.
/// Usa PolygonCollider2D para detección precisa de clics.
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(PolygonCollider2D))]
public class HiddenObjectItem : MonoBehaviour
{
    [Header("═══ DATOS ═══")]
    [SerializeField] private HiddenObjectData objectData;
    
    [Header("═══ ESTADO ═══")]
    [SerializeField] private bool isFound = false;
    [SerializeField] private bool isHinted = false;
    
    [Header("═══ VISUAL ═══")]
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color hintGlowColor = new Color(1f, 0.92f, 0.6f, 1f);
    [SerializeField] private Color foundColor = new Color(1f, 1f, 1f, 0.4f);
    [SerializeField] private float glowPulseSpeed = 2.5f;
    [SerializeField] private float foundFadeDuration = 0.5f;
    
    [Header("═══ EVENTOS ═══")]
    public UnityEvent<HiddenObjectItem> OnObjectFound;
    public UnityEvent<HiddenObjectItem> OnHintActivated;
    
    // Componentes
    private SpriteRenderer spriteRenderer;
    private PolygonCollider2D polygonCollider;
    private Coroutine hintCoroutine;
    private Coroutine fadeCoroutine;
    
    // Propiedades públicas
    public HiddenObjectData Data => objectData;
    public string ObjectID => objectData != null ? objectData.ObjectID : "";
    public bool IsFound => isFound;
    public bool IsHinted => isHinted;
    
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        polygonCollider = GetComponent<PolygonCollider2D>();
    }
    
    private void Start()
    {
        InitializeFromData();
        if (spriteRenderer != null) spriteRenderer.color = normalColor;
    }
    
    /// <summary>
    /// Configura el objeto desde HiddenObjectData.
    /// Sprite Stacking: todos en posición (0,0).
    /// </summary>
    public void InitializeFromData()
    {
        if (objectData == null) return;
        
        if (spriteRenderer != null && objectData.ObjectSprite != null)
        {
            spriteRenderer.sprite = objectData.ObjectSprite;
            spriteRenderer.sortingOrder = objectData.SortingOrder;
        }
        
        transform.position = Vector3.zero;
        RegenerateCollider();
        gameObject.name = $"HO_{objectData.ObjectID}";
    }
    
    /// <summary>
    /// Regenera PolygonCollider2D desde el Physics Shape del sprite.
    /// Esto permite clics precisos solo en áreas opacas.
    /// </summary>
    [ContextMenu("Regenerar Collider")]
    public void RegenerateCollider()
    {
        if (polygonCollider == null) polygonCollider = GetComponent<PolygonCollider2D>();
        if (spriteRenderer == null || spriteRenderer.sprite == null) return;
        
        Sprite sprite = spriteRenderer.sprite;
        int shapeCount = sprite.GetPhysicsShapeCount();
        
        if (shapeCount == 0)
        {
            Debug.LogWarning($"[HiddenObjectItem] {name}: Sprite sin Physics Shape.");
            return;
        }
        
        polygonCollider.pathCount = shapeCount;
        for (int i = 0; i < shapeCount; i++)
        {
            var path = new System.Collections.Generic.List<Vector2>();
            sprite.GetPhysicsShape(i, path);
            polygonCollider.SetPath(i, path);
        }
    }
    
    /// <summary>
    /// Llamado cuando el jugador encuentra el objeto.
    /// </summary>
    public void OnDiscovered()
    {
        if (isFound) return;
        
        isFound = true;
        isHinted = false;
        
        if (hintCoroutine != null) StopCoroutine(hintCoroutine);
        
        PlayDiscoveryEffects();
        fadeCoroutine = StartCoroutine(FadeToFoundCoroutine());
        OnObjectFound?.Invoke(this);
    }
    
    private void PlayDiscoveryEffects()
    {
        if (objectData == null) return;
        
        if (objectData.DiscoveryEffectPrefab != null)
        {
            Vector3 pos = spriteRenderer != null ? spriteRenderer.bounds.center : transform.position;
            GameObject effect = Instantiate(objectData.DiscoveryEffectPrefab, pos, Quaternion.identity);
            Destroy(effect, 3f);
        }
        
        if (objectData.FoundSound != null)
        {
            AudioSource.PlayClipAtPoint(objectData.FoundSound, Camera.main.transform.position);
        }
    }
    
    private IEnumerator FadeToFoundCoroutine()
    {
        if (spriteRenderer == null) yield break;
        
        Color startColor = spriteRenderer.color;
        float elapsed = 0f;
        
        while (elapsed < foundFadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = 1f - Mathf.Pow(1f - (elapsed / foundFadeDuration), 3f);
            spriteRenderer.color = Color.Lerp(startColor, foundColor, t);
            yield return null;
        }
        
        spriteRenderer.color = foundColor;
        if (polygonCollider != null) polygonCollider.enabled = false;
    }
    
    /// <summary>
    /// Activa pista visual (brillo pulsante).
    /// </summary>
    public void ActivateHint()
    {
        if (isFound || isHinted) return;
        isHinted = true;
        hintCoroutine = StartCoroutine(HintGlowCoroutine());
        OnHintActivated?.Invoke(this);
    }
    
    public void DeactivateHint()
    {
        isHinted = false;
        if (hintCoroutine != null) StopCoroutine(hintCoroutine);
        if (spriteRenderer != null && !isFound) spriteRenderer.color = normalColor;
    }
    
    private IEnumerator HintGlowCoroutine()
    {
        float time = 0f;
        while (isHinted && !isFound)
        {
            time += Time.deltaTime * glowPulseSpeed;
            float pulse = (Mathf.Sin(time) + 1f) / 2f;
            spriteRenderer.color = Color.Lerp(normalColor, hintGlowColor, pulse);
            yield return null;
        }
        if (!isFound) spriteRenderer.color = normalColor;
    }
    
    public void ResetToInitialState()
    {
        isFound = false;
        isHinted = false;
        if (hintCoroutine != null) StopCoroutine(hintCoroutine);
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        if (spriteRenderer != null) spriteRenderer.color = normalColor;
        if (polygonCollider != null) polygonCollider.enabled = true;
    }
    
    private void OnValidate()
    {
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
        if (objectData != null && spriteRenderer != null && objectData.ObjectSprite != null)
        {
            spriteRenderer.sprite = objectData.ObjectSprite;
            spriteRenderer.sortingOrder = objectData.SortingOrder;
            if (!string.IsNullOrEmpty(objectData.ObjectID))
                gameObject.name = $"HO_{objectData.ObjectID}";
        }
    }
}
