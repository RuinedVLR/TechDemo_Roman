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

    [Header("Floating Text Settings")]
    [SerializeField] TMP_Text _floatingTextPrefab;
    [SerializeField] Transform _scoreDisplayLocation;
    [SerializeField] float _floatingTextDuration = 1.5f;
    [SerializeField] float _floatingTextUpDistance = 50f;

    GameManager _gameManager;
    PlayerInteractionController _playerInteractionController;

    bool _isFading;

    public bool IsFadedIn => !_isFadingIn && _fadeCanvasGroup.alpha >= 1f;

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
            _fadeCanvas.SetActive(true);
            _fadeCanvasGroup.alpha += Time.deltaTime;
            if (_fadeCanvasGroup.alpha >= 1)
            {
                _fadeCanvasGroup.alpha = 1;
                _isFadingIn = false;
            }
        }
        else if (_isFadingOut)
        {
            _fadeCanvasGroup.alpha -= Time.deltaTime;
            if (_fadeCanvasGroup.alpha <= 0)
            {
                _fadeCanvasGroup.alpha = 0;
                _isFadingOut = false;
                _isFading = false;
                _fadeCanvas.SetActive(false);
            }
        }
    }

    public void ShowUI()
    {
        if (_isFading) return;

        _isFading = true;
        _fadeCanvas.SetActive(true);
        _isFadingOut = false;
        _isFadingIn = true;
    }

    public void HideUI()
    {
        _isFadingOut = true;
        _isFadingIn = false;
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

    public void ShowFloatingText(int amount)
    {
        StartCoroutine(FloatingTextCoroutine(amount));
    }

    public IEnumerator OnLevelLoad()
    {
        _fadeCanvas.SetActive(true);
        _fadeCanvasGroup.alpha = 1;
        yield return new WaitForSeconds(1f);
        HideUI();
    }

    private IEnumerator FloatingTextCoroutine(int amount)
    {
        if (_floatingTextPrefab == null)
        {
            yield break;
        }

        if (_scoreDisplayLocation == null)
        {
            yield break;
        }

        TMP_Text floatingText = Instantiate(_floatingTextPrefab, _scoreDisplayLocation);
        floatingText.gameObject.SetActive(true);
        floatingText.text = $"+{amount}";
        floatingText.color = Color.green;

        RectTransform rectTransform = floatingText.GetComponent<RectTransform>();
        
        if (rectTransform == null)
        {
            Destroy(floatingText.gameObject);
            yield break;
        }

        float randomX = Random.Range(45f, 300f);
        float randomY = Random.Range(-100f, -20f);
        rectTransform.anchoredPosition = new Vector2(randomX, randomY);
        Vector3 startPos = rectTransform.localPosition;

        float elapsedTime = 0f;

        while (elapsedTime < _floatingTextDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / _floatingTextDuration;

            rectTransform.localPosition = startPos + Vector3.up * (_floatingTextUpDistance * progress);
            floatingText.color = Color.green * (1 - progress);

            yield return null;
        }

        Destroy(floatingText.gameObject);
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
