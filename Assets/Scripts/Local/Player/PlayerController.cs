using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private int launchChance;
    private int currentLaunchChance;
    
    [SerializeField] private float tapDuration;
    [SerializeField] private float power;
    [SerializeField] private float rotationSpeed;
    [Range(0.1f, 1f)][SerializeField] private float slowMotionSpeed;

    [SerializeField] private float forceLimit;

    [SerializeField] private Transform directionRenderer;

    [SerializeField] private ParticleSystem[] colorSwitchParticles;
    [SerializeField] private ParticleSystem[] launchParticles;
    [SerializeField] private ParticleSystem screenPressParticles;

    private Camera mainCamera;
    private LineRenderer lineRenderer;
    private TrailRenderer trailRenderer;
    private Vector3 startPointHolder;
    private Vector3 startPoint;
    private Vector3 currentPoint;
    private Vector3 endPoint;
    private Color color;

    [HideInInspector] public bool CanDrag;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public SpriteRenderer spriteRenderer;

    private GameController gameController;

    public ColorPalettes MyColorPalette;
    public Colors MyCurrentColor;

    #region Unity Functions
    private void Start()
    {
        mainCamera = Camera.main;
        lineRenderer = directionRenderer.GetComponent<LineRenderer>();
        trailRenderer = GetComponent<TrailRenderer>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        gameController = FindObjectOfType<GameController>();
        
        CanDrag = true;
        currentLaunchChance = launchChance;

        MyColorPalette = ColorPalettes.ColorPalette_02;

        StartCoroutine(RandomizeColor());
        StartCoroutine(EnableDragging());
        //gameController.UpdateLaunchNumberUI(currentLaunchChance);
    }
    #endregion

    #region Controls
    IEnumerator EnableDragging()
    {
        float touchTime = 0f;
        bool isDrag = false;

        while(currentLaunchChance > 0)
        {
            #if UNITY_IOS || UNITY_ANDROID
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    ////Enable Gravity
                    //if (rb.gravityScale < 1f)
                    //    rb.gravityScale = 1f;

                    touchTime = Time.time;
                    startPointHolder = touch.position;
                }

                else if (touch.phase == TouchPhase.Moved)
                {
                    float touchDuration = Time.time - touchTime;

                    if (touchDuration > tapDuration)
                    {
                        isDrag = true;
                        OnDrag();

                        if (!screenPressParticles.isPlaying)
                        {
                            //Spawn particle effect
                            screenPressParticles.transform.position = mainCamera.ScreenToWorldPoint(startPointHolder);
                            screenPressParticles.Play();
                        }
                    }
                }

                else if (touch.phase == TouchPhase.Ended)
                {
                    float touchDuration = Time.time - touchTime;

                    //If player tap on screen
                    if (touchDuration <= tapDuration)
                    {
                        OnTap();
                    }

                    //Else, if drag
                    else
                    {
                        if(isDrag)
                        {
                            Launch();
                        }
                    }

                    //Reset drag flag
                    isDrag = false;

                    //Reset touch time
                    touchTime = 0f;

                    //Disable Slow Motion
                    TimeManager.Instance.ResetTimeScale();

                    //Despawn particle effect
                    screenPressParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                }
            }
            #endif

            #if UNITY_EDITOR
            //Controls for UnityEditor. Can remove during release or just keep it here for future implementations
            if (Input.GetMouseButtonDown(0))
            {
                ////Enable Gravity
                //if (rb.gravityScale < 1f)
                //    rb.gravityScale = 1f;

                touchTime = Time.time;
                startPointHolder = Input.mousePosition;
            }

            else if (Input.GetMouseButton(0))
            {
                float touchDuration = Time.time - touchTime;
                
                if (touchDuration > tapDuration)
                {
                    OnDrag();

                    if (!screenPressParticles.isPlaying)
                    {
                        //Spawn particle effect
                        screenPressParticles.transform.position = mainCamera.ScreenToWorldPoint(startPointHolder);
                        screenPressParticles.Play();
                    }
                }
            }

            else if (Input.GetMouseButtonUp(0))
            {
                float touchDuration = Time.time - touchTime;

                //If player tap on screen
                if (touchDuration <= tapDuration)
                {
                    OnTap();
                }

                //Else, if drag
                else
                {
                    if(startPointHolder != Input.mousePosition)
                        Launch();
                }

                //Reset touch time
                touchTime = 0f;

                //Disable Slow Motion
                TimeManager.Instance.ResetTimeScale();

                //Despawn particle effect
                screenPressParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }
            #endif

            yield return null;
        }
    }
    #endregion

    #region Functionalities
    void OnDrag()
    {
        //Enable Slow Motion during drag
        TimeManager.Instance.ManipulateTimeScale(slowMotionSpeed);

        startPoint = mainCamera.ScreenToWorldPoint(startPointHolder);
        currentPoint = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        DrawTrajectoryLine();
        RotateRenderer();
    }

    void OnTap()
    {
        //Remove trajectory
        RemoveTrajectoryLine();

        //Change related component's color
        StartCoroutine(ChangeColor());
    }

    void Launch()
    {
        //Spawn Particle Effect
        AvailableParticle(launchParticles).Play();

        //Launching code snippet
        startPoint = mainCamera.ScreenToWorldPoint(startPointHolder);
        endPoint = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        //Reset velocity
        NullifyVelocity();

        ////Disable Gravity
        //rb.gravityScale = 0f;

        //Add force to ball based on drag direction
        Vector2 force = new Vector2(Mathf.Clamp(startPoint.x - endPoint.x, -forceLimit, forceLimit), Mathf.Clamp(startPoint.y - endPoint.y, -forceLimit, forceLimit));
        rb.AddForce(force * power, ForceMode2D.Impulse);

        //Remove trajectory line after launching
        RemoveTrajectoryLine();

        currentLaunchChance--;
        //gameController.UpdateLaunchNumberUI(currentLaunchChance);
    }

    public void Die()
    {
        //Despawn particle effect
        screenPressParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        //Vibrate device
        #if UNITY_IPHONE || UNITY_ANDROID
        //if(SettingManager.Instance.CanVibrate)
            Handheld.Vibrate();
        #endif

        //Spawn particles

        Destroy(gameObject);
    }
    #endregion

    #region Particle 
    ParticleSystem AvailableParticle(ParticleSystem[] particle)
    {
        foreach(ParticleSystem p in particle)
        {
            if (p.isPlaying)
                continue;

            else
            {
                return p;
            }
        }

        Debug.LogError("Not enough " + particle[0].name);
        return null;
    }
    #endregion

    #region Color
    IEnumerator RandomizeColor()
    {
        int randomColorInt = Random.Range(0, ColorDictionary.Instance.Dict_ColorPalettes[MyColorPalette].Length);
        color = ColorDictionary.Instance.Dict_ColorPalettes[MyColorPalette][randomColorInt];
        MyCurrentColor = (Colors)randomColorInt;

        SetColor();
        SetRendererColor();
        SetTrailColor();

        //Spawn particle effect
        SetParticleColor(AvailableParticle(colorSwitchParticles));

        yield return null; //Wait a frame before playing particle effect

        AvailableParticle(colorSwitchParticles).Play();
    }

    IEnumerator ChangeColor()
    {
        MyCurrentColor = GameUtility.NextEnumValue(MyCurrentColor);
        color = ColorDictionary.Instance.Dict_ColorPalettes[MyColorPalette][(int)MyCurrentColor];

        SetColor(); 
        SetRendererColor();
        SetTrailColor();

        //Spawn particle effect
        SetParticleColor(AvailableParticle(colorSwitchParticles));

        yield return null; //Wait a frame before playing particle effect

        AvailableParticle(colorSwitchParticles).Play();
    }

    void SetColor()
    {
        spriteRenderer.color = color;
    }

    void SetRendererColor()
    {
        float alphaValue = lineRenderer.endColor.a;

        lineRenderer.startColor = color;
        lineRenderer.endColor = new Color(color.r, color.g, color.b, alphaValue);
    }

    void SetTrailColor()
    {
        trailRenderer.startColor = color;
    }

    void SetParticleColor(ParticleSystem particle)
    {
        ParticleSystem[] allParticles = particle.GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem p in allParticles)
        {
            ParticleSystem.MainModule newModule = p.main;
            newModule.startColor = color;
        }
    }
    #endregion

    #region TrajectoryLine
    void DrawTrajectoryLine()
    {
        Vector3[] points = new Vector3[2];
        float dragDistance = Mathf.Clamp(Vector2.Distance(startPoint, currentPoint), -forceLimit, forceLimit);

        points[0] = Vector3.zero;
        points[1] = new Vector3(dragDistance, 0f, 0f);

        lineRenderer.positionCount = 2;
        lineRenderer.SetPositions(points);
    }

    void RemoveTrajectoryLine()
    {
        lineRenderer.positionCount = 0;
    }

    void RotateRenderer()
    {
        Vector3 difference = startPoint - currentPoint;
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        Quaternion lookRotation = Quaternion.Euler(new Vector3(directionRenderer.rotation.x, directionRenderer.rotation.y, rotationZ));
        directionRenderer.rotation = Quaternion.Slerp(directionRenderer.rotation, lookRotation, Time.fixedUnscaledDeltaTime * rotationSpeed);
    }
    #endregion

    #region Collision
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Fatal")
        {
            Die();
        }
    }
    #endregion

    #region GlobalFunctions
    public void NullifyVelocity()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = 0f;
    }

    public void AddForce(float multiplier)
    {
        Vector2 normalizedVelocity = rb.velocity.normalized;
        rb.AddForce(normalizedVelocity * multiplier, ForceMode2D.Impulse);
    }
    #endregion
}
