using UnityEngine;

public class GameplayScaler : MonoBehaviour
{

    private float _targetWidth = 1600f;
    private float _targetHeight = 900f;
    private float _targetRatio;

    private float _currentRatio;

    [SerializeField] private float _testWidth;
    [SerializeField] private float _testHeight;


    private void Awake()
    {
        Resize();
    }


    private void OnValidate()
    {
        Resize();
    }

    private void Resize()
    {
        _targetRatio = _targetWidth / _targetHeight;

        var currentWidth = (float)Screen.currentResolution.width;
        var currentHeight = (float)Screen.currentResolution.height;

        _currentRatio = currentWidth / currentHeight;

        _currentRatio = _testWidth / _testHeight;

        float scale = 1f;
        if (_targetRatio > _currentRatio)
        {
            scale = _currentRatio / _targetRatio;
        }



            Debug.Log("target: " + _targetRatio);
        Debug.Log("current: " + _currentRatio);
        Vector3 newScale = new Vector3(scale, scale, scale);


        gameObject.transform.localScale = newScale;
    }



}
