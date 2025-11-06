using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundAnimation : MonoBehaviour
{
    [SerializeField] private Image _sourceImage;
    [SerializeField] private List<Sprite> _animationFrames = new List<Sprite>();
    [SerializeField] private float _timeBetweenFrames = 0.04f;
    private float _timer = 0f;
    private int _currentFrameIndex;

    private void OnEnable()
    {
        _timer = 0f;
        _currentFrameIndex = 0;
    }

    private void OnDisable()
    {
        _timer = 0f;
        _currentFrameIndex = 0;
    }

    private void LateUpdate()
    {
        _timer += Time.deltaTime;

        if (_timer >= _timeBetweenFrames)
        {
            _timer -= _timeBetweenFrames;
            _currentFrameIndex++;

            if (_currentFrameIndex >= _animationFrames.Count - 1)
                _currentFrameIndex = 0;

            _sourceImage.sprite = _animationFrames[_currentFrameIndex];
        }
    }
}
