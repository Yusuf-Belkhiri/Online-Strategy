using Mirror;
using UnityEngine;

/// <summary>
/// Network behaviour is used to check the ownership (
/// </summary>
public class Targetable : NetworkBehaviour
{
    [SerializeField] private Transform _aimAtPos;       // where the targeter will aim/shoot

    public Transform GetAimAtPos()
    {
        return _aimAtPos;
    }
}
