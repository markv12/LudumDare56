using System;
using UnityEngine;

public class FanPointAnomaly : RoomAnomaly {
    public GameObject pointingGnome;
    public Spinner fanSpinner;
    public Transform spinT;
    public float[] rotationsLCR;
    private float jiggleRotation = -1000;
    private void Awake() {
        pointingGnome.SetActive(false);
    }
    public override void Activate(int correctDoorIndex) {
        pointingGnome.SetActive(true);
        fanSpinner.enabled = false;
        jiggleRotation = rotationsLCR[correctDoorIndex];
    }

    private void Update() {
        if(jiggleRotation > -360) {
            spinT.localRotation = Quaternion.Euler(0, jiggleRotation + Mathf.Sin(Time.time * 5) * 0.35f, 0);
        }
    }
}
