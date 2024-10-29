using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class XpUIController : MonoBehaviour
{
    [SerializeField] private TMP_Text _xpText;

    private void OnEnable()
    {
        XpManager.Instance.OnXpChanged += UpdateXpText;
    }

    private void OnDisable()
    {
        if (XpManager.Instance is null) return;
        XpManager.Instance.OnXpChanged -= UpdateXpText;
    }

    private void Start()
    {
        UpdateXpText();
    }

    private void UpdateXpText()
    {
        _xpText.text = $"{XpManager.Instance.CurrentXp}";
    }
}
