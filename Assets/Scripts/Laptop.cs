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

    public string Description { get; private set; }
    public void Interact()
    {
        _startTime = Time.time;
        StartCoroutine(ChangeLidRotation(_open ? _screenOpen : _screenClosed, _open ? _screenClosed : _screenOpen));
    }

    private IEnumerator ChangeLidRotation(Quaternion start, Quaternion end)
    {
        while (_startTime > 0)
        {
            var step = (Time.time - _startTime) * Speed;
            Screen.localRotation = Quaternion.Lerp(start, end, step);
            yield return null;
        }

        _open = !_open;
        var col = GetComponent<BoxCollider>();
        col.center = new Vector3(0, _open ? 0.1f : 0.01f);
        col.size = new Vector3(0.3f, _open ? 0.2f : 0.02f, 0.2f);
    }
}
