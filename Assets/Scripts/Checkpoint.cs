using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] bool _isTaken;

    [SerializeField] GameManager _gameManager;
    [SerializeField] Renderer _checkpointRenderer;
    Color _previousColor;

    private void Awake()
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
            _isTaken = true;
            _gameManager._currentCheckpoint = gameObject;
            _checkpointRenderer.material.color = new Color(_previousColor.r, _previousColor.g, _previousColor.b + 255, _previousColor.a);
        }
    }
}
