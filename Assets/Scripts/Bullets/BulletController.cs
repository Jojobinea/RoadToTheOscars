using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BulletController : NetworkBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private CircleCollider2D _circleCollider;

    private int _bulletDamage = 5;
    private float _speed = 3f;
    private bool _canMove = false;
    private float _currentspeed;


    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnEnable()
    {
        _currentspeed = _speed;
        _canMove = true;
        _circleCollider.enabled = true;

    }

    private void OnDisable()
    {
        _canMove = false;
        _circleCollider.enabled = false;
    }

    public int GetBulletDamage(){
        return _bulletDamage;
    }

    // Update is called once per frame
    void Update()
    {
        //if (!_canMove) return;
        //transform.Translate(Vector3.right * Time.deltaTime * _speed);


    }

    public void SendBullet(Vector3 targetPosition)
    {
        _canMove = true;

        // Calcula a direção correta
        Vector3 direction = (targetPosition - transform.position).normalized;


        // Move o objeto na direção desejada
        StartCoroutine(MoveTowardsTarget(targetPosition, direction));
        
    }

    private IEnumerator MoveTowardsTarget(Vector3 targetPosition, Vector3 direction)
    {
        while (IsSpawned)
        {
            transform.position += direction * Time.deltaTime * _currentspeed;
            
            yield return null;
        }

        // _canMove = false; // Para o movimento ao chegar ao destino
    }

    // public void LoadDefaultConfigBulletConfig(float gunSpeed)
    // {

    //     if (IsServer)
    //     {
    //         _speed = gunSpeed;
    //         SetBulletStatusClientRpc(gunSpeed);
    //     }

    // }

    private void OnTriggerEnter2D(Collider2D other)
    {

        //if (!IsServer || !NetworkObject.IsSpawned) return;

        if (other.gameObject.tag == "Enemy")
        {
            // //enemy health
            // NetworkObject test = other.gameObject.GetComponent<NetworkObject>();
            // test.Despawn(true);
            Debug.Log("acertou");
            DestroyBullet();

        }else{
            return;
        }

    }
        

    private void DestroyBullet()
{
    //Debug.Log("destroy");
    _canMove = false;
    _currentspeed = 0;
    NetworkObject.Despawn(true);
}
}
