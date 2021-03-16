using UnityEngine;

public class EnemyAnimation
{
    private Animator animator;

    public EnemyAnimation(Animator animator)
    {
        this.animator = animator;
    }

    public void PlayDeadAnimation()
    {
        animator.SetTrigger("Dead");
    }

    public void PlayFlinchAnimation()
    {
        animator.SetTrigger("Flinch");
    }
}
