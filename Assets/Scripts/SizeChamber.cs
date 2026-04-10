using UnityEngine;

public class SizeChamber : MonoBehaviour
{
    private enum SizeChamberType
    {
        Shrinking,
        Growing
    }

    [SerializeField] private SizeChamberType _sizeChange;

    private void OnTriggerStay(Collider other)
    {
        other.gameObject.transform.localScale += _sizeChange == SizeChamberType.Shrinking ? Vector3.one * -0.01f : Vector3.one * 0.01f;
        other.gameObject.GetComponent<Rigidbody>().mass += _sizeChange == SizeChamberType.Shrinking ? -0.05f : 0.05f;

        if (other.gameObject.transform.localScale.x < 0.1f)
        {
            other.gameObject.transform.localScale = Vector3.one * 0.1f;
        }
        else if (other.gameObject.transform.localScale.x > 3f)
            {
                other.gameObject.transform.localScale = Vector3.one * 3f;
        }
    }
}
