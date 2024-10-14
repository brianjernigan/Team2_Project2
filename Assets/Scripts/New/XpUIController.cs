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
        ExperienceManager.Instance.OnXpChanged += UpdateXpText;
    }

    private void OnDisable()
    {
        if (ExperienceManager.Instance is not null)
        {
            ExperienceManager.Instance.OnXpChanged -= UpdateXpText;
        }
    }

    private void Start()
    {
        UpdateXpText();
    }

    private void UpdateXpText()
    {
        _xpText.text = $"XP: {ExperienceManager.Instance.CurrentXp} / {ExperienceManager.Instance.CurrentXpThreshold}";
    }
}
