using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class RelayManager : NetworkBehaviour
{
    public void CreateHost()
    {
        NetworkManager.Singleton.StartHost();
    }

    public void CreateClient()
    {
        NetworkManager.Singleton.StartClient();
    }
}
