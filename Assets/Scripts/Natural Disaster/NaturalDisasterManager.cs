using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NaturalDisasterManager : MonoBehaviour
{
    [SerializeField] private List<NaturalDisaster> _disasters;
    [SerializeField] private float _minInterval = 30f;
    [SerializeField] private float _maxInterval = 120f;
    [SerializeField] private float _timeToDisasterStart = 3f;
    private NaturalDisaster _currentDisaster;
    private WaveManager _waveManager;

    bool _isCoroutineRunning = false;

    [SerializeField] private GameObject _radioObject;
    [SerializeField] private SpriteRenderer _disasterSymbol;

    private void Awake()
    {
        if (_disasters.Count == 0)
            return;

        _radioObject.SetActive(false);
        _disasterSymbol.sprite = null;

        foreach (var disaster in _disasters)
            disaster?.Init();
    }

    private void Start()
    {
        EventProvider.Subscribe<IStartFixedDisasterEvent>(StartFixedDisasterCoroutine);
        ServiceProvider.TryGetService(out _waveManager);
    }

    private void Update()
    {
        if (_disasters.Count == 0 || !_waveManager.WavesStarted)
            return;

        if (!_isCoroutineRunning)
            StartCoroutine(RandomDisasterCoroutine());

        if (_currentDisaster != null)
            (_currentDisaster as IDisasterUpdate)?.UpdateDisaster();
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        _currentDisaster = null;

        EventProvider.Unsubscribe<IStartFixedDisasterEvent>(StartFixedDisasterCoroutine);
    }

    private IEnumerator RandomDisasterCoroutine()
    {
        if (_disasters.Count == 0)
            yield break;

        if (_isCoroutineRunning) yield break;
        _isCoroutineRunning = true;

        yield return new WaitForSeconds(Random.Range(_minInterval, _maxInterval));
        SelectRandomDisaster();
        _disasterSymbol.sprite = _currentDisaster.Icon;
        _radioObject.SetActive(true);

        yield return new WaitForSeconds(_timeToDisasterStart);
        EventTriggerer.Trigger<IOnDisasterStartEvent>(new OnDisasterStartEvent(_currentDisaster));
        StartRandomDisaster();
        _radioObject.SetActive(false);


        var anim = _currentDisaster.AnimationLogic;
        float animDuration = 0;
        if (anim != null)
        {
            animDuration = anim.Armature.armature.animation.animations[anim.Armature.armature.animation.animationNames[0]].duration;

            if (anim.HasAnimation && animDuration > 0)
            {
                string first = anim.Armature.animation.animationNames[0];
                float duration = anim.Armature.armature.animation.animations[first].duration;
                yield return new WaitForSeconds(duration);
                EventTriggerer.Trigger<IOnDisasterLoopEvent>(new OnDisasterLoopEvent(_currentDisaster));
            }
            else
                animDuration = 0;
        }


        yield return new WaitForSeconds(_currentDisaster.Duration - animDuration);
        EventTriggerer.Trigger<IOnDisasterEndEvent>(new OnDisasterEndEvent(_currentDisaster));
        _currentDisaster.EndDisaster();
        //_isCoroutineRunning = false;

        StartCoroutine(WaitForDisasterEndVisual());
    }

    public IEnumerator DisasterCoroutine(NaturalDisaster disaster)
    {
        if (_isCoroutineRunning) yield break;
        _isCoroutineRunning = true;

        _currentDisaster = disaster;
        _disasterSymbol.sprite = _currentDisaster.Icon;
        _radioObject.SetActive(true);

        yield return new WaitForSeconds(_timeToDisasterStart);
        EventTriggerer.Trigger<IOnDisasterStartEvent>(new OnDisasterStartEvent(_currentDisaster));
        _currentDisaster.StartDisaster();
        _radioObject.SetActive(false);

        var anim = _currentDisaster.AnimationLogic;
        float animDuration = 0;
        if (anim != null)
        {
            animDuration = anim.Armature.armature.animation.animations[anim.Armature.armature.animation.animationNames[0]].duration;

            if (anim.HasAnimation && animDuration > 0)
            {
                string first = anim.Armature.animation.animationNames[0];
                float duration = anim.Armature.armature.animation.animations[first].duration;
                yield return new WaitForSeconds(duration);
                EventTriggerer.Trigger<IOnDisasterLoopEvent>(new OnDisasterLoopEvent(_currentDisaster));
            }
            else
                animDuration = 0;
        }

        yield return new WaitForSeconds(_currentDisaster.Duration - animDuration);
        EventTriggerer.Trigger<IOnDisasterEndEvent>(new OnDisasterEndEvent(_currentDisaster));
        _currentDisaster.EndDisaster();

        StartCoroutine(WaitForDisasterEndVisual());
        _currentDisaster = null;
    }

    private IEnumerator WaitForDisasterEndVisual()
    {
        var anim = _currentDisaster.AnimationLogic;

        if (anim != null && anim.HasAnimation)
        {
            string endName = anim.Armature.animation.animationNames.Last();
            float duration = anim.Armature.armature.animation.animations[endName].duration;

            yield return new WaitForSeconds(duration);
        }

        EventTriggerer.Trigger<IOnNoDisasterEvent>(new OnNoDisasterEvent());
        _isCoroutineRunning = false;
    }

    private void SelectRandomDisaster()
    {
        if (_disasters.Count == 0)
        {
            Debug.LogWarning("No disasters available to start.");
            return;
        }

        if (_disasters.Count > 1)
            _currentDisaster = _disasters[Random.Range(0, _disasters.Count)];
        else
            _currentDisaster = _disasters[0];
    }

    private void StartRandomDisaster()
    {
        _currentDisaster.StartDisaster();
    }

    public void StartFixedDisasterCoroutine(IStartFixedDisasterEvent @event)
    {
        StartCoroutine(DisasterCoroutine(@event.Disaster));
    }
}
