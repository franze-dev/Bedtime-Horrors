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
        _currentRatio = (float)Screen.currentResolution.width / (float)Screen.currentResolution.height;

        //Debug.Log((float)Screen.currentResolution.width + " " + (float)Screen.currentResolution.height);
        //
        //_targetRatio = _testWidth / _testHeight;

        var scale = _targetRatio / _currentRatio;
        Vector3 newScale = new Vector3(scale, scale, scale);


        gameObject.transform.localScale = newScale;
    }



}
