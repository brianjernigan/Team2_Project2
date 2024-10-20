using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RingDoorbellUIController : MonoBehaviour
{
    private readonly Color _startColor = Color.white;
    private readonly Color _endColor = Color.green;

    [SerializeField] private TMP_Text _interactionText;

    private const float OriginalFontSize = 36f;
    private const float ScaledFontSize = 52f;

    public void ShowDoorbellText()
    {
        _interactionText.gameObject.SetActive(true);
        _interactionText.color = _startColor;
        _interactionText.fontSize = OriginalFontSize;
    }

    public void HideDoorbellText()
    {
        _interactionText.gameObject.SetActive(false);
    }

    public void UpdateDoorbellText(float lerpValue)
    {
        _interactionText.color = Color.Lerp(_startColor, _endColor, lerpValue);
        _interactionText.fontSize = Mathf.Lerp(OriginalFontSize, ScaledFontSize, lerpValue);
    }

    public void ResetDoorbellText()
    {
        _interactionText.color = _startColor;
        _interactionText.fontSize = OriginalFontSize;
    }
}
