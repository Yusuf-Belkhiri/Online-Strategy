using System;
using Mirror;
using UnityEngine;

public class Health : NetworkBehaviour
{
    [SerializeField] private int _maxHealth = 100;

    [SyncVar] private int _currentHealth;       // Only the server can manipulate it

    public event Action ServerOnDie;

    #region SERVER

    public override void OnStartServer()
    {
        _currentHealth = _maxHealth;
    }

    [Server]
    public void ReceiveDamage(int damageAmount)
    {
        if (_currentHealth == 0) return;

        _currentHealth = Mathf.Max(_currentHealth - damageAmount, 0);

        if (_currentHealth != 0) return;

        // Died
        ServerOnDie?.Invoke();
        print("Died");
    }

    #endregion

    #region CLIENT

    #endregion
}
