using System.Collections;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject _canvas;
    [SerializeField] CanvasGroup _canvasGroup;

    [SerializeField] bool _isFadingIn;
    [SerializeField] bool _isFadingOut;

    public IEnumerator OnLevelLoad()
    {
        _canvas.SetActive(true);
        _canvasGroup.alpha = 1;
        yield return new WaitForSeconds(1f);
        HideUI();
    }

    private void Awake()
    {
        StartCoroutine(OnLevelLoad());
    }

    public void ShowUI()
    {
        _isFadingIn = true;
    }

    public void HideUI()
    {
        _isFadingOut = true;
    }

    private void Update()
    {
        if (_isFadingIn)
        {
            _canvasGroup.alpha += Time.deltaTime;
            if (_canvasGroup.alpha >= 1) _isFadingIn = false;
        }
        else if (_isFadingOut)
        {
            _canvasGroup.alpha -= Time.deltaTime;
            if (_canvasGroup.alpha <= 0) _isFadingOut = false;
        }
    }


}
