using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
    [SerializeField] private FruitObject _currentFruitObject;

    private bool _doneGrowing = false;

    private PlayerHoldItem playerHoldItem;
    
    private void Awake()
    {
        StartGrowing();
    }

    public void StartGrowing()
    {
        StartCoroutine(Grow());
    }
    
    private IEnumerator Grow()
    {
        _doneGrowing = false;
        
        yield return new WaitForSeconds(10);
        _doneGrowing = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Player" && _doneGrowing)
        {
            playerHoldItem = other.GetComponent<PlayerHoldItem>();
            if (other.GetComponent<PlayerHoldItem>().holdingFruitObject == Resources.Load("Fruits/Objects/Air"))
            {
                playerHoldItem.holdingFruitObject = _currentFruitObject;
                StartGrowing();
            }
        }
    }
}
