using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InteriorCatalog : MonoBehaviour {
    public Furniture[] Furniture { get; private set; }
    public int HorizontalOffset = 230;
    public int VerticalOffset = -205;
    public GameObject FurnitureUiPrefab;
    public GameObject FurnitureUi;

	// Use this for initialization
	void Start ()
	{
	    Furniture = Resources.LoadAll("Interior", typeof(Furniture)).Select(o => (Furniture) o).ToArray();
	    Debug.Log("Loaded " + Furniture.Length + " models");

	    for (var i = 0; i < Furniture.Length; i++)
	    {
	        var row = i / 2;
	        var col = i % 2;

	        var fInst = Instantiate(FurnitureUiPrefab);
	        var fRect = fInst.GetComponent<RectTransform>();
	        fRect.SetParent(FurnitureUi.transform, false);
            fRect.anchoredPosition = fRect.anchoredPosition + new Vector2(col * HorizontalOffset, row * VerticalOffset);
	    }
	}
}
