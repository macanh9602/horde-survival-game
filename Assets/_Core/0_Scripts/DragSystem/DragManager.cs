using System.Collections.Generic;
using UnityEngine;

namespace DucDevGame
{
    public class DragManager : MonoBehaviour
    {
        [SerializeField] private LayerMask draggableLayer;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private HexGridView gridView;

        private IDraggable currentDragged;
        private Camera mainCamera;
        private Vector3 _dragOffset;
        private CellView currentHoverCell;
        private void Awake()
        {
            mainCamera = Camera.main;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && !isDragging)
            {
                TryStartDrag();
            }

            if (isDragging && Input.GetMouseButton(0))
            {
                UpdateDrag();
            }

            if (isDragging && Input.GetMouseButtonUp(0))
            {
                StopDrag();
            }
        }

        private bool isDragging => currentDragged != null;

        private void TryStartDrag()
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, draggableLayer))
            {
                // Try to get IDraggable from the object or its parents
                var draggable = hit.collider.GetComponentInParent<IDraggable>();
                if (draggable != null)
                {
                    currentDragged = draggable;
                    currentDragged.OnDragStart();
                }
            }
        }

        private void UpdateDrag()
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundLayer))
            {
                Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.green);
                Vector3 worldPos = hit.point + _dragOffset;
                Vector3 targetPos = worldPos;
                //Debug.Log($"<color=green>[DA]</color> {allTargets.Count}");
                currentDragged.OnDragUpdate(targetPos);
                if (gridView.TryGetCube(worldPos, out Vector3Int cube))
                {
                    CellView view = gridView.GetCellView(cube);
                    if (view != null)
                    {
                        if (currentHoverCell != null && currentHoverCell == view) return;
                        if (currentHoverCell == null) currentHoverCell = view;
                        currentHoverCell.ActivateHighlight(false);
                        currentHoverCell = view;
                        view.ActivateHighlight(true);
                    }

                }
                else
                {
                    if (currentHoverCell != null)
                    {
                        currentHoverCell.ActivateHighlight(false);
                    }
                }
            }
        }

        private void StopDrag()
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            Vector3 finalPos = currentDragged.GetTransform().position;
            if (currentHoverCell != null)
            {
                finalPos = currentHoverCell.transform.position;
                currentHoverCell.ActivateHighlight(false);
                currentHoverCell = null;
            }

            currentDragged.OnDrop(finalPos);
            currentDragged = null;
        }
    }
}