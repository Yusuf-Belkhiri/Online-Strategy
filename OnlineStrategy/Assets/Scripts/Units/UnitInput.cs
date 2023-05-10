using System;
using UnityEngine;
using UnityEngine.InputSystem;

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

        TryMove(hit.point);
    }

    private void TryMove(Vector3 point)
    {
        throw new NotImplementedException();
    }
}
