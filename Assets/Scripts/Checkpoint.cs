using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] bool _isTaken;

    [SerializeField] GameManager _gameManager;
    [SerializeField] Renderer _checkpointRenderer;
    Color _previousColor;

    private void Start()
    {
        _gameManager = ServiceHub.Instance.GameManager;
        _checkpointRenderer = GetComponent<Renderer>();

        if (_gameManager == null) Debug.LogError($"Game Manager not found on object {gameObject.name}");

        _previousColor = _checkpointRenderer.material.color;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_isTaken)
        {
            if(_gameManager._lastCheckpoint != this)
            {
                if (_gameManager._lastCheckpoint != null)
                    _gameManager._lastCheckpoint.GetComponent<Checkpoint>().ResetCheckpoint();
                _gameManager._lastCheckpoint = gameObject;
            }

            _isTaken = true;
            _gameManager._currentCheckpoint = gameObject;
            _checkpointRenderer.material.color = new Color(_previousColor.r, _previousColor.g, _previousColor.b + 255, _previousColor.a);
        }
    }

    public void ResetCheckpoint()
    {
        _isTaken = false;
        _checkpointRenderer.material.color = _previousColor;
    }
}
