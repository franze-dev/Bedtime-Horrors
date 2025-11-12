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

    private float _fadeInTime;
    private float _fadeOutTime;
    private float _holdTime;
    private float _elapsed;

    private Color _minColor;
    private Color _maxColor;
    private FadeState _phase;

    private enum FadeState
    {
        None,
        FadingIn,
        Holding,
        FadingOut
    }

    public override void Init()
    {
        Duration = _duration;

        _darkRectGO = Instantiate(_darkRectPrefab);
        _darkRectImage = _darkRectGO.GetComponentInChildren<Image>();
        _darkRectGO.SetActive(false);

        _minColor = _darkRectImage.color;
        _maxColor = _darkRectImage.color;
        _minColor.a = 0f;
        _maxColor.a = _maxAlphaValue;

        _darkRectImage.color = _minColor;

        _fadeInTime = 1f / _darkeningSpeed;
        _fadeOutTime = 1f / _lighteningSpeed;

        _holdTime = Duration - _fadeInTime - _fadeOutTime;

        _elapsed = 0f;
        _phase = FadeState.None;
    }

    public override void StartDisaster()
    {
        EventTriggerer.Trigger<ILogMessageEvent>(new LogMessageEvent("Lights off!", null));
        _darkRectGO.SetActive(true);

        _elapsed = 0f;
        _phase = FadeState.FadingIn;
    }

    public override void EndDisaster()
    {
        _darkRectImage.color = _minColor;
        _darkRectGO.SetActive(false);
        _phase = FadeState.None;
        _elapsed = 0f;

        EventTriggerer.Trigger<ILogMessageEvent>(new LogMessageEvent("Lights On!", null));
    }

    public void UpdateDisaster()
    {
        if (_phase == FadeState.None)
            return;

        _elapsed += Time.deltaTime;

        switch (_phase)
        {
            case FadeState.FadingIn:
                {
                    float t = _elapsed / _fadeInTime;
                    _darkRectImage.color = Color.Lerp(_minColor, _maxColor, Mathf.Clamp01(t));
                    if (_elapsed >= _fadeInTime)
                    {
                        _elapsed = 0f;
                        _phase = FadeState.Holding;
                    }
                }
                break;

            case FadeState.Holding:
                {
                    if (_elapsed >= _holdTime)
                    {
                        _elapsed = 0f;
                        _phase = FadeState.FadingOut;
                    }
                }
                break;

            case FadeState.FadingOut:
                {
                    float t = _elapsed / _fadeOutTime;
                    _darkRectImage.color = Color.Lerp(_maxColor, _minColor, Mathf.Clamp01(t));
                    if (_elapsed >= _fadeOutTime)
                    {
                        _elapsed = 0f;
                        _phase = FadeState.None;
                        EndDisaster();
                    }
                }
                break;
        }
    }
}
