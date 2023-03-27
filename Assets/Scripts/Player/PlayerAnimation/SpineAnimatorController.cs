using Spine;
using Spine.Unity;
using UnityEngine;

namespace Player.PlayerAnimation
{
    [RequireComponent(typeof(SkeletonAnimation))]
    public class SpineAnimatorController : AnimatorController
    {
        [SpineAnimation, SerializeField] private string _idleAnimationKey;
        [SpineAnimation, SerializeField] private string _walkAnimationKey;
        [SpineAnimation, SerializeField] private string _runAnimationKey;
        [SpineAnimation, SerializeField] private string _jumpAnimationKey;
        [SpineAnimation, SerializeField] private string _attackAnimationKey;
        
        private SkeletonAnimation _skeletonAnimation;
        private void Start() => _skeletonAnimation = GetComponent<SkeletonAnimation>();
        
        protected override void PlayAnimation(AnimationType animationType)
        {
            string animationName = GetAnimationName(animationType);

            if(_skeletonAnimation.AnimationState.GetCurrent(0).Animation.Name == animationName)
                return;
            
            TrackEntry te = _skeletonAnimation.AnimationState.SetAnimation(0, animationName, true);
            te.Complete += (tE) => 
            {
                OnActionRequested();
                OnAnimationEnded();
            };

        }

        private string GetAnimationName(AnimationType animationType)
        {
            return animationType switch
            {
                AnimationType.idle => _idleAnimationKey,
                AnimationType.walk => _walkAnimationKey,
                AnimationType.run => _runAnimationKey,
                AnimationType.Jump => _jumpAnimationKey,
                AnimationType.attack => _attackAnimationKey,
                _ => _idleAnimationKey
            };
        }
    }
}