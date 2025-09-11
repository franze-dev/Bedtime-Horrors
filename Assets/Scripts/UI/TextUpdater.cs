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
        //Debug.Log("TextUpdater script ChangeText called");


        textMesh.text = newText;
    }
}
