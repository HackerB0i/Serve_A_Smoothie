using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameloop : MonoBehaviour
{
    [SerializeField] private int difficultyFactor;
    [SerializeField] private List<FruitObject> lvl1Smoothies = new();

    private void SpawnCustomer()
    {
        var customer = Instantiate(Resources.Load<Transform>("Prefabs/Customer"), transform);
        List<FruitObject> order = new();
        for (int i = 0; i < difficultyFactor + 1; i++)
        {
            order.Add(lvl1Smoothies[Random.Range(0, lvl1Smoothies.Count)]);
        }
        customer.GetComponent<Customer>().SetOrder(difficultyFactor, order);
    }

    private void Update()
    {
        if (transform.childCount < 1)
        {
            SpawnCustomer();
        }
    }
}
