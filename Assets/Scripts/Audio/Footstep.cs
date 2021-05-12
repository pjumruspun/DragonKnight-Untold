using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footstep : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SoundManager.Play(SFXName.Footstep);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SoundManager.Stop(SFXName.Footstep);
    }
}
