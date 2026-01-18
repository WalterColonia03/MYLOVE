using UnityEngine;

/// <summary>
/// INPUT CONTROLLER - Maneja entrada del jugador con Raycast 2D.
/// El PolygonCollider2D en cada objeto asegura detección precisa.
/// </summary>
public class InputController : MonoBehaviour
{
    [Header("═══ CONFIGURACIÓN ═══")]
    [Tooltip("Layer de los objetos ocultos (crear en: Edit > Project Settings > Tags and Layers)")]
    [SerializeField] private LayerMask hiddenObjectLayer;
    
    [Tooltip("Referencia a la cámara del juego")]
    [SerializeField] private Camera gameCamera;
    
    [Header("═══ DEBUG ═══")]
    [SerializeField] private bool showDebugRays = false;
    
    private void Start()
    {
        if (gameCamera == null)
            gameCamera = Camera.main;
        
        if (gameCamera == null)
            Debug.LogError("[InputController] No se encontró cámara!");
    }
    
    private void Update()
    {
        // Solo procesar input en estado Gameplay
        if (GameController.Instance == null) return;
        if (GameController.Instance.CurrentState != GameController.GameState.Gameplay) return;
        
        // Click del mouse
        if (Input.GetMouseButtonDown(0))
        {
            ProcessClick(Input.mousePosition);
        }
        
        // Touch en móvil
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            ProcessClick(Input.GetTouch(0).position);
        }
    }
    
    /// <summary>
    /// Procesa un clic/tap en la posición de pantalla dada.
    /// Usa Physics2D.Raycast que respeta la forma del PolygonCollider2D.
    /// </summary>
    private void ProcessClick(Vector2 screenPosition)
    {
        if (gameCamera == null) return;
        
        // Convertir posición de pantalla a mundo
        Vector2 worldPos = gameCamera.ScreenToWorldPoint(screenPosition);
        
        // Raycast 2D - El PolygonCollider2D solo registra hits en áreas del sprite
        RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero, 0f, hiddenObjectLayer);
        
        if (showDebugRays)
        {
            Color debugColor = hit.collider != null ? Color.green : Color.red;
            Debug.DrawRay(worldPos, Vector3.forward * 0.5f, debugColor, 1f);
        }
        
        if (hit.collider != null)
        {
            HiddenObjectItem item = hit.collider.GetComponent<HiddenObjectItem>();
            
            if (item != null && !item.IsFound)
            {
                GameController.Instance.RegisterFoundObject(item);
            }
        }
    }
}
