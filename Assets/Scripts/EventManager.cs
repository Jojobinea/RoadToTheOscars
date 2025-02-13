using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventManager
{
    #region Events
    public delegate void OnSpawnTroop(int troopId);
    public static event OnSpawnTroop onSpawnTroopEvent;

    public delegate void OnEnemyReachedOscar();
    public static event OnEnemyReachedOscar onEnemyReachedOscarEvent;

    public delegate void OnGameOver();
    public static event OnGameOver onGameOverEvent;

    public delegate void OnTroopBought(int troopValue);
    public static event OnTroopBought onTroopBoughtEvent;
    #endregion

    #region Triggers
    public static void OnTroopSpawnTrigger(int troopId)
    {
        onSpawnTroopEvent?.Invoke(troopId);
    }

    public static void OnEnemyReachedOscarTrigger()
    {
        onEnemyReachedOscarEvent?.Invoke();
    }

    public static void OnGameOverTrigger()
    {
        onGameOverEvent?.Invoke();
    }

    public static void OnTroopBoughtTrigger(int troopValue)
    {
        onTroopBoughtEvent?.Invoke(troopValue);
    }
    #endregion
}
