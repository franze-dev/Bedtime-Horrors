using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TargetManager : MonoBehaviour
{
    public List<GameObject> Targets = new List<GameObject>();
    public GameObject FinalTarget;
    [SerializeField] private GameObject _startPoint;

    private void Awake()
    {
        EventProvider.Subscribe<IPathDirectionChangeEvent>(OnPathDirectionChange);
    }

    private void OnDestroy()
    {
        EventProvider.Unsubscribe<IPathDirectionChangeEvent>(OnPathDirectionChange);
    }

    private void OnPathDirectionChange(IPathDirectionChangeEvent @event)
    {
        if (@event == null || @event.TriggeredByGO == null)
            return;

        if (@event.TriggeredByGO == _startPoint || @event.TriggeredByGO == FinalTarget)
            return;

        if (Targets.Contains(@event.TriggeredByGO))
            return;

        Targets.Add(@event.TriggeredByGO);
    }
}
