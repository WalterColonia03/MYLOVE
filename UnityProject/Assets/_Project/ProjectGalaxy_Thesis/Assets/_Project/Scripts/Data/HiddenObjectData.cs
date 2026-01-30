using UnityEngine;

/// <summary>
/// ═══════════════════════════════════════════════════════════════════════════
/// HIDDEN OBJECT DATA - ScriptableObject
/// ═══════════════════════════════════════════════════════════════════════════
/// Define los datos de UN objeto oculto individual.
/// TODOS los campos son PUBLIC para acceso desde UIManager y otros scripts.
/// ═══════════════════════════════════════════════════════════════════════════
/// </summary>
[CreateAssetMenu(fileName = "HiddenObject_Nuevo", menuName = "HOPA/Hidden Object Data", order = 1)]
public class HiddenObjectData : ScriptableObject
{
    // ═══════════════════════════════════════════════════════════════════════
    // IDENTIFICACIÓN (PUBLIC)
    // ═══════════════════════════════════════════════════════════════════════
    
    [Header("IDENTIFICACIÓN")]
    [Tooltip("ID único (ej: 'pajaro_jk', 'camisa_jk')")]
    public string objectID;
    
    [Tooltip("Nombre para mostrar al jugador")]
    public string displayName;
    
    // ═══════════════════════════════════════════════════════════════════════
    // SPRITES (PUBLIC)
    // ═══════════════════════════════════════════════════════════════════════
    
    [Header("SPRITES")]
    [Tooltip("Sprite COMPLETO 1920x1080 con transparencia")]
    public Sprite objectSprite;
    
    [Tooltip("Icono pequeño para la UI/inventario")]
    public Sprite iconSprite;
    
    [Tooltip("Orden de dibujo (Mayor = más al frente)")]
    [Range(0, 100)]
    public int sortingOrder = 10;
    
    // ═══════════════════════════════════════════════════════════════════════
    // GAMEPLAY (PUBLIC)
    // ═══════════════════════════════════════════════════════════════════════
    
    [Header("GAMEPLAY")]
    [Tooltip("¿Es una pista importante para la historia?")]
    public bool isKeyClue = false;
    
    [Tooltip("Texto de ayuda")]
    [TextArea(2, 4)]
    public string hintText;
    
    [Tooltip("Puntos al encontrar este objeto")]
    public int pointsValue = 100;
    
    // ═══════════════════════════════════════════════════════════════════════
    // EFECTOS (PUBLIC)
    // ═══════════════════════════════════════════════════════════════════════
    
    [Header("EFECTOS")]
    public AudioClip foundSound;
    public GameObject effectPrefab;
    
    // ═══════════════════════════════════════════════════════════════════════
    // MÉTODOS PÚBLICOS
    // ═══════════════════════════════════════════════════════════════════════
    
    /// <summary>
    /// Devuelve el icono para UI. Si no hay icono, usa el sprite principal.
    /// </summary>
    public Sprite GetIconSprite()
    {
        return iconSprite != null ? iconSprite : objectSprite;
    }
    
    private void OnValidate()
    {
        if (string.IsNullOrEmpty(objectID))
            Debug.LogWarning($"[HiddenObjectData] '{name}': objectID vacío!");
        if (objectSprite == null)
            Debug.LogWarning($"[HiddenObjectData] '{name}': Sin sprite!");
    }
}
