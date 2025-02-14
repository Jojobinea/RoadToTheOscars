using Unity.Services.Core;
using Unity.Services.Authentication;
using UnityEngine;
using Unity.Services.Relay.Models;
using Unity.Services.Relay;
using Unity.Netcode.Transports.UTP;
using Unity.Netcode;
using TMPro;

public class ConnectionManager : MonoBehaviour
{   
    [SerializeField] private UnityTransport _unityTransport;
    private async void Start()
    {
        EventManager.onCreateHostEvent += CreateRelay;
        EventManager.onJoinGameEvent += JoinRelay;
        EventManager.onCloseRoomEvent += CloseRoom;

        //Autenrticação no Unity Services
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Connected " + AuthenticationService.Instance.PlayerId);
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    private void OnDestroy()
    {
        EventManager.onCreateHostEvent -= CreateRelay;
        EventManager.onJoinGameEvent -= JoinRelay;
        EventManager.onCloseRoomEvent -= CloseRoom;
    }

    private async void CreateRelay()
    {
        Allocation allocation = await RelayService.Instance.CreateAllocationAsync(1);
        string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
        Debug.Log(joinCode);
        
        _unityTransport.SetHostRelayData(
                    allocation.RelayServer.IpV4,
                    (ushort)allocation.RelayServer.Port,
                    allocation.AllocationIdBytes,
                    allocation.Key, allocation.ConnectionData
                    );
        
        
        NetworkManager.Singleton.StartHost();
        EventManager.OnSetRoomCodeTxtTrigger(joinCode);
    }
    
    private async void JoinRelay(string joinCode)
    {
        try
        {
            Debug.Log("Joing with code " + joinCode);
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
            _unityTransport.SetClientRelayData(joinAllocation.RelayServer.IpV4,
                    (ushort)joinAllocation.RelayServer.Port,
                    joinAllocation.AllocationIdBytes,
                    joinAllocation.Key,
                    joinAllocation.ConnectionData,
                    joinAllocation.HostConnectionData);
            NetworkManager.Singleton.StartClient();

            EventManager.OnHideMenuTrigger();
        }
        catch (RelayServiceException ex) 
        {
            Debug.Log(ex);
        }
    }

    private async void CloseRoom()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            Debug.Log("Closing room...");
            
            // Stop the host
            NetworkManager.Singleton.Shutdown();
        }
    }
}
