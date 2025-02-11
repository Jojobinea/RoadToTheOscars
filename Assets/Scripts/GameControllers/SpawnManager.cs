using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.BossRoom.Infrastructure;

public class SpawnManager : NetworkBehaviour
{
    [SerializeField] private EnemyWaveSO[] _waves;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private float _timeBetweenWaves;
    [SerializeField] private float _timeBetweenEnemies;
    private GameObject _player;

    private void Start()
    {
        NetworkManager.Singleton.OnServerStarted += CallEnemySpawn;
    }

    private void OnNetworkDespawn()
    {
        NetworkManager.Singleton.OnServerStarted -= CallEnemySpawn;
    }

    private void CallEnemySpawn()
    {
        if(IsServer)
        {
            //InvokeRepeating("SpawnEnemies", 0.5f, 1f);
            StartCoroutine(Interval());
            _player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    private void SpawnEnemies()
    {
        //if(!_player) return;
        //int index = Random.Range(0, spawnPoint.Length);

        //NetworkObject enemy = NetworkObjectPool.Singleton.GetNetworkObject(_enemyPrefab, spawnPoint[index].position, Quaternion.identity);
        //enemy.Spawn();
    }

    private IEnumerator WaveController(int waveIndex)
    {
        for(int i = 0; i < _waves[waveIndex].enemyList.Length; i++)
        {
            NetworkObject enemy = NetworkObjectPool.Singleton.GetNetworkObject(_waves[waveIndex].enemyList[i], spawnPoint.position, Quaternion.identity);
            enemy.Spawn();
            yield return new WaitForSeconds(_timeBetweenEnemies);
        }
    }
    
    private IEnumerator Interval()
    {
        for(int i = 0; i < _waves.Length; i++)
        {
            if(i == 0)
            {
                StartCoroutine(WaveController(i));
            }
            else
            {
                yield return new WaitForSeconds(_timeBetweenWaves);
                StartCoroutine(WaveController(i));
            }
            
        }
    }
}
