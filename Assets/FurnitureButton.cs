using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public class FurnitureButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject FurnitureDetails;
    public Image FurnitureImage;
    private Furniture _furniture;

    public void OnPointerEnter(PointerEventData eData)
    {
        FurnitureDetails.SetActive(true);
        FurnitureImage.enabled = false;
    }

    public void OnPointerExit(PointerEventData eData)
    {
        FurnitureDetails.SetActive(false);
        FurnitureImage.enabled = true;
    }

    public void DisplayProduct(Furniture product)
    {
        _furniture = product;
        if (product.SampleImage != null)
            FurnitureImage.sprite = product.SampleImage;
        GetComponentInChildren<Text>().text = product.Name;
        var details = FurnitureDetails.GetComponentsInChildren<Text>();
        details[0].text = product.Manufacturer;
        details[1].text = product.Price.ToString() + "$";
        details[2].text = product.Description;
    }

    public void OnClick()
    {
        Builder.Instance.BeginBuilding(_furniture);
    }
}
