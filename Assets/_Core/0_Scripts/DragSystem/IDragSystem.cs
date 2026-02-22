using UnityEngine;

namespace DucDevGame
{
    public interface IDraggable
    {
        void OnDragStart();
        void OnDragUpdate(Vector3 worldPos);
        void OnDrop(Vector3 finalPos);
        void ResetPosition();
        Transform GetTransform();
    }
}
