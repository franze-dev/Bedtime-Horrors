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

    private List<Menu> _menus = new();
    public Menu baseMenu;
    private Menu _activeMenu;

    [SerializeField] private InputActionReference _navigateAction;
    private Vector2 _navigateInput = Vector2.zero;

    /// <summary>
    /// Initializes the event system, adds all child menus to the list,
    /// sets the base menu as active, and stores the first selected button
    /// </summary>
    private void Awake()
    {
        ServiceProvider.SetService(this);

        _eventSystem = GetComponent<EventSystem>();
        _eventSystem.firstSelectedGameObject = null;
        _lastSelectedOption = _eventSystem.firstSelectedGameObject;
        AddMenusToList();
        _activeMenu = baseMenu.GetComponent<Menu>();
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
        //Debug.Log(GameManager.Instance.CurrentState);

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
                //SoundManager.Instance.PlaySound(SoundType.SelectButton);
            }
        }
        else
        {
            Debug.Log("Event system is null");
        }
    }

    public void GoToMenu(IMenuState menuState)
    {

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
        SetMenuActive(mainMenuGO);
    }

    /// <summary>
    /// Activates the specified menu and deactivates the rest.
    /// Sets focus on the first button of the active menu
    /// </summary>
    public void SetMenuActive(GameObject menuToActivate)
    {
        if (menuToActivate == null)
        {
            Debug.LogWarning("Menu to activate is null!");
            return;
        }

        foreach (var menu in _menus)
        {
            bool isActive = menu.gameObject == menuToActivate;
            menu.gameObject.SetActive(isActive);

            if (isActive)
            {
                _activeMenu = menu;
                _eventSystem.SetSelectedGameObject(menu.firstButton);
            }
            else
                menu.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Deactivates all menus under this object
    /// </summary>
    public void SetAllInactive()
    {
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

    public void ShowMenu(GameObject menuGO)
    {
        SetAllInactive();
        menuGO.SetActive(true);
    }
}
