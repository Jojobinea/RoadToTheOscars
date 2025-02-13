using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class GameplayCanvas : MonoBehaviour
{
    [SerializeField] private GameObject _gameOverScreen;
    [SerializeField] private GameObject _gameWinScreen;
    [SerializeField] private Image[] _miniOscars;
    [SerializeField] private Sprite _lostOscarSprte;
    private int _miniOscarIndex = 0;

    private void Start()
    {
        EventManager.onGameOverEvent += EnableGameOverScreenClientRpc;
        EventManager.onEnemyReachedOscarEvent += LostOscarClientRpc;
    }

    private void OnDestroy()
    {
        EventManager.onGameOverEvent -= EnableGameOverScreenClientRpc;
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
    private void LostOscarClientRpc()
    {
        _miniOscars[_miniOscarIndex].sprite = _lostOscarSprte;
        _miniOscarIndex += 1;
    }
}
