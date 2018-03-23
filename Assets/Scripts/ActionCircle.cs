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
        else if (_open && Input.GetMouseButtonUp(1))
            CloseActionCircle(true);
    }

    public void OpenActionCircle()
    {
        _open = true;
        var cg = GetComponent<CanvasGroup>();
        cg.Open();
    }

    public void CloseActionCircle(bool hideCursor = false)
    {
        _open = false;
        var cg = GetComponent<CanvasGroup>();
        cg.Close(hideCursor);
    }

    public void BeginMove()
    {
        Builder.Instance.BeginSelection(SelectionMode.Move);
        CloseActionCircle(true);
    }

    public void BeginDelete()
    {
        Builder.Instance.BeginSelection(SelectionMode.Delete);
        CloseActionCircle(true);
    }

    public void BeginInspect()
    {
        Builder.Instance.BeginSelection(SelectionMode.Inspect);
        CloseActionCircle(true);
    }
}
