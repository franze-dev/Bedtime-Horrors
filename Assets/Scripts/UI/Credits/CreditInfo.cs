using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CreditInfo 
{
    [SerializeField] private string _credit;
    [SerializeField] private List<Link> _links;

    public string Credit => _credit;

    public List<Link> Link => _links;
}

[Serializable]
public class Link
{
    [SerializeField] public string URL;
    [SerializeField] public string name;
}