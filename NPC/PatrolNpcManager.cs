using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolNpcManager : MonoBehaviour
{
    [SerializeField] private Transform patrolPoints;
    [SerializeField] private GameObject[] patrolNpcAvatars;
    private List<Transform> patrolPointList;

    private void Start()
    {
        CreatePatrolNpc();
    }

    private void CreatePatrolNpc()
    {
        if (patrolPoints != null && patrolPoints.childCount > 0)
        {
            patrolPointList = new List<Transform>();
            for (int patrolPointIdx = 0; patrolPointIdx < patrolPoints.childCount; patrolPointIdx++)
                patrolPointList.Add(patrolPoints.GetChild(patrolPointIdx));
        }

        if (patrolNpcAvatars.Length > 0 && patrolPointList.Count > 0)
        {
            foreach (var npc in patrolNpcAvatars)
            {
                PatrolNpcController controller = npc.GetComponent<PatrolNpcController>();
                controller.StartPatrol(patrolPointList);
            }
        }
    }

    private void OnDestroy()
    {
        GameObject patrolNpcGo = GameObject.Find("PatrolNpc");
        if (patrolNpcGo != null)
            Destroy(patrolNpcGo.gameObject);
    }
}