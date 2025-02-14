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

    private void Start()
    {
        //NetworkManager.OnServerStarted += InitializeBallot;
        NetworkManager.OnClientStarted += InitializeBallot;

        EventManager.onTroopBoughtEvent += AddToBallotCount;
        EventManager.onWalterSalesEvent += WalterSalesEffectClientRpc;
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        //NetworkManager.OnServerStarted -= InitializeBallot;
        NetworkManager.OnClientStarted -= InitializeBallot;
    }

    private void OnDestroy()
    {
        EventManager.onTroopBoughtEvent -= AddToBallotCount;
        EventManager.onWalterSalesEvent -= WalterSalesEffectClientRpc;
    }

    private void InitializeBallot()
    {
        _ballot.quantVotos = 0;

        _txt.text = _ballot.quantVotos.ToString();

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

        _txt.text = _ballot.quantVotos.ToString();
    }

    [ClientRpc]
    private void WalterSalesEffectClientRpc(int addAmount)
    {
        _ballot.quantVotos += addAmount;

        _txt.text = _ballot.quantVotos.ToString();
    }
}
