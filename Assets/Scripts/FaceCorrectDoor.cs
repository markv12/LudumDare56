using UnityEngine;

public class FaceCorrectDoor : MonoBehaviour {
    public Transform mainT;
    public Room room;
    private void Start() {
        Vector3 direction = room.CorrectDoorPos - mainT.position;
        direction.y = 0;
        if (direction != Vector3.zero) {
            Quaternion rotation = Quaternion.LookRotation(direction);
            mainT.rotation = rotation;
        }
    }
}
