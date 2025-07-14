using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "FruitDataObject", menuName = "Custom/Data")]
public class FruitObject : ScriptableObject
{
    public string fruitName;
    public Sprite fruitSprite;

    public enum Type
    {
        Fruit,
        Cut,
        Smoothie,
        Tool,
        Special,
        Nothing
    }

    public Type type;
}
