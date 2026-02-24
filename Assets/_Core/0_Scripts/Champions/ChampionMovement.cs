using System.Collections.Generic;
using UnityEngine;

namespace DucDevGame
{
    /// <summary>
    /// Handles champion movement along hex path
    /// Decoupled from behavior - only handles pathfinding -> movement animation
    /// </summary>
    public class ChampionMovement : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 5f;
        private List<Vector3> pathWorldPositions = new();
        private int pathIndex = 0;
        private bool isMoving = false;
        private BaseChampionBehavior champion;
        private Transform modelRoot;

        public bool IsMoving => isMoving;
        public float MoveSpeed
        {
            get => moveSpeed;
            set => moveSpeed = value;
        }

        private void Awake()
        {
            champion = GetComponent<BaseChampionBehavior>();
        }

        /// <summary>
        /// Start moving along path with world positions
        /// pathWorldPositions: list of target world positions to visit in order
        /// </summary>
        public void MoveTo(List<Vector3> pathWorldPositions)
        {
            if (pathWorldPositions == null || pathWorldPositions.Count == 0)
                return;

            this.pathWorldPositions = new List<Vector3>(pathWorldPositions);
            pathIndex = 0;
            isMoving = true;

            if (champion != null)
                champion.RequestAction(ChampionAction.Walk);
        }

        /// <summary>
        /// Alternative: move along hex cell coordinates, requires worldPositionMap
        /// </summary>
        public void MoveAlongPath(List<Vector3Int> hexPath)
        {
            if (hexPath == null || hexPath.Count == 0)
                return;

            List<Vector3> worldPath = new List<Vector3>();
            foreach (var hexCoord in hexPath)
            {
                worldPath.Add(Board.Instance.GridView.CubeToWorldInternal(hexCoord.x, hexCoord.y, hexCoord.z));
            }

            MoveTo(worldPath);
        }

        private void Update()
        {
            if (!isMoving || pathWorldPositions.Count == 0)
                return;

            // Get model root từ behavior's graphic instance
            if (modelRoot == null && champion?.CurrentGraphic != null)
                modelRoot = champion.CurrentGraphic.transform;

            if (modelRoot == null)
                return;

            Vector3 targetWorldPos = pathWorldPositions[pathIndex];

            // Move towards target
            Vector3 direction = (targetWorldPos - modelRoot.position).normalized;
            modelRoot.position = Vector3.MoveTowards(modelRoot.position, targetWorldPos, moveSpeed * Time.deltaTime);

            // Rotate to face direction
            if (direction.magnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                modelRoot.rotation = Quaternion.Lerp(modelRoot.rotation, targetRotation, Time.deltaTime * 10f);
            }

            // Check if reached waypoint
            if (Vector3.Distance(modelRoot.position, targetWorldPos) < 0.1f)
            {
                modelRoot.position = targetWorldPos;
                pathIndex++;

                if (pathIndex >= pathWorldPositions.Count)
                {
                    isMoving = false;
                    pathWorldPositions.Clear();
                    if (champion != null)
                        champion.RequestAction(ChampionAction.Idle);
                }
            }
        }

        public void StopMovement()
        {
            isMoving = false;
            pathWorldPositions.Clear();
            pathIndex = 0;
            if (champion != null)
                champion.RequestAction(ChampionAction.Idle);
        }

        public void ResumeMovement()
        {
            if (pathWorldPositions.Count > 0 && !isMoving)
            {
                isMoving = true;
                if (champion != null)
                    champion.RequestAction(ChampionAction.Walk);
            }
        }

        //===================================================
        #region [Test]
        public void MoveTo(Vector3 start, Vector3 target)
        {

        }
        #endregion
        //===================================================
    }
}
