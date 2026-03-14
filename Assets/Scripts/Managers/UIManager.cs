using System.Collections;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Canvases References")]
    [SerializeField] GameObject _gameplayCanvas;
    [SerializeField] GameObject _pauseCanvas;

    [Header("Fade Settings")]
    [SerializeField] GameObject _fadeCanvas;
    [SerializeField] CanvasGroup _fadeCanvasGroup;
    [SerializeField] bool _isFadingIn;
    [SerializeField] bool _isFadingOut;

    [SerializeField] TMP_Text _interactionText;

    GameManager _gameManager;
    PlayerInteractionController _playerInteractionController;

    public IEnumerator OnLevelLoad()
    {
        _fadeCanvas.SetActive(true);
        _fadeCanvasGroup.alpha = 1;
        yield return new WaitForSeconds(1f);
        HideUI();
    }

    private void Awake()
    {
        StartCoroutine(OnLevelLoad());
    }

    private void Start()
    {
        _gameManager = ServiceHub.Instance.GameManager;
        _playerInteractionController = ServiceHub.Instance.PlayerInteractionController;
    }

    private void Update()
    {
        if (_isFadingIn)
        {
            _fadeCanvasGroup.alpha += Time.deltaTime;
            if (_fadeCanvasGroup.alpha >= 1) _isFadingIn = false;
        }
        else if (_isFadingOut)
        {
            _fadeCanvasGroup.alpha -= Time.deltaTime;
            if (_fadeCanvasGroup.alpha <= 0) _isFadingOut = false;
        }
    }

    public void ShowUI()
    {
        _isFadingIn = true;
    }

    public void HideUI()
    {
        _isFadingOut = true;
    }

    public void ShowGameplayUI()
    {
        _gameplayCanvas.SetActive(true);
    }

    public void ShowPauseMenu()
    {
        _pauseCanvas.SetActive(true);
    }

    public void HideAllUI()
    {
        _gameplayCanvas.SetActive(false);
        _pauseCanvas.SetActive(false);
    }

    public void EnableInteractionText(string message)
    {
        _interactionText.text = $"Press {_playerInteractionController._interactionKey} " + message;
    }

    public void DisableInteractionText()
    {
        _interactionText.text = "";
    }

    public void Continue()
    {
        _gameManager._isPaused = false;
        _gameManager.GameState = GameState.Playing;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
