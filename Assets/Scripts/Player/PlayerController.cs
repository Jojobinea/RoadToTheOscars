using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using Unity.BossRoom.Infrastructure;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private Transform _firePoint;

    public float fireRate;

    private Transform target;

    private List<GameObject> _enemiesOnRadar;

    private bool _isShooting;
    // Start is called before the first frame update
    void Start()
    {
        _isShooting = false;
        _enemiesOnRadar = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Enemy")){
            //Debug.Log("trigger shoot");
            _enemiesOnRadar.Add(other.gameObject);
            //target = _enemiesOnRadar[0].transform;

            if(!_isShooting){
                StartCoroutine("ShootRoutine");
            }
            

        }
    }

    void OnTriggerExit2D(Collider2D other){
        if(other.CompareTag("Enemy")){
            _enemiesOnRadar.Remove(other.gameObject);

            if(_enemiesOnRadar.Count == 0){
                StopCoroutine("ShootRoutine");
                target = null;
                _isShooting = false;
            }
            
            
        }
    }

    IEnumerator ShootRoutine()
    {
        //Debug.Log("shooting routine");
        _isShooting = true;
        while (_enemiesOnRadar.Count >0) // Enquanto houver um inimigo no radar
        {
            target = _enemiesOnRadar[0].transform;
            ShootServerRpc();
            yield return new WaitForSeconds(fireRate); // Tempo entre os tiros
        }
    }

    [ServerRpc (RequireOwnership = false)]
    void ShootServerRpc()
    {
        if (_projectilePrefab != null && _firePoint != null && target!=null)
        {
            NetworkObject bullet = NetworkObjectPool.Singleton.GetNetworkObject(_projectilePrefab, _firePoint.position, transform.rotation);
            bullet.Spawn();
            bullet.GetComponent<BulletController>().SendBullet(target.position);
            // GameObject projectile = Instantiate(_projectilePrefab, _firePoint.position, Quaternion.identity);
            // Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            
            // if (rb != null)
            // {
            //     Vector2 direction = (target.position - _firePoint.position).normalized;
                
            //     rb.velocity = direction * projectileSpeed; // Atira na direção do inimigo
            // }
        }
    }


}
