﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//https://www.youtube.com/watch?v=Xgh4v1w5DxU
//Referenced this tutorial for initial grapple implementation but improved the code quite a bit 

//Didn't implement this code but might look at it when working on grappling mechanics in the future
//https://www.raywenderlich.com/348-make-a-2d-grappling-hook-game-in-unity-part-1
//Look at this code and maybe see about grappling around objects?
//https://www.raywenderlich.com/312-make-a-2d-grappling-hook-game-in-unity-part-2


//Physics ideas:
//Reduce player drag to 0 (or closer to 0) when grapple drag set to inifinity so they orbit around grapple point with more speed
//Reduce x and y drag only when in air?
//Add a trick system. 360's and other stuff.  
//  Keep track of Mathf.abs(currentRotation - lastRotation) since touching the ground?
//  Maybe press shift to boost forward based on tricks?
//Add a sliding mechanic when pressing shift(maybe different button) to slide on ground (could make for interesting secions in levels)



//A Rigidbody character controller used for the Player 
//I decided it would be easier to implement this way since the player will primarily move with a grappling hook
public class Player : MonoBehaviour
{
    [SerializeField] public float _lookSensitivity = default;
    [SerializeField] GameObject _cam = default;
    [SerializeField] LayerMask _groundLayerMask = default;
    [SerializeField] float _movementSpeed = default;
    [SerializeField] float _jumpHeight = default;
    float _currentTilt = 0f;
    Rigidbody _rb;
    Vector3 _movementVector;
    bool _isGrounded;
    [SerializeField] float _groundDistance = default;
    [SerializeField] RuntimeData _runtimeData;
    [SerializeField] float _grappleMovementMultiplier;    

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        //Fixes issue where grapple gun keeps pointing at position from last scene if LMB held down
        _runtimeData.playerIsGrappling = false;

        _rb = GetComponent<Rigidbody>();
        _rb.maxAngularVelocity = 0;
        _runtimeData.playerVelocity = 0;

        //This fixes the bugs that were driving me crazy!
        //Needed to set center of mass of the rigid body otherwise grappling slightly rotates the player
        _rb.centerOfMass = Vector3.zero;
    }
    void Update()
    {
        //Checks if player is grounded using Ground LayerMask
        //QueryTriggerInteraction.Ignore causes this to ignore any triggers instead of counting as a collision
        _isGrounded = Physics.CheckSphere(gameObject.transform.position, _groundDistance, _groundLayerMask, QueryTriggerInteraction.Ignore);

        AimCamera();

        if(_runtimeData.currentGameplayState == GameplayState.FreeMove)
        {
            CalculateMovementVector();

            //Jump
            if (_isGrounded && Input.GetButtonDown("Jump"))
            {
                Vector3 _jumpVector = Vector3.up * _jumpHeight;
                _rb.AddForce(_jumpVector, ForceMode.VelocityChange);
            }

            _runtimeData.playerVelocity = _rb.velocity.magnitude;

            _rb.inertiaTensorRotation = Quaternion.identity;
        }

        

        //Set player position in _runtimeData
        _runtimeData.playerPosition = gameObject.transform.position;
    }

    void FixedUpdate()
    {
        //Important to keep this in FixedUpdate
        Movement();
    }

    void AimCamera()
    {
        //Pretty basic 3D movement from the 3D tutorial lecture
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        transform.Rotate(Vector3.up, mouseX * _lookSensitivity);

        _currentTilt -= mouseY * _lookSensitivity;
        _currentTilt = Mathf.Clamp(_currentTilt, -90, 90);

        _cam.transform.localEulerAngles = new Vector3(_currentTilt, 0, 0);
    }

    void CalculateMovementVector()
    {
        Vector3 horizontalMovement = transform.right * Input.GetAxisRaw("Horizontal");
        Vector3 verticalMovement = transform.forward * Input.GetAxisRaw("Vertical");
        _movementVector = (horizontalMovement + verticalMovement).normalized * Time.fixedDeltaTime * _movementSpeed;
    }

    void Movement()
    {
        //If player isn't grappling the player can move
        if(!_runtimeData.playerIsGrappling)
        {
            _rb.MovePosition(_rb.position + _movementVector);
        }
        else
        {
            _rb.MovePosition(_rb.position + _movementVector * _grappleMovementMultiplier);
        }
    }

    void OnCollisionEnter(Collision other) {
        //This resets the x and z axes after a collision. The player was getting slightly off tilt even though x and z axis are frozen
        //I think this is due to something in the SpringJoint physics
        //Not sure if this is still needed but don't want to break anything before midterm submission
        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
    }
}
