using UnityEngine;

public class SpeedButton : MonoBehaviour
{
    [SerializeField] GameObject _speedActiveButtonGO;
    [SerializeField] GameObject _speedDisabledButtonGO;
    [SerializeField] float _speedUpValue = 1.5f;

    public float CurrentSpeed => _speedActiveButtonGO.activeSelf ? _speedUpValue : 1f;

    private void Awake()
    {
        ServiceProvider.SetService(this, true);
    }

    private void OnDestroy()
    {
        ServiceProvider.SetService<SpeedButton>(null);

        Time.timeScale = 1f;
    }

    public void OnSpeedChange()
    {
        if (_speedActiveButtonGO.activeSelf)
        {
            _speedActiveButtonGO.SetActive(false);
            _speedDisabledButtonGO.SetActive(true);
            
            ResetTime();

            AkUnitySoundEngine.PostEvent("UI_Button_Normal", gameObject);
        }
        else
        {
            _speedActiveButtonGO.SetActive(true);
            _speedDisabledButtonGO.SetActive(false);
            
            SpeedUpTime();

            AkUnitySoundEngine.PostEvent("UI_Button_Normal", gameObject);
        }
    }

    private void SpeedUpTime()
    {
        if (Time.timeScale > 0f)
            Time.timeScale = _speedUpValue;
    }

    private void ResetTime()
    {
        if (Time.timeScale > 0f)
            Time.timeScale = 1f;
    }
}
