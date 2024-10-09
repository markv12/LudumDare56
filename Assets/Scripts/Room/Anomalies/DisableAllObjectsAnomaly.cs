using UnityEngine;

public class DisableAllObjectsAnomaly : RoomAnomaly {
    public EnableObjectAnomaly.ObjectSet[] objectSetsLCR; //Left Center Right
    public override void Activate(int correctDoorIndex) {
        for (int i = 0; i < objectSetsLCR.Length; i++) {
            if (i == correctDoorIndex) {
                GameObject[] toDeactivate = objectSetsLCR[i].objects;
                for (int j = 0; j < toDeactivate.Length; j++) {
                    toDeactivate[j].SetActive(false);
                }
            }
        }
    }
}
