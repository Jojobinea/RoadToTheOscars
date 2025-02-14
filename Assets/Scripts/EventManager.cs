using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
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

    public delegate void OnGameWin();
    public static event OnGameWin onGameWinEvent;

    public delegate void OnTroopBought(int troopValue);
    public static event OnTroopBought onTroopBoughtEvent;

    public delegate void OnWalterSales(int add);
    public static event OnWalterSales onWalterSalesEvent;

    public delegate void OnCreateHost();
    public static event OnCreateHost onCreateHostEvent;

    public delegate void OnJoinGame(string joinCode);
    public static event OnJoinGame onJoinGameEvent;

    public delegate void HideMenu();
    public static event HideMenu onHideMenuEvent;

    public delegate void OnSetRoomCodeTxt(string roomCodeTxt);
    public static event OnSetRoomCodeTxt onSetRoomCodeTxtEvent;

    public delegate void OnEnemyDeath();
    public static event OnEnemyDeath onEnemyDeathEvent;

    public delegate void OnCloseRoom();
    public static event OnCloseRoom onCloseRoomEvent;
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

    public static void OnGameWinTrigger()
    {
        onGameWinEvent?.Invoke();
    }

    public static void OnTroopBoughtTrigger(int troopValue)
    {
        onTroopBoughtEvent?.Invoke(troopValue);
    }

    public static void OnWalterSalesTriggger(int add)
    {
        onWalterSalesEvent?.Invoke(add);
    }

    public static void OnCreateHostTrigger()
    {
        onCreateHostEvent?.Invoke();
    }

    public static void OnJoinGameTrigger(string joinCode)
    {
        onJoinGameEvent?.Invoke(joinCode);
    }

    public static void OnHideMenuTrigger()
    {
        onHideMenuEvent?.Invoke();
    }

    public static void OnSetRoomCodeTxtTrigger(string roomCodeTxt)
    {
        onSetRoomCodeTxtEvent?.Invoke(roomCodeTxt);
    }

    public static void OnEnemyDeathTrigger()
    {
        onEnemyDeathEvent?.Invoke();
    }

    public static void OnCloseRoomTrigger()
    {
        onCloseRoomEvent?.Invoke();
    }
    #endregion
}
