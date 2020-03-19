using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorDictionary : MonoBehaviour
{
    public Color[] ColorPalette_01;
    public Color[] ColorPalette_02;
    public Dictionary<ColorPalettes, Color[]> Dict_ColorPalettes = new Dictionary<ColorPalettes, Color[]>();

    #region Singleton
    private static ColorDictionary mInstance;

    public static ColorDictionary Instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = FindObjectOfType<ColorDictionary>();
                if (mInstance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(ColorDictionary).Name;
                    mInstance = obj.AddComponent<ColorDictionary>();
                }
            }
            return mInstance;
        }
    }

    public virtual void Awake()
    {
        if (mInstance == null)
        {
            mInstance = this as ColorDictionary;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        //Add all dictionaries into main color palette dictionary
        Dict_ColorPalettes.Add(ColorPalettes.ColorPalette_01, ColorPalette_01);
        Dict_ColorPalettes.Add(ColorPalettes.ColorPalette_02, ColorPalette_02);
    }
    #endregion
}
