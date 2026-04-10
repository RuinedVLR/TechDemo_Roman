using Assets.Scripts;
using TMPro;
using UnityEngine;

public class PlayerInteractionController : MonoBehaviour
{
    [Header("Activation")]
    public KeyCode _interactionKey = KeyCode.E;
    //public KeyCode _pickUpKey = KeyCode.Mouse0;

    [Header("References")]
    [SerializeField] GameObject _camera;
    [SerializeField] float _playerReach;
    [SerializeField] public Transform _objectHolder;
    [SerializeField] LayerMask _interactableLayer;
    [SerializeField] int _score;
    [SerializeField] TMP_Text _scoreText;
    [SerializeField] AudioClip _pickUpSound;
    AudioSource _audioSource;

    Interactable _currentInteractable;

    public Rigidbody _grabbedRb;

    int _zone1Index = 0;
    int _zone2Index = 1;

    UIManager _uiManager;
    GameManager _gameManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _uiManager = ServiceHub.Instance.UIManager;
        _gameManager = ServiceHub.Instance.GameManager;
        _audioSource = GetComponentInParent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_camera != null)
            CheckInteraction();
        
        if (Input.GetKeyDown(_interactionKey))
        {
            if (_grabbedRb != null)
            {
                ReleaseObject();
            }
            else if (_currentInteractable != null && _currentInteractable._interactableType == InteractableType.MovableObject)
            {
                Rigidbody rb = _currentInteractable.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    GrabObject(rb);
                }
            }
            else if (_currentInteractable != null)
            {
                _currentInteractable.Interact();
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _gameManager.Warp(_zone1Index);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _gameManager.Warp(_zone2Index);
        }
    }

    void CheckInteraction()
    {
        RaycastHit hit;
        Ray ray = new Ray(_camera.transform.position, _camera.transform.forward);
        if (Physics.Raycast(ray, out hit, _playerReach, _interactableLayer))
        {
            if (hit.collider.TryGetComponent<Interactable>(out Interactable newInteractable))
            {
                if (_currentInteractable && newInteractable != _currentInteractable)
                {
                    _currentInteractable.DisableOutline();
                }
                if (newInteractable.enabled)
                {
                    SetNewCurrentInteractable(newInteractable);
                }
                else
                {
                    DisableCurrentInteractable();
                }
            }
            else
            {
                DisableCurrentInteractable();
            }
        }
        else
        {
            DisableCurrentInteractable();
        }

        Debug.DrawRay(_camera.transform.position, _camera.transform.forward, Color.red);
    }

    void GrabObject(Rigidbody rb)
    {
        Debug.Log("Holding");
        _grabbedRb = rb;
        _grabbedRb.GetComponent<Interactable>()._isHolding = true;
        _grabbedRb.useGravity = false;
    }

    void ReleaseObject()
    {
        Debug.Log("Released");
        _grabbedRb.GetComponent<Interactable>()._isHolding = false;
        _grabbedRb.useGravity = true;
        _grabbedRb = null;
    }

    void SetNewCurrentInteractable(Interactable newInteractable)
    {
        _currentInteractable = newInteractable;
        _currentInteractable.EnableOutline();
        _uiManager.EnableInteractionText(_currentInteractable._message);
    }

    void DisableCurrentInteractable()
    {
        _uiManager.DisableInteractionText();
        if (_currentInteractable)
        {
            _currentInteractable.DisableOutline();
            _currentInteractable = null;
        }
    }

    public void PickUp(GameObject pickUp)
    {
        _audioSource.PlayOneShot(_pickUpSound);
        _score += 10;
        _scoreText.text = $"Score: {_score}";
        Destroy(pickUp);
    }
}
