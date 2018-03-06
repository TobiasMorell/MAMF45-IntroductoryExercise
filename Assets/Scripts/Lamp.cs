using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class Lamp : MonoBehaviour, IInteractable
{
    public Light Light;

    public string Description
    {
        get { return "Toggle light"; }
    }

    public void Interact()
    {
        Light.enabled = !Light.enabled;
    }
}
