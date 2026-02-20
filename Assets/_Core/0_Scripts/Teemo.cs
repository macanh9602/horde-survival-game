using System.Collections.Generic;
using System.Linq;
using BehaviorDesigner.Runtime;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DucDevGame
{
    public class Teemo : TacticianBase
    {
        [SerializeField] private BehaviorTree behaviorTree;
        [SerializeField] private Animator animator;
        [SerializeField] private InputHandle inputHandle;
        [SerializeField, ValueDropdown(nameof(GetAnimationClip))] AnimationClip walkClip;
        [SerializeField, ValueDropdown(nameof(GetAnimationClip))] AnimationClip idleClip;
        private const string TargetPositionKey = "TargetPosition";
        private const string IsMovingKey = "IsMoving";
        public bool isMoving = false;
        public Vector3 GetTargetPosition()
        {
            return inputHandle.cachedMouseWorldPos;
        }

        public override void MoveTo(Vector3 worldPos)
        {
            behaviorTree.SetVariableValue(TargetPositionKey, worldPos);
            behaviorTree.SetVariableValue(IsMovingKey, isMoving);
            isMoving = !isMoving;
            //Debug.Log($"<color=green>[DA]</color> ping {behaviorTree.GetVariable(IsMovingKey)} -> {behaviorTree.GetVariable(TargetPositionKey)}");
        }

        public void Walk()
        {
            animator.Play(walkClip.name);
        }

        public void Idle()
        {
            animator.Play(idleClip.name);
            behaviorTree.SetVariableValue(IsMovingKey, true);
        }

        public IEnumerable<AnimationClip> GetAnimationClip()
        {
            if (animator == null || animator.runtimeAnimatorController == null)
                return new List<AnimationClip>();

            return animator.runtimeAnimatorController.animationClips
                .Select(clip => clip)
                .Distinct()
                .ToList();
        }
        public void SetSpeed(float _speed)
        {
            animator.speed = _speed;
        }
    }
}