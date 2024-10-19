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
        ExperienceManagerSingleton.Instance.OnXpChanged += UpdateXpText;
    }

    private void OnDisable()
    {
        if (ExperienceManagerSingleton.Instance is not null)
        {
            ExperienceManagerSingleton.Instance.OnXpChanged -= UpdateXpText;
        }
    }

    private void Start()
    {
        UpdateXpText();
    }

    private void UpdateXpText()
    {
        _xpText.text = $"XP: {ExperienceManagerSingleton.Instance.CurrentXp} / {ExperienceManagerSingleton.Instance.CurrentXpThreshold}";
    }
}