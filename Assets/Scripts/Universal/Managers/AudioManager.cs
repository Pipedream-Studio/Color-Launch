using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
	private AudioSource myBGMSource;
    private AudioSource mySFXSource;

    //[Space(3)]
    //[Header("Setting Components")]
    //[SerializeField] private Text BGMText;
    //[SerializeField] private Text SFXText;
    //[SerializeField] private Slider BGMSlider;
    //[SerializeField] private Slider SFXSlider;

    #region Singleton
    private static AudioManager mInstance;

    public static AudioManager Instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = FindObjectOfType<AudioManager>();
                if (mInstance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(AudioManager).Name;
                    mInstance = obj.AddComponent<AudioManager>();
                }
            }
            return mInstance;
        }
    }

    public virtual void Awake()
    {
        if (mInstance == null)
        {
            mInstance = this as AudioManager;
            myBGMSource = GetComponentsInChildren<AudioSource>()[0];
            mySFXSource = GetComponentsInChildren<AudioSource>()[1];
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
        myBGMSource.volume = PlayerPrefs.GetInt("BGMOn", 1);
        mySFXSource.volume = PlayerPrefs.GetInt("SFXOn", 1);

        //myBGMSource.volume = PlayerPrefs.GetFloat("BGMVolume", .75f);
        //mySFXSource.volume = PlayerPrefs.GetFloat("SFXVolume", .75f);

        //BGMSlider.value = myBGMSource.volume;
        //SFXSlider.value = mySFXSource.volume;
    }

	public void PlaySFX(AudioClip audio)
	{
		mySFXSource.PlayOneShot(audio);
	}

    public void PlayBGM(AudioClip audio)
    {
        myBGMSource.PlayOneShot(audio);
    }

    #region Settings
    public void EnableBGM(bool enableBGM)
    {
        myBGMSource.enabled = enableBGM;

        PlayerPrefs.SetInt("BGMOn", enableBGM.GetHashCode());
        PlayerPrefs.Save();
    }

    public void EnableSFX(bool enableSFX)
    {
        mySFXSource.enabled = enableSFX;

        PlayerPrefs.SetInt("SFXOn", enableSFX.GetHashCode());
        PlayerPrefs.Save();
    }

    //public void ChangeBGMVolume(Slider mySlider)
    //{
    //    myBGMSource.volume = mySlider.value;

    //    int roundOffValue = (int)(mySlider.value * 100);
    //    BGMText.text = roundOffValue.ToString();

    //    PlayerPrefs.SetFloat("BGMVolume", myBGMSource.volume);
    //    PlayerPrefs.Save();
    //}

    //public void ChangeSFXVolume(Slider mySlider)
    //{
    //    mySFXSource.volume = mySlider.value;

    //    int roundOffValue = (int)(mySlider.value * 100);
    //    SFXText.text = roundOffValue.ToString();

    //    PlayerPrefs.SetFloat("SFXVolume", mySFXSource.volume);
    //    PlayerPrefs.Save();
    //}
    #endregion
}
