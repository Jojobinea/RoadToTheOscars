using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;

public class BallotController : NetworkBehaviour
{
    [SerializeField] private VotosSO _ballot;
    [SerializeField] private float _ballotTimer;
    [SerializeField] private int _ballotToAddByTime;
    [SerializeField] private TMP_Text _txt;
    private NetworkVariable<int> _networkBallotCount = new NetworkVariable<int>();

    private void Start()
    {
        NetworkManager.OnServerStarted += InitializeBallot;
        NetworkManager.OnClientStarted += InitializeBallot;

        EventManager.onTroopBoughtEvent += AddToBallotCount;
    }

    private void OnDestroy()
    {
        NetworkManager.OnServerStarted -= InitializeBallot;
        NetworkManager.OnClientStarted -= InitializeBallot;

        EventManager.onTroopBoughtEvent -= AddToBallotCount;
    }

    private void InitializeBallot()
    {
        if(!IsOwner) return;

        _ballot.quantVotos = 0;
        if(_networkBallotCount.Value != _ballot.quantVotos)
        {
            _networkBallotCount.Value = _ballot.quantVotos;
        }
        _txt.text = _networkBallotCount.Value.ToString();

        StartBallotCoroutine();
    }

    private void StartBallotCoroutine()
    {
        StartCoroutine(BallotCoroutine());
    }

    private IEnumerator BallotCoroutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(_ballotTimer);
            AddToBallotCount(_ballotToAddByTime);
        }
    }

    private void AddToBallotCount(int addAmount)
    {
        _ballot.quantVotos += addAmount;

        if(_networkBallotCount.Value != _ballot.quantVotos)
        {
            _networkBallotCount.Value = _ballot.quantVotos;
        }

        _txt.text = _networkBallotCount.Value.ToString();
    }
}
