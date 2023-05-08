using Mirror;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Unit Selection: This can be only a client side script, server doesn't actually need to know which units are selected, because as soon as
/// we want to move we tell the server to move the selected units
///
/// Unit box collider (not the nav mesh collider): is used for select (mouse click raycast) detection & hits (ex: projectiles)
/// </summary>
public class Unit : NetworkBehaviour
{
    [SerializeField] private UnityEvent _onSelected;
    [SerializeField] private UnityEvent _onDeselected;


    #region CLIENT

    [Client]
    public void Select()
    {
        if(!isOwned) return;

        _onSelected?.Invoke();        
    }

    [Client]
    public void Deselect()
    {
        if(!isOwned) return;

        _onDeselected?.Invoke();        
    }    

    #endregion
    
}
