using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using VTLTools;

namespace DucDevGame
{
    public class DragManager : MonoBehaviour
    {
        [SerializeField] private LayerMask draggableLayer;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private HexGridView gridView;
        [SerializeField] private BenchGridView benchGridView;
        [SerializeField] private GridZoneIdentifier gridZoneIdentifier;

        private IDraggable currentDragged;
        private Camera mainCamera;
        private Vector3 _dragOffset;
        private HexCellView currentHoverHexCell;
        private BenchCellView currentHoverBenchCell;

        public HexGridView GridView => gridView;

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
        [ShowInInspector]
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
                currentDragged.OnDragUpdate(targetPos);
                GridZoneType zoneType = gridZoneIdentifier.GetZoneType(worldPos);
                switch (zoneType)
                {
                    case GridZoneType.None:
                        //Debug.Log("Dragging over: None");
                        break;
                    case GridZoneType.Hex:
                        if (currentHoverBenchCell != null)
                        {
                            currentHoverBenchCell.ActivateHighlight(false);
                            currentHoverBenchCell = null;
                        }
                        if (gridView.TryGetCube(worldPos, out Vector3Int cube))
                        {
                            HexCellView view = gridView.GetHexCellView(cube);
                            if (view != null)
                            {
                                if (currentHoverHexCell != null && currentHoverHexCell == view) return;
                                if (currentHoverHexCell == null) currentHoverHexCell = view;
                                currentHoverHexCell.ActivateHighlight(false);
                                currentHoverHexCell = view;
                                view.ActivateHighlight(true);
                            }

                        }
                        else
                        {
                            if (currentHoverHexCell != null)
                            {
                                currentHoverHexCell.ActivateHighlight(false);
                            }
                        }
                        break;
                    case GridZoneType.Bench:
                        if (currentHoverHexCell != null)
                        {
                            currentHoverHexCell.ActivateHighlight(false);
                            currentHoverHexCell = null;
                        }
                        if (benchGridView.TryGetCell(worldPos, out Vector2Int benchCell))
                        {
                            BenchCellView view = benchGridView.GetCellView(benchCell);
                            if (view != null)
                            {
                                if (currentHoverBenchCell != null && currentHoverBenchCell == view) return;
                                if (currentHoverBenchCell == null) currentHoverBenchCell = view;
                                currentHoverBenchCell.ActivateHighlight(false);
                                currentHoverBenchCell = view;
                                view.ActivateHighlight(true);
                            }

                        }
                        else
                        {
                            if (currentHoverBenchCell != null)
                            {
                                currentHoverBenchCell.ActivateHighlight(false);
                            }
                        }
                        break;
                }

            }
        }

        private void StopDrag()
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            Vector3 finalPos = currentDragged.GetTransform().position;
            if (currentHoverHexCell != null)
            {
                finalPos = currentHoverHexCell.transform.position;
                currentHoverHexCell.ActivateHighlight(false);
                currentHoverHexCell = null;
            }

            if (currentHoverBenchCell != null)
            {
                finalPos = currentHoverBenchCell.transform.position;
                currentHoverBenchCell.ActivateHighlight(false);
                currentHoverBenchCell = null;
            }
            //DebugUtils.DrawWireSphere(finalPos, 0.5f, Color.red, 2f);
            currentDragged.OnDrop(finalPos);
            currentDragged = null;
        }
    }
}