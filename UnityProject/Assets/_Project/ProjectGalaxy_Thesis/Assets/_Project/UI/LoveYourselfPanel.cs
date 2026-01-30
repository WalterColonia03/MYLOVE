using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ═══════════════════════════════════════════════════════════════════════════
/// LOVE YOURSELF PANEL - Helper para estilos de panel
/// ═══════════════════════════════════════════════════════════════════════════
/// Aplica automáticamente el estilo "Love Yourself" a paneles UI.
/// ═══════════════════════════════════════════════════════════════════════════
/// </summary>
[RequireComponent(typeof(Image))]
public class LoveYourselfPanel : MonoBehaviour
{
    [Header("═══ COLORES ═══")]
    [SerializeField] private Color backgroundColor = new Color(0.953f, 0.898f, 0.961f, 0.95f);
    [SerializeField] private Color borderColor = new Color(0.878f, 0.969f, 0.980f, 1f);
    
    [Header("═══ REFERENCIAS ═══")]
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image borderImage;
    
    private void Awake()
    {
        if (backgroundImage == null)
            backgroundImage = GetComponent<Image>();
        
        ApplyStyle();
    }
    
    [ContextMenu("Aplicar Estilo Love Yourself")]
    public void ApplyStyle()
    {
        if (backgroundImage != null)
            backgroundImage.color = backgroundColor;
        
        if (borderImage != null)
            borderImage.color = borderColor;
    }
    
    private void OnValidate()
    {
        ApplyStyle();
    }
}
