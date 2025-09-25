using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] protected float cooldown;
    [SerializeField] private GameObject _selectionGO;
    protected float _timer = 0;
    public int price;

    protected virtual void Awake()
    {
        EventTriggerer.Trigger<ITurretSpawnEvent>(new TurretSpawnEvent(this.gameObject));

        if (_selectionGO == null)
            Debug.LogError("Selection GO not found on " + gameObject.name);
        else
            _selectionGO.SetActive(false);
    }

    protected virtual void Update()
    {
        _timer += Time.deltaTime;
    }

    public void Select()
    {
        _selectionGO.SetActive(true);
    }

    public void Deselect()
    {
        _selectionGO.SetActive(false);
    }
}
