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
        _gameOverScreen.SetActive(true);
        Time.timeScale = 0;
    }

    [ClientRpc]
    private void EnableGameWinScreenClientRpc()
    {
        _gameWinScreen.SetActive(true);
        Time.timeScale = 0;
    }

    [ClientRpc]
    private void LostOscarClientRpc()
    {
        _miniOscars[_miniOscarIndex].sprite = _lostOscarSprte;
        _wonOscars[_miniOscarIndex].sprite = _lostOscarSprte;
        _miniOscarIndex += 1;
    }

    public void BackTeMenuButton()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
