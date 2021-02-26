using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierScript : MonoBehaviour
{
    [SerializeField] float Penalizer = 50f;

    public float GetPenalizer() {
        return Penalizer;
    }
}
