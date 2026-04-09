using DG.Tweening;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] Transform _startPosition;
    [SerializeField] Transform _finishPosition;
    [SerializeField] float _cycleLength;

    Tween _currentTween;
    Rigidbody _playerRb;
    Vector3 _previousPosition;
    bool _playerOnPlatform;

    void Update()
    {
        if (_playerOnPlatform && _playerRb != null)
        {
            // calculate delta and move player by same amount as platform
            Vector3 delta = transform.position - _previousPosition;
            _playerRb.MovePosition(_playerRb.position + delta);
        }

        _previousPosition = transform.position;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerRb = other.GetComponent<Rigidbody>() ?? other.GetComponentInParent<Rigidbody>();
            _playerOnPlatform = true;

            _currentTween?.Kill();
            _currentTween = transform.DOMove(_finishPosition.position, _cycleLength)
                .SetEase(Ease.InOutSine);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerOnPlatform = false;
            _playerRb = null;

            _currentTween?.Kill();
            _currentTween = transform.DOMove(_startPosition.position, _cycleLength)
                .SetEase(Ease.InOutSine);
        }
    }
}
