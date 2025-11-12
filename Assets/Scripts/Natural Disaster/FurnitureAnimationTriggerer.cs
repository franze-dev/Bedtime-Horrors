using UnityEngine;
using DragonBones;

public class FurnitureAnimationTriggerer : MonoBehaviour
{
    [SerializeField] private UnityArmatureComponent _armature;
    [SerializeField] private string _animationName;
    [SerializeField] NaturalDisaster _disasterToReact;

    private void Awake()
    {
        EventProvider.Subscribe<IOnDisasterStartEvent>(PlayAnimation);
        EventProvider.Subscribe<IOnDisasterEndEvent>(StopAnimation);
    }

    private void OnDestroy()
    {
        EventProvider.Unsubscribe<IOnDisasterStartEvent>(PlayAnimation);
        EventProvider.Unsubscribe<IOnDisasterEndEvent>(StopAnimation);
    }

    private void PlayAnimation(IOnDisasterStartEvent @event)
    {
        if (@event.Disaster == _disasterToReact)
        {
            _armature.gameObject.SetActive(true);
            _armature.animation.Play(_animationName);
        }
    }

    private void StopAnimation(IOnDisasterEndEvent @event)
    {
        if (@event.Disaster == _disasterToReact)
        {
            if (_armature.animation.animations.Count > 1)
            {
                _armature.animation.Play(_armature.animation.animationNames[0]);
            }
            else
            {
                _armature.gameObject.SetActive(false);
                _armature.animation.Stop();
            }
        }
    }

}
