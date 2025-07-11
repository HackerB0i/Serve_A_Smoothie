using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantSlot : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    
    private FruitObject _currentFruitObject;
    
    private Animator _animator;

    private bool _doneGrowing = false;

    private PlayerHoldItem playerHoldItem;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        StartGrowing(Resources.Load<FruitObject>("Fruits/Objects/Strawberry"));
    }

    public void StartGrowing(FruitObject fruitObject)
    {
        _currentFruitObject = fruitObject;
        _animator.SetBool("grow", true);
        StartCoroutine(Grow());
    }

    private void Update()
    {
        spriteRenderer.sprite = _currentFruitObject.fruitSprite;
    }

    private IEnumerator Grow()
    {
        _doneGrowing = false;
        yield return new WaitForSeconds(15.9f);
        _doneGrowing = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Player" && _doneGrowing)
        {
            playerHoldItem = other.GetComponent<PlayerHoldItem>();
            if (other.GetComponent<PlayerHoldItem>().holdingFruitObject == Resources.Load("Fruits/Objects/Air"))
            {
                _animator.SetBool("grow", false);
                playerHoldItem.holdingFruitObject = _currentFruitObject;
                _currentFruitObject = Resources.Load<FruitObject>("Fruits/Objects/Air");
            }
        }
    }
}
