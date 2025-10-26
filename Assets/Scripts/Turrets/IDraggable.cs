public interface IDraggable
{
    void OnDragStart(IClickEvent @event);
    void OnDrag(IDragEvent @event);
    void OnDragEnd(IClickReleaseEvent @event);
}