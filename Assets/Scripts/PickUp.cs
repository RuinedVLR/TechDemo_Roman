using DG.Tweening;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public Transform _topPosition;
    public Transform _bottomPosition;

    [SerializeField] float _rotationSpeed = 50f;
    [SerializeField] float _moveDuration = 1f;

    void Start()
    {
        MoveToTop();
    }

    void Update()
    {
        transform.Rotate(0, _rotationSpeed * Time.deltaTime, 0);
    }

    void MoveToTop()
    {
        transform.DOMove(_topPosition.position, _moveDuration)
            .SetEase(Ease.InOutSine)
            .OnComplete(MoveToBottom);
    }

    void MoveToBottom()
    {
        transform.DOMove(_bottomPosition.position, _moveDuration)
            .SetEase(Ease.InOutSine)
            .OnComplete(MoveToTop);
    }
}
