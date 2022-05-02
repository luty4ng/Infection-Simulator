using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using GameKit;
using Fsm;


public class AgentCrazy : IState
{
    public AgentAI controller;
    private AgentData agentData;
    private System.Random random = new System.Random(1000);
    private const float stayTime = 30f;
    private float countTime = 0f;
    private float pauseTime;
    private float pauseCountTime;

    public AgentCrazy(AgentAI agentAI)
    {
        this.controller = agentAI;
        this.agentData = controller.agentData;
        this.pauseTime = Random.Range(1.5f, 3f);
    }
    public void Update()
    {
        countTime += Time.deltaTime;
        if (countTime >= stayTime)
        {
            controller.isRoaming = true;
        }

        pauseCountTime += Time.deltaTime;
        if (pauseCountTime >= pauseTime)
        {
            pauseCountTime = 0;
            Vector2 targetPos = (Vector2)controller.transform.position + GetCirclePoint(controller.roamingRadius * 3);
            PathFindingManager.current.RequestPath(controller.transform.position, targetPos, controller.OnPathFound);
        }
    }
    public void OnEnter()
    {

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
        endPoint = PathFindingManager.current.grid.Clamp(endPoint);
        return endPoint;
    }
    private double GetRandomValue(double min, double max)
    {
        double v = random.NextDouble() * (max - min) + min;
        return v;
    }
}

