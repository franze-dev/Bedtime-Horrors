using UnityEngine;

public class CreativityUpdater : MonoBehaviour
{
    [SerializeField] private StatTextUpdater _statTextUpdater;
    [SerializeField] private int value = 1000;

    private float timer = 0f;
    private float timeToUpdate = 1f;
    private int valueToAdd = 2;


    private void Awake()
    {
        if (_statTextUpdater == null)
            GetComponent<StatTextUpdater>();

        EventProvider.Subscribe<ICreativityUpdateEvent>(UpdateText);
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
            EventTriggerer.Trigger<ICreativityUpdateEvent>(new CreativityUpdaterEvent(gameObject, valueToAdd));
            timer = 0f;
        }
    }

    public int GetCreativityValue()
    {
        return value;
    }
}
