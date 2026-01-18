using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// ============================================================================
/// CASE DATA - ScriptableObject Principal
/// ============================================================================
/// 
/// Propósito (Tesis):
/// Este ScriptableObject actúa como el "contenedor maestro" de datos para
/// cada CASO (escenario/nivel) del juego HOPA. Centraliza toda la información
/// necesaria para cargar y ejecutar un nivel completo.
/// 
/// Patrón de Diseño:
/// - Data-Oriented Design: Los datos están separados de la lógica
/// - Composition over Inheritance: Compone datos de otros SOs (HiddenObjectData)
/// - Single Responsibility: Solo almacena datos, no lógica de gameplay
/// 
/// Flujo de Uso:
/// 1. Crear nuevo CaseData: Create → HOPA → Case Data
/// 2. Configurar escenario, miembro BTS, objetos ocultos
/// 3. GameController carga el CaseData y genera la escena
/// 
/// ============================================================================
/// </summary>
[CreateAssetMenu(fileName = "Case_XX_MemberName", menuName = "HOPA/Case Data", order = 0)]
public class CaseData : ScriptableObject
{
    // =========================================================================
    // IDENTIFICACIÓN DEL CASO
    // =========================================================================
    
    [Header("═══════════════ IDENTIFICACIÓN DEL CASO ═══════════════")]
    
    [Tooltip("ID único del caso. Formato sugerido: 'Case_01_JK', 'Case_02_SG', etc.")]
    [SerializeField] private string caseID;
    
    [Tooltip("Nombre del caso para mostrar en UI (ej: 'El Misterio de Jungkook')")]
    [SerializeField] private string caseName;
    
    [Tooltip("Número de orden del caso (1-7). Usado para secuenciación.")]
    [Range(1, 7)]
    [SerializeField] private int caseNumber = 1;
    
    /// <summary>
    /// Identificador único del caso para save/load y tracking.
    /// </summary>
    public string CaseID => caseID;
    
    /// <summary>
    /// Título del caso para UI.
    /// </summary>
    public string CaseName => caseName;
    
    /// <summary>
    /// Posición en la secuencia de casos (1 = primero).
    /// </summary>
    public int CaseNumber => caseNumber;
    
    // =========================================================================
    // INFORMACIÓN DEL MIEMBRO BTS
    // =========================================================================
    
    [Header("═══════════════ MIEMBRO BTS ═══════════════")]
    
    [Tooltip("Nombre del miembro protagonista de este caso")]
    [SerializeField] private string memberName;
    
    [Tooltip("Retrato del miembro para UI (panel informativo, diálogos)")]
    [SerializeField] private Sprite memberPortrait;
    
    [Tooltip("Color temático asociado al miembro (para UI dinámica)")]
    [SerializeField] private Color memberThemeColor = Color.white;
    
    /// <summary>
    /// Nombre del miembro BTS (Jungkook, Suga, etc.)
    /// </summary>
    public string MemberName => memberName;
    
    /// <summary>
    /// Imagen de retrato para mostrar en UI.
    /// </summary>
    public Sprite MemberPortrait => memberPortrait;
    
    /// <summary>
    /// Color para personalizar elementos de UI por miembro.
    /// </summary>
    public Color MemberThemeColor => memberThemeColor;
    
    // =========================================================================
    // VISUALES DE ESCENA - SPRITE STACKING SYSTEM
    // =========================================================================
    
    [Header("═══════════════ VISUALES DE ESCENA ═══════════════")]
    
    [Tooltip("Imagen de fondo LIMPIO a 1920x1080. Esta es la capa base sin objetos.")]
    [SerializeField] private Sprite backgroundBase;
    
    [Tooltip("Capas de overlay opcionales (efectos de clima, iluminación, etc.) " +
             "Se renderizan ENCIMA del background pero DEBAJO de los objetos.")]
    [SerializeField] private Sprite[] overlayLayers;
    
    /// <summary>
    /// Sprite del fondo base. Todos los objetos se superponen sobre este.
    /// IMPORTANTE: Debe ser 1920x1080 para alineación perfecta.
    /// </summary>
    public Sprite BackgroundBase => backgroundBase;
    
    /// <summary>
    /// Capas adicionales de ambiente (lluvia, niebla, luz).
    /// </summary>
    public Sprite[] OverlayLayers => overlayLayers;
    
