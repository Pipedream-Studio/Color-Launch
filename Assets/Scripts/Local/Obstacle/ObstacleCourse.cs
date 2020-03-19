using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleCourse : MonoBehaviour
{
    [SerializeField] private Course[] courses_Beginner;
    [SerializeField] private Course[] courses_Intermediate;
    [SerializeField] private Course[] courses_Expert;
    [SerializeField] private Course[] courses_Hellish;

    private Course[] obstacleCourses;
    private GameController gameController;
    private ObstacleSpawner obstacleSpawner;
    private BoxCollider2D boxCollider;
    private Vector3 screenSize;
    
    private void Awake()
    {
        //Assignation
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        obstacleSpawner = FindObjectOfType<ObstacleSpawner>();
        boxCollider = GetComponent<BoxCollider2D>();

        //Store all courses
        obstacleCourses = GetComponentsInChildren<Course>();

        //Calculate screen size
        screenSize = new Vector3((Camera.main.orthographicSize * Screen.width / Screen.height) * 2, Camera.main.orthographicSize * 2, 1);

        //Set Collider size
        boxCollider.size = screenSize;
    }

    private void OnEnable()
    {
        //Add this into [GameController's] obstacle course list
        gameController.obstacleCourses.Add(this);

        //Disable all courses
        foreach (Course course in obstacleCourses)
        {
            course.gameObject.SetActive(false);
        }

        //Randomize a course based on enum
        System.Random rand = new System.Random();

        //Randomize obstacle course based on current difficulty
        switch(gameController._CourseDifficulty)
        {
            case CourseDifficulty.Intermediate:
                RandomizeCourse(courses_Intermediate);
                break;

            case CourseDifficulty.Expert:
                RandomizeCourse(courses_Expert);
                break;

            case CourseDifficulty.Hellish:
                RandomizeCourse(courses_Hellish);
                break;

            default:
                RandomizeCourse(courses_Beginner);
                break;
        }
    }

    void RandomizeCourse(Course[] courses)
    {
        StartCoroutine(_RandomizeCourse(courses));
    }

    IEnumerator _RandomizeCourse(Course[] courses)
    {
        yield return null;

        //Checker
        if (courses.Length == 0)
        {
            Debug.Log("Not enough courses");
            yield break;
        }

        int randInt = Random.Range(0, courses.Length);

        //Enable randomized course 
        courses[randInt].gameObject.SetActive(true);
    }

    public void DisableCourse()
    {
        gameController.obstacleCourses.Remove(this);
        ObjectPoolingManager.Instance.DespawnPrefabObject(gameObject.GetComponent<PrefabObject>(), name);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //If this obstacle course is the newest course, disable oldest course
        if(other.tag == "Player" && this == gameController.obstacleCourses[gameController.obstacleCourses.Count - 1])
        {
            obstacleSpawner.SpawnObstacles();
            gameController.obstacleCourses[0].DisableCourse();
        }
    }
}
