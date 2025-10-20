using UnityEngine;

public class TutorialPanel : MonoBehaviour
{
    [SerializeField] private GameObject _handGO;
    
    private void OnEnable()
    {
        ShowHand(true);
    }

    private void OnDisable()
    {
        ShowHand(false);
    }

    public void ShowHand(bool show)
    {
        if (_handGO == null)
            return;

        _handGO.SetActive(show);
    }
}
