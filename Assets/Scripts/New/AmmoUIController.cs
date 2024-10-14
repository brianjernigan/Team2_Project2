using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AmmoUIController : MonoBehaviour
{
    [SerializeField] private TMP_Text _ammoText;

    private void OnEnable()
    {
        AmmoManager.Instance.OnAmmoChanged += UpdateAmmoText;
    }

    private void OnDisable()
    {
        if (AmmoManager.Instance is not null)
        {
            AmmoManager.Instance.OnAmmoChanged -= UpdateAmmoText;
        }
    }

    private void Start()
    {
        UpdateAmmoText();
    }

    private void UpdateAmmoText()
    {
        _ammoText.text = $"Ammo: {PlayerStats.Instance.CurrentAmmo} / {PlayerStats.Instance.CurrentMaxAmmo}";
    }
}
