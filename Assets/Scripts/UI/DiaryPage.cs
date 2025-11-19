using UnityEngine;

[CreateAssetMenu(fileName = "DiaryPage", menuName = "ScriptableObjects/DiaryPage")]
public class DiaryPage : ScriptableObject
{
    public string title;
    public string story;
    public string tutorial;
    public GameObject photoPrefab;
}