using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] protected bool rotate;
    [SerializeField] protected float rotationSpeed;
    [SerializeField] protected int score;

    protected SpriteRenderer spriteRenderer;
    protected ColorPalettes MyColorPalette;
    protected Colors MyCurrentColor;

    protected GameController gameController;

    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
    }

    protected virtual void Start()
    {
        if(rotate)
            StartCoroutine(Rotate());

        MyColorPalette = ColorPalettes.ColorPalette_02; //TO-DO : Retrieve currently equipped color palette from a manager or saved data
    }

    protected IEnumerator Rotate()
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
}
