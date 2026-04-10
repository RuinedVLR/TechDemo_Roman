using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
    public enum InteractableType
    {
        SpawnObjectButton,
        WarpPanel,
        MovableObject
    }

    public class Interactable : MonoBehaviour, IInteractable
    {
        [Header("Type of Interactable")]
        [SerializeField] public InteractableType _interactableType;

        [Header("Object to spawn")]
        [SerializeField] GameObject _objectToSpawn;
        [SerializeField] Transform _spawnPoint;

        [Header("Movable Object")]
        public bool _isRainbow;
        [SerializeField] float _rainbowSpeed;
        private float _hue;
        private float _sat;
        private float _bri;
        private MeshRenderer _meshRenderer;
        float _lerpSpeed = 20f;
        public bool _isHolding;

        PlayerInteractionController _playerInteraction;

        public string _message;

        string IInteractable.Message => _message;

        Outline _outline;

        Rigidbody _rigidbody;

        private void Start()
        {
            _outline = GetComponent<Outline>();
            if (_outline == null)
                _outline = gameObject.AddComponent<Outline>();
            _outline.enabled = false;

            if(_interactableType == InteractableType.MovableObject)
            {
                _rigidbody = GetComponent<Rigidbody>();
                if (_rigidbody == null)
                    _rigidbody = gameObject.AddComponent<Rigidbody>();
                _meshRenderer = GetComponent<MeshRenderer>();
            }

            _playerInteraction = ServiceHub.Instance.PlayerInteractionController;
        }

        private void Update()
        {
            if (_isHolding)
            {
                if (_playerInteraction._grabbedRb)
                {
                    Vector3 targetPosition = _playerInteraction._objectHolder.transform.position;
                    Vector3 newPosition = Vector3.Lerp(_playerInteraction._grabbedRb.position, targetPosition, Time.deltaTime * _lerpSpeed);

                    // calculate velocity as delta position over delta time
                    _playerInteraction._grabbedRb.linearVelocity = (newPosition - _playerInteraction._grabbedRb.position) / Time.deltaTime;
                    _playerInteraction._grabbedRb.MovePosition(newPosition);

                    if (Input.GetMouseButtonDown(0))
                    {
                        // save the current velocity for throwing
                        Vector3 throwVelocity = _playerInteraction._grabbedRb.linearVelocity;

                        _playerInteraction._grabbedRb.useGravity = true;
                        _playerInteraction._grabbedRb.linearVelocity = throwVelocity;
                        _playerInteraction._grabbedRb = null;
                        _isHolding = false;
                    }
                }
            }

            if (_isRainbow)
            {
                Color.RGBToHSV(_meshRenderer.material.color, out _hue, out _sat, out _bri);
                _hue += _rainbowSpeed / 10000;
                if (_hue >= 1)
                {
                    _hue = 0;
                }
                _sat = 1;
                _bri = 1;
                _meshRenderer.material.color = Color.HSVToRGB(_hue, _sat, _bri);
            }
        }

        public void Interact()
        {
            if(_interactableType == InteractableType.SpawnObjectButton)
            {
                SpawnObject();
            }
            else if(_interactableType == InteractableType.WarpPanel)
            {

            }
            else if(_interactableType == InteractableType.MovableObject)
            {
                
            }
        }

        public void SpawnObject()
        {
            if(_objectToSpawn != null && _spawnPoint != null)
                Instantiate(_objectToSpawn, _spawnPoint.position, _spawnPoint.rotation);
        }

        public void Warp()
        {

        }

        public void MoveObject()
        {

        }

        public void EnableOutline()
        {
            if (_outline != null)
                _outline.enabled = true;
        }

        public void DisableOutline()
        {
            if (_outline != null)
                _outline.enabled = false;
        }
    }
}
