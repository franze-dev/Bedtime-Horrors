using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CreditInfo 
{
    [SerializeField] private string _credit;
    [SerializeField] private List<string> _links;

    public string Credit => _credit;

    public List<string> Link => _links;
}