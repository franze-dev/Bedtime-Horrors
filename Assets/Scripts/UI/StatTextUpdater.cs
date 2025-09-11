using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class StatTextUpdater : MonoBehaviour
{
    [SerializeField] private string _baseText;
    [SerializeField] private TextMeshProUGUI _textMesh;
    [SerializeField] bool _startAtZero = true;

    private void Awake()
    {
        if (_textMesh == null)
            _textMesh = GetComponent<TextMeshProUGUI>();

        if (_textMesh == null)
            Debug.LogError("TextMeshProUGUI component not found in " + gameObject.name);

        if (_startAtZero)
            _textMesh.text = _baseText + "0";
    }

    public void UpdateText(int value)
    {
        _textMesh.text = _baseText + value.ToString();
    }

    public void UpdateText(string replaceText)
    {
        _textMesh.text = replaceText;
    }

    public IEnumerator UpdateTextAfterDelay(string newText, float secondsToWait)
    {
        yield return new WaitForSeconds(secondsToWait);
        UpdateText(newText);
    }
}
