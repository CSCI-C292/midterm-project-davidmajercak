﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour
{

    [SerializeField] float _moveXSpeed;
    [SerializeField] float _rotateXSpeed;
    [SerializeField] float _moveYSpeed;
    [SerializeField] float _rotateYSpeed;
    [SerializeField] float _moveZSpeed;
    [SerializeField] float _rotateZSpeed;
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

        if(_rotateXSpeed != 0 || _rotateYSpeed != 0 || _rotateZSpeed != 0)
            gameObject.transform.Rotate(_rotateXSpeed * Time.deltaTime, _rotateYSpeed * Time.deltaTime, _rotateZSpeed * Time.deltaTime);
    }
}