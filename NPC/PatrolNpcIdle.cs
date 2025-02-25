using UnityEngine;

public class PatrolNpcIdle : BasePatrolNpc
{
    public override void OnStart()
    {
        controller.Idle();
        controller.SetIdleTime();
    }

    public override void OnUpdate(float deltaTime)
    {
        Transform player = controller.SearchPlayer();
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
        else if (controller.IsNextMove())
        {
            controller.ChangeState<PatrolNpcMove>();
        }
    }

    public override void OnEnd()
    {
        
    }
}
