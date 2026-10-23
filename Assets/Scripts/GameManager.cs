using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Start,
    Playing,
    Paused,
    Victory,
    GameOver
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    

    [Header("State Settings")]
    [SerializeField] private GameState currentState;

    [Header("Settings Enemy")]
    [SerializeField] private EnemyAI[] enemies;
    [SerializeField] private int totalEnemies;
    [SerializeField] private int enemiesRemaining;

    //public event Action<GameState> OnChangeState;
    //public GameState CurrentState => currentState;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

        }
        else
        {
            Debug.LogWarning("Existe una instancia duplicada, destruyendo el objeto");
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        //EnemyAI[] Enemies = FindObjectsByType<EnemyAI>(FindObjectsSortMode.None);
        totalEnemies = enemies.Length;
        enemiesRemaining = totalEnemies;

        ChangeState(GameState.Playing);
        Debug.Log($"<color=cyan>Game Manager</color> Nivel iniciado, total de enemigos: <b>{enemiesRemaining}</b>");
    }

    private void Update()
    {
        if (/*Input.GetKeyDown(KeyCode.Escape) ||*/ Input.GetKeyDown(KeyCode.P))
        {
            Pause();
        }
    }


    public void ChangeState(GameState newState)
    {

        currentState = newState;

        switch (currentState)
        {
            case GameState.Start:
                Time.timeScale = 1f;
                break;

            case GameState.Playing:
                Time.timeScale = 1f;
                Debug.Log("<color=green>Game Manager</color> Estado: <b>PLAYING</b> El juego esta en ejecucion");
                break;

            case GameState.Paused:
                Time.timeScale = 0f; // Congela fisicas y temporizadores, si usa time.deltaTime se congela
                Debug.Log("<color=yellow>Game Manager</color> Estado: <b>PAUSED</b> El tiempo ha sido congelado");
                break;

            case GameState.Victory:
                Time.timeScale = 0f; // Congela el juego al ganar
                Debug.Log("<color=lime>Game Manager</color> <b>VICTORIA</b> Has eliminado a todos los enemigos");
                break;

            case GameState.GameOver:
                Time.timeScale = 0f; // Congela el juego al perder
                Debug.Log("<color=red>Game Manager</color>  <b>GAME OVER</b> El jugador ha sido derrotado");
                break;
        }
    }

    public void Pause()
    {
        if (currentState == GameState.Playing)
        {
            ChangeState(GameState.Paused);
        }
        else if (currentState == GameState.Paused)
        {
            ChangeState(GameState.Playing);
        }
    }


    public void EnemyDefeat()
    {
        // Solo continuar si la partida esta en ejecucion
        if (currentState != GameState.Playing) return;

        enemiesRemaining--;
        Debug.Log($"<color=orange>Game Manager</color> Enemigo eliminad, quedan <b>{enemiesRemaining}</b> enemigos");

        // Evaluar condicion de Victoria
        if (enemiesRemaining <= 0)
        {
            ChangeState(GameState.Victory);
        }
    }


    public void PlayerDead()
    {
        // Solo continaur si la partida esta en ejecucion
        if (currentState != GameState.Playing) return;

        // Cambia a estado de Derrota
        ChangeState(GameState.GameOver);
    }


    public void RestartLevel()
    {
        Time.timeScale = 1f; // Restaurar siempre la escala de tiempo antes de cargar
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}
