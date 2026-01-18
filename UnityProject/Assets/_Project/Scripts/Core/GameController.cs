using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// GAME CONTROLLER - Controlador principal con State Machine.
/// Patrón Singleton + MVC Controller.
/// </summary>
public class GameController : MonoBehaviour
{
    #region Singleton
    public static GameController Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    #endregion
    
    #region State Machine
    /// <summary>
    /// Estados del juego HOPA.
    /// </summary>
    public enum GameState
    {
        MainMenu,       // Menú principal
        Intro,          // Novela visual introductoria
        Gameplay,       // Búsqueda de objetos
        Pause,          // Juego pausado
        Victory,        // Caso completado
        Transition,     // Transición entre casos
        GameComplete    // Todos los casos terminados
    }
    
    [Header("═══ ESTADO ACTUAL ═══")]
    [SerializeField] private GameState currentState = GameState.MainMenu;
    public GameState CurrentState => currentState;
    #endregion
    
    #region Case Management
    [Header("═══ CASOS ═══")]
    [Tooltip("Lista ordenada de todos los casos del juego")]
    [SerializeField] private List<CaseData> allCases = new List<CaseData>();
    [SerializeField] private int currentCaseIndex = 0;
    
    public CaseData CurrentCase => 
        currentCaseIndex < allCases.Count ? allCases[currentCaseIndex] : null;
    public int CurrentCaseNumber => currentCaseIndex + 1;
    public int TotalCases => allCases.Count;
    #endregion
    
    #region Session Tracking
    [Header("═══ SESIÓN ACTUAL ═══")]
    private List<string> foundObjectIDs = new List<string>();
    private List<HiddenObjectItem> sceneObjects = new List<HiddenObjectItem>();
    private int currentScore = 0;
    
    public int FoundCount => foundObjectIDs.Count;
    public int TotalCount => CurrentCase?.TotalObjectCount ?? 0;
    public float Progress => TotalCount > 0 ? (float)FoundCount / TotalCount : 0f;
    public int CurrentScore => currentScore;
    #endregion
    
    #region Hint System
    [Header("═══ SISTEMA DE PISTAS ═══")]
    [SerializeField] private float defaultHintDelay = 20f;
    private Coroutine hintCoroutine;
    #endregion
    
    #region Audio
    [Header("═══ AUDIO ═══")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioClip defaultFoundSound;
    #endregion
    
    #region Events
    [Header("═══ EVENTOS ═══")]
    public UnityEvent<GameState> OnStateChanged;
    public UnityEvent<CaseData> OnCaseLoaded;
    public UnityEvent<HiddenObjectItem> OnObjectFound;
    public UnityEvent<float> OnProgressUpdated;
    public UnityEvent<int> OnScoreUpdated;
    public UnityEvent OnCaseCompleted;
    public UnityEvent OnAllCasesCompleted;
    public UnityEvent<HiddenObjectData> OnHintActivated;
    #endregion
    
    #region Unity Lifecycle
    private void Start()
    {
        if (musicSource == null)
            musicSource = gameObject.AddComponent<AudioSource>();
        
        ChangeState(GameState.MainMenu);
    }
    
    private void Update()
    {
        // Pausa con Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentState == GameState.Gameplay)
                PauseGame();
            else if (currentState == GameState.Pause)
                ResumeGame();
        }
    }
    #endregion
    
    #region State Management
    /// <summary>
    /// Cambia el estado del juego y ejecuta lógica asociada.
    /// </summary>
    public void ChangeState(GameState newState)
    {
        GameState previousState = currentState;
        currentState = newState;
        
        Debug.Log($"[GameController] Estado: {previousState} → {newState}");
        OnStateChanged?.Invoke(newState);
        
        switch (newState)
        {
            case GameState.MainMenu:
                Time.timeScale = 1f;
                StopMusic();
                break;
                
            case GameState.Intro:
                Time.timeScale = 1f;
                break;
                
            case GameState.Gameplay:
                Time.timeScale = 1f;
                StartGameplay();
                break;
                
            case GameState.Pause:
                Time.timeScale = 0f;
                break;
                
            case GameState.Victory:
                HandleVictory();
                break;
                
            case GameState.GameComplete:
                HandleGameComplete();
                break;
        }
    }
    #endregion
    
    #region Game Flow
    /// <summary>
    /// Inicia nuevo juego desde el caso 1.
    /// </summary>
    public void StartNewGame()
    {
        currentCaseIndex = 0;
        currentScore = 0;
        LoadCurrentCase();
    }
    
    /// <summary>
    /// Carga el caso actual y prepara la escena.
    /// </summary>
    public void LoadCurrentCase()
    {
        if (CurrentCase == null)
        {
            Debug.LogError("[GameController] No hay caso para cargar!");
            return;
        }
        
        foundObjectIDs.Clear();
        sceneObjects.Clear();
        
        OnCaseLoaded?.Invoke(CurrentCase);
        
        if (CurrentCase.introStory != null)
            ChangeState(GameState.Intro);
        else
            ChangeState(GameState.Gameplay);
    }
    
