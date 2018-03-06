using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class Laptop : MonoBehaviour, IInteractable
{
    public float Speed = 0.5f;
    public Transform Screen;

    private float _startTime = 0;
    private bool _open = true;
    private Quaternion _screenOpen;
    private Quaternion _screenClosed;

    void Start()
    {
        _screenOpen = Screen.localRotation;
        _screenClosed = Quaternion.Euler(90f, 0f, 0f);
    }
	
	// Update is called once per frame
	void Update () {
	    if (_startTime > 0)
	    {
	        var step = (Time.time - _startTime) * Speed;
	        Screen.localRotation = _open
	            ? Quaternion.Lerp(_screenClosed, _screenOpen, step)
	            : Quaternion.Lerp(_screenOpen, _screenClosed, step);
	    }
    }

    public string Description { get; private set; }
    public void Interact()
    {
        _open = !_open;
        _startTime = Time.time;
    }
}
