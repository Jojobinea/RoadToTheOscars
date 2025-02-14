using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuPanel : MonoBehaviour
{
    [SerializeField] private GameObject _menuPart;
    [SerializeField] private GameObject _joinPart;
    [SerializeField] private TMP_InputField _inputCode;

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _audioClip;

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
        _audioSource.PlayOneShot(_audioClip);
        EventManager.OnCreateHostTrigger();
        HideMenu();
    }

    public void JoinButton()
    {
        _audioSource.PlayOneShot(_audioClip);
        _menuPart.SetActive(false);
        _joinPart.SetActive(true);
    }

    public void BackButotn()
    {
         _audioSource.PlayOneShot(_audioClip);
        _menuPart.SetActive(true);
        _joinPart.SetActive(false);
    }

    public void JoinGame()
    { 
        _audioSource.PlayOneShot(_audioClip);
        EventManager.OnJoinGameTrigger(_inputCode.text);
    }

    public void ExitButton()
    {
        _audioSource.PlayOneShot(_audioClip);
        Application.Quit();
    }

    private void HideMenu()
    {
        gameObject.SetActive(false);
    }
}
