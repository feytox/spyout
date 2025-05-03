using UnityEngine;

public class NPCAnimController : CharacterAnimController
{
    private static readonly int IsDead = Animator.StringToHash("isDead");
    
    public void OnDeath() => Animator.SetBool(IsDead, true);
}