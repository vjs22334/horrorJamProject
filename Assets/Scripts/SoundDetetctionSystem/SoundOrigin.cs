using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundOrigin : MonoBehaviour
{
    Transform SrcTransform;
    [SerializeField] float SoundPower = 100f;               //The intensity from the sound in the source point 
    [SerializeField] float BarrierPenalizers = 50f;         //Amount of sound power that takes to penetrate a wall
    [SerializeField] float DistancePenalizer = 5f;          //Amount of sound power that takes to travel a unity of distance
    [SerializeField] LayerMask Walls;                       //What's a wall?


    private void Awake() {
        SrcTransform = GetComponent<Transform>();
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            MakeSound();
        }
    }

    void MakeSound() {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Detector");
        foreach (GameObject Detector in targets) {
            float currPower = SoundPower;
            Transform DetectorTransform = Detector.GetComponent<Transform>();
            float distanceToObject = Vector2.Distance(SrcTransform.position, DetectorTransform.position);                   //Counts the number of wall between the sound source
            Vector2 Direction = DetectorTransform.position - SrcTransform.position;                                         //and all the sound detectors (Enemies for now)
            RaycastHit2D[] hits;
            hits = Physics2D.RaycastAll(SrcTransform.position, Direction, distanceToObject, Walls);
            foreach(RaycastHit2D Barrier in hits) {
                BarrierScript bs = Barrier.transform.gameObject.GetComponent<BarrierScript>();
                currPower -= bs.GetPenalizer();
            }

            float lossPower = distanceToObject * DistancePenalizer;                                                         //Substracts the penalizers from the ray power
            currPower -= lossPower;
            if(distanceToObject <= SoundPower / 25) {
                currPower += 999999999;
            }
            SoundReceive  sr = Detector.GetComponent<SoundReceive>();                                                       //The detector receives the SoundRay
            sr.ReceiveSound(currPower);

            Debug.Log(currPower);                                                                                   //Debug
            Debug.DrawRay(SrcTransform.position, Direction, Color.green, 3f);
        }

    }
    void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(SrcTransform.position, SoundPower / 25f);
    }
}
