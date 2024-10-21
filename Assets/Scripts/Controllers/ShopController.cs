using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    [SerializeField] private GameObject _shopTextBox;
    [SerializeField] private TMP_Text _shopText;

    private GameObject _player;
    
    public bool IsAtShop { get; set; }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && IsAtShop)
        {
            EnterShop();
        } 
    }

    private void EnterShop()
    {
        var playerController = _player.GetComponent<PlayerController>();
        playerController.enabled = false;

        UpgradeManager.Instance.ActivateUpgradePanel();
    }

    private void ExitShop()
    {
        var playerController = _player.GetComponent<PlayerController>();
        playerController.enabled = true;

        UpgradeManager.Instance.DeactivateUpgradePanel();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IsAtShop = true;
            _shopTextBox.SetActive(true);
            _shopText.text = "Press P to enter the shop!";
            _player = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IsAtShop = false;
            _shopTextBox.SetActive(false);
            _player = null;
        }
    }

    public void OnClickConfirmButton()
    {
        ExitShop();
    }
}
