using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Furniture", menuName = "Interior/Furniture", order = 1)]
public class Furniture : ScriptableObject
{
    public string Name;
    public string Manufacturer;
    public string Description;
    public bool SnapsToWall;
    public bool SnapsToFloor;
    public GameObject Model;
    public int Price;
    public Sprite SampleImage;
}
