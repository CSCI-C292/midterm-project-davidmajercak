using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] float _rotationSpeed;
    [SerializeField] float _frequency;
    [SerializeField] float _magnitude;
    Vector3 _originalPosition;

    void Start()
    {
        _originalPosition = transform.position;
    }
    void Update()
    {
        transform.RotateAround(transform.position, new Vector3(0, 35, 45), _rotationSpeed * Time.deltaTime);
        transform.position = _originalPosition + Vector3.up * Mathf.Sin(Time.time * _frequency) * _magnitude;
    }
}
