using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//https://www.youtube.com/watch?v=Xgh4v1w5DxU
//Referenced this tutorial but improved the code quite a bit 
//https://www.raywenderlich.com/348-make-a-2d-grappling-hook-game-in-unity-part-1
//Look at this code and maybe see about grappling around objects?
//https://www.raywenderlich.com/312-make-a-2d-grappling-hook-game-in-unity-part-2

//Changed gravity to -13 from -9.81
//Adjusted player mass from 1 to 1.4



//A Rigidbody character controller used for the Player 
//I decided it would be easier to implement this way since the player will primarily move with a grappling hook
public class Player : MonoBehaviour
{
    [SerializeField] float _mouseSensitivity = default;
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

        _rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        

        //Checks if player is grounded using Ground LayerMask
        //QueryTriggerInteraction.Ignore causes this to ignore any triggers instead of counting as a collision
        _isGrounded = Physics.CheckSphere(gameObject.transform.position, _groundDistance, _groundLayerMask, QueryTriggerInteraction.Ignore);

        AimCamera();
        CalculateMovementVector();

        //Jump
        if(_isGrounded && Input.GetButtonDown("Jump"))
        {
            Vector3 _jumpVector = Vector3.up * _jumpHeight;
            _rb.AddForce(_jumpVector, ForceMode.VelocityChange);
        }

    }

    void FixedUpdate()
    {
        //Important to keep this in FixedUpdate
        Movement();
    }

    void LateUpdate() {
        //Resets x and y rotation, need to leave this in until I figure out the issue (probably with SpringJoint)
        //Moving this to LateUpdate has definitely reduced the weird camera movement when colliding with an object
        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
    }

    void AimCamera()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        transform.Rotate(Vector3.up, mouseX * _mouseSensitivity);

        _currentTilt -= mouseY * _mouseSensitivity;
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
        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
    }
}
