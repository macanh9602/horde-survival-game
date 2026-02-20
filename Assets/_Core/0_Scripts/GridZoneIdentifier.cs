using System.Collections.Generic;
using UnityEngine;
using VTLTools;
namespace DucDevGame
{

    public class GridZoneIdentifier : MonoBehaviour
    {
        public List<GridZoneConfig> gridZones = new List<GridZoneConfig>();

        public GridZoneType GetZoneType(Vector3 worldPos)
        {
            worldPos.y = 0;
            foreach (var zone in gridZones)
            {
                if (IsPointInZone(worldPos, zone))
                {
                    return zone.zoneType;
                }
            }
            return GridZoneType.None;
        }

        private bool IsPointInZone(Vector3 point, GridZoneConfig zone)
        {
            Vector3 halfSize = zone.size * 0.5f;
            Vector3 min = zone.center - halfSize;
            Vector3 max = zone.center + halfSize;

            return (point.x >= min.x && point.x <= max.x) &&
                   (point.y >= min.y && point.y <= max.y) &&
                   (point.z >= min.z && point.z <= max.z);
        }

        private void OnDrawGizmos()
        {
            if (gridZones == null) return;

            foreach (var zone in gridZones)
            {
                Gizmos.color = zone.gizmosColor;
                Gizmos.DrawWireCube(zone.center, zone.size);
            }
        }

        public void Update()
        {
            //test
            // if (Input.GetMouseButtonDown(0))
            // {
            //     Vector3 mousePos = Input.mousePosition;
            //     Ray ray = Camera.main.ScreenPointToRay(mousePos);
            //     if (Physics.Raycast(ray, out RaycastHit hit))
            //     {
            //         DebugUtils.DrawWireSphere(hit.point, 0.2f, Color.blue, 2f);
            //         GridZoneType zoneType = GetZoneType(hit.point);
            //         Debug.Log($"Clicked on zone: {zoneType}");
            //     }
            // }
        }
    }

    public enum GridZoneType
    {
        None,
        Hex,
        Bench
    }
    [System.Serializable]
    public class GridZoneConfig
    {
        public GridZoneType zoneType;
        public Vector3 center;
        public Vector3 size;
        public Color gizmosColor = Color.white;
    }
}