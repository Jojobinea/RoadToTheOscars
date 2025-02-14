using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using Unity.BossRoom.Infrastructure;
using UnityEngine.EventSystems;
public class TroopSpawnerController : NetworkBehaviour
{
    public List<GameObject> actors; //list of towers (prefabs) that will instantiate
    public Transform spawnActor; //Transform of the spawning towers (Root Object)
    int spawnID = -1; //id of tower to spawn
    private PolygonCollider2D _routeCollider;

    [SerializeField] private AudioSource _audioSource;
    public List<AudioClip> audioClips;

    private void Start()
    {
        EventManager.onSpawnTroopEvent += SelectTower;

        _routeCollider = GameObject.FindGameObjectWithTag("Route").GetComponent<PolygonCollider2D>();
    }

    private void OnDestroy()
    {
        EventManager.onSpawnTroopEvent -= SelectTower;
    }

    void Update()
    {
        //if(!IsServer) return;

        if (CanSpawn())
            DetectSpawnPoint();
    }

    bool CanSpawn()
    { 
        if (spawnID == -1)
        {
            //Debug.Log("nao pode");
            return false;
        }
        else
            //Debug.Log("pode");
            return true;
    }

    void DetectSpawnPoint()
    {
        //Detect when mouse is clicked (first touch clicked)
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            //Debug.Log("test");
            //get the world space postion of the mouse
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log(mousePos);

            SpawnTower(mousePos);
        }
    }

    void SpawnTower(Vector3 position)
    {
        if (IsClient && !IsServer)
        {
            // If this is a client, request the server to spawn
            SpawnTowerServerRpc(position, spawnID, OwnerClientId);
            return;
        }

        // This part runs only for the server
        Vector3 test = new Vector3(position.x, position.y, 0);
        if (!CheckCollider(test))
        {
            NetworkObject actor = NetworkObjectPool.Singleton.GetNetworkObject(actors[spawnID], test, Quaternion.identity);
            actor.Spawn();
            PlaySoundServerRpc(spawnID);
            
            //actor.transform.position = test;
            DeselectTowers();
        }
        else
        {
            //Debug.Log("proibido spawnar");
        }
    }


    [ServerRpc (RequireOwnership = false)]
    void SpawnTowerServerRpc(Vector3 position, int spawnId, ulong clientId)
    {
        if (spawnId < 0 || spawnId >= actors.Count) return;

        Vector3 test = new Vector3(position.x, position.y, 0);

        if (!CheckCollider(test))
        {
            NetworkObject actor = NetworkObjectPool.Singleton.GetNetworkObject(actors[spawnId], test, Quaternion.identity);
            actor.SpawnWithOwnership(clientId); // Assign ownership to the client
            PlaySoundServerRpc(spawnID);
            

            //actor.transform.position = test;
        }
        else
        {
            //Debug.Log("proibido spawnar");
        }
        
    }

     [ServerRpc(RequireOwnership = false)]
    private void PlaySoundServerRpc(int spawnID)
    {
        Debug.Log("teste"+ spawnID);
        PlaySoundClientRpc(spawnID);
    }

    [ClientRpc]
    private void PlaySoundClientRpc(int spawnId)
    {
        if (_audioSource != null)
        {
            _audioSource.PlayOneShot(audioClips[spawnID]);
        }
    }

    public void SelectTower(int id)
    {
        
        DeselectTowers();
        //Set the spawnID
        spawnID = id;
    }

    public void DeselectTowers()
    {
        spawnID = -1;
    }

    private bool CheckCollider(Vector3 mousePos)
    {
        if (_routeCollider.OverlapPoint(mousePos))
        {
            //Debug.Log("clidiu");
            return true;
        }
        //Debug.Log("nao colidiu");
        return false;
    }


}
