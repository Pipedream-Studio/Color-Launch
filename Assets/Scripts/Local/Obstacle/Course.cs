using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Course : MonoBehaviour
{
    private void OnEnable()
    {
        Obstacle[] obstacles = GetComponentsInChildren<Obstacle>();
        foreach (Obstacle obstacle in obstacles)
        {
            obstacle.gameObject.SetActive(true);
        }
    }

    private void OnDisable()
    {
        Obstacle[] obstacles = GetComponentsInChildren<Obstacle>();
        foreach(Obstacle obstacle in obstacles)
        {
            obstacle.gameObject.SetActive(false);
        }
    }
}
