using DG.Tweening;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public Transform _topPosition;
    public Transform _bottomPosition;

    [SerializeField] ParticleSystem _pickUpVFX;

    [SerializeField] float _rotationSpeed = 50f;
    int _rand;

    void Start()
    {
        transform.position = Vector3.Lerp(_bottomPosition.position, _topPosition.position, Random.Range(0f,1f));
        
        _rand = Random.Range(0, 101);
    }

    void Update()
    {
        transform.Rotate(0, _rotationSpeed * Time.deltaTime, 0);
        float value = (Mathf.Sin((Time.time + _rand) * 3f) + 1f) / 2f;
        transform.position = Vector3.Lerp(_bottomPosition.position, _topPosition.position, value);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            int pickupAmount = 10;
            ServiceHub.Instance.PlayerInteractionController.PickUp(gameObject);
            ServiceHub.Instance.UIManager.ShowFloatingText(pickupAmount);
            _pickUpVFX.Play();
            Destroy(gameObject, _pickUpVFX.main.duration);
        }
    }
}
