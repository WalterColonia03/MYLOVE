using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

/// <summary>
/// ═══════════════════════════════════════════════════════════════════════════
/// UI MANAGER - Sistema de Interfaz de Usuario
/// ═══════════════════════════════════════════════════════════════════════════
/// Gestiona toda la UI del juego. Se suscribe a eventos del GameController.
/// ═══════════════════════════════════════════════════════════════════════════
/// </summary>
public class UIManager : MonoBehaviour
{
    // ═══════════════════════════════════════════════════════════════════════
    // SINGLETON
    // ═══════════════════════════════════════════════════════════════════════
    
    public static UIManager Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    
    // ═══════════════════════════════════════════════════════════════════════
    // PALETA DE COLORES - "Love Yourself"
    // ═══════════════════════════════════════════════════════════════════════
    
    [Header("═══ PALETA LOVE YOURSELF ═══")]
    public Color colorPanelBg = new Color(0.953f, 0.898f, 0.961f, 0.9f);
    public Color colorCyan = new Color(0.878f, 0.969f, 0.980f, 1f);
    public Color colorPurpleDeep = new Color(0.290f, 0.078f, 0.549f, 1f);
    public Color colorPink = new Color(0.976f, 0.733f, 0.816f, 1f);
    public Color colorGold = new Color(1f, 0.843f, 0.4f, 1f);
    
    // ═══════════════════════════════════════════════════════════════════════
    // REFERENCIAS DE PANELES
    // ═══════════════════════════════════════════════════════════════════════
    
    [Header("═══ PANELES ═══")]
    [SerializeField] private GameObject panelMainMenu;
    [SerializeField] private GameObject panelGameplay;
    [SerializeField] private GameObject panelPause;
    [SerializeField] private GameObject panelVictory;
    [SerializeField] private GameObject panelGameComplete;
    
    // ═══════════════════════════════════════════════════════════════════════
    // HUD DE GAMEPLAY
    // ═══════════════════════════════════════════════════════════════════════
    
    [Header("═══ HUD GAMEPLAY ═══")]
    [SerializeField] private TextMeshProUGUI txtMemberName;
    [SerializeField] private TextMeshProUGUI txtProgress;
    [SerializeField] private TextMeshProUGUI txtScore;
    [SerializeField] private TextMeshProUGUI txtHint;
    [SerializeField] private Slider progressBar;
    [SerializeField] private Image imgMemberPortrait;
    [SerializeField] private Transform objectIconsContainer;
    [SerializeField] private GameObject objectIconPrefab;
    
    // ═══════════════════════════════════════════════════════════════════════
    // VICTORIA
    // ═══════════════════════════════════════════════════════════════════════
    
    [Header("═══ VICTORIA ═══")]
    [SerializeField] private TextMeshProUGUI txtVictoryTitle;
    [SerializeField] private TextMeshProUGUI txtVictoryMessage;
    [SerializeField] private Image imgVictoryPortrait;
    
    // ═══════════════════════════════════════════════════════════════════════
    // ANIMACIÓN
    // ═══════════════════════════════════════════════════════════════════════
    
    [Header("═══ ANIMACIÓN ═══")]
    [SerializeField] private float fadeSpeed = 0.3f;
    [SerializeField] private float pulseSpeed = 2f;
    
    private Dictionary<string, ObjectIconUI> objectIcons = new Dictionary<string, ObjectIconUI>();
    
    // ═══════════════════════════════════════════════════════════════════════
    // UNITY LIFECYCLE
    // ═══════════════════════════════════════════════════════════════════════
    
    private void Start()
    {
        SubscribeToEvents();
        HideAllPanels();
        ShowPanel(panelMainMenu);
    }
    
    private void OnDestroy()
    {
        UnsubscribeFromEvents();
    }
    
    // ═══════════════════════════════════════════════════════════════════════
    // EVENTOS
    // ═══════════════════════════════════════════════════════════════════════
    
    private void SubscribeToEvents()
    {
        if (GameController.Instance == null) return;
        
        GameController.Instance.OnStateChanged.AddListener(HandleStateChanged);
        GameController.Instance.OnCaseLoaded.AddListener(HandleCaseLoaded);
        GameController.Instance.OnObjectFound.AddListener(HandleObjectFound);
        GameController.Instance.OnProgressChanged.AddListener(HandleProgressChanged);
        GameController.Instance.OnScoreChanged.AddListener(HandleScoreChanged);
        GameController.Instance.OnHintTextChanged.AddListener(HandleHintChanged);
        GameController.Instance.OnCaseComplete.AddListener(HandleCaseComplete);
        GameController.Instance.OnGameComplete.AddListener(HandleGameComplete);
    }
    
    private void UnsubscribeFromEvents()
    {
        if (GameController.Instance == null) return;
        
        GameController.Instance.OnStateChanged.RemoveListener(HandleStateChanged);
        GameController.Instance.OnCaseLoaded.RemoveListener(HandleCaseLoaded);
        GameController.Instance.OnObjectFound.RemoveListener(HandleObjectFound);
        GameController.Instance.OnProgressChanged.RemoveListener(HandleProgressChanged);
        GameController.Instance.OnScoreChanged.RemoveListener(HandleScoreChanged);
        GameController.Instance.OnHintTextChanged.RemoveListener(HandleHintChanged);
        GameController.Instance.OnCaseComplete.RemoveListener(HandleCaseComplete);
        GameController.Instance.OnGameComplete.RemoveListener(HandleGameComplete);
    }
    
