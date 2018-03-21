using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public delegate void ColliderOverlapEvent(object source, ColliderOverlapEventArgs eventArgs);

public class ColliderOverlapEventArgs : EventArgs
{
    public readonly bool IsOverlappingColliders;

    public ColliderOverlapEventArgs(bool isOverlappingColliders)
    {
        IsOverlappingColliders = isOverlappingColliders;
    }
}

public class BuildingColliderTrigger : MonoBehaviour
{
    public event ColliderOverlapEvent OnColliderOverlapChanged;
    private readonly HashSet<Collider> _overlappingColliders = new HashSet<Collider>();

    void OnTriggerEnter(Collider other)
    {
        _overlappingColliders.Add(other);
        if(_overlappingColliders.Count == 1)
            OnColliderOverlapChanged(this, new ColliderOverlapEventArgs(true));
    }

    void OnTriggerExit(Collider other)
    {
        _overlappingColliders.Remove(other);
        if (_overlappingColliders.Count != 0) return;
        if (OnColliderOverlapChanged != null)
            OnColliderOverlapChanged.Invoke(this, new ColliderOverlapEventArgs(false));
    }
}
