using System;
using Mirror;
using UnityEngine;

public class Health : NetworkBehaviour
{
    [SerializeField] private int _maxHealth = 100;

    [SyncVar(hook = nameof(HandleHealthUpdated))] private int _currentHealth;       // Only the server can manipulate it

    public event Action ServerOnDie;

    public event Action<int, int> ClientOnHealthUpdated;       // <currentHealth, maxHealth> 

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

    // Sync hook of currentHealth 
    private void HandleHealthUpdated(int oldHealth, int newHealth)
    {
        ClientOnHealthUpdated?.Invoke(newHealth, _maxHealth);
    }
    #endregion
}
