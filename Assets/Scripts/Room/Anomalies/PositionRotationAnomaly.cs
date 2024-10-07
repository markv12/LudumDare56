using UnityEngine;

public class PositionRotationAnomaly : RoomAnomaly {
    public GameObject fullObject;
    public Transform toMoveT;
    public PosRotSet[] posRotSetsLCR;
    private void Awake() {
        fullObject.SetActive(false);
    }
    public override void Activate(int correctDoorIndex) {
        PosRotSet prs = posRotSetsLCR[correctDoorIndex];
        toMoveT.SetLocalPositionAndRotation(prs.position, Quaternion.Euler(prs.rotation));
        fullObject.SetActive(true);
    }

    [System.Serializable]
    public class PosRotSet {
        public Vector3 position;
        public Vector3 rotation;
    }
}
