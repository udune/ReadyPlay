using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolNpcMove : BasePatrolNpc
{
    public override void OnStart()
    {
        if (controller.PatrolNpcInfo.GetTargetWayPoint() == null)
            controller.FindWayPoint();

        if (controller.PatrolNpcInfo.GetTargetWayPoint())
        {
            controller.SetDestination(controller.PatrolNpcInfo.GetTargetWayPoint().position);
            controller.Move();
        }
        
        Invoke(nameof(ResetIsSearchPlayer), 3.0f);
    }

    public override void OnUpdate(float deltaTime)
    {
        Transform player = isSearchPlayer ? controller.SearchPlayer() : null;
        if (player != null)
        {
            if (controller.IsContact())
            {
                controller.ChangeState<PatrolNpcContact>();
            }
            else
            {
                
            }
        }
        else
        {
            if (controller.IsArrive())
            {
                controller.SetDestination();
                controller.ChangeState<PatrolNpcIdle>();
            }
            else
            {
                controller.Move();
            }
        }
    }

    public void ResetIsSearchPlayer()
    {
        isSearchPlayer = true;
    }

    public override void OnEnd()
    {
        controller.Idle();
        controller.ResetPath();
    }
}
