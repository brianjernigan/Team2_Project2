using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    [SerializeField] private GameObject _shopTextBox;
    [SerializeField] private TMP_Text _shopText;
    
    public bool IsAtShop { get; set; }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IsAtShop = true;
            _shopTextBox.SetActive(true);
            _shopText.text = "Hello";
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IsAtShop = false;
            _shopTextBox.SetActive(false);
        }
    }
}
