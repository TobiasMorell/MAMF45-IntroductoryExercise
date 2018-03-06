using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBlade : MonoBehaviour
{
    public float Speed = 0.5f;

    private float _startTime = 0;
    private bool _isOpen = true;
    private Quaternion _bladeOpen;
    private Quaternion _bladeClosed;

    void Start()
    {
        _bladeOpen = transform.localRotation;
        _bladeClosed = Quaternion.Euler(-90f, 0f, 180f);
    }

    void OnMouseDown()
    {
        _isOpen = !_isOpen;
        transform.localRotation = _isOpen ? _bladeOpen : _bladeClosed;
        _startTime = Time.time;
    }

    void Update()
    {
        if (_startTime > 0)
        {
            var step = (Time.time - _startTime) * Speed;
            transform.localRotation = _isOpen
                ? Quaternion.Lerp(_bladeClosed, _bladeOpen, step)
                : Quaternion.Lerp(_bladeOpen, _bladeClosed, step);
        }
    }
}
