using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryFrame : MonoBehaviour
{
    public FruitObject currentFruitObject;
    [SerializeField] private Image holdingImage;
    [SerializeField] private Image selectedEffect;
    public bool isSelected = false;
    public bool isPreview = false;
    
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

        if (isPreview)
        {
            SetItemOpacity(0.7f);
        }
        else
        {
            SetItemOpacity(1);
        }
    }

    public void SetItemOpacity(float opacity)
    {
        holdingImage.color = new Color(1, 1, 1, opacity);
    }
}
