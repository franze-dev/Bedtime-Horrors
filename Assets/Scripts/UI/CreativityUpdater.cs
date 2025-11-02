using UnityEngine;

public class CreativityUpdater : MonoBehaviour
{
    [SerializeField] private StatTextUpdater _statTextUpdater;
    [SerializeField] private int value = 100;
    [SerializeField] private int ValueAddPerSecond = 2;

    private float timer = 0f;
    private float timeToUpdate = 1f;

    private void Awake()
    {
        ServiceProvider.SetService(this, true);

        if (_statTextUpdater == null)
            GetComponent<StatTextUpdater>();

        EventProvider.Subscribe<ICreativityUpdateEvent>(UpdateText);
    }

    private void OnDestroy()
    {
        ServiceProvider.SetService<CreativityUpdater>(null);
        EventProvider.Unsubscribe<ICreativityUpdateEvent>(UpdateText);
    }

    private void UpdateText(ICreativityUpdateEvent @event)
    {
        value += @event.Value;

        _statTextUpdater.UpdateText(value);
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= timeToUpdate)
        {
            EventTriggerer.Trigger<ICreativityUpdateEvent>(new CreativityUpdaterEvent(gameObject, ValueAddPerSecond));
            timer = 0f;
        }
    }

    public int GetCreativityValue()
    {
        return value;
    }
}
