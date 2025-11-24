using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CreditSectionInfo", menuName = "ScriptableObjects/CreditSectionInfo")]
public class CreditsSectionInfo : ScriptableObject
{
    [SerializeField] private string _title;
    [SerializeField] List<CreditInfo> _credits;
    [SerializeField] private Sprite _background;

    public Sprite Background => _background;

    public string Title => _title;

    public List<CreditInfo> Credits => _credits;
}
