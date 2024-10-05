using System;
using UnityEngine;

public class AirlockPassage : MonoBehaviour {
    public Transform mainT;
    public Door entranceDoor;
    public Door exitDoor;
    private bool isTriggered = false;
    private Rooms nextRoom;

    public void Setup(Rooms _nextRoom) {
        nextRoom = _nextRoom;
    }
    private void OnTriggerEnter(Collider other) {
        if (!isTriggered && other.gameObject.IsPlayer()) {
            isTriggered = true;
            Trigger();
        }
    }

    private void Trigger() {
        entranceDoor.interactable = false;
        entranceDoor.Close();
        GameObject newRoom = Instantiate(RoomMasterList.Instance.GetRoomForEnum(nextRoom));
        newRoom.transform.SetPositionAndRotation(exitDoor.mainT.position, exitDoor.mainT.rotation);
    }
}
