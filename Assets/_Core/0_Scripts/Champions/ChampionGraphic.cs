using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
namespace DucDevGame
{
    public class ChampionGraphic : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField, ValueDropdown(nameof(GetAnimationClip))] AnimationClip walkClip;
        [SerializeField, ValueDropdown(nameof(GetAnimationClip))] AnimationClip idleClip;
        [SerializeField, ValueDropdown(nameof(GetAnimationClip))] AnimationClip normalAttackClip;
        [SerializeField, ValueDropdown(nameof(GetAnimationClip))] AnimationClip castSkillClip;
        [SerializeField, ValueDropdown(nameof(GetAnimationClip))] AnimationClip deathClip;
        [SerializeField, ValueDropdown(nameof(GetAnimationClip))] AnimationClip danceClip;

        public IEnumerable<AnimationClip> GetAnimationClip()
        {
            if (animator == null || animator.runtimeAnimatorController == null)
                return new List<AnimationClip>();

            return animator.runtimeAnimatorController.animationClips
                .Select(clip => clip)
                .Distinct()
                .ToList();
        }

        public void PlayActionAnimation(ChampionAction action)
        {
            switch (action)
            {
                case ChampionAction.Idle:
                    PlayIdleAnimation();
                    break;
                case ChampionAction.Walk:
                    PlayWalkAnimation();
                    break;
                case ChampionAction.BasicAttack:
                    PlayBasicAttackAnimation();
                    break;
                case ChampionAction.CastSkill:
                    PlaySkillAnimation();
                    break;
                case ChampionAction.Death:
                    PlayDeathAnimation();
                    break;
                case ChampionAction.Dance:
                    PlayDanceAnimation();
                    break;
            }
        }

        public void PlayWalkAnimation()
        {
            animator.Play(walkClip.name);
        }

        public void PlayIdleAnimation()
        {
            animator.Play(idleClip.name);
        }

        public void PlayBasicAttackAnimation()
        {
            animator.Play(normalAttackClip.name);
        }

        public void PlaySkillAnimation()
        {
            animator.Play(castSkillClip.name);
        }

        public void PlayDeathAnimation()
        {
            animator.Play(deathClip.name);
        }

        public void PlayDanceAnimation()
        {
            animator.Play(danceClip.name);
        }


    }
}