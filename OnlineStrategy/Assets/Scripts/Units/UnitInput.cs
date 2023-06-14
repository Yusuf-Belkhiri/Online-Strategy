using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Manager (as UnitSelectionHandler)
/// </summary>
public class UnitInput : MonoBehaviour
{
    [SerializeField] private UnitSelectionHandler _unitSelectionHandler = null;
    [SerializeField] private LayerMask _layerMask = new LayerMask();
    private Camera _mainCamera;

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        if(!Mouse.current.rightButton.wasPressedThisFrame) return;

        Ray ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        
        if(!Physics.Raycast(ray, out RaycastHit hit,Mathf.Infinity, _layerMask)) return;

        if (hit.collider.TryGetComponent<Targetable>(out Targetable target))
        {
            if (target.isOwned)     // can be replaced & removed ig
            {
                TryMove(hit.point);
                return;
            }
            // In case of clicking on another targetable (not an ally)
            TryTarget(target);
            return;
        }
        TryMove(hit.point);
    }

    private void TryMove(Vector3 point)
    {
        foreach (var unit in _unitSelectionHandler.SelectedUnits)
        {
            unit.GetUnitMovement().CmdMove(point);
        }        
    }
    private void TryTarget(Targetable target)
    {
        foreach (var unit in _unitSelectionHandler.SelectedUnits)
        {
            unit.GetTargeter().CmdSetTarget(target.gameObject);
        }        
    }
}
