using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(RectTransform))]
public class TooltipUI : MonoBehaviour
{
    [SerializeField] private RectTransform _backgroundTransform;
    [SerializeField] private RectTransform _canvasTransform;
    [SerializeField] private TextMeshProUGUI _textMesh;
    [SerializeField] private Vector2 _paddingSize = new(20f, 20f);
    [SerializeField] private Vector2 _spaceFromMouse = new(0.2f, 0.2f);
    private RectTransform _rectTransform;
    private ITooltipInfo _currentInfoShowing = null;
    private PauseController _pauseController;
    private TutorialManager _tutorialManager;

    private void Awake()
    {
        if (_backgroundTransform == null)
            Debug.LogError("No tooltip bg found");

        if (_textMesh == null)
            Debug.LogError("No tooltip text found");

        _rectTransform = GetComponent<RectTransform>();

        Enable(false);
    }

    private void Start()
    {
        ServiceProvider.TryGetService(out _pauseController);
    }

    private void Enable(bool enable)
    {
        _backgroundTransform.gameObject.SetActive(enable);
        _textMesh.gameObject.SetActive(enable);
    }

    private void SetText(string text)
    {
        _textMesh.text = text;

        _textMesh.ForceMeshUpdate();

        Vector2 textSize = _textMesh.GetRenderedValues(false);

        _backgroundTransform.sizeDelta = textSize + _paddingSize;
    }

    private void CheckHover()
    {
        if (!_pauseController)
            ServiceProvider.TryGetService(out _pauseController);

        if (!_tutorialManager)
            ServiceProvider.TryGetService(out _tutorialManager);

        if ((_pauseController && _pauseController.IsPaused) ||
           (_tutorialManager && !_tutorialManager.IsClickAllowed))
        {
            Enable(false);
            return;
        }

        var screenPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        var hits = Physics2D.RaycastAll(screenPos, Vector2.zero);

        foreach (var hit in hits)
        {
            if (hit.collider == null) continue;

            if (hit.collider.gameObject.TryGetComponent<ITooltipInfo>(out var tooltip))
            {
                _currentInfoShowing = tooltip;
                SetText(_currentInfoShowing.Text);
                Enable(true);
                return;
            }
        }

        _currentInfoShowing = null;
        Enable(false);
    }

    private void Update()
    {
        CheckHover();

        if (_currentInfoShowing == null)
            return;

        var mousePos = Mouse.current.position.ReadValue();

        RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasTransform, mousePos,
                                                                null, out var anchorPos);

        _rectTransform.anchoredPosition = anchorPos + _spaceFromMouse;
    }
}

public interface ITooltipInfo
{
    string Text { get; }
}