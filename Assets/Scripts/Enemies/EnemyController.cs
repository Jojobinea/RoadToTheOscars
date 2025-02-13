using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.VisualScripting;

public class EnemyController : NetworkBehaviour
{
    [SerializeField] private EnemyStatsSO _enemyStatsSO;
    private Transform _route;
    private Transform _targetPos;
    private Transform[] _waypoints;
    private float _speed;
    private int _waypointIndex;

    private int _currentLife;

    void OnEnable()
    {
        _currentLife = _enemyStatsSO.health;
        _speed = _enemyStatsSO.speed;
        _route = GameObject.FindGameObjectWithTag("Route").transform;

        // Initialize the array
        _waypoints = new Transform[_route.childCount];

        for (int i = 0; i < _route.childCount; i++)
        {
            _waypoints[i] = _route.GetChild(i);
        }

        _waypointIndex = 0;
        _targetPos = _waypoints[_waypointIndex];
    }

    void Update()
    {
        if (!IsServer) return;

        SetTargetPos();
        transform.position = Vector3.MoveTowards(transform.position, _targetPos.position, _speed * Time.deltaTime);
    }

    private void SetTargetPos()
    {
        if (Vector3.Distance(transform.position, _targetPos.position) <= 0.1f) // Small threshold
        {
            if (_waypointIndex < _waypoints.Length - 1)
            {
                _waypointIndex++;
                _targetPos = _waypoints[_waypointIndex];
            }
            else
            {
                EventManager.OnEnemyReachedOscarTrigger();
                NetworkObject.Despawn();
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            Debug.Log("colidiu");
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Bullet"){
            Debug.Log("life: "+ _currentLife);
            int test = collision.GetComponent<BulletController>().GetBulletDamage();
            _currentLife -= test;
            if(_currentLife <=0){
                NetworkObject.Despawn();
            }
        }
    }
}
