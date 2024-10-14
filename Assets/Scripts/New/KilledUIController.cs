using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KilledUIController : MonoBehaviour
{
    [SerializeField] private TMP_Text _killedText;

    private void OnEnable()
    {
        StatManager.Instance.OnEnemyKilled += UpdateKilledText;
    }

    private void OnDisable()
    {
        if (StatManager.Instance is not null)
        {
            StatManager.Instance.OnEnemyKilled -= UpdateKilledText;
        }
    }

    private void Start()
    {
        UpdateKilledText();
    }

    private void UpdateKilledText()
    {
        _killedText.text = $"Enemies Killed: {StatManager.Instance.NumEnemiesKilled}";
    }
}
