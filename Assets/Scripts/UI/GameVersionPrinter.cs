using TMPro;
using UnityEngine;

public class GameVersionPrinter : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI versionTextMesh;
    [SerializeField] string versionPrefix = "v";

    private void Start()
    {
        if (versionTextMesh == null)
            versionTextMesh = GetComponent<TextMeshProUGUI>();

        if (versionTextMesh == null)
            Debug.LogError("No textmeshprogui found on " + gameObject.name);

        versionTextMesh.text = versionPrefix + Application.version;
    }

}
