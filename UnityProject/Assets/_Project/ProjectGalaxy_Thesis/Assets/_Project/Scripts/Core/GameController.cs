using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// ═══════════════════════════════════════════════════════════════════════════
/// GAME CONTROLLER - Controlador Principal del Juego HOPA
/// ═══════════════════════════════════════════════════════════════════════════
/// Maneja: Estado del juego, progreso, puntaje, pistas, y eventos.
/// ═══════════════════════════════════════════════════════════════════════════
/// </summary>
public class GameController : MonoBehaviour
{
    // ═══════════════════════════════════════════════════════════════════════
    // SINGLETON
    // ═══════════════════════════════════════════════════════════════════════
    
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
    
    // ═══════════════════════════════════════════════════════════════════════
    // ESTADOS DEL JUEGO
    // ═══════════════════════════════════════════════════════════════════════
    
    public enum GameState
    {
        MainMenu,
        Loading,
        Intro,
        Playing,
        Paused,
        Victory,
        GameOver,
        Complete
    }
    
    [Header("═══ ESTADO ═══")]
    [SerializeField] private GameState currentState = GameState.MainMenu;
    public GameState CurrentState => currentState;
    
    // ═══════════════════════════════════════════════════════════════════════
    // CASOS
    // ═══════════════════════════════════════════════════════════════════════
    
    [Header("═══ CASOS ═══")]
    [SerializeField] private List<CaseData> allCases = new List<CaseData>();
    [SerializeField] private int currentCaseIndex = 0;
    
    public CaseData CurrentCase => 
        (currentCaseIndex >= 0 && currentCaseIndex < allCases.Count) 
            ? allCases[currentCaseIndex] : null;
    
    public int CurrentCaseNumber => currentCaseIndex + 1;
    public int TotalCases => allCases.Count;
    
    // ═══════════════════════════════════════════════════════════════════════
    // TRACKING
    // ═══════════════════════════════════════════════════════════════════════
    
    private List<string> foundObjectIDs = new List<string>();
    private List<HiddenObjectItem> sceneObjects = new List<HiddenObjectItem>();
    private int score = 0;
    
    public int FoundCount => foundObjectIDs.Count;
    public int TotalObjects => CurrentCase?.TotalObjects ?? 0;
    public float Progress => TotalObjects > 0 ? (float)FoundCount / TotalObjects : 0f;
    public int Score => score;
    
    // ═══════════════════════════════════════════════════════════════════════
    // PISTAS
    // ═══════════════════════════════════════════════════════════════════════
    
    [Header("═══ PISTAS ═══")]
    [SerializeField] private float defaultHintDelay = 20f;
    private Coroutine hintTimer;
    
    // ═══════════════════════════════════════════════════════════════════════
    // AUDIO
    // ═══════════════════════════════════════════════════════════════════════
    
    [Header("═══ AUDIO ═══")]
    [SerializeField] private AudioSource musicSource;
    
    // ═══════════════════════════════════════════════════════════════════════
    // EVENTOS - UnityEvent para Inspector
    // ═══════════════════════════════════════════════════════════════════════
    
    [Header("═══ EVENTOS ═══")]
    public UnityEvent<GameState> OnStateChanged;
    public UnityEvent<CaseData> OnCaseLoaded;
    public UnityEvent<HiddenObjectItem> OnObjectFound;
    public UnityEvent<float> OnProgressChanged;
    public UnityEvent<int> OnScoreChanged;
    public UnityEvent<string> OnHintTextChanged;
    public UnityEvent OnCaseComplete;
    public UnityEvent OnGameComplete;
    
    // ═══════════════════════════════════════════════════════════════════════
    // UNITY LIFECYCLE
    // ═══════════════════════════════════════════════════════════════════════
    
    private void Start()
    {
        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.loop = true;
        }
        
