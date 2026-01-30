using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// ═══════════════════════════════════════════════════════════════════════════
/// CASE DATA - ScriptableObject Principal
/// ═══════════════════════════════════════════════════════════════════════════
/// Define TODA la información de un caso/nivel.
/// TODOS los campos son PUBLIC para acceso desde UIManager y GameController.
/// ═══════════════════════════════════════════════════════════════════════════
/// </summary>
[CreateAssetMenu(fileName = "Case_XX_Nombre", menuName = "HOPA/Case Data", order = 0)]
public class CaseData : ScriptableObject
{
    // ═══════════════════════════════════════════════════════════════════════
    // IDENTIFICACIÓN (PUBLIC)
    // ═══════════════════════════════════════════════════════════════════════
    
    [Header("IDENTIFICACIÓN")]
    public string caseID;
    public string caseName;
    
    [Range(1, 7)]
    public int caseNumber = 1;
    
    // ═══════════════════════════════════════════════════════════════════════
    // MIEMBRO BTS (PUBLIC)
    // ═══════════════════════════════════════════════════════════════════════
    
    [Header("MIEMBRO BTS")]
    public string memberName;
    public Sprite memberPortrait;
    
    // ═══════════════════════════════════════════════════════════════════════
    // ESCENA (PUBLIC)
    // ═══════════════════════════════════════════════════════════════════════
    
    [Header("ESCENA")]
    public Sprite backgroundSprite;
    
    // ═══════════════════════════════════════════════════════════════════════
    // OBJETOS OCULTOS (PUBLIC)
    // ═══════════════════════════════════════════════════════════════════════
    
    [Header("OBJETOS OCULTOS")]
    public List<HiddenObjectData> hiddenObjects = new List<HiddenObjectData>();
    
    /// <summary>Total de objetos en este caso</summary>
    public int TotalObjects => hiddenObjects != null ? hiddenObjects.Count : 0;
    
    // ═══════════════════════════════════════════════════════════════════════
    // AUDIO (PUBLIC)
    // ═══════════════════════════════════════════════════════════════════════
    
    [Header("AUDIO")]
    public AudioClip backgroundMusic;
    public float musicStartTime = 0f;
    
    [Range(0f, 1f)]
    public float musicVolume = 0.7f;
    
    // ═══════════════════════════════════════════════════════════════════════
    // CONFIGURACIÓN (PUBLIC)
    // ═══════════════════════════════════════════════════════════════════════
    
    [Header("CONFIGURACIÓN")]
    public float hintDelay = 20f;
    public float timeLimit = 0f;
    
    // ═══════════════════════════════════════════════════════════════════════
    // NARRATIVA (PUBLIC)
    // ═══════════════════════════════════════════════════════════════════════
    
    [Header("NARRATIVA")]
    [TextArea(2, 4)]
    public string introMessage;
    
    [TextArea(2, 4)]
    public string storyContext;
    
    [TextArea(2, 4)]
    public string victoryMessage;
    
    [TextArea(3, 6)]
    public string introStory;
    
    // ═══════════════════════════════════════════════════════════════════════
    // MÉTODOS PÚBLICOS
    // ═══════════════════════════════════════════════════════════════════════
    
    public List<HiddenObjectData> GetKeyClues()
    {
        List<HiddenObjectData> keys = new List<HiddenObjectData>();
        if (hiddenObjects != null)
        {
            foreach (var obj in hiddenObjects)
            {
                if (obj != null && obj.isKeyClue)
                    keys.Add(obj);
            }
        }
        return keys;
    }
    
    public HiddenObjectData GetObjectByID(string id)
    {
        if (hiddenObjects == null) return null;
        foreach (var obj in hiddenObjects)
        {
            if (obj != null && obj.objectID == id)
                return obj;
        }
        return null;
    }
    
    public int GetMaxScore()
    {
        int total = 0;
        if (hiddenObjects != null)
        {
            foreach (var obj in hiddenObjects)
            {
                if (obj != null)
                    total += obj.pointsValue;
            }
        }
        return total;
    }
    
    public bool IsValid()
    {
        return !string.IsNullOrEmpty(caseID) && 
               backgroundSprite != null && 
               hiddenObjects != null && 
               hiddenObjects.Count > 0;
    }
    
    private void OnValidate()
    {
        if (string.IsNullOrEmpty(caseID))
            Debug.LogError($"[CaseData] '{name}': caseID vacío!");
        if (backgroundSprite == null)
            Debug.LogError($"[CaseData] '{name}': Sin fondo!");
        if (hiddenObjects == null || hiddenObjects.Count == 0)
            Debug.LogWarning($"[CaseData] '{name}': Sin objetos!");
    }
}
