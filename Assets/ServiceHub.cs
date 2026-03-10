using UnityEngine;

public class ServiceHub : MonoBehaviour
{
    public static ServiceHub Instance { get; private set; }

    [Header("System References")]
    [SerializeField] GameManager _gameManager;
    [SerializeField] UIManager _uiManager;

    public GameManager GameManager => _gameManager;
    public UIManager UIManager => _uiManager;

    private void Awake()
    {
        #region Singleton Pattern

        // Simple singleton setup for a single-scene game

        Instance = this;

        DontDestroyOnLoad(gameObject);
        #endregion

        // Populate references to each system 
    }
}
