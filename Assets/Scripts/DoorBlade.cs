using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class DoorBlade : MonoBehaviour, IInteractable
{
    public float Speed = 0.5f;
    public float OpenRotation = 0f;

    private float _startTime = 0;
    private bool _isOpen = true;
    private Quaternion _bladeOpen;
    private Quaternion _bladeClosed;

    void Start()
    {
        if (Math.Abs(OpenRotation) < Mathf.Epsilon)
            _bladeOpen = transform.localRotation;
        else
        {
            _isOpen = false;
            _bladeOpen = Quaternion.Euler(-90f, OpenRotation, 180f);
        }
        _bladeClosed = Quaternion.Euler(-90f, 0f, 180f);
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

    void OpenDoor()
    {
        _isOpen = !_isOpen;
        _startTime = Time.time;
    }

    public string Description
    {
        get { return _isOpen ? "Close door" : "Open door"; }
    }

    public void Interact()
    {
        OpenDoor();
    }
}
