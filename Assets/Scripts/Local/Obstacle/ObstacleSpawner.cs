using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private int initialAmountToSpawn;
    [SerializeField] private float spawnHeightOffset;

    private Vector2 nextSpawnPos;

    private GameController gameController;
    private ObstaclePattern _ObstaclePattern;
    private System.Random rand = new System.Random();

    private void Awake()
    {
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
    }

    private void Start()
    {
        for (int i = 0; i < initialAmountToSpawn; i++)
        {
            SpawnObstacles();
        }
    }

    public void SpawnObstacles()
    {
        _ObstaclePattern = GameUtility.RandomEnumValue<ObstaclePattern>(rand);

        if (gameController.obstacleCourses.Count > 0)
        {
            GameObject latestObstacleCourse = gameController.obstacleCourses[gameController.obstacleCourses.Count - 1].gameObject;
            Vector3 size = latestObstacleCourse.GetComponent<BoxCollider2D>().bounds.size;
            nextSpawnPos = latestObstacleCourse.transform.position + new Vector3(0, size.y + spawnHeightOffset);
        }

        switch (_ObstaclePattern)
        {
            case ObstaclePattern.Circle:
                SpawnCircleObstacle();
                break;

            case ObstaclePattern.Square:
                SpawnSquareObstacle();
                break;

            default:
                SpawnWaveObstacle();
                break;
        }
    }

    void SpawnCircleObstacle()
    {
         ObjectPoolingManager.Instance.SpawnPrefabObject("ObstacleCourse_Circle", nextSpawnPos);
    }

    void SpawnSquareObstacle()
    {
        ObjectPoolingManager.Instance.SpawnPrefabObject("ObstacleCourse_Square", nextSpawnPos);
    }

    void SpawnWaveObstacle()
    {
       ObjectPoolingManager.Instance.SpawnPrefabObject("ObstacleCourse_Wave", nextSpawnPos);
    }
}
