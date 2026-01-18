using UnityEngine;

/// <summary>
/// ============================================================================
/// HIDDEN OBJECT DATA - ScriptableObject
/// ============================================================================
/// 
/// Propósito (Tesis):
/// Este ScriptableObject representa los DATOS de un objeto oculto individual.
/// Sigue el principio de "Data-Oriented Design" donde separamos los datos
/// de la lógica, permitiendo configurar objetos desde el Inspector sin código.
/// 
/// Características Clave:
/// - Sprite a tamaño completo (1920x1080) para Sprite Stacking
/// - Icono separado para mostrar en UI/Inventario
/// - Configuración de gameplay (pista clave vs relleno)
/// 
/// Uso:
/// 1. Click derecho en Project → Create → HOPA → Hidden Object Data
/// 2. Configurar sprite, icono, pista, etc.
/// 3. Agregar al CaseData correspondiente
/// 
/// ============================================================================
/// </summary>
[CreateAssetMenu(fileName = "HiddenObject_New", menuName = "HOPA/Hidden Object Data", order = 1)]
public class HiddenObjectData : ScriptableObject
{
    // =========================================================================
    // IDENTIFICACIÓN
    // =========================================================================
    
    [Header("═══════════════ IDENTIFICACIÓN ═══════════════")]
    
    [Tooltip("ID único para este objeto. Usar formato: 'objeto_escena' (ej: 'pajaro_jungkook')")]
    [SerializeField] private string objectID;
    
    [Tooltip("Nombre que se mostrará al jugador en la UI")]
    [SerializeField] private string displayName;
    
    /// <summary>
    /// Identificador único del objeto. Usado para tracking de progreso.
    /// </summary>
    public string ObjectID => objectID;
    
    /// <summary>
    /// Nombre localizable para mostrar en UI.
    /// </summary>
    public string DisplayName => displayName;
    
    // =========================================================================
    // VISUALES - SPRITE STACKING SYSTEM
    // =========================================================================
    
    [Header("═══════════════ VISUALES (Sprite Stacking) ═══════════════")]
    
    [Tooltip("Sprite del objeto a TAMAÑO COMPLETO (1920x1080 con transparencia). " +
             "Este sprite se superpone exactamente sobre el fondo en posición (0,0).")]
    [SerializeField] private Sprite objectSprite;
    
    [Tooltip("Icono pequeño del objeto para mostrar en la UI del inventario o lista de objetivos. " +
             "Puede ser un recorte del sprite o un ícono estilizado.")]
    [SerializeField] private Sprite inventoryIcon;
    
    [Tooltip("Orden de renderizado en el Sorting Layer. Mayor número = más al frente. " +
             "Rango sugerido: 10-99 para objetos, 0-9 para fondos, 100+ para UI.")]
    [Range(0, 100)]
    [SerializeField] private int sortingOrder = 10;
    
    /// <summary>
    /// Sprite completo 1920x1080 para renderizar en escena.
    /// IMPORTANTE: Este sprite debe exportarse SIN recortar (trim) desde Photoshop.
    /// </summary>
    public Sprite ObjectSprite => objectSprite;
    
    /// <summary>
    /// Icono para UI (inventario, lista de objetos, etc.)
    /// </summary>
    public Sprite InventoryIcon => inventoryIcon;
    
    /// <summary>
    /// Orden en el Sorting Layer del SpriteRenderer.
    /// </summary>
    public int SortingOrder => sortingOrder;
    
    // =========================================================================
    // GAMEPLAY
    // =========================================================================
    
    [Header("═══════════════ GAMEPLAY ═══════════════")]
    
    [Tooltip("¿Es este objeto una PISTA CLAVE para la historia? " +
             "Los objetos clave pueden desbloquear narrativa adicional o ser obligatorios.")]
    [SerializeField] private bool isKeyClue = false;
    
    [Tooltip("Texto de pista que se muestra cuando el jugador necesita ayuda. " +
             "Usar emojis para hacerlo más visual (ej: '🐦 Un ave que sueña con volar...')")]
    [TextArea(2, 4)]
    [SerializeField] private string hintText;
    