        ChangeState(GameState.MainMenu);
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentState == GameState.Playing)
                Pause();
            else if (currentState == GameState.Paused)
                Resume();
        }
    }
    
    // ═══════════════════════════════════════════════════════════════════════
    // CAMBIO DE ESTADO
    // ═══════════════════════════════════════════════════════════════════════
    
    public void ChangeState(GameState newState)
    {
        GameState oldState = currentState;
        currentState = newState;
        
        Debug.Log($"[GameController] {oldState} → {newState}");
        
        switch (newState)
        {
            case GameState.MainMenu:
                Time.timeScale = 1f;
                StopMusic();
                break;
                
            case GameState.Playing:
                Time.timeScale = 1f;
                StartGameplay();
                break;
                
            case GameState.Paused:
                Time.timeScale = 0f;
                break;
                
            case GameState.Victory:
                StopHintTimer();
                OnCaseComplete?.Invoke();
                break;
                
            case GameState.Complete:
                StopMusic();
                OnGameComplete?.Invoke();
                break;
        }
        
        OnStateChanged?.Invoke(newState);
    }
    
    // ═══════════════════════════════════════════════════════════════════════
    // FLUJO DEL JUEGO
    // ═══════════════════════════════════════════════════════════════════════
    
    public void StartGame()
    {
        currentCaseIndex = 0;
        score = 0;
        LoadCurrentCase();
    }
    
    public void LoadCurrentCase()
    {
        if (CurrentCase == null)
        {
            Debug.LogError("[GameController] No hay caso disponible!");
            return;
        }
        
        foundObjectIDs.Clear();
        sceneObjects.Clear();
        
        OnCaseLoaded?.Invoke(CurrentCase);
        
        if (!string.IsNullOrEmpty(CurrentCase.introStory))
            ChangeState(GameState.Intro);
        else
            ChangeState(GameState.Playing);
    }
    
    private void StartGameplay()
    {
        HiddenObjectItem[] items = FindObjectsByType<HiddenObjectItem>(FindObjectsSortMode.None);
        sceneObjects.AddRange(items);
        
        foreach (var item in sceneObjects)
        {
            item.OnFound.RemoveAllListeners();
            item.OnFound.AddListener(HandleObjectFound);
        }
        
        PlayMusic();
        StartHintTimer();
        
        OnProgressChanged?.Invoke(0f);
        OnScoreChanged?.Invoke(score);
        UpdateHintText();
    }
    
    public void RegisterClick(HiddenObjectItem item)
    {
        if (currentState != GameState.Playing) return;
        if (item == null || item.IsFound) return;
        if (foundObjectIDs.Contains(item.ObjectID)) return;
        
        item.Discover();
    }
    
    private void HandleObjectFound(HiddenObjectItem item)
    {
        foundObjectIDs.Add(item.ObjectID);
        
        if (item.Data != null)
            score += item.Data.pointsValue;
        
        OnObjectFound?.Invoke(item);
        OnProgressChanged?.Invoke(Progress);
        OnScoreChanged?.Invoke(score);
        
        StartHintTimer();
        UpdateHintText();
        
        if (FoundCount >= TotalObjects)
            ChangeState(GameState.Victory);
    }
    
    public void NextCase()
    {
        currentCaseIndex++;
        
        if (currentCaseIndex >= allCases.Count)
            ChangeState(GameState.Complete);
        else
            LoadCurrentCase();
    }
    
    // ═══════════════════════════════════════════════════════════════════════
    // PAUSA
    // ═══════════════════════════════════════════════════════════════════════
    
    public void Pause()
    {
        if (currentState == GameState.Playing)
            ChangeState(GameState.Paused);
    }
    
    public void Resume()
    {
        if (currentState == GameState.Paused)
        {
            currentState = GameState.Playing;
            Time.timeScale = 1f;
            OnStateChanged?.Invoke(GameState.Playing);
        }
    }
    
    // ═══════════════════════════════════════════════════════════════════════
    // PISTAS
    // ═══════════════════════════════════════════════════════════════════════
    
    private void StartHintTimer()
    {
        StopHintTimer();
        float delay = CurrentCase?.hintDelay ?? defaultHintDelay;
        if (delay > 0)
            hintTimer = StartCoroutine(HintTimerCoroutine(delay));
    }
    
    private void StopHintTimer()
    {
        if (hintTimer != null)
        {
            StopCoroutine(hintTimer);
            hintTimer = null;
        }
    }
    
    private IEnumerator HintTimerCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        foreach (var item in sceneObjects)
        {
            if (!item.IsFound)
            {
                item.ShowHint();
                break;
            }
        }
    }
    
    public void RevealAll()
    {
        foreach (var item in sceneObjects)
        {
            if (!item.IsFound)
                item.ShowHint();
        }
    }
    
    private void UpdateHintText()
    {
        foreach (var item in sceneObjects)
        {
            if (!item.IsFound && item.Data != null)
            {
                OnHintTextChanged?.Invoke(item.Data.hintText);
                return;
            }
        }
        OnHintTextChanged?.Invoke("");
    }
    
    // ═══════════════════════════════════════════════════════════════════════
    // AUDIO
    // ═══════════════════════════════════════════════════════════════════════
    
    private void PlayMusic()
    {
        if (CurrentCase == null || CurrentCase.backgroundMusic == null) return;
        if (musicSource == null) return;
        
        musicSource.clip = CurrentCase.backgroundMusic;
        musicSource.time = CurrentCase.musicStartTime;
        musicSource.volume = CurrentCase.musicVolume;
        musicSource.Play();
    }
    
    private void StopMusic()
    {
        if (musicSource != null && musicSource.isPlaying)
            musicSource.Stop();
    }
    
    // ═══════════════════════════════════════════════════════════════════════
    // UTILIDADES PÚBLICAS
    // ═══════════════════════════════════════════════════════════════════════
    
    public void RestartCase()
    {
        foreach (var item in sceneObjects)
            item.ResetState();
        
        foundObjectIDs.Clear();
        score = 0;
        ChangeState(GameState.Playing);
    }
    
    public void GoToMainMenu()
    {
        ChangeState(GameState.MainMenu);
    }
    
    public void SkipIntro()
    {
        if (currentState == GameState.Intro)
            ChangeState(GameState.Playing);
    }
}
