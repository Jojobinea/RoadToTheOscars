using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
using System;

public class LobbyController : NetworkBehaviour
{
    public static LobbyController Instance;
    [SerializeField] private TMP_Text statusText;
    [SerializeField] private TMP_Text _playerQuant;
    [SerializeField] private TMP_Text _roomCode;

    private NetworkVariable<int> readyCount = new NetworkVariable<int>(0);
    private int totalPlayers;
    private int _maxPlayers = 1;
    private HashSet<ulong> readyPlayers = new HashSet<ulong>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        Time.timeScale = 0;
        readyCount.OnValueChanged += UpdateReadyText;
    }

    private void UpdateReadyText(int previousValue, int newValue)
    {
        _playerQuant.text = readyCount.Value + "/2";
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            totalPlayers = NetworkManager.Singleton.ConnectedClientsIds.Count;
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
            EventManager.onSetRoomCodeTxtEvent += SetRoomCode;
        }
        else
        {
            _roomCode.gameObject.SetActive(false);
        }
    }

    public override void OnNetworkDespawn()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnected;
            EventManager.onSetRoomCodeTxtEvent -= SetRoomCode;
        }

        readyCount.OnValueChanged -= UpdateReadyText;

        base.OnNetworkDespawn();
    }

    private void OnClientConnected(ulong clientId)
    {
        totalPlayers = NetworkManager.Singleton.ConnectedClientsIds.Count;
        Debug.Log(totalPlayers + " player");
    }

    private void OnClientDisconnected(ulong clientId)
    {
        if (readyPlayers.Contains(clientId))
        {
            readyPlayers.Remove(clientId);
            readyCount.Value = readyPlayers.Count;
        }
        totalPlayers = NetworkManager.Singleton.ConnectedClientsIds.Count;
    }

    public void ReadyButton()
    {
        if(IsClient)
            SetPlayerReadyServerRpc(NetworkManager.Singleton.LocalClientId);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerReadyServerRpc(ulong clientId)
    {
        if (!readyPlayers.Contains(clientId))
        {
            readyPlayers.Add(clientId);
            readyCount.Value = readyPlayers.Count;
            Debug.Log("ready players = " + readyCount);
        }

        if (readyCount.Value == _maxPlayers)
        {
            StartGameClientRpc();
        }
    }

    [ClientRpc]
    private void StartGameClientRpc()
    {
        Debug.Log("All players are ready! Starting game...");
        statusText.text = "Game Starting...";

        gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    private void SetRoomCode(string roomCode)
    {
        _roomCode.text = roomCode + "\n<size=60>Room Code";
    }
}
