using UnityEngine;

namespace Watermelon.SquadShooter.Template
{
    public class CharacterGraphicsTemplate : BaseCharacterGraphicsTemplate
    {
        private static readonly int SpeedHash = Animator.StringToHash("Speed");

        public override void OnMoving(float speedPercent, Vector3 direction, bool hasTarget)
        {
            if (characterAnimator == null)
                return;

            characterAnimator.SetFloat(SpeedHash, speedPercent);
        }

        public override void Reload()
        {
        }

        public override void Activate()
        {
        }

        public override void Disable()
        {
        }
    }
}
