using UnityEngine;

[CreateAssetMenu(fileName = "DiaryPage", menuName = "ScriptableObjects/DiaryPage")]
public class DiaryPage : ScriptableObject
{
    public string title;
    [TextArea] public string story;
    [TextArea] public string tutorial;
    public GameObject photoPrefab;
}