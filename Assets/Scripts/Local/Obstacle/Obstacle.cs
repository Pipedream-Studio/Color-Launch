using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public bool rotate;

    [SerializeField] private float rotationSpeed;
    [SerializeField] private int score;
    [SerializeField] private int scoreMultiplier;
    [SerializeField] private float multiplierDuration;
    [SerializeField] private float addForce;

    [SerializeField] private ObstacleEffect myEffect;

    private SpriteRenderer spriteRenderer;
    private ColorPalettes MyColorPalette;
    private Colors MyCurrentColor;

    private GameController gameController;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
    }

    private void Start()
    {
        MyColorPalette = ColorPalettes.ColorPalette_02; //TO-DO : Retrieve currently equipped color palette from a manager or saved data

        //Set color to a random color based on color palette 
        //TO-DO: Set color from obstacle course controller instead if obstacles will be controlled
        int randomColorInt = Random.Range(0, ColorDictionary.Instance.Dict_ColorPalettes[MyColorPalette].Length);
        SetMyColor(randomColorInt);

        if(rotate)
            StartCoroutine(Rotate());
    }

    IEnumerator Rotate()
    {
        while (true)
        {
            transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
            yield return null;
        }
    }

    public void SetMyColor(int colorInt)
    {
        spriteRenderer.color = ColorDictionary.Instance.Dict_ColorPalettes[MyColorPalette][colorInt];
        MyCurrentColor = (Colors)colorInt;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player.MyCurrentColor == MyCurrentColor)
            {
                switch(myEffect)
                {
                    case ObstacleEffect.AddForce:
                        player.AddForce(addForce);
                        break;

                    case ObstacleEffect.NullifyVelocity:
                        player.NullifyVelocity();
                        break;

                    case ObstacleEffect.ScoreMultiplier:
                        gameController.UpdateScoreMultiplier(scoreMultiplier, multiplierDuration);
                        break;
                        
                }

                gameController.UpdateScore(score);
                gameObject.SetActive(false);
            }

            else
            {
                other.GetComponent<PlayerController>().Die();
            }
        }
    }
}
