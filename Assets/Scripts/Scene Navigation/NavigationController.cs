using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class NavigationController : MonoBehaviour
{
    [SerializeField] private EventSystem _eventSystem;
    private GameObject _lastSelectedOption;

    public GameObject mainMenuGO;
    public GameObject pauseMenuGO;
    public GameObject winMenuGO;
    public GameObject loseMenuGO;
    public GameObject settingsMenuGO;
    public GameObject creditsMenuGO;
    public GameObject diaryMenuGO;

    private List<Menu> _menus = new();
    public Menu baseMenu;
    private IMenuState _activeState;
    private IMenuState _previousState;

    public IMenuState PreviousMenu => _previousState;

    [SerializeField] private InputActionReference _navigateAction;
    private Vector2 _navigateInput = Vector2.zero;

    /// <summary>
    /// Initializes the event system, adds all child menus to the list,
    /// sets the base menu as active, and stores the first selected button
    /// </summary>
    private void Awake()
    {
        ServiceProvider.SetService(this, true);

        _eventSystem = GetComponent<EventSystem>();
        _eventSystem.firstSelectedGameObject = null;
        _lastSelectedOption = _eventSystem.firstSelectedGameObject;

        AddMenusToList();

        ShowMenu(mainMenuGO, new MainMenuState());
    }

    private void OnDestroy()
    {
        ServiceProvider.SetService<NavigationController>(null);
    }

    /// <summary>
    /// Sets the base menu as the currently active menu and unlocks the cursor
    /// </summary>
    private void Start()
    {
        SetAllInactive();

        SetBaseMenuActive();
    }

    /// <summary>
    /// Subscribes to the navigation input action
    /// </summary>
    private void OnEnable()
    {
        if (_navigateAction != null)
        {
            _navigateAction.action.performed += OnNavigate;
            _navigateAction.action.canceled += OnNavigate;
        }
    }

    /// <summary>
    /// Checks current selection in the event system.
    /// If nothing is selected and input was pressed, re-selects the last valid option.
    /// Plays sound if a new option is selected
    /// </summary>
    private void Update()
    {
        if (_eventSystem != null)
        {
            if (_eventSystem.currentSelectedGameObject == null)
            {
                if (WasNavigatePressed())
                    _eventSystem.SetSelectedGameObject(_lastSelectedOption);
            }
            else if (_lastSelectedOption != _eventSystem.currentSelectedGameObject)
            {
                _lastSelectedOption = _eventSystem.currentSelectedGameObject;
            }
        }
        else
        {
            Debug.Log("Event system is null");
        }
    }

    public void GoToMenu(IMenuState menuState, bool deactivatePrevious = true)
    {
        if (deactivatePrevious)
            _activeState.Exit(this);

        menuState.Enter(this);
    }

    /// <summary>
    /// Searches for all Menu components under this object and adds them to the internal list
    /// </summary>
    private void AddMenusToList()
    {
        _menus.Clear();

        foreach (Transform child in transform)
        {
            if (child.TryGetComponent<Menu>(out var menu))
                _menus.Add(menu);
        }
    }

    /// <summary>
    /// Activates the base menu defined in the inspector
    /// </summary>
    private void SetBaseMenuActive()
    {
        _previousState = _activeState;
        GoToMenu(new MainMenuState());
    }

    ///// <summary>
    ///// Activates the specified menu and deactivates the rest.
    ///// Sets focus on the first button of the active menu
    ///// </summary>
    //public void SetMenuActive(GameObject menuToActivate, bool deactivatePrevious = true)
    //{
    //    if (menuToActivate == null)
    //    {
    //        Debug.LogWarning("Menu to activate is null!");
    //        return;
    //    }



    //    foreach (var menu in _menus)
    //    {
    //        bool isCurrentActive = false;

    //        if (!menu.gameObject.activeSelf)
    //        {
    //            isCurrentActive = menu.gameObject == menuToActivate;
    //            menu.gameObject.SetActive(isCurrentActive);
    //        }
    //        else
    //        {
    //            if (menu.gameObject != menuToActivate)
    //            {
    //                if (deactivatePrevious)
    //                    menu.gameObject.SetActive(false);
    //                else
    //                    menu.gameObject.SetActive(true);
    //            }
    //        }

    //        if (isCurrentActive)
    //        {
    //            _previousState = _activeState;
    //            _activeState = menuToActivate;
    //            _eventSystem.SetSelectedGameObject(menu.firstButton);
    //        }
    //        else
    //            menu.gameObject.SetActive(false);
    //    }
    //}

    /// <summary>
    /// Deactivates all menus under this object
    /// </summary>
    public void SetAllInactive()
    {
        _previousState = _activeState;
        foreach (var menu in _menus)
            menu.gameObject.SetActive(false);
    }

    /// <summary>
    /// Reads the navigation input direction and stores it
    /// </summary>
    private void OnNavigate(InputAction.CallbackContext obj)
    {
        _navigateInput = obj.ReadValue<Vector2>();
    }

    /// <summary>
    /// Returns true if a directional input is currently being pressed
    /// </summary>
    private bool WasNavigatePressed()
    {
        return _navigateInput != Vector2.zero;
    }

    public void ShowMenu(GameObject menuGO, IMenuState state)
    {
        _previousState = _activeState;
        _activeState = state;
        menuGO.SetActive(true);
    }

    public void HideMenu(GameObject menuGO)
    {
        menuGO.SetActive(false);
    }
}
