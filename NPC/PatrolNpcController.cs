using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class PatrolNpcController : MonoBehaviour
{
    [Header("Detection Settings")]
    public float viewRange;
    public LayerMask playerMask;

    [Header("Idle Time Settings")]
    [Range(0f, 10f)] public float minIdleTime = 0f;
    [Range(0f, 10f)] public float maxIdleTime = 10f;

    private NavMeshAgent agent;
    private PatrolNpcInfo patrolNpcInfo;
    public PatrolNpcInfo PatrolNpcInfo => patrolNpcInfo;

    private Transform player;
    private float idleTime;
    private float elapsedTime;
    private BasePatrolNpc currentState;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        patrolNpcInfo = GetComponentInChildren<PatrolNpcInfo>();
        SetupHierarchy();
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        currentState?.OnUpdate(Time.deltaTime);
    }

    private void SetupHierarchy()
    {
        transform.SetParent(ObjectManager.PatrolNpc, true);
        transform.localScale = Vector3.one;
    }

    public void StartPatrol(List<Transform> patrolPoints)
    {
        List<Transform> shuffledWaypoints = GetShuffledWaypoints(patrolPoints);
        patrolNpcInfo.SetWayPointList(shuffledWaypoints);
        transform.position = patrolNpcInfo.GetFirstWayPointPosition();

        ChangeState<PatrolNpcMove>();
    }

    private List<Transform> GetShuffledWaypoints(List<Transform> originalPoints)
    {
        var shuffled = new List<Transform>();
        var usedIndices = new HashSet<int>();

        while (shuffled.Count < originalPoints.Count)
        {
            int index = Random.Range(0, originalPoints.Count);
            if (usedIndices.Add(index))
                shuffled.Add(originalPoints[index]);
        }

        return shuffled;
    }

    public T ChangeState<T>(bool isSearchPlayer = true) where T : BasePatrolNpc
    {
        if (currentState != null)
        {
            if (currentState.GetType() == typeof(T))
                return currentState as T;

            currentState.OnEnd();
        }

        DestroyComponent<BasePatrolNpc>();
        currentState = AddComponent<T>();
        currentState.SetIsSearchPlayer(isSearchPlayer);
        currentState.OnStart();
        elapsedTime = 0f;

        return currentState as T;
    }

    private void DestroyComponent<T>() where T : Component
    {
        T component = GetComponent<T>();
        if (component != null)
            Destroy(component);
    }

    private T AddComponent<T>() where T : Component => gameObject.AddComponent<T>();

    public Transform SearchPlayer()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, viewRange, playerMask);
        player = hits.Length > 0 ? hits[0].transform : null;
        return player;
    }

    public void SetDestination()
    {
        Transform next = GetNextWayPoint();
        if (next != null)
            agent.SetDestination(next.position);
    }

    public void SetDestination(Vector3 position) => agent.SetDestination(position);

    public Transform GetNextWayPoint()
    {
        if (patrolNpcInfo.GetWayPointListCount() == 0)
            return null;

        patrolNpcInfo.SetTargetWayPoint(patrolNpcInfo.GetWayPointList()[patrolNpcInfo.GetWayPointIndex()]);
        patrolNpcInfo.SetWayPointIndex((patrolNpcInfo.GetWayPointIndex() + 1) % patrolNpcInfo.GetWayPointListCount());
        return patrolNpcInfo.GetTargetWayPoint();
    }

    public Quaternion GetLookRotationToPlayer()
    {
        if (player == null) return transform.rotation;
        Vector3 direction = player.position - transform.position;
        direction.y = 0f;
        return Quaternion.LookRotation(direction.normalized);
    }

    public void Idle()
    {
        patrolNpcInfo.PlayFloatAnimation(SalinConstants.AnimationType.Move.ToString(), 0f);
        agent.isStopped = true;
    }

    public void Move()
    {
        agent.isStopped = false;
        patrolNpcInfo.PlayFloatAnimation(SalinConstants.AnimationType.Move.ToString(), 0.75f);
    }

    public bool IsContactWithPlayer()
    {
        if (player == null) return false;
        return Vector3.Distance(transform.position, player.position) <= viewRange;
    }

    public bool HasArrived()
    {
        return !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance;
    }

    public bool HasFinishedIdle() => elapsedTime > idleTime;

    public void SetRandomIdleTime()
    {
        idleTime = Random.Range(minIdleTime, maxIdleTime);
    }

    public void ResetPath() => agent.ResetPath();

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, viewRange);
    }
}
