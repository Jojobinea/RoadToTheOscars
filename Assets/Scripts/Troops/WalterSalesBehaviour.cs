using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class WalterSalesBehaviour : NetworkBehaviour
{
    [SerializeField] private VotosSO _votos;
    [SerializeField] private float _interval;
    [SerializeField] private int _votosToAdd;
    [SerializeField] private ParticleSystem _particleSystem;

    private void OnEnable()
    {           
        CallCompraDeVotosCoroutine();
    }

    private void CallCompraDeVotosCoroutine()
    {
        StartCoroutine(CompraDeVotos());
    }

    private IEnumerator CompraDeVotos()
    {
        while(true)
        {
            yield return new WaitForSeconds(_interval);
            EventManager.OnWalterSalesTriggger(_votosToAdd);
            _particleSystem.Play();
        }
    }
}
