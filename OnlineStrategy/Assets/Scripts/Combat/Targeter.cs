using Mirror;
using UnityEngine;

public class Targeter : NetworkBehaviour
{
    // remove SF
    [SerializeField] private Targetable _target;


    #region SERVER

    [Command]
    public void CmdSetTarget(GameObject targetGameObject)
    {
        if (!targetGameObject.TryGetComponent<Targetable>(out Targetable target)) return;
        _target = target;
    }

    [Server]
    public void ClearTarget()
    {
        _target = null;
    }
    #endregion
}