    // =========================================================================
    // OBJETOS OCULTOS
    // =========================================================================
    
    [Header("═══════════════ OBJETOS OCULTOS ═══════════════")]
    
    [Tooltip("Lista de todos los objetos ocultos en este caso. " +
             "Cada HiddenObjectData contiene su sprite, pista, etc.")]
    [SerializeField] private List<HiddenObjectData> hiddenObjects = new List<HiddenObjectData>();
    
    /// <summary>
    /// Colección de objetos ocultos a encontrar en este caso.
    /// El GameController itera sobre esta lista para spawn y tracking.
    /// </summary>
    public List<HiddenObjectData> HiddenObjects => hiddenObjects;
    
    /// <summary>
    /// Cantidad total de objetos a encontrar.
    /// </summary>
    public int TotalObjectCount => hiddenObjects.Count;
    
    // =========================================================================
    // AUDIO
    // =========================================================================
    
    [Header("═══════════════ AUDIO ═══════════════")]
    
    [Tooltip("Música de fondo para este caso. Se reproduce en loop durante gameplay.")]
    [SerializeField] private AudioClip backgroundMusic;
    
    [Tooltip("Segundo donde debe empezar la música (útil para saltar intros largas)")]
    [SerializeField] private float musicStartTime = 0f;
    
    [Tooltip("Volumen de la música (0-1). Permite balancear entre casos.")]
    [Range(0f, 1f)]
    [SerializeField] private float musicVolume = 0.7f;
    
    /// <summary>
    /// Clip de música de ambiente.
    /// </summary>
    public AudioClip BackgroundMusic => backgroundMusic;
    
    /// <summary>
    /// Posición inicial de reproducción en segundos.
    /// </summary>
    public float MusicStartTime => musicStartTime;
    
    /// <summary>
    /// Nivel de volumen normalizado.
    /// </summary>
    public float MusicVolume => musicVolume;
    
    // =========================================================================
    // CONFIGURACIÓN DE GAMEPLAY
    // =========================================================================
    
    [Header("═══════════════ CONFIGURACIÓN DE GAMEPLAY ═══════════════")]
    
    [Tooltip("Tiempo límite en segundos. 0 = sin límite de tiempo.")]
    [SerializeField] private float timeLimit = 0f;
    
    [Tooltip("Segundos de inactividad antes de activar la pista visual (glow)")]
    [SerializeField] private float hintDelaySeconds = 20f;
    
    [Tooltip("¿Permitir usar el botón 'Revelar' en este caso?")]
    [SerializeField] private bool allowRevealButton = true;
    
    /// <summary>
    /// Límite de tiempo para completar el caso (0 = infinito).
    /// </summary>
    public float TimeLimit => timeLimit;
    
    /// <summary>
    /// Delay antes de mostrar pista visual automática.
    /// </summary>
    public float HintDelaySeconds => hintDelaySeconds;
    
    /// <summary>
    /// Si el jugador puede usar ayuda de revelado.
    /// </summary>
    public bool AllowRevealButton => allowRevealButton;
    
    // =========================================================================
    // MENSAJES NARRATIVOS
    // =========================================================================
    
    [Header("═══════════════ MENSAJES NARRATIVOS ═══════════════")]
    
    [Tooltip("Mensaje mostrado al iniciar el caso. Contexto para el jugador.")]
    [TextArea(2, 4)]
    [SerializeField] private string introMessage;
    
    [Tooltip("Contexto narrativo / backstory del caso.")]
    [TextArea(3, 6)]
    [SerializeField] private string storyContext;
    
    [Tooltip("Mensaje de victoria al completar el caso.")]
    [TextArea(2, 4)]
    [SerializeField] private string completionMessage;
    
    /// <summary>
    /// Texto de introducción breve.
    /// </summary>
    public string IntroMessage => introMessage;
    
    /// <summary>
    /// Texto de historia/contexto más extenso.
    /// </summary>
    public string StoryContext => storyContext;
    
    /// <summary>
    /// Mensaje de felicitación al terminar.
    /// </summary>
    public string CompletionMessage => completionMessage;
    
    // =========================================================================
    // MÉTODOS DE UTILIDAD
    // =========================================================================
    
