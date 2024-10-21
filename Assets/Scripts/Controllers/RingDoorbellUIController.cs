using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RingDoorbellUIController : MonoBehaviour
{
    private readonly Color _startColor = Color.white;
    private readonly Color _endColor = Color.green;

    [SerializeField] private TMP_Text _ringText;

    private const float OriginalFontSize = 36f;
    private const float ScaledFontSize = 52f;

    public void ShowDoorbellText()
    {
        _ringText.gameObject.SetActive(true);
        _ringText.color = _startColor;
        _ringText.fontSize = OriginalFontSize;
    }

    public void HideDoorbellText()
    {
        _ringText.gameObject.SetActive(false);
    }

    public void UpdateDoorbellText(float lerpValue)
    {
        _ringText.color = Color.Lerp(_startColor, _endColor, lerpValue);
        _ringText.fontSize = Mathf.Lerp(OriginalFontSize, ScaledFontSize, lerpValue);
    }

    public void ResetDoorbellText()
    {
        _ringText.color = _startColor;
        _ringText.fontSize = OriginalFontSize;
    }
}
