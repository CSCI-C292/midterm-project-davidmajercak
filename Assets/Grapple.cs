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
    bool _reachedMinDistance;
    GameObject _GrapplePointTracker;
    [SerializeField] float _AbsMinDistanceMultiplier; //Aboslute Min Distance multiplier on distance from grapple point. 
                                                      //At this point the SpringJoint damper will be set to Infinity (grapple will no longer change size)

    LineTextureMode _textureMode = LineTextureMode.Stretch;
    bool canGrapple;

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
            if(_reachedMinDistance)
                return;
            //Maybe set player damper to a lower value while grappling to allow momentum to be transferred more easily?
            if((_lastDistanceFromPoint < Vector3.Distance(_player.transform.position, _GrapplePointTracker.transform.position)) && 
                (_initialDistanceFromPoint * _AbsMinDistanceMultiplier >= Vector3.Distance(_player.transform.position, _GrapplePointTracker.transform.position)))
            {
                _joint.damper = Mathf.Infinity;
                _reachedMinDistance = true;
                StartCoroutine(DamperInfinityTimer());
            }
            //If we're closer to the grapple point than we were when we started the grapple
            else if(_distanceFromPoint > Vector3.Distance(_player.transform.position, _GrapplePointTracker.transform.position))
            {
                //Update the maxDistance so that our SpringJoint doesn't let us get as far away again, allowing tighter grapples around objects
                _distanceFromPoint = Vector3.Distance(_player.transform.position, _GrapplePointTracker.transform.position);
                _joint.maxDistance = _distanceFromPoint * _jointMaxDistanceMultiplier;
                _joint.minDistance = _distanceFromPoint * _jointMinDistanceMultiplier;
            }
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
        _reachedMinDistance = false;
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
        _reachedMinDistance = false;
        _line.positionCount = 0;
        Destroy(_joint);
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

    IEnumerator DamperInfinityTimer()
    {
        yield return new WaitForSeconds(.3f * Time.timeScale);

        if(_joint)
        {
            _joint.damper = _jointDamper;
            _reachedMinDistance = false;
        }
    }
}
