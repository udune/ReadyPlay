using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolNpcMove : BasePatrolNpc
{
    public override void OnStart()
    {
        SetupInitialWaypoint();
        TryMoveToTargetWaypoint();
        Invoke(nameof(EnablePlayerSearch), 3.0f);
    }

    public override void OnUpdate(float deltaTime)
    {
        if (isSearchPlayer && TrySearchPlayer(out Transform player))
        {
            HandlePlayerDetection();
        }
        else
        {
            HandlePatrolMovement();
        }
    }

    public override void OnEnd()
    {
        controller.Idle();
        controller.ResetPath();
    }

    private void SetupInitialWaypoint()
    {
        if (controller.PatrolNpcInfo.GetTargetWayPoint() == null)
        {
            controller.FindWayPoint();
        }
    }

    private void TryMoveToTargetWaypoint()
    {
        Transform waypoint = controller.PatrolNpcInfo.GetTargetWayPoint();
        if (waypoint != null)
        {
            controller.SetDestination(waypoint.position);
            controller.Move();
        }
    }

    private bool TrySearchPlayer(out Transform player)
    {
        player = controller.SearchPlayer();
        return player != null;
    }

    private void HandlePlayerDetection()
    {
        if (controller.IsContact())
        {
            controller.ChangeState<PatrolNpcContact>();
        }
        // else 부분이 비어 있으므로 삭제
    }

    private void HandlePatrolMovement()
    {
        if (controller.IsArrive())
        {
            controller.SetDestination(); // 다음 목적지 설정
            controller.ChangeState<PatrolNpcIdle>();
        }
        else
        {
            controller.Move();
        }
    }

    private void EnablePlayerSearch()
    {
        isSearchPlayer = true;
    }
}
