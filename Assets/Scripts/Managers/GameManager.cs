using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using URC.Camera;

public enum GameState
{
    Playing,
    Paused,
    Dead
}

public class GameManager : MonoBehaviour
{
    public GameState GameState { get; set; } = GameState.Playing;

    UIManager _uiManager;

    public GameObject _currentCheckpoint;

    public GameObject _lastCheckpoint;

    public GameObject _player;
    [SerializeField] CameraController _camera;

    public bool _isPaused;

    public void Start()
    {
        _uiManager = ServiceHub.Instance.UIManager;
    }

    private void Update()
    {
        switch (GameState)
        {
            case GameState.Playing:
                OnStatePlaying();
                break;
            case GameState.Paused:
                OnStatePause();
                break;
            case GameState.Dead:
                OnStateDead();
                break;
        }
    }

    void OnStatePlaying()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
        _uiManager.HideAllUI();
        _uiManager.ShowGameplayUI();
    }
    
    void OnStatePause()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        _uiManager.HideAllUI();
        _uiManager.ShowPauseMenu();
        Time.timeScale = 0f;
    }

    void OnStateDead()
    {
        StartCoroutine(Death());
        Time.timeScale = 1f;
    }

    public void Paused()
    {
        _isPaused = !_isPaused;

        
        if (_isPaused)
        {
            GameState = GameState.Paused;
            Debug.Log("Paused");
        }
        else
        {
            GameState = GameState.Playing;
            Debug.Log("Unpaused");
        }
    }

    public IEnumerator Death()
    {
        ServiceHub.Instance.UIManager.ShowUI();

        yield return new WaitForSeconds(1f);

        GameState = GameState.Playing;

        if (_currentCheckpoint != null)
            _player.transform.position = _currentCheckpoint.transform.position;
        else
            _player.transform.position = new Vector3(0, 1, 0);

        ServiceHub.Instance.UIManager.HideUI();
    }
}
