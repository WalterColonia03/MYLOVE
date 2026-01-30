using UnityEngine;

/// <summary>
/// ═══════════════════════════════════════════════════════════════════════════
/// INPUT CONTROLLER - Gestión de Entrada del Jugador
/// ═══════════════════════════════════════════════════════════════════════════
/// Detecta clicks/toques y envía al GameController para procesar.
/// Usa Physics2D.Raycast que respeta PolygonCollider2D.
/// ═══════════════════════════════════════════════════════════════════════════
/// </summary>
public class InputController : MonoBehaviour
{
    [Header("═══ CONFIGURACIÓN ═══")]
    [Tooltip("Capa donde están los objetos ocultos")]
    [SerializeField] private LayerMask hiddenObjectsLayer;
    
    [Tooltip("Distancia máxima del raycast")]
    [SerializeField] private float rayDistance = 100f;
    
    private Camera mainCamera;
    
    private void Start()
    {
        mainCamera = Camera.main;
        
        if (mainCamera == null)
        {
            Debug.LogError("[InputController] No se encontró Main Camera!");
        }
    }
    
    private void Update()
    {
        // Solo procesar si estamos jugando
        if (GameController.Instance == null) return;
        if (GameController.Instance.CurrentState != GameController.GameState.Playing) return;
        
        // Detectar click/touch
        if (Input.GetMouseButtonDown(0))
        {
            ProcessClick(Input.mousePosition);
        }
        
        // Touch support para móviles/WebGL
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                ProcessClick(touch.position);
            }
        }
    }
    
    private void ProcessClick(Vector2 screenPosition)
    {
        if (mainCamera == null) return;
        
        // Convertir posición de pantalla a mundo
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(screenPosition);
        Vector2 rayOrigin = new Vector2(worldPos.x, worldPos.y);
        
        // Raycast 2D - respeta PolygonCollider2D
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.zero, rayDistance, hiddenObjectsLayer);
        
        if (hit.collider != null)
        {
            HiddenObjectItem item = hit.collider.GetComponent<HiddenObjectItem>();
            if (item != null)
            {
                GameController.Instance.RegisterClick(item);
            }
        }
    }
}
