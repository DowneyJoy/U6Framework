using Downey.AdvancedController;
using UnityEngine;

namespace Downey.Platformer
{
    public class JumpState : BaseState
    {
        public JumpState(PlayerController player, Animator animator) : base(player, animator) { }

        public override void OnEnter()
        {
            animator.CrossFade(JumpHash,crossFadeDuration);
        }

        public override void FixedUpdate()
        {
            //base.FixedUpdate();
            //player
        }
    }
}