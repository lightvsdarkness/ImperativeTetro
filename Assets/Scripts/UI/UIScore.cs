using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public interface IIndicator {
    void IndicateValue(string text);
    void Construct(string text);
}

public class UIScore : MonoBehaviour, IIndicator {
    public TextMeshProUGUI UIText;

    public string DefaultValue;

    private void Start() {
        Construct(DefaultValue);
    }

    public void IndicateValue(string text) {
        if (UIText == null)
            UIText = GetComponent<TextMeshProUGUI>();
        UIText.text = text;
    }

    public void Construct(string defaultText) {
        if (UIText == null)
            UIText = GetComponent<TextMeshProUGUI>();

        if (UIText == null) {
            Debug.LogWarning("Pre-add UI component", this);
            UIText = gameObject.AddComponent<TextMeshProUGUI>();
        }
        UIText.text = defaultText;
    }
}