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
        [SerializeField] InteractableType _interactableType;

        [Header("Object to spawn")]
        [SerializeField] GameObject _objectToSpawn;
        [SerializeField] Transform _spawnPoint;

        public string _message;

        string IInteractable.Message => _message;

        Outline _outline;

        Rigidbody _rigidbody;

        private void Awake()
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
                //if(PlayerInteractionController.Instance._raycastPosition != null)
                //    _rigidbody.AddForce(PlayerInteractionController.Instance._raycastPosition.forward * 5f, ForceMode.Impulse);
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
