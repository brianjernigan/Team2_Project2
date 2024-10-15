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
        AmmoManagerSingleton.Instance.OnAmmoChanged += UpdateAmmoText;
    }

    private void OnDisable()
    {
        if (AmmoManagerSingleton.Instance is not null)
        {
            AmmoManagerSingleton.Instance.OnAmmoChanged -= UpdateAmmoText;
        }
    }

    private void Start()
    {
        UpdateAmmoText();
    }

    private void UpdateAmmoText()
    {
        _ammoText.text = $"Ammo: {PlayerStatManagerSingleton.Instance.CurrentAmmo} / {PlayerStatManagerSingleton.Instance.CurrentMaxAmmo}";
    }
}
