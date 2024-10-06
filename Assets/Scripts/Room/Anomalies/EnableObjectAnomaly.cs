using UnityEngine;

public class EnableObjectAnomaly : RoomAnomaly {
    public ObjectSet[] objectSetsLCR; //Left Center Right

    private void Awake() {
        Activate(-1); //Disable all
    }

    public override void Activate(int correctDoorIndex) {
        for (int i = 0; i < objectSetsLCR.Length; i++) {
            ObjectSet objectSet = objectSetsLCR[i];
            int randomIndex = (i == correctDoorIndex) ? Random.Range(0, objectSet.objects.Length) : -1;
            for (int j = 0; j < objectSet.objects.Length; j++) {
                objectSet.objects[i].SetActive(j == randomIndex);
            }
        }
    }

    [System.Serializable]
    public class ObjectSet {
        public GameObject[] objects;
    }
}
