using UnityEngine;

namespace DucDevGame
{
    public abstract class TacticianBase : MonoBehaviour, IMovement
    {
        private bool hasMoveRequest;
        private Vector3 moveRequestPosition;

        public virtual void MoveTo(Vector3 worldPos)
        {
            hasMoveRequest = true;
            moveRequestPosition = worldPos;
        }

        public bool TryConsumeMoveRequest(out Vector3 worldPos)
        {
            if (hasMoveRequest)
            {
                worldPos = moveRequestPosition;
                hasMoveRequest = false;
                return true;
            }

            worldPos = Vector3.zero;
            return false;
        }
    }
}