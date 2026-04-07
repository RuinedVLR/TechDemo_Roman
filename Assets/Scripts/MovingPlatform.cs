using DG.Tweening;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] Transform _startPosition;
    [SerializeField] Transform _finishPosition;
    [SerializeField] float _cycleLength;

    bool _isActivated;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            gameObject.transform.DOMove(_finishPosition.position, _cycleLength).SetEase(Ease.InOutSine);
            _isActivated = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        DOTween.Clear();
        gameObject.transform.DOMove(_startPosition.position, _cycleLength).SetEase(Ease.InOutSine);
    }
}
