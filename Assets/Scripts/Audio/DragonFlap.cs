using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonFlap : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SoundManager.Play(SFXName.DragonFlap);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SoundManager.Stop(SFXName.DragonFlap);
    }
}
