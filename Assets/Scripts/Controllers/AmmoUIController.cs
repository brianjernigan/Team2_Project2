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
        PlayerStatManager.Instance.OnAmmoChanged += UpdateAmmoText;
    }

    private void OnDisable()
    {
        if (PlayerStatManager.Instance is not null)
        {
            PlayerStatManager.Instance.OnAmmoChanged -= UpdateAmmoText;
        }
    }

    private void Start()
    {
        UpdateAmmoText();
    }

    private void UpdateAmmoText()
    {
        _ammoText.text = $"{PlayerStatManager.Instance.CurrentAmmo}/{PlayerStatManager.Instance.CurrentMaxAmmo}";
    }
}