    /// <summary>
    /// Obtiene solo los objetos marcados como pistas clave.
    /// Útil para diferenciar objetos de historia vs decorativos.
    /// </summary>
    /// <returns>Lista filtrada de HiddenObjectData donde IsKeyClue == true</returns>
    public List<HiddenObjectData> GetKeyClues()
    {
        return hiddenObjects.FindAll(obj => obj != null && obj.IsKeyClue);
    }
    
    /// <summary>
    /// Obtiene un objeto por su ID.
    /// </summary>
    /// <param name="objectID">ID del objeto a buscar</param>
    /// <returns>HiddenObjectData o null si no existe</returns>
    public HiddenObjectData GetObjectByID(string objectID)
    {
        return hiddenObjects.Find(obj => obj != null && obj.ObjectID == objectID);
    }
    
    /// <summary>
    /// Calcula los puntos totales posibles en este caso.
    /// </summary>
    /// <returns>Suma de PointsValue de todos los objetos</returns>
    public int GetTotalPossiblePoints()
    {
        int total = 0;
        foreach (var obj in hiddenObjects)
        {
            if (obj != null)
                total += obj.PointsValue;
        }
        return total;
    }
    
    /// <summary>
    /// Verifica si el caso tiene todos los datos mínimos necesarios.
    /// </summary>
    /// <returns>True si el caso se puede cargar correctamente</returns>
    public bool IsValid()
    {
        if (string.IsNullOrWhiteSpace(caseID)) return false;
        if (backgroundBase == null) return false;
        if (hiddenObjects == null || hiddenObjects.Count == 0) return false;
        
        // Verificar que no hay objetos null en la lista
        foreach (var obj in hiddenObjects)
        {
            if (obj == null) return false;
        }
        
        return true;
    }
    
    // =========================================================================
    // VALIDACIÓN EN EDITOR
    // =========================================================================
    
    /// <summary>
    /// Validación automática en el Editor.
    /// Detecta problemas de configuración antes de ejecutar.
    /// </summary>
    private void OnValidate()
    {
        // Validar ID
        if (string.IsNullOrWhiteSpace(caseID))
        {
            Debug.LogError($"[CaseData] '{name}': ¡caseID está vacío! " +
                          "Esto causará problemas de save/load.");
        }
        
        // Validar fondo
        if (backgroundBase == null)
        {
            Debug.LogError($"[CaseData] '{name}': ¡backgroundBase no asignado! " +
                          "El caso no puede cargarse sin fondo.");
        }
        
        // Validar objetos
        if (hiddenObjects == null || hiddenObjects.Count == 0)
        {
            Debug.LogWarning($"[CaseData] '{name}': ¡No hay objetos ocultos definidos!");
        }
        else
        {
            // Verificar objetos null
            for (int i = 0; i < hiddenObjects.Count; i++)
            {
                if (hiddenObjects[i] == null)
                {
                    Debug.LogError($"[CaseData] '{name}': Objeto en índice [{i}] es NULL. " +
                                  "Elimínalo o asigna un HiddenObjectData.");
                }
            }
            
            // Verificar IDs duplicados
            HashSet<string> seenIDs = new HashSet<string>();
            foreach (var obj in hiddenObjects)
            {
                if (obj != null && !string.IsNullOrEmpty(obj.ObjectID))
                {
                    if (seenIDs.Contains(obj.ObjectID))
                    {
                        Debug.LogWarning($"[CaseData] '{name}': ID duplicado detectado: '{obj.ObjectID}'. " +
                                        "Cada objeto debe tener un ID único.");
                    }
                    seenIDs.Add(obj.ObjectID);
                }
            }
        }
        
        // Validar miembro
        if (string.IsNullOrWhiteSpace(memberName))
        {
            Debug.LogWarning($"[CaseData] '{name}': memberName vacío.");
        }
        
        // Auto-generar nombre si está vacío
        if (string.IsNullOrWhiteSpace(caseName) && !string.IsNullOrWhiteSpace(memberName))
        {
            caseName = $"El Misterio de {memberName}";
        }
    }
    
    /// <summary>
    /// Descripción formateada para debug.
    /// </summary>
    public override string ToString()
    {
        return $"[Case: {caseID}] {caseName} - {memberName} ({TotalObjectCount} objetos)";
    }
}
