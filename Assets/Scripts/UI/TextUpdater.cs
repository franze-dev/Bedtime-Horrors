using TMPro;
using UnityEngine;

public class TextUpdater : MonoBehaviour
{
    private TextMeshProUGUI textMesh;

    private void Awake()
    {
        textMesh = GetComponentInChildren<TMPro.TextMeshProUGUI>();
    }

    public void ChangeText(string newText)
    {
        if (textMesh != null)
            textMesh.text = newText;
    }

    public void ChangeColor(Color newColor)
    {
        if (textMesh != null)
            textMesh.color = newColor;
    }
}