    // ═══════════════════════════════════════════════════════════════════════
    // HANDLERS
    // ═══════════════════════════════════════════════════════════════════════
    
    private void HandleStateChanged(GameController.GameState newState)
    {
        HideAllPanels();
        
        switch (newState)
        {
            case GameController.GameState.MainMenu:
                ShowPanel(panelMainMenu);
                break;
            case GameController.GameState.Playing:
                ShowPanel(panelGameplay);
                break;
            case GameController.GameState.Paused:
                ShowPanel(panelGameplay);
                ShowPanel(panelPause);
                break;
            case GameController.GameState.Victory:
                ShowPanel(panelVictory);
                break;
            case GameController.GameState.Complete:
                ShowPanel(panelGameComplete);
                break;
        }
    }
    
    private void HandleCaseLoaded(CaseData caseData)
    {
        if (caseData == null) return;
        
        if (txtMemberName != null)
            txtMemberName.text = caseData.memberName;
        
        if (imgMemberPortrait != null && caseData.memberPortrait != null)
            imgMemberPortrait.sprite = caseData.memberPortrait;
        
        CreateObjectIcons(caseData);
        UpdateProgress(0f);
        if (txtScore != null) txtScore.text = "0";
    }
    
    private void HandleObjectFound(HiddenObjectItem item)
    {
        if (item == null || item.Data == null) return;
        
        string id = item.Data.objectID;
        if (objectIcons.ContainsKey(id))
            objectIcons[id].MarkAsFound();
    }
    
    private void HandleProgressChanged(float progress)
    {
        UpdateProgress(progress);
    }
    
    private void HandleScoreChanged(int score)
    {
        if (txtScore != null)
            txtScore.text = score.ToString();
    }
    
    private void HandleHintChanged(string hint)
    {
        if (txtHint != null)
            txtHint.text = hint;
    }
    
    private void HandleCaseComplete()
    {
        var caseData = GameController.Instance.CurrentCase;
        if (caseData == null) return;
        
        if (txtVictoryTitle != null)
            txtVictoryTitle.text = $"✨ {caseData.memberName} ✨";
        
        if (txtVictoryMessage != null)
            txtVictoryMessage.text = caseData.victoryMessage;
        
        if (imgVictoryPortrait != null && caseData.memberPortrait != null)
            imgVictoryPortrait.sprite = caseData.memberPortrait;
    }
    
    private void HandleGameComplete()
    {
        // Panel se muestra automáticamente
    }
    
    // ═══════════════════════════════════════════════════════════════════════
    // PANELES
    // ═══════════════════════════════════════════════════════════════════════
    
    private void HideAllPanels()
    {
        SetPanelActive(panelMainMenu, false);
        SetPanelActive(panelGameplay, false);
        SetPanelActive(panelPause, false);
        SetPanelActive(panelVictory, false);
        SetPanelActive(panelGameComplete, false);
    }
    
    private void ShowPanel(GameObject panel)
    {
        if (panel != null)
            panel.SetActive(true);
    }
    
    private void SetPanelActive(GameObject panel, bool active)
    {
        if (panel != null)
            panel.SetActive(active);
    }
    
    // ═══════════════════════════════════════════════════════════════════════
    // ICONOS
    // ═══════════════════════════════════════════════════════════════════════
    
    private void CreateObjectIcons(CaseData caseData)
    {
        ClearObjectIcons();
        
        if (objectIconsContainer == null || objectIconPrefab == null) return;
        
        foreach (var objData in caseData.hiddenObjects)
        {
            if (objData == null) continue;
            
            GameObject iconGO = Instantiate(objectIconPrefab, objectIconsContainer);
            ObjectIconUI iconUI = iconGO.GetComponent<ObjectIconUI>();
            
            if (iconUI != null)
            {
                iconUI.Setup(objData);
                objectIcons[objData.objectID] = iconUI;
            }
        }
    }
    
    private void ClearObjectIcons()
    {
        foreach (var kvp in objectIcons)
        {
            if (kvp.Value != null)
                Destroy(kvp.Value.gameObject);
        }
        objectIcons.Clear();
    }
    
    private void UpdateProgress(float progress)
    {
        if (progressBar != null)
            progressBar.value = progress;
        
        if (txtProgress != null && GameController.Instance != null)
        {
            int found = GameController.Instance.FoundCount;
            int total = GameController.Instance.TotalObjects;
            txtProgress.text = $"{found}/{total}";
        }
    }
    
    // ═══════════════════════════════════════════════════════════════════════
    // BOTONES
    // ═══════════════════════════════════════════════════════════════════════
    
    public void OnClickStartGame()
    {
        GameController.Instance?.StartGame();
    }
    
    public void OnClickPause()
    {
        GameController.Instance?.Pause();
    }
    
    public void OnClickResume()
    {
        GameController.Instance?.Resume();
    }
    
    public void OnClickNextCase()
    {
        GameController.Instance?.NextCase();
    }
    
    public void OnClickRestart()
    {
        GameController.Instance?.RestartCase();
    }
    
    public void OnClickMainMenu()
    {
        GameController.Instance?.GoToMainMenu();
    }
    
    public void OnClickReveal()
    {
        GameController.Instance?.RevealAll();
    }
}