    [Tooltip("Puntos otorgados al encontrar este objeto. " +
             "Pistas clave pueden dar más puntos.")]
    [SerializeField] private int pointsValue = 100;
    
    /// <summary>
    /// Indica si este objeto es crítico para la narrativa.
    /// Los objetos clave se pueden tratar diferente en la UI.
    /// </summary>
    public bool IsKeyClue => isKeyClue;
    
    /// <summary>
    /// Texto de ayuda mostrado después de un tiempo de inactividad.
    /// </summary>
    public string HintText => hintText;
    
    /// <summary>
    /// Valor en puntos para el sistema de scoring.
    /// </summary>
    public int PointsValue => pointsValue;
    
    // =========================================================================
    // AUDIO Y EFECTOS
    // =========================================================================
    
    [Header("═══════════════ AUDIO Y EFECTOS ═══════════════")]
    
    [Tooltip("Sonido que se reproduce al encontrar este objeto. " +
             "Si es null, se usará el sonido por defecto del GameController.")]
    [SerializeField] private AudioClip foundSound;
    
    [Tooltip("Prefab del efecto de partículas a instanciar cuando se descubre. " +
             "Ejemplo: explosión de estrellas, brillo, etc.")]
    [SerializeField] private GameObject discoveryEffectPrefab;
    
    /// <summary>
    /// Clip de audio opcional para feedback auditivo personalizado.
    /// </summary>
    public AudioClip FoundSound => foundSound;
    
    /// <summary>
    /// Sistema de partículas para celebración visual.
    /// </summary>
    public GameObject DiscoveryEffectPrefab => discoveryEffectPrefab;
    
    // =========================================================================
    // VALIDACIÓN EN EDITOR
    // =========================================================================
    
    /// <summary>
    /// OnValidate se ejecuta en el Editor cada vez que cambia un valor.
    /// Útil para detectar errores de configuración temprano.
    /// </summary>
    private void OnValidate()
    {
        // Validar ID único
        if (string.IsNullOrWhiteSpace(objectID))
        {
            Debug.LogWarning($"[HiddenObjectData] '{name}': ¡objectID está vacío! " +
                           "Asigna un ID único para este objeto.");
        }
        
        // Validar sprite principal
        if (objectSprite == null)
        {
            Debug.LogWarning($"[HiddenObjectData] '{name}': ¡objectSprite no asignado! " +
                           "El objeto no será visible en la escena.");
        }
        
        // Validar icono (opcional pero recomendado)
        if (inventoryIcon == null && objectSprite != null)
        {
            Debug.Log($"[HiddenObjectData] '{name}': inventoryIcon vacío. " +
                     "Se usará objectSprite como fallback en UI si es necesario.");
        }
        
        // Validar pista
        if (string.IsNullOrWhiteSpace(hintText))
        {
            Debug.Log($"[HiddenObjectData] '{name}': hintText vacío. " +
                     "Considera agregar una pista para mejorar la experiencia.");
        }
        
        // Auto-generar ID desde nombre si está vacío
        if (string.IsNullOrWhiteSpace(objectID) && !string.IsNullOrWhiteSpace(name))
        {
            objectID = name.ToLower().Replace(" ", "_");
        }
    }
    
    // =========================================================================
    // MÉTODOS DE UTILIDAD
    // =========================================================================
    
    /// <summary>
    /// Obtiene el sprite adecuado para mostrar en UI.
    /// Prioriza inventoryIcon, usa objectSprite como fallback.
    /// </summary>
    /// <returns>Sprite para UI o null si no hay ninguno</returns>
    public Sprite GetUISprite()
    {
        return inventoryIcon != null ? inventoryIcon : objectSprite;
    }
    
    /// <summary>
    /// Genera una descripción formateada para debug o logs.
    /// </summary>
    public override string ToString()
    {
        return $"[HiddenObject: {objectID}] {displayName} " +
               $"(KeyClue: {isKeyClue}, Points: {pointsValue})";
    }
}
