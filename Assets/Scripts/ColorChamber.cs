using Assets.Scripts;
using UnityEngine;

public class ColorChamber : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Interactable>(out Interactable cube))
        {
            cube._isRainbow = !cube._isRainbow;
        }
    }
}
