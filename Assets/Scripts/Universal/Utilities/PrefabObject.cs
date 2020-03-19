using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PrefabObject : MonoBehaviour
{
    [Header("Variables")] [Tooltip("The amount of instances of this prefab to spawn at the start of the game")]
    public int AmountToSpawn;
}
