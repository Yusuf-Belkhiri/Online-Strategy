using Mirror;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This can be only a client side script, server doesn't actually need to know which units are selected, because as soon as
/// we want to move we tell the server to move the selected units 
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
