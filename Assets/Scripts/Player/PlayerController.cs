using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private Transform _firePoint;

    public float fireRate = 1f;
    public float projectileSpeed = 10f;

    private Transform target;
    private bool canShoot=false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Enemy")){
            Debug.Log("coliudsiu");
            target = other.transform;
            StartCoroutine(ShootRoutine());
        }
    }

    void OnTriggerExit2D(Collider2D other){
        if(other.CompareTag("Enemy")){
            target = null;
        }
    }

    IEnumerator ShootRoutine()
    {
        while (target != null) // Enquanto houver um inimigo no radar
        {
            Shoot();
            yield return new WaitForSeconds(1f / fireRate); // Tempo entre os tiros
        }
    }
    void Shoot()
    {
        if (_projectilePrefab != null && _firePoint != null)
        {
            GameObject projectile = Instantiate(_projectilePrefab, _firePoint.position, Quaternion.identity);
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 direction = (target.position - _firePoint.position).normalized;
                rb.velocity = direction * projectileSpeed; // Atira na direção do inimigo
            }
        }
    }


}
