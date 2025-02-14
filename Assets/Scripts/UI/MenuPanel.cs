using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuPanel : MonoBehaviour
{
    [SerializeField] private GameObject _menuPart;
    [SerializeField] private GameObject _joinPart;
    [SerializeField] private TMP_InputField _inputCode;

    private void Start()
    {
        EventManager.onHideMenuEvent += HideMenu;
    }

    private void OnDestroy()
    {
        EventManager.onHideMenuEvent -= HideMenu;
    }

    public void CreateHost()
    {
        EventManager.OnCreateHostTrigger();
        HideMenu();
    }

    public void JoinButton()
    {
        _menuPart.SetActive(false);
        _joinPart.SetActive(true);
    }

    public void BackButotn()
    {
        _menuPart.SetActive(true);
        _joinPart.SetActive(false);
    }

    public void JoinGame()
    {
        EventManager.OnJoinGameTrigger(_inputCode.text);
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    private void HideMenu()
    {
        gameObject.SetActive(false);
    }
}
