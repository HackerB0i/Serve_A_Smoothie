using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryFrame : MonoBehaviour
{
    [SerializeField] private FruitObject currentFruitObject;
    [SerializeField] private Image holdingImage;
    [SerializeField] private Image selectedEffect;
    public bool isSelected = false;

    void Update()
    {
        holdingImage.sprite = currentFruitObject.fruitSprite;
        if (isSelected)
        {
            selectedEffect.enabled = true;
        }
        else
        {
            selectedEffect.enabled = false; 
        }
    }
}
