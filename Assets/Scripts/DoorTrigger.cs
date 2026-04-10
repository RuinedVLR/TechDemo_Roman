using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] GameObject _vfx;
    [SerializeField] GameObject _door;
    [SerializeField] Transform _doorOpenPos;
    [SerializeField] Transform _doorClosePos;
    [SerializeField] float _doorSpeed = 2f;

    private Vector3 _targetPosition;
    private bool _isMoving = false;

    private void Start()
    {
        _targetPosition = _door.transform.position;
    }

    private void Update()
    {
        if (_isMoving)
        {
            _vfx.SetActive(true);
            _door.transform.position = Vector3.Lerp(
                _door.transform.position,
                _targetPosition,
                Time.deltaTime * _doorSpeed
            );

            if (Vector3.Distance(_door.transform.position, _targetPosition) < 0.01f)
            {
                _door.transform.position = _targetPosition;
                _isMoving = false;
                _vfx.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _targetPosition = _doorOpenPos.position;
            _isMoving = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _targetPosition = _doorClosePos.position;
            _isMoving = true;
        }
    }
}
