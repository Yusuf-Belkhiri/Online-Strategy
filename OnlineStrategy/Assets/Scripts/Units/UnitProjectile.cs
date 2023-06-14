using System;
using Mirror;
using UnityEngine;

public class UnitProjectile : NetworkBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _launchForce = 10f;
    [SerializeField] private float _lifeTime = 5f;      // time to be destroyed if it doesn't hit any object

    [SerializeField] private int _damagePoints = 20;

    // We don't need to sync the position, because they move in a straight line with a constant speed (unlike units) 
    private void Start()
    {
        _rb.velocity = transform.forward * _launchForce;
    }

    public override void OnStartServer()
    {
        Invoke(nameof(DestroyProjectile), _lifeTime);
    }

    private void DestroyProjectile()
    {
        NetworkServer.Destroy(gameObject);
    }


    [ServerCallback]
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<NetworkIdentity>(out NetworkIdentity networkIdentity))
        {
            if (networkIdentity.connectionToClient == connectionToClient) return;
        }

        if (other.TryGetComponent<Health>(out Health health))
        {
            health.ReceiveDamage(_damagePoints);
        }
        
        DestroyProjectile();
    }
}
