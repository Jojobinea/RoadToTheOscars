using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventManager
{
    #region Events
    public delegate void OnSpawnTroop(int troopId);
    public static event OnSpawnTroop onSpawnTroopEvent;
    #endregion

    #region Triggers
    public static void OnTroopSpawnTrigger(int troopId)
    {
        onSpawnTroopEvent?.Invoke(troopId);
    }
    #endregion
}
