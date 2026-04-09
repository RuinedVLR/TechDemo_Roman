using Assets.Scripts;
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

    Interactable _currentInteractable;

    public Rigidbody _grabbedRb;

    //public Transform _raycastPosition;

    UIManager _uiManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _uiManager = ServiceHub.Instance.UIManager;
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
}
