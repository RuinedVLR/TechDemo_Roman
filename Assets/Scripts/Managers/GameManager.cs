using System.Collections;
using UnityEngine;

public enum GameState
{
    Playing,
    Paused,
    Dead
}

public class GameManager : MonoBehaviour
{
    public GameState GameState { get; set; } = GameState.Playing;

    public GameObject _currentCheckpoint;

    public GameObject _lastCheckpoint;

    public GameObject _player;

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

    private void Update()
    {
        switch (GameState)
        {
            case GameState.Playing:
                Time.timeScale = 1f;
                break;
            case GameState.Paused:
                Time.timeScale = 0f;
                break;
            case GameState.Dead:
                StartCoroutine(Death());
                Time.timeScale = 1f;
                break;
        }


    }
}
