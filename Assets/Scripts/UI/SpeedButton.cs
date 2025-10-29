using UnityEngine;

public class SpeedButton : MonoBehaviour
{
    [SerializeField] GameObject _speedActiveButtonGO;
    [SerializeField] GameObject _speedDisabledButtonGO;
    [SerializeField] float _speedUpValue = 1.5f;

    private void OnDestroy()
    {
        Time.timeScale = 1.0f;
    }

    public void OnSpeedChange()
    {
        if (_speedActiveButtonGO.activeSelf)
        {
            _speedActiveButtonGO.SetActive(false);
            _speedDisabledButtonGO.SetActive(true);
            ResetTime();
        }
        else
        {
            _speedActiveButtonGO.SetActive(true);
            _speedDisabledButtonGO.SetActive(false);
            SpeedUpTime();
        }
    }

    private void SpeedUpTime()
    {
        Time.timeScale = _speedUpValue;
    }

    private void ResetTime()
    {
        Time.timeScale = 1.0f;
    }
}
