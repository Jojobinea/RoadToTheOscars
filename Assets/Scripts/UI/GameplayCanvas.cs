using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayCanvas : MonoBehaviour
{
    public void TroopButtonClicked(int troopId)
    {
        EventManager.OnTroopSpawnTrigger(troopId);
    }
}
