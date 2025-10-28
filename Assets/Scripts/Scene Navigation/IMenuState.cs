
public interface IMenuState
{
    public void Enter(NavigationController controller);
    public void Exit(NavigationController controller);
}

public class MainMenuState : IMenuState
{
    public void Enter(NavigationController controller)
    {
        controller.ShowMenu(controller.mainMenuGO, this);
    }

    public void Exit(NavigationController controller)
    {
        controller.HideMenu(controller.mainMenuGO);
    }
}

public class WinMenuState : IMenuState
{
    public void Enter(NavigationController controller)
    {
        controller.ShowMenu(controller.winMenuGO, this);
    }

    public void Exit(NavigationController controller)
    {
        controller.HideMenu(controller.winMenuGO);
    }
}

public class LoseMenuState : IMenuState
{
    public void Enter(NavigationController controller)
    {
        controller.ShowMenu(controller.loseMenuGO, this);
    }

    public void Exit(NavigationController controller)
    {
        controller.HideMenu(controller.loseMenuGO);
    }
}

public class SettingsMenuState : IMenuState
{
    public void Enter(NavigationController controller)
    {
        controller.ShowMenu(controller.settingsMenuGO, this);
    }

    public void Exit(NavigationController controller)
    {
        controller.HideMenu(controller.settingsMenuGO);
    }
}

public class CreditsMenuState : IMenuState
{
    public void Enter(NavigationController controller)
    {
        controller.ShowMenu(controller.creditsMenuGO, this);
    }

    public void Exit(NavigationController controller)
    {
        controller.HideMenu(controller.creditsMenuGO);
    }
}

public class DiaryMenuState : IMenuState
{
    public void Enter(NavigationController controller)
    {
        controller.ShowMenu(controller.diaryMenuGO, this);
    }

    public void Exit(NavigationController controller)
    {
        controller.HideMenu(controller.diaryMenuGO);
    }
}

public class PauseMenuState : IMenuState
{
    public void Enter(NavigationController controller)
    {
        controller.ShowMenu(controller.pauseMenuGO, this);
    }

    public void Exit(NavigationController controller)
    {
        controller.HideMenu(controller.pauseMenuGO);
    }
}