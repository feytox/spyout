using UnityEngine;

public class NpcAnimController : CharacterAnimController
{
    private static readonly int IsDead = Animator.StringToHash("isDead");

    public override void OnDeath()
    {
        Animator.SetBool(IsDead, true);
        base.OnDeath();
    }
}