using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractionTextUIController : MonoBehaviour
{
    private readonly Color _startColor = Color.white;
    private readonly Color _endColor = Color.green;

    [SerializeField] private TMP_Text _interactionText;
    [SerializeField] private GameObject _interactionPanel;

    private const float OriginalFontSize = 36f;
    private const float ScaledFontSize = 60f;

    public void ShowInteractionPanel()
    {
        _interactionPanel.SetActive(true);
        _interactionText.gameObject.SetActive(true);
        _interactionText.color = _startColor;
        _interactionText.fontSize = OriginalFontSize;
    }

    public void HideInteractionPanel()
    {
        _interactionPanel.SetActive(false);
        _interactionText.gameObject.SetActive(false);
    }

    public void UpdateInteractionText(float lerpValue)
    {
        _interactionText.color = Color.Lerp(_startColor, _endColor, lerpValue);
        _interactionText.fontSize = Mathf.Lerp(OriginalFontSize, ScaledFontSize, lerpValue);
    }

    public void ResetInteractionText()
    {
        _interactionText.color = _startColor;
        _interactionText.fontSize = OriginalFontSize;
    }
}
