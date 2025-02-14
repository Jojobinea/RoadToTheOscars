using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameplayCanvas : NetworkBehaviour
{
    [SerializeField] private GameObject _gameOverScreen;
    [SerializeField] private GameObject _gameWinScreen;
    [SerializeField] private Image[] _miniOscars;
    [SerializeField] private Image[] _wonOscars;
    [SerializeField] private Sprite _lostOscarSprte;

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _winClip;
    [SerializeField] private AudioClip _gameoverClip;

    [SerializeField] private AudioClip _oscarCrumblingClip;
    [SerializeField] private AudioClip _buttonClip;
    private int _miniOscarIndex = 0;

    private void Start()
    {
        EventManager.onGameOverEvent += EnableGameOverScreenClientRpc;
        EventManager.onGameWinEvent += EnableGameWinScreenClientRpc;
        EventManager.onEnemyReachedOscarEvent += LostOscarClientRpc;
    }

    private void OnDestroy()
    {
        EventManager.onGameOverEvent -= EnableGameOverScreenClientRpc;
        EventManager.onGameWinEvent -= EnableGameWinScreenClientRpc;
        EventManager.onEnemyReachedOscarEvent -= LostOscarClientRpc;
    }

    public void TroopButtonClicked(int troopId)
    {
        EventManager.OnTroopSpawnTrigger(troopId);
    }

    [ClientRpc]
    private void EnableGameOverScreenClientRpc()
    {
        _audioSource.PlayOneShot(_gameoverClip);
        _gameOverScreen.SetActive(true);
        Time.timeScale = 0;
    }

    [ClientRpc]
    private void EnableGameWinScreenClientRpc()
    {
        _audioSource.PlayOneShot(_winClip);
        _gameWinScreen.SetActive(true);
        Time.timeScale = 0;
    }

    [ClientRpc]
    private void LostOscarClientRpc()
    {
        
        _audioSource.PlayOneShot(_oscarCrumblingClip);
        _miniOscars[_miniOscarIndex].sprite = _lostOscarSprte;
        _wonOscars[_miniOscarIndex].sprite = _lostOscarSprte;
        _miniOscarIndex += 1;
    }

    public void BackTeMenuButton()
    {
         _audioSource.PlayOneShot(_buttonClip);
        SceneManager.LoadScene("SampleScene");
    }
}
