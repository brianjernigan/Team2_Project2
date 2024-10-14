using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AmmoUIController : MonoBehaviour
{
    [SerializeField] private TMP_Text _ammoText;

    private PlayerShootingController _playerShootingController;

    private void Awake()
    {
        _playerShootingController = PlayerStats.Instance.GetComponent<PlayerShootingController>();
        _playerShootingController.OnAmmoChanged += UpdateAmmoText;
    }

    private void Start()
    {
        UpdateAmmoText();
    }

    private void OnDisable()
    {
        if (_playerShootingController is not null)
        {
            _playerShootingController.OnAmmoChanged -= UpdateAmmoText;
        }
    }

    private void UpdateAmmoText()
    {
        _ammoText.text = $"Ammo: {PlayerStats.Instance.CurrentAmmo} / {PlayerStats.Instance.CurrentMaxAmmo}";
    }
}
