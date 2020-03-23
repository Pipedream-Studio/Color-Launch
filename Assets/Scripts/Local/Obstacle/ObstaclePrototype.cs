using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclePrototype : Obstacle
{
    [SerializeField] private int scoreMultiplier;
    [SerializeField] private float multiplierDuration;
    [SerializeField] private float addForce;

    [SerializeField] private ObstacleEffect myEffect;

    protected override void Start()
    {
        base.Start();

        //Set color to a random color based on color palette 
        //TO-DO: Set color from obstacle course controller instead if obstacles will be controlled
        int randomColorInt = Random.Range(0, ColorDictionary.Instance.Dict_ColorPalettes[MyColorPalette].Length);
        SetMyColor(randomColorInt);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            if (player.MyCurrentColor == MyCurrentColor)
            {
                switch (myEffect)
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

                    default:
                        break;

                }

                gameController.UpdateScore(score);
                gameObject.SetActive(false);
            }

            else
            {
                player.Die();
            }
        }
    }
}