    private void StartGameplay()
    {
        // Encontrar objetos en escena
        sceneObjects.AddRange(FindObjectsByType<HiddenObjectItem>(FindObjectsSortMode.None));
        
        // Suscribirse a eventos de cada objeto
        foreach (var item in sceneObjects)
        {
            item.OnObjectFound.RemoveListener(HandleObjectFound);
            item.OnObjectFound.AddListener(HandleObjectFound);
        }
        
        // Iniciar música
        PlayCaseMusic();
        
        // Iniciar timer de pistas
        StartHintTimer();
        
        OnProgressUpdated?.Invoke(0f);
        OnScoreUpdated?.Invoke(currentScore);
    }
    
    /// <summary>
    /// Registra un objeto encontrado (llamado por InputController).
    /// </summary>
    public void RegisterFoundObject(HiddenObjectItem item)
    {
        if (currentState != GameState.Gameplay) return;
        if (item.IsFound) return;
        if (foundObjectIDs.Contains(item.ObjectID)) return;
        
        item.OnDiscovered();
    }
    
    private void HandleObjectFound(HiddenObjectItem item)
    {
        foundObjectIDs.Add(item.ObjectID);
        
        // Sumar puntos
        if (item.Data != null)
            currentScore += item.Data.PointsValue;
        
        // Disparar eventos
        OnObjectFound?.Invoke(item);
        OnProgressUpdated?.Invoke(Progress);
        OnScoreUpdated?.Invoke(currentScore);
        
        // Reiniciar timer de pista
        StartHintTimer();
        
        // Verificar victoria
        if (FoundCount >= TotalCount)
            ChangeState(GameState.Victory);
    }
    
    private void HandleVictory()
    {
        StopHintTimer();
        OnCaseCompleted?.Invoke();
    }
    
    /// <summary>
    /// Avanza al siguiente caso.
    /// </summary>
    public void ProceedToNextCase()
    {
        currentCaseIndex++;
        
        if (currentCaseIndex >= allCases.Count)
            ChangeState(GameState.GameComplete);
        else
        {
            ChangeState(GameState.Transition);
            StartCoroutine(TransitionCoroutine());
        }
    }
    
    private IEnumerator TransitionCoroutine()
    {
        yield return new WaitForSeconds(1.5f);
        LoadCurrentCase();
    }
    
    private void HandleGameComplete()
    {
        StopMusic();
        OnAllCasesCompleted?.Invoke();
    }
    #endregion
    
    #region Pause
    public void PauseGame()
    {
        if (currentState == GameState.Gameplay)
            ChangeState(GameState.Pause);
    }
    
    public void ResumeGame()
    {
        if (currentState == GameState.Pause)
        {
            Time.timeScale = 1f;
            currentState = GameState.Gameplay;
            OnStateChanged?.Invoke(GameState.Gameplay);
        }
    }
    #endregion
    
    #region Hint System
    private void StartHintTimer()
    {
        StopHintTimer();
        float delay = CurrentCase?.HintDelaySeconds ?? defaultHintDelay;
        hintCoroutine = StartCoroutine(HintTimerCoroutine(delay));
    }
    
    private void StopHintTimer()
    {
        if (hintCoroutine != null)
        {
            StopCoroutine(hintCoroutine);
            hintCoroutine = null;
        }
    }
    
    private IEnumerator HintTimerCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        foreach (var item in sceneObjects)
        {
            if (!item.IsFound)
            {
                item.ActivateHint();
                OnHintActivated?.Invoke(item.Data);
                break;
            }
        }
    }
    
    /// <summary>
    /// Revela todos los objetos (cheat/accesibilidad).
    /// </summary>
    public void RevealAllObjects()
    {
        foreach (var item in sceneObjects)
        {
            if (!item.IsFound)
                item.ActivateHint();
        }
    }
    #endregion
    
    #region Audio
    private void PlayCaseMusic()
    {
        if (CurrentCase == null || CurrentCase.BackgroundMusic == null) return;
        if (musicSource == null) return;
        
        musicSource.clip = CurrentCase.BackgroundMusic;
        musicSource.time = CurrentCase.MusicStartTime;
        musicSource.volume = CurrentCase.MusicVolume;
        musicSource.loop = true;
        musicSource.Play();
    }
    
    private void StopMusic()
    {
        if (musicSource != null)
            musicSource.Stop();
    }
    #endregion
    
    #region Public Helpers
    /// <summary>
    /// Llamar cuando termina el intro de novela visual.
    /// </summary>
    public void OnIntroComplete()
    {
        if (currentState == GameState.Intro)
            ChangeState(GameState.Gameplay);
    }
    
    /// <summary>
    /// Obtiene pista del siguiente objeto no encontrado.
    /// </summary>
    public string GetNextHint()
    {
        foreach (var item in sceneObjects)
        {
            if (!item.IsFound && item.Data != null)
                return item.Data.HintText;
        }
        return "";
    }
    #endregion
}
