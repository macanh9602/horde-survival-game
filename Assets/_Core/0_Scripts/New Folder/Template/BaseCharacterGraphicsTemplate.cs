using UnityEngine;

namespace Watermelon.SquadShooter.Template
{
    public abstract class BaseCharacterGraphicsTemplate : MonoBehaviour
    {
        [SerializeField] protected Animator characterAnimator;

        public virtual void Initialise(CharacterBehaviourTemplate owner)
        {
        }

        public abstract void OnMoving(float speedPercent, Vector3 direction, bool hasTarget);
        public abstract void Reload();
        public abstract void Activate();
        public abstract void Disable();
    }
}
