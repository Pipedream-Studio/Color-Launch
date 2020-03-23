using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : Obstacle
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();

            SetMyColor((int)player.MyCurrentColor);
            gameController.UpdateScore(score);
        }
    }
}
