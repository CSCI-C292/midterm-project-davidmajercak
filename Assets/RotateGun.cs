using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateGun : MonoBehaviour
{
    [SerializeField] Grapple grapple;
    [SerializeField] RuntimeData _runtimeData;
    [SerializeField] float _gunRotationSpeed;
    Quaternion _desiredRotation;


    void Update()
    {
        //If not grappling, reset gun rotation to look forward
        if(!_runtimeData.playerIsGrappling)
            _desiredRotation = transform.parent.rotation;
        else
            _desiredRotation = Quaternion.LookRotation(_runtimeData.playerGrapplePoint - transform.position);

        //Use Lerp to interpolate between the current rotation and the desired rotation based on deltaTime and _gunRotationSpeed
        transform.rotation = Quaternion.Lerp(transform.rotation, _desiredRotation, Time.deltaTime * _gunRotationSpeed);
    }
}
