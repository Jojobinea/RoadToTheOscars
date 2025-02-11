using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/Enemy Wave")]
public class EnemyWaveSO : ScriptableObject
{
    /*
    public EnemyWave[] wave;  

    [System.Serializable]
    public struct EnemyWave
    {
        public GameObject enemy;
        public int number;
    }
    */

    public GameObject[] enemyList;
}
