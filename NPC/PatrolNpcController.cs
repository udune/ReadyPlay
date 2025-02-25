using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class PatrolNpcController : MonoBehaviour
{
    public float viewRange;
    public LayerMask playerMask;
    [Range(0.0f, 10.0f)] public float minIdleTime = 0.0f;
    [Range(0.0f, 10.0f)] public float maxIdleTime = 10.0f;
    
    private NavMeshAgent agent;
    private PatrolNpcInfo patrolNpcInfo; public PatrolNpcInfo PatrolNpcInfo => patrolNpcInfo;
    private Transform player;
    private float idleTime;
    private float duration;
    private BasePatrolNpc curState;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        patrolNpcInfo = GetComponentInChildren<PatrolNpcInfo>();
        transform.SetParent(ObjectManager.PatrolNpc, true);
        transform.localScale = Vector3.one;
    }

    private void Update()
    {
        duration += Time.deltaTime;
        if (curState != null)
            curState.OnUpdate(Time.deltaTime);
    }

    public void StartPatrol(List<Transform> patrolPoints)
    {
        List<Transform> wayPointList = new List<Transform>();
        List<int> wayPointIdxList = new List<int>();
        int pointCount = patrolPoints.Count;
        for (var i = 0; i < pointCount; i++)
        {
            int patrolIdx = Random.Range(0, pointCount);
            if (wayPointIdxList.Contains(patrolIdx))
                continue;
            wayPointIdxList.Add(patrolIdx);
            if (wayPointIdxList.Count == pointCount)
                break;
        }
        foreach (var idx in wayPointIdxList)
            wayPointList.Add(patrolPoints[idx]);
        patrolNpcInfo.SetWayPointList(wayPointList);
        transform.position = patrolNpcInfo.GetFirstWayPointPosition();
        ChangeState<PatrolNpcMove>();
    }

    public T ChangeState<T>(bool isSearchPlayer = true) where T : BasePatrolNpc
    {
        if (curState != null)
        {
            if (curState.GetType() == typeof(T))
                return curState as T;
            
            curState.OnEnd();
        }
        
        DestroyState<BasePatrolNpc>();
        curState = AddState<T>();
        curState.SetIsSearchPlayer(isSearchPlayer);
        curState.OnStart();
        duration = 0.0f;

        return curState as T;
    }

    private void DestroyState<T>() where T : Component
    {
        var state = GetComponent<T>();
        if (state != null)
            Destroy(state);
    }
    
    private T AddState<T>() where T : Component
    {
        return gameObject.AddComponent<T>();
    }

    public Transform SearchPlayer()
    {
        player = null;
        Collider[] playerInView = Physics.OverlapSphere(transform.position, viewRange, playerMask);
        if (playerInView.Length > 0)
            player = playerInView[0].transform;
        return player;
    }

    public void SetDestination()
    {
        Transform destination = FindWayPoint();
        if (destination != null)
            agent.SetDestination(destination.position);
    }

    public void SetDestination(Vector3 position)
    {
        agent.SetDestination(position);
    }

    public Transform FindWayPoint()
    {
        if (patrolNpcInfo.GetWayPointListCount() > 0)
            patrolNpcInfo.SetTargetWayPoint(patrolNpcInfo.GetWayPointList()[patrolNpcInfo.GetWayPointIndex()]);
        patrolNpcInfo.SetWayPointIndex((patrolNpcInfo.GetWayPointIndex() + 1) % patrolNpcInfo.GetWayPointListCount());
        return patrolNpcInfo.GetTargetWayPoint();
    }

    public Quaternion LookPlayer()
    {
        var dir = player.position - transform.position; dir.y = 0.0f;
        return Quaternion.LookRotation(dir.normalized);
    }

    public void Idle()
    {
        patrolNpcInfo.PlayFloatAnimation(SalinConstants.AnimationType.Move.ToString(), 0.0f);
        agent.isStopped = true;
    }

    public void Move()
    {
        agent.isStopped = false;
        patrolNpcInfo.PlayFloatAnimation(SalinConstants.AnimationType.Move.ToString(), 0.75f);
    }

    public bool IsContact()
    {
        if (player == null)
            return false;

        float distance = Vector3.Distance(transform.position, player.position);
        return distance <= viewRange;
    }

    public bool IsArrive()
    {
        return !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance;
    }

    public bool IsNextMove()
    {
        return duration > idleTime;
    }

    public void SetIdleTime()
    {
        idleTime = Random.Range(minIdleTime, maxIdleTime);
    }

    public void ResetPath()
    {
        agent.ResetPath();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, viewRange);
    }
}
