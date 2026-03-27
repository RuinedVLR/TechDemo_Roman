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
        float _lerpSpeed = 20f;
        float _throwForce = 20f;
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
            }

            _playerInteraction = ServiceHub.Instance.PlayerInteractionController;
        }

        private void Update()
        {
            if (_isHolding)
            {
                if (_playerInteraction._grabbedRb)
                {
                    _playerInteraction._grabbedRb.MovePosition(Vector3.Lerp(_playerInteraction._grabbedRb.position, _playerInteraction._objectHolder.transform.position, Time.deltaTime * _lerpSpeed));

                    if (Input.GetMouseButtonDown(0))
                    {
                        _playerInteraction._grabbedRb.isKinematic = false;
                        _playerInteraction._grabbedRb.AddForce(Camera.main.transform.forward * _throwForce, ForceMode.VelocityChange);
                        _playerInteraction._grabbedRb = null;
                        _isHolding = false;
                    }
                }
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
