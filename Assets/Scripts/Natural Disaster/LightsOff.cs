using DragonBones;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "LightsOff", menuName = "ScriptableObjects/NaturalDisasters/LightsOff")]
public class LightsOff : NaturalDisaster, IDisasterUpdate
{
    [SerializeField] private float _duration = 3f;
    [Header("Lights parameters")]
    [SerializeField] private GameObject _darkRectPrefab;
    [SerializeField] private float _maxAlphaValue = 0.9f;
    [SerializeField] private float _darkeningSpeed = 1.0f;
    [SerializeField] private float _lighteningSpeed = 1.0f;

    private GameObject _darkRectGO;
    private Image _darkRectImage;
    private float _fadeOutTime;
    private float _fadeInTime;
    private float _elapsed;

    private Color _minColor;
    private Color _midColor;
    private Color _maxColor;

    public override void EndDisaster()
    {
        AkUnitySoundEngine.SetSwitch("Disaster_Type", "LightsOff", WwiseAudioHelper.DisasterSoundEmitter);
        AkUnitySoundEngine.PostEvent("Disaster_End", WwiseAudioHelper.DisasterSoundEmitter);
                
        _darkRectImage.color = _minColor;
        _elapsed = 0;
        _midColor = _minColor;
        _darkRectGO.SetActive(false);

        EventTriggerer.Trigger<ILogMessageEvent>(new LogMessageEvent("Lights On!", null));
    }

    public override void Init()
    {
        Duration = _duration;
        _darkRectGO = Instantiate(_darkRectPrefab);
        _darkRectImage = _darkRectGO.GetComponentInChildren<Image>();
        _darkRectGO?.SetActive(false);

        _minColor = _darkRectImage.color;
        _maxColor = _darkRectImage.color;

        _minColor.a = 0f;
        _midColor = _minColor;
        _maxColor.a = _maxAlphaValue;

        _darkRectImage.color = _minColor;

        _elapsed = 0;

        _fadeInTime = 1 / _darkeningSpeed;
        _fadeOutTime = 1 / _lighteningSpeed;
    }

    public override void StartDisaster()
    {
        EventTriggerer.Trigger<ILogMessageEvent>(new LogMessageEvent("Lights off!", null));

        AkUnitySoundEngine.SetSwitch("Disaster_Type", "LightsOff", WwiseAudioHelper.DisasterSoundEmitter);
        AkUnitySoundEngine.PostEvent("Disaster_Start", WwiseAudioHelper.DisasterSoundEmitter);

        _darkRectGO?.SetActive(true);
    }

    public void UpdateDisaster()
    {
        _elapsed += Time.deltaTime;

        if (_elapsed < _fadeInTime)
        {
            var t = _elapsed / _fadeInTime;

            _midColor = Color.Lerp(_minColor, _maxColor, Mathf.Clamp01(t));
        }
        else if (_elapsed >= Duration - _fadeOutTime)
        {
            var t = (_elapsed - (Duration - _fadeOutTime)) / _fadeOutTime;

            _midColor = Color.Lerp(_maxColor, _minColor, Mathf.Clamp01(t));
        }

        _darkRectImage.color = _midColor;
    }
}
