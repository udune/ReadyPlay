using System;
using System.Collections.Generic;
using UnityEngine;

public class BasePatrolNpc : MonoBehaviour, IPatrolNpc
{
    protected PatrolNpcController controller;
    protected bool isSearchPlayer;

    private void Awake()
    {
        controller = GetComponent<PatrolNpcController>();
    }

    public void SetIsSearchPlayer(bool isSearchPlayer)
    {
        this.isSearchPlayer = isSearchPlayer;
    }

    public virtual void OnStart() { }
    public virtual void OnEnd() { }
    public virtual void OnUpdate(float deltaTime) { }
}
