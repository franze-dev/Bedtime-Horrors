public interface IMenuState
{
    public void Enter(NavigationController controller);
}

public class MainMenuState : IMenuState
{
    public void Enter(NavigationController controller)
    {
        controller.ShowMenu(controller.mainMenuGO);
    }
}

public class WinMenuState : IMenuState
{
    public void Enter(NavigationController controller)
    {
        controller.ShowMenu(controller.winMenuGO);
    }
}

public class LoseMenuState : IMenuState
{
    public void Enter(NavigationController controller)
    {
        controller.ShowMenu(controller.loseMenuGO);
    }
}

public class SettingsMenuState : IMenuState
{
    public void Enter(NavigationController controller)
    {
        controller.ShowMenu(controller.settingsMenuGO);
    }
}

public class CreditsMenuState : IMenuState
{
    public void Enter(NavigationController controller)
    {
        controller.ShowMenu(controller.creditsMenuGO);
    }
}

