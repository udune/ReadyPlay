using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolNpcInfo : MonoBehaviour
{
    private Animator avatarAni;
    private List<Transform> wayPointList;
    private Transform targetWayPoint;
    private int wayPointIdx;

    private void Awake()
    {
        avatarAni = GetComponent<Animator>();
    }

    public Transform GetTargetWayPoint()
    {
        return targetWayPoint;
    }

    public void SetTargetWayPoint(Transform targetWayPoint)
    {
        this.targetWayPoint = targetWayPoint;
    }
    
    public int GetWayPointIndex()
    {
        return wayPointIdx;
    }

    public void SetWayPointIndex(int wayPointIdx)
    {
        this.wayPointIdx = wayPointIdx;
    }
    
    public List<Transform> GetWayPointList()
    {
        return wayPointList;
    }

    public int GetWayPointListCount()
    {
        return wayPointList.Count;
    }

    public Vector3 GetFirstWayPointPosition()
    {
        return wayPointList[0].position;
    }

    public void SetWayPointList(List<Transform> wayPointList)
    {
        this.wayPointList = wayPointList;
    }
    
    public void PlayFloatAnimation(string clipName, float move)
    {
        avatarAni.SetFloat(clipName, move);
    }
}