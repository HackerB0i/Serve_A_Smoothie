using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHoldItem : MonoBehaviour
{
    public FruitObject holdingFruitObject;
    [SerializeField] private SpriteRenderer holdingFruitSprite;

    private void Update()
    {
        holdingFruitSprite.sprite = holdingFruitObject.fruitSprite;
    }

    public void SetFruitObject(FruitObject fruitObject)
    {
        holdingFruitObject = fruitObject;
    }
}
