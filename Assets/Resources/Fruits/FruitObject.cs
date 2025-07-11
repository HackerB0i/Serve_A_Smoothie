using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FruitDataObject", menuName = "Custom/Data")]
public class FruitObject : ScriptableObject
{
    public string fruitName;
    public Sprite fruitSprite;
}
