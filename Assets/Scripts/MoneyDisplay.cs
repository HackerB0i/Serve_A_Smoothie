using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyDisplay : MonoBehaviour
{
    public static MoneyDisplay Instance {get; private set;}
    
    [SerializeField] public int Money;
    [SerializeField] private TextMeshProUGUI moneyText;

    private void Awake()
    {
        Instance = this;
    }
    
    void Update()
    {
        moneyText.text = $"{Money}¢";
    }
}
