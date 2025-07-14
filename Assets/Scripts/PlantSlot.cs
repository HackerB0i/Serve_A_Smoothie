using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlantSlot : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    
    public FruitObject _currentFruitObject;
    
    [SerializeField] private Animator _animator;

    public bool _doneGrowing = false;

    private PlayerHoldItem playerHoldItem;
    
    private void Awake()
    {
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
        yield return new WaitForSeconds(17f);
        _doneGrowing = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Player" && _doneGrowing)
        {
            StartCoroutine(ResetPlot(other));
        }
    }

    private IEnumerator ResetPlot(Collider2D collider)
    {
        playerHoldItem = collider.GetComponent<PlayerHoldItem>();
        if (collider.GetComponent<PlayerHoldItem>().holdingFruitObject == Resources.Load("Fruits/Objects/Air"))
        {
            _animator.SetBool("grow", false);
            playerHoldItem.holdingFruitObject = _currentFruitObject;
            yield return new WaitForSeconds(1);
            StartGrowing(GardenManager.Instance.fruits[Random.Range(0,GardenManager.Instance.fruits.Count)]);
        }

        yield return null;
    }
}
