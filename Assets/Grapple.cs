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
    [SerializeField] Camera _cam;
    SpringJoint _joint;
    [SerializeField] RuntimeData _runtimeData;
    float _distanceFromPoint;
    float _lastDistanceFromPoint;
    float _initialDistanceFromPoint;
    GameObject _GrapplePointTracker;

    LineTextureMode _textureMode = LineTextureMode.Stretch;

    bool canGrapple;
    [SerializeField] Material _gunMaterialDefault;
    [SerializeField] Material _gunMaterialLocked;
    [SerializeField] Material _grappleMaterialDefault;
    [SerializeField] Material _grappleMaterialLocked;
    Renderer _gunRenderer;

    void Awake()
    {
        _line = GetComponent<LineRenderer>();
        //Gets rid of annoying line that appears at start otherwise
        _line.SetPosition(0, Vector3.up * -100);
        _line.SetPosition(1, Vector3.up * -100);
        _line.textureMode = _textureMode;
    }

    void Start()
    {
        canGrapple = false;
        _GrapplePointTracker = new GameObject();
        _gunRenderer = gameObject.GetComponent<Renderer>();
    }

    void Update()
    {
        _runtimeData.playerGrapplePoint = _GrapplePointTracker.transform.position;
        if(_runtimeData.playerIsGrappling)
            _joint.connectedAnchor = _GrapplePointTracker.transform.position;
        SetCanGrapple();


        //This if statement allows player to grapple an object even if mouse is down before they mouse over an object
        //This makes it so grappling is less dependent on twitch reflexes
        if(Input.GetMouseButton(0) && !_runtimeData.playerIsGrappling)
        {
            StartGrapple();
        }
        else if(Input.GetMouseButtonUp(0))
        {
            StopGrapple();
        }
        else if(_runtimeData.playerIsGrappling)
        {
            if(Input.GetMouseButton(1))
            {
                if (_lastDistanceFromPoint < Vector3.Distance(_player.transform.position, _GrapplePointTracker.transform.position))
                {
                    SetGrappleDamperInfinity();
                }
            }
            else if(Input.GetMouseButtonUp(1))
            {
                SetGrappleDamperDefault();
            }

            _distanceFromPoint = Vector3.Distance(_player.transform.position, _GrapplePointTracker.transform.position);
            _joint.maxDistance = _distanceFromPoint * _jointMaxDistanceMultiplier;
            _joint.minDistance = _distanceFromPoint * _jointMinDistanceMultiplier;

            _lastDistanceFromPoint = _distanceFromPoint;
        }
    }

    void LateUpdate()
    {
        //DrawRope should be in LateUpdate since we need to make physics calculations first and then draw the rope with updated information
        DrawRope();
    }

    void StartGrapple()
    {
        RaycastHit hit;
        if(Physics.Raycast(_cam.transform.position, _cam.transform.forward, out hit, _grappleDistance, _Grappleable))
        {
            _GrapplePointTracker = Instantiate(_GrapplePointTracker, hit.point, Quaternion.identity);
            _GrapplePointTracker.transform.parent = hit.collider.transform;

            _runtimeData.playerIsGrappling = true;
            _runtimeData.playerGrapplePoint = _GrapplePointTracker.transform.position;

            _joint = _player.AddComponent<SpringJoint>();
            _joint.autoConfigureConnectedAnchor = false;
            _joint.connectedAnchor = _GrapplePointTracker.transform.position;

            _distanceFromPoint = Vector3.Distance(_player.transform.position, _GrapplePointTracker.transform.position);
            _initialDistanceFromPoint = _distanceFromPoint;
            _lastDistanceFromPoint = _distanceFromPoint;
            

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
        _line.SetPosition(1, _GrapplePointTracker.transform.position);

    }

    void StopGrapple()
    {
        _runtimeData.playerIsGrappling = false;
        _line.positionCount = 0;
        Destroy(_joint);
        SetGrappleGunMaterialDefault();
    }

    void SetCanGrapple()
    {
        RaycastHit hit;
        if (Physics.Raycast(_cam.transform.position, _cam.transform.forward, out hit, _grappleDistance, _Grappleable))
        {
            canGrapple = true;
        }
        else
        {
            canGrapple = false;
        }

        _runtimeData.canGrapple = canGrapple;
    }

    void SetGrappleDamperInfinity()
    {
        if(_joint)
        {
            _joint.damper = Mathf.Infinity;
            SetGrappleGunMaterialLocked();
        }
    }

    void SetGrappleDamperDefault()
    {
        if (_joint)
        {
            _joint.damper = _jointDamper;
            SetGrappleGunMaterialDefault();
        }
    }

    void SetGrappleGunMaterialDefault()
    {
        _line.material = _grappleMaterialDefault;
        _gunRenderer.material = _gunMaterialDefault;
    }

    void SetGrappleGunMaterialLocked()
    {
        _line.material = _grappleMaterialLocked;
        _gunRenderer.material = _gunMaterialLocked;
    }
}
