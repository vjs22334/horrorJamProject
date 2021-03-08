using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newShotPattern", menuName = "ScriptableObjects/ShotPattern")]
public class ShotPattern : ScriptableObject
{
    public GameObject bulletPrefab;
    public float[] spreadAngles;
}
