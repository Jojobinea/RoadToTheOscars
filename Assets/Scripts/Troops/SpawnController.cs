using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnController : MonoBehaviour
{



    //list of towers (prefabs) that will instantiate
    public List<GameObject> actors;
    //Transform of the spawning towers (Root Object)
    public Transform spawnActor;
    //id of tower to spawn
    int spawnID = -1;

    [SerializeField] PolygonCollider2D _collider;

    void Update()
    {
        //Debug.Log(spawnID);
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
            Debug.Log("test");
            //get the world space postion of the mouse
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log(mousePos);

            SpawnTower(mousePos);
        }
    }


    void SpawnTower(Vector3 position)
    {
        Vector3 test = new Vector3(position.x, position.y, 0);
        if (!CheckCollider(test))
        {
            GameObject actor = Instantiate(actors[spawnID], spawnActor);
            //actor.transform.position = position;
            
            CheckCollider(test);
            actor.transform.position = test;

            DeselectTowers();
        }else{
            Debug.Log("proibido spawnar");
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
        if (_collider.OverlapPoint(mousePos))
        {
            Debug.Log("clidiu");
            return true;
        }
        Debug.Log("nao colidiu");
        return false;
    }


}
