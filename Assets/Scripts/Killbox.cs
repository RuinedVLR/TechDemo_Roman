using UnityEngine;

public class Killbox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ServiceHub.Instance.GameManager.GameState = GameState.Dead;
        }
    }
}
