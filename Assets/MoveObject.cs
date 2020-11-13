using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour
{

    [SerializeField] float _moveXSpeed;
    [SerializeField] float _moveYSpeed;
    [SerializeField] float _moveZSpeed;
    Vector3 _initialPosition;
    
    void Start()
    {
        _initialPosition = gameObject.transform.position;
    }

    void Update()
    {
        if(_moveXSpeed != 0 || _moveYSpeed != 0 || _moveZSpeed != 0)
            gameObject.transform.position = new Vector3(gameObject.transform.position.x + _moveXSpeed * Time.deltaTime, 
                gameObject.transform.position.y + _moveYSpeed * Time.deltaTime,
                gameObject.transform.position.z + _moveZSpeed * Time.deltaTime);

    }
}
