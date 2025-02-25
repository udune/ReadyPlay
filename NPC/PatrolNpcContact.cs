using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PatrolNpcContact : BasePatrolNpc
{
    public override void OnStart()
    {
        transform.DORotateQuaternion(controller.LookPlayer(), 0.3f);
        controller.Idle();
        controller.SetIdleTime();
    }

    public override void OnUpdate(float deltaTime)
    {
        if (controller.IsNextMove())
            controller.ChangeState<PatrolNpcMove>(false);
    }

    public override void OnEnd()
    {
        
    }
}
