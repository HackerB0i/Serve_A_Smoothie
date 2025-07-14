using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlantSlot : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    
    public FruitObject _currentFruitObject;
    
    private Animator _animator;

    public bool _doneGrowing = false;

    private PlayerHoldItem playerHoldItem;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        StartGrowing(GardenManager.Instance.fruits[Random.Range(0,GardenManager.Instance.fruits.Count)]);
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
        yield return new WaitForSeconds(16f);
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
                StartGrowing(GardenManager.Instance.fruits[Random.Range(0,GardenManager.Instance.fruits.Count)]);
            }
        }
    }
}
