using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using GameKit;
using Fsm;


public class AgentRoaming : IState
{
    public AgentAI controller;
    private AgentData agentData;
    private System.Random random = new System.Random(1000);
    private float pauseTime;
    public float desideTime;
    private float pauseCountTime;
    private float desideCountTime;
    public AgentRoaming(AgentAI agentAI)
    {
        this.controller = agentAI;
        this.agentData = controller.agentData;
        this.pauseTime = Random.Range(0.5f, 2f);
        this.desideTime = Random.Range(4f, 6f);
        this.pauseCountTime = 0;
    }
    public void Update()
    {
        pauseCountTime += Time.deltaTime;
        desideCountTime += Time.deltaTime;
        if (pauseCountTime >= pauseTime)
        {
            pauseCountTime = 0;
            Vector2 targetPos = (Vector2)controller.transform.position + GetCirclePoint(controller.roamingRadius);
            PathFindingManager.current.RequestPath(controller.transform.position, targetPos, controller.OnPathFound);
        }

        if (desideCountTime >= desideTime)
        {
            desideCountTime = 0;
            controller.isDeside = true;
        }
    }
    public void OnEnter()
    {
        controller.targetTrans = null;
    }
    public void OnExit()
    {

    }

    private Vector2 GetCirclePoint(float m_Radius)
    {
        float radin = (float)GetRandomValue(0, 2 * Mathf.PI);
        float x = m_Radius * Mathf.Cos(radin);
        float y = m_Radius * Mathf.Sin(radin);
        Vector2 endPoint = new Vector2(x, y);
        return endPoint;
    }
    private double GetRandomValue(double min, double max)
    {
        double v = random.NextDouble() * (max - min) + min;
        return v;
    }
}

