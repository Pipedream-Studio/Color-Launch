using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Course : MonoBehaviour
{
    private Obstacle[] obstacles;

    private void Awake()
    {
        obstacles = GetComponentsInChildren<Obstacle>();
    }

    private void OnEnable()
    {
        foreach (Obstacle obstacle in obstacles)
        {
            obstacle.gameObject.SetActive(true);
        }
    }

    private void OnDisable()
    {
        foreach(Obstacle obstacle in obstacles)
        {
            obstacle.gameObject.SetActive(false);
        }
    }
}
