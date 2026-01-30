using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// ═══════════════════════════════════════════════════════════════════════════
/// OBJECT ICON UI - Icono de objeto en el HUD
/// ═══════════════════════════════════════════════════════════════════════════
/// </summary>
public class ObjectIconUI : MonoBehaviour
{
    [Header("═══ REFERENCIAS ═══")]
    [SerializeField] private Image iconImage;
    [SerializeField] private Image borderImage;
    [SerializeField] private Image checkmarkImage;
    
    [Header("═══ COLORES ═══")]
    [SerializeField] private Color colorNotFound = new Color(1f, 1f, 1f, 0.6f);
    [SerializeField] private Color colorFound = new Color(0.4f, 1f, 0.6f, 1f);
    [SerializeField] private Color colorBorderNormal = new Color(0.878f, 0.969f, 0.980f, 1f);
    [SerializeField] private Color colorBorderFound = new Color(1f, 0.843f, 0.4f, 1f);
    
    [Header("═══ ANIMACIÓN ═══")]
    [SerializeField] private float foundAnimDuration = 0.5f;
    
    private HiddenObjectData objectData;
    private bool isFound = false;
    
    public string ObjectID => objectData != null ? objectData.objectID : "";
    
    private void Awake()
    {
        if (iconImage == null)
            iconImage = transform.Find("Icon")?.GetComponent<Image>();
        
        if (borderImage == null)
            borderImage = transform.Find("Border")?.GetComponent<Image>();
        
        if (checkmarkImage == null)
            checkmarkImage = transform.Find("Checkmark")?.GetComponent<Image>();
        
        if (checkmarkImage != null)
            checkmarkImage.gameObject.SetActive(false);
    }
    
    public void Setup(HiddenObjectData data)
    {
        objectData = data;
        isFound = false;
        
        if (data == null) return;
        
        if (iconImage != null)
        {
            iconImage.sprite = data.GetIconSprite();
            iconImage.color = colorNotFound;
        }
        
        if (borderImage != null)
            borderImage.color = colorBorderNormal;
        
        if (checkmarkImage != null)
            checkmarkImage.gameObject.SetActive(false);
        
        gameObject.name = $"Icon_{data.objectID}";
    }
    
    public void MarkAsFound()
    {
        if (isFound) return;
        isFound = true;
        
        StartCoroutine(FoundAnimation());
    }
    
    private IEnumerator FoundAnimation()
    {
        float elapsed = 0f;
        Vector3 originalScale = transform.localScale;
        Vector3 largeScale = originalScale * 1.3f;
        
        // Agrandar
        while (elapsed < foundAnimDuration * 0.3f)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / (foundAnimDuration * 0.3f);
            transform.localScale = Vector3.Lerp(originalScale, largeScale, t);
            yield return null;
        }
        
        // Checkmark
        if (checkmarkImage != null)
        {
            checkmarkImage.gameObject.SetActive(true);
            checkmarkImage.color = Color.white;
        }
        
        // Colores
        if (iconImage != null)
            iconImage.color = colorFound;
        
        if (borderImage != null)
            borderImage.color = colorBorderFound;
        
        // Regresar
        elapsed = 0f;
        while (elapsed < foundAnimDuration * 0.5f)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / (foundAnimDuration * 0.5f);
            transform.localScale = Vector3.Lerp(largeScale, originalScale, t);
            yield return null;
        }
        
        transform.localScale = originalScale;
    }
    
    public void ResetIcon()
    {
        isFound = false;
        
        if (iconImage != null)
            iconImage.color = colorNotFound;
        
        if (borderImage != null)
            borderImage.color = colorBorderNormal;
        
        if (checkmarkImage != null)
            checkmarkImage.gameObject.SetActive(false);
        
        transform.localScale = Vector3.one;
    }
}
