using UnityEngine;

public class AirlockPassage : MonoBehaviour {
    public Transform mainT;
    public Transform entranceDoorT;
    public Collider entranceDoorCollider;
    public Transform exitDoorT;
    public Collider editDoorCollider;
    private Rooms nextRoom;

    public void Setup(Rooms _nextRoom) {
        nextRoom = _nextRoom;
    }
}
