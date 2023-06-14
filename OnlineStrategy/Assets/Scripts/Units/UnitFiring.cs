using Mirror;
using UnityEngine;

public class UnitFiring : NetworkBehaviour
{
    [SerializeField] private Targeter _targeter;
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private Transform _projectileSpawnPos;
    [SerializeField] private float _fireRange = 5f;
    [SerializeField] private float _rotationSpeed = 20f;    // to rotate & face the target
    [SerializeField] private float _fireRate = 1f;      // how many projectiles per second
    private float _lastFireTime;

    [ServerCallback]
    private void Update()
    {
        Targetable target = _targeter.GetTarget();
        if (target == null) return;
        if (!CanFireAtTarget()) return;
        
        // Rotation: look towards the target
        Quaternion targetRotation =
            Quaternion.LookRotation(target.transform.position - transform.position);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);

        // Firing: check cooldown
        if (Time.time - _lastFireTime > 1 / _fireRate)
        {
            Quaternion projectileRotation =
                Quaternion.LookRotation(target.GetAimAtPos().position - _projectileSpawnPos.position);
            
            GameObject projectileInstance =
                GameObject.Instantiate(_projectilePrefab, _projectileSpawnPos.position, projectileRotation);
            
            NetworkServer.Spawn(projectileInstance, connectionToClient);
            
            _lastFireTime = Time.time;
        }
    }
    
    /// <summary>
    /// Depending on the distance
    /// </summary>
    /// <returns></returns>
    [Server]
    private bool CanFireAtTarget()
    {
        return (_targeter.GetTarget().transform.position - transform.position).sqrMagnitude <= _fireRange * _fireRange;
    }
}
