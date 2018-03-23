using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionCircle : MonoBehaviour
{
    private bool _open;

    void Update()
    {
        if (!_open && Input.GetMouseButtonUp(1))
            OpenActionCircle();
        else if (_open && Input.GetKeyUp(KeyCode.Escape))
            CloseActionCircle(true);
    }

    void OpenActionCircle()
    {
        _open = true;
        var cg = GetComponent<CanvasGroup>();
        cg.Open();
    }

    void CloseActionCircle(bool hideCursor = false)
    {
        _open = false;
        var cg = GetComponent<CanvasGroup>();
        cg.Close(hideCursor);
    }
}
