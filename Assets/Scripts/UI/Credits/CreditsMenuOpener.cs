using UnityEngine;

public class CreditsMenuOpener : MonoBehaviour
{
    [SerializeField] private CreditsSectionInfo _creditsToOpen;

    public void OpenCreditsMenu()
    {
        if (_creditsToOpen != null)
            EventTriggerer.Trigger<IOpenCreditMenuEvent>(new OpenCreditMenuEvent(_creditsToOpen));
        else
            EventTriggerer.Trigger<ICloseCreditMenuEvent>(new CloseCreditMenuEvent());
    }
}
