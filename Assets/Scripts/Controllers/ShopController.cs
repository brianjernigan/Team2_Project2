using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    private GameObject _shopTextBox;
    private TMP_Text _shopText;
    
    [SerializeField] private ParticleSystem _particles;
    
    public bool IsAtShop { get; set; }

    private void Awake()
    {
        _shopTextBox = transform.parent.Find("ShopCanvas/ShopPanel/ShopTextBox").gameObject;
        _shopText = transform.parent.Find("ShopCanvas/ShopPanel/ShopTextBox/ShopText").GetComponent<TMP_Text>();
    }
    
    private void Update()
    {
        HandleParticles();
        
        if (Input.GetKeyDown(KeyCode.P) && IsAtShop)
        {
            UpgradeManager.Instance.EnterShop();
        } 
    }

    private void HandleParticles()
    {
        if (XpManager.Instance.CurrentXp >= 10)
        {
            if (!_particles.isPlaying)
            {
                _particles.Play();
            }
        }
        else
        {
            if (_particles.isPlaying)
            {
                _particles.Stop();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IsAtShop = true;
            _shopTextBox.SetActive(true);
            _shopText.text = "Press P to enter the shop!";
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
