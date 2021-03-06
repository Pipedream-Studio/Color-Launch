﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    //offset from the viewport center to fix damping
    public float m_DampTime = 10f;
    private Transform m_Target;
    public float m_XOffset = 0;
    public float m_YOffset = 0;

    private float margin = 0.1f;

    private float maxHeight = 0f;

    private GameController gameController;

    void Awake()
    {
        if (m_Target == null)
        {
            m_Target = GameObject.FindGameObjectWithTag("Player").transform;
            gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        }
    }

    void FixedUpdate()
    {
        if (gameController.currentCourse == null)
            return;

        if (m_Target)
        {   
            float targetY = m_Target.position.y + m_YOffset;

            if (maxHeight >= gameController.currentCourse.transform.position.y)
                return;

            if (Mathf.Abs(transform.position.y - targetY) > margin)
            targetY = Mathf.Lerp(transform.position.y, targetY, m_DampTime * Time.deltaTime);

            if (targetY >= maxHeight)
            {
                transform.position = new Vector3(transform.position.x, targetY, transform.position.z);
                maxHeight = targetY;
            }
        }
    }
}
