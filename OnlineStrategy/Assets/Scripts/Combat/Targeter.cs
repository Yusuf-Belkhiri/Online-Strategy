using Mirror;
using UnityEngine;

public class Targeter : NetworkBehaviour
{
    private Targetable _target;

    public Targetable GetTarget()
    {
        return _target;
    }

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
