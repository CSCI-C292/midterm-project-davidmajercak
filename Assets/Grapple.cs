using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapple : MonoBehaviour
{
    LineRenderer _line;
    Vector3 grapplePoint;
    [SerializeField] float _grappleDistance;
    [SerializeField] float _jointMaxDistanceMultiplier;
    [SerializeField] float _jointMinDistanceMultiplier;
    [SerializeField] float _jointSpring;
    [SerializeField] float _jointDamper;
    [SerializeField] float _jointMassScale;
    [SerializeField] LayerMask _Grappleable;
    [SerializeField] Transform _grappleStartPoint;
    [SerializeField] GameObject _player;
    [SerializeField] int _linePositionCount;
    Vector3 _grapplePoint;
    [SerializeField] Camera _cam;
    SpringJoint _joint;
    [SerializeField] RuntimeData _runtimeData;
    float _distanceFromPoint;

    void Awake()
    {
        _line = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            StartGrapple();
        }
        else if(Input.GetMouseButtonUp(0))
        {
            StopGrapple();
        }
        else if(_runtimeData.playerIsGrappling)
        {
            //If we're closer to the grapple point than we were when we started the grapple
            if(_distanceFromPoint > Vector3.Distance(_player.transform.position, _grapplePoint))
            {
                //Update the maxDistance so that our SpringJoint doesn't let us get as far away again, allowing tighter grapples around objects
                _distanceFromPoint = Vector3.Distance(_player.transform.position, _grapplePoint);
                _joint.maxDistance = _distanceFromPoint * _jointMaxDistanceMultiplier;
            }
        }
    }

    void LateUpdate()
    {
        DrawRope();
    }

    void StartGrapple()
    {
        RaycastHit hit;
        if(Physics.Raycast(_cam.transform.position, _cam.transform.forward, out hit, _grappleDistance, _Grappleable))
        {
            _grapplePoint = hit.point;
            _runtimeData.playerIsGrappling = true;
            _runtimeData.playerGrapplePoint = _grapplePoint;

            _joint = _player.AddComponent<SpringJoint>();
            _joint.autoConfigureConnectedAnchor = false;
            _joint.connectedAnchor = _grapplePoint;

            _distanceFromPoint = Vector3.Distance(_player.transform.position, _grapplePoint);
            _joint.maxDistance = _distanceFromPoint * _jointMaxDistanceMultiplier;
            _joint.minDistance = _distanceFromPoint * _jointMinDistanceMultiplier;

            _joint.spring = _jointSpring;
            _joint.damper = _jointDamper;
            _joint.massScale = _jointMassScale;

            _line.positionCount = _linePositionCount;

        }
        //If Raycast doesn't hit anything, nothing to do since we can't grapple anything 
    }

    void DrawRope()
    {
        if(!_joint)
            return;
        _line.SetPosition(0, _grappleStartPoint.position);
        _line.SetPosition(1, _grapplePoint);

    }

    void StopGrapple()
    {
        _runtimeData.playerIsGrappling = false;
        _line.positionCount = 0;
        Destroy(_joint);
    }
}
