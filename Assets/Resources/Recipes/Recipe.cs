using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "FruitDataObject", menuName = "Custom/Recipe")]
public class Recipe : ScriptableObject
{
    public List<FruitObject> recipeList = new();

    public FruitObject resultFruitObject;
}
