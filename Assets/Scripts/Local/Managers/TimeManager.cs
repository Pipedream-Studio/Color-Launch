using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private float timeScale;
    private float fixedDeltaTime;

    #region Singleton
    private static TimeManager mInstance;

    public static TimeManager Instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = FindObjectOfType<TimeManager>();
                if (mInstance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(TimeManager).Name;
                    mInstance = obj.AddComponent<TimeManager>();
                }
            }
            return mInstance;
        }
    }

    public virtual void Awake()
    {
        if (mInstance == null)
        {
            mInstance = this as TimeManager;
            DontDestroyOnLoad(gameObject.transform.root);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    private void Start()
    {
        timeScale = Time.timeScale;
        fixedDeltaTime = Time.fixedDeltaTime;
    }

    #region TimeManager Functions
    public void ManipulateTimeScale(float targetTimeScale)
    {
        Time.timeScale = targetTimeScale;
        Time.fixedDeltaTime = targetTimeScale * fixedDeltaTime;
    }

    public void ResetTimeScale()
    {
        Time.timeScale = timeScale;
        Time.fixedDeltaTime = fixedDeltaTime;
    }
    #endregion
}
