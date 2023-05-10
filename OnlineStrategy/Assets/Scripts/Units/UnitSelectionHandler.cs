using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

/// <summary>
/// Manager
/// Unit Selection: This can be only a client side script, server doesn't actually need to know which units are selected, because as soon as
/// we want to move we tell the server to move the selected units 
/// </summary>
public class UnitSelectionHandler : NetworkBehaviour
{
    [SerializeField] private LayerMask _layerMask = new LayerMask();        // Default
    private Camera _mainCamera;


    public List<Unit> SelectedUnits { get; } = new();
    private void Start()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            // Remove previous selection & Start new selection area
            foreach (Unit selectedUnit in SelectedUnits)
            {
                selectedUnit.Deselect();
            }
            SelectedUnits.Clear();
        }
        else if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            ClearSelectionArea();
        }
    }

    private void ClearSelectionArea()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _layerMask))  return;

        if (!hit.collider.TryGetComponent<Unit>(out Unit unit)) return;

        if (!unit.isOwned)  return;
        
        SelectedUnits.Add(unit);

        foreach (Unit selectedUnit in SelectedUnits)
        {
            selectedUnit.Select();
        }
    }
}
