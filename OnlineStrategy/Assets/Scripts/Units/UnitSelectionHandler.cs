using System.Collections.Generic;
using Mirror;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Manager
/// Unit Selection: This can be only a client side script, server doesn't actually need to know which units are selected, because as soon as
/// we want to move we tell the server to move the selected units 
/// </summary>
public class UnitSelectionHandler : NetworkBehaviour
{
    public List<Unit> SelectedUnits { get; } = new();
    
    [SerializeField] private LayerMask _layerMask = new LayerMask();        // Default
    private Camera _mainCamera;

    [SerializeField] private RectTransform _unitsSelectionArea;
    private RTSPlayer _player;      // reference to the RTSPlayer 
    private Vector2 _startDragPos;
    
    
    private void Start()
    {
        _mainCamera = Camera.main;
        _player = NetworkClient.connection.identity.GetComponent<RTSPlayer>();
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            StartSelectionArea();
        }
        else if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            ClearSelectionArea();
        }
        else if (Mouse.current.leftButton.isPressed)
        {
            UpdateSelectionArea();
        }
    }

    private void StartSelectionArea()
    {
        // Remove previous selection & Start new selection area
        foreach (Unit selectedUnit in SelectedUnits)
        {
            selectedUnit.Deselect();
        }
        SelectedUnits.Clear();
        
        _unitsSelectionArea.gameObject.SetActive(true);
        _startDragPos = Mouse.current.position.ReadValue();
        UpdateSelectionArea();
    }
    private void UpdateSelectionArea()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();

        //float areaWidth = mousePos.x - _startDragPos.x;
        Vector2 areaSize = mousePos - _startDragPos;
        _unitsSelectionArea.sizeDelta = areaSize.Abs();
        _unitsSelectionArea.anchoredPosition = _startDragPos + areaSize / 2;
    }

    private void ClearSelectionArea()
    {
        _unitsSelectionArea.gameObject.SetActive(false);

        if (_unitsSelectionArea.sizeDelta.magnitude == 0)
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
            
            return;
        }
        
        // Check for units inside the unitsSelectionArea (pos > minPos(bottom left) & pos < maxPos(upper right)) 
        Vector2 minPos = _unitsSelectionArea.anchoredPosition - _unitsSelectionArea.sizeDelta / 2;
        Vector2 maxPos = _unitsSelectionArea.anchoredPosition + _unitsSelectionArea.sizeDelta / 2;
        foreach (var unit in _player.GetMyUnits())
        {
            Vector3 unitScreenPos = _mainCamera.WorldToScreenPoint(unit.transform.position);
            if (unitScreenPos.x > minPos.x && 
                unitScreenPos.y > minPos.y && 
                unitScreenPos.x < maxPos.x && 
                unitScreenPos.y < maxPos.y)
            {
                SelectedUnits.Add(unit);
                unit.Select();
            }
        }
    }
}
