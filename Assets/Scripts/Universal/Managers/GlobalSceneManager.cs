using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

public class GlobalSceneManager : MonoBehaviour
{
    public List<SceneField> gameScenes;

    #region Singleton
    private static GlobalSceneManager mInstance;

    public static GlobalSceneManager Instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = FindObjectOfType<GlobalSceneManager>();
                if (mInstance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(GlobalSceneManager).Name;
                    mInstance = obj.AddComponent<GlobalSceneManager>();
                }
            }
            return mInstance;
        }
    }

    public virtual void Awake()
    {
        if (mInstance == null)
        {
            mInstance = this as GlobalSceneManager;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    #region SceneManager Functions
    public void ChangeScene(int sceneInt)
    {
        SceneManager.LoadScene(sceneInt);
        AudioManager.Instance.myBGMSource.Stop();
        AudioManager.Instance.myBGMSource.clip = null;
    }

    public void ChangeScene(string sceneString)
    {
        SceneManager.LoadScene(sceneString);
        AudioManager.Instance.myBGMSource.Stop();
        AudioManager.Instance.myBGMSource.clip = null;
    }

    public void ChangeScene(SceneField scene)
    {
        SceneManager.LoadScene(scene);
        AudioManager.Instance.myBGMSource.Stop();
        AudioManager.Instance.myBGMSource.clip = null;
    }

    public void ChangeSceneWithDelay(int sceneInt, float waitDuration)
    {
        StartCoroutine(DelayChange(sceneInt, waitDuration));
        AudioManager.Instance.myBGMSource.Stop();
        AudioManager.Instance.myBGMSource.clip = null;
    }

    public void ChangeSceneWithDelay(string sceneName, float waitDuration)
    {
        StartCoroutine(DelayChange(sceneName, waitDuration));
        AudioManager.Instance.myBGMSource.Stop();
        AudioManager.Instance.myBGMSource.clip = null;
    }

    public void ChangeSceneWithDelay(SceneField scene, float waitDuration)
    {
        StartCoroutine(DelayChange(scene, waitDuration));
        AudioManager.Instance.myBGMSource.Stop();
        AudioManager.Instance.myBGMSource.clip = null;
    }

    IEnumerator DelayChange(int sceneInt, float waitDuration)
    {
        yield return new WaitForSeconds(waitDuration);
        SceneManager.LoadScene(sceneInt);
    }

    IEnumerator DelayChange(string sceneName, float waitDuration)
    {
        yield return new WaitForSeconds(waitDuration);
        SceneManager.LoadScene(sceneName);
    }

    IEnumerator DelayChange(SceneField scene, float waitDuration)
    {
        yield return new WaitForSeconds(waitDuration);
        SceneManager.LoadScene(scene);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
    #endregion
}
