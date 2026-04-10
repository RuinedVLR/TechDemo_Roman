using UnityEngine;

public class RotateGun : MonoBehaviour
{
    public GrapplingGun _grappling;

    private Quaternion _desiredRotation;
    private float _rotationSpeed = 5f;

    private void Update()
    {
        if(!_grappling.IsGrappling())
            _desiredRotation = transform.parent.rotation;
        else
            _desiredRotation = Quaternion.LookRotation(_grappling.GetGrapplePoint() - transform.position);

        transform.rotation = Quaternion.Lerp(transform.rotation, _desiredRotation, Time.deltaTime * _rotationSpeed);
    }
}
