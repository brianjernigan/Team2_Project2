using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeHudController : MonoBehaviour
{
    [System.Serializable]
    public class StatUI
    {
        public GameObject statImage;
        public TMP_Text statText;
    }

    [SerializeField] private StatUI[] _statUIs;

    public void UpdateStatHud(int statIndex, int statLevel)
    {
        if (statIndex < 0 || statIndex >= _statUIs.Length) return;

        _statUIs[statIndex].statImage.SetActive(true);
        _statUIs[statIndex].statText.text = $"+ {statLevel}";
    }
}
