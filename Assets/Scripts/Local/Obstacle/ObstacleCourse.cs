using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleCourse : MonoBehaviour
{
    private List<Course> courses_Beginner = new List<Course>();
    private List<Course> courses_Intermediate = new List<Course>();
    private List<Course> courses_Expert = new List<Course>();
    private List<Course> courses_Hellish = new List<Course>();

    private Course[] obstacleCourses;
    private GameController gameController;
    private ObstacleSpawner obstacleSpawner;
    private BoxCollider2D boxCollider;
    
    private void Awake()
    {
        //Assignation
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        obstacleSpawner = FindObjectOfType<ObstacleSpawner>();
        boxCollider = GetComponent<BoxCollider2D>();

        //Store all courses
        obstacleCourses = GetComponentsInChildren<Course>();

        foreach(Course course in obstacleCourses)
        {
            if(course.name.Contains("Beginner"))
            {
                courses_Beginner.Add(course);
            }

            else if (course.name.Contains("Intermediate"))
            {
                courses_Intermediate.Add(course);
            }

            else if (course.name.Contains("Expert"))
            {
                courses_Expert.Add(course);
            }

            else if (course.name.Contains("Hellish"))
            {
                courses_Hellish.Add(course);
            }
        }

        //Set Collider size
        boxCollider.size = gameController.ScreenSize;
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

    void RandomizeCourse(List<Course> courses)
    {
        StartCoroutine(_RandomizeCourse(courses));
    }

    IEnumerator _RandomizeCourse(List<Course> courses)
    {
        yield return null;

        //Checker
        if (courses.Count == 0)
        {
            Debug.Log("Not enough courses");
            yield break;
        }

        int randInt = Random.Range(0, courses.Count);

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
