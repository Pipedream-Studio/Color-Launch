using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI launchCountText;
    [SerializeField] private TextMeshProUGUI scoreText;

    [SerializeField] private int[] difficultyThresholds;

    private int currentScore = 0;
    private int scoreMultiplier;

    [HideInInspector] public Vector3 ScreenSize;
    [HideInInspector] public List<ObstacleCourse> obstacleCourses = new List<ObstacleCourse>();
    public CourseDifficulty _CourseDifficulty;

    private void Awake()
    {
        //Calculate screen size
        ScreenSize = new Vector3((Camera.main.orthographicSize * Screen.width / Screen.height) * 2, Camera.main.orthographicSize * 2, 1);
    }

    private void Start()
    {
        scoreMultiplier = 1;
    }

    public void UpdateLaunchNumberUI(int launchCount)
    {
        launchCountText.text = launchCount.ToString();
    }

    public void UpdateScore(int score)
    {
        currentScore += score * scoreMultiplier;
        scoreText.text = currentScore.ToString();

        UpdateDifficulty();
    }

    void UpdateDifficulty()
    {
        if (_CourseDifficulty == CourseDifficulty.Hellish)
            return;

        CourseDifficulty gameDifficulty = _CourseDifficulty;

        for(int i = 0; i < difficultyThresholds.Length; i++)
        {
            if(currentScore >= difficultyThresholds[i])
            {
                gameDifficulty = (CourseDifficulty)i;
            }
        }

        _CourseDifficulty = gameDifficulty;
    }

    public void UpdateScoreMultiplier(int multiplier, float duration = Mathf.Infinity)
    {
        StartCoroutine(_UpdateScoreMultiplier(multiplier, duration));
    }

    IEnumerator _UpdateScoreMultiplier(int multiplier, float duration)
    {
        scoreMultiplier = multiplier;

        yield return new WaitForSeconds(duration);

        scoreMultiplier = 1;
    }
}
