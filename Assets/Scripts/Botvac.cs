﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Botvac : MonoBehaviour
{
    public Transform LeftSensor;
    public Transform RightSensor;
    public Transform DownSensor;

    private float _rayLength = 0.1f;
    private Rigidbody _rigidbody;

	// Use this for initialization
	void Start ()
	{
	    _rigidbody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
    void FixedUpdate()
    {
        if (!Physics.Raycast(DownSensor.position, -transform.up, _rayLength))
            return;

        var close = false;
        if (Physics.Raycast(LeftSensor.position, transform.forward, _rayLength))
        {
            close = true;
        }
        else if (Physics.Raycast(RightSensor.position, transform.forward, _rayLength))
        {
            close = true;
        }

        if (close)
        {
            _rigidbody.MoveRotation(
                Quaternion.Euler(transform.localRotation.eulerAngles + transform.up * Time.fixedDeltaTime * 90f));
        }
        else
        {
            _rigidbody.MovePosition(transform.position + transform.forward * Time.fixedDeltaTime);
        }
    }
}
