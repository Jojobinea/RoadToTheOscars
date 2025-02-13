using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OscarsController : MonoBehaviour
{
    [SerializeField] private Animator[] _oscars;
    private int _oscarIndex;

    private void Start()
    {
        EventManager.onEnemyReachedOscarEvent += BreakOscars;

        _oscarIndex = 0;
    }

    private void OnDestroy()
    {
        EventManager.onEnemyReachedOscarEvent -= BreakOscars;
    }

    private void BreakOscars()
    {
        _oscars[_oscarIndex].SetTrigger("BreakOscar");
        _oscarIndex += 1;

        if(_oscarIndex >= _oscars.Length)
        {
            EventManager.OnGameOverTrigger();
            Debug.Log("game over");
        }
    }
}
