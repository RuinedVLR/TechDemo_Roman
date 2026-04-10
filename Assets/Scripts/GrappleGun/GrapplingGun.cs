using UnityEngine;

public class GrapplingGun : MonoBehaviour
{
    private LineRenderer _lr;
    private Vector3 _grapplePoint;
    private float _maxDistance = 100f;
    public LayerMask _whatIsGrappleable;
    public Transform _gunTip;
    public Transform _camera;
    private Transform _player;
    private SpringJoint _joint;

    private AudioSource _audioSource;
    public AudioClip _grappleOn;
    public AudioClip _grappleOff;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _lr = GetComponent<LineRenderer>();
        _player = ServiceHub.Instance.Player.transform;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartGrapple();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            StopGrapple();
        }
    }

    private void LateUpdate()
    {
        DrawRope();
    }

    void StartGrapple()
    {
        
        
        RaycastHit hit;
        if (Physics.Raycast(_camera.position, _camera.forward, out hit, _maxDistance, _whatIsGrappleable))
        {
            _audioSource.PlayOneShot(_grappleOn);
            _grapplePoint = hit.point;
            _joint = _player.gameObject.AddComponent<SpringJoint>();
            _joint.autoConfigureConnectedAnchor = false;
            _joint.connectedAnchor = _grapplePoint;

            float distanceFromPoint = Vector3.Distance(_player.position, _grapplePoint);

            _joint.maxDistance = distanceFromPoint * 0.8f;
            _joint.minDistance = distanceFromPoint * 0.25f;

            _joint.spring = 25f;
            _joint.damper = 5f;
            _joint.massScale = 15f;

            _lr.positionCount = 2;
            _currentGrapplePosition = _gunTip.position;
        }
    }

    private Vector3 _currentGrapplePosition;

    void DrawRope()
    {
        if(!_joint) return;

        _currentGrapplePosition = Vector3.Lerp(_currentGrapplePosition, _grapplePoint, Time.deltaTime * 20f);

        _lr.SetPosition(0, _gunTip.position);
        _lr.SetPosition(1, _grapplePoint);
    }

    void StopGrapple()
    {
        if(_joint != null)
        {
            _audioSource.PlayOneShot(_grappleOff);
        }

        _lr.positionCount = 0;
        Destroy(_joint);
    }

    public bool IsGrappling()
    {
        return _joint != null;
    }

    public Vector3 GetGrapplePoint()
    {
        return _grapplePoint;
    }
}
