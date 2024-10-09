using System.Collections;
using UnityEngine;

public class AirlockPassage : MonoBehaviour {
    public Transform mainT;
    public Door entranceDoor;
    public Door exitDoor;
    private bool isTriggered = false;
    private GameObject roomToDelete;
    private Rooms nextRoom;

    private void Awake() {
        exitDoor.OnOpen += ExitDoor_OnOpen;
    }

    private bool openedOnce = false;
    private void ExitDoor_OnOpen() {
        if(!openedOnce && nextRoom == Rooms.StartRoom) {
            openedOnce = true;
            AudioManager.Instance.PlayIncorrectRoomSound();
        }
    }

    public void Setup(Rooms _nextRoom, GameObject _roomToDelete) {
        nextRoom = _nextRoom;
        roomToDelete = _roomToDelete;
    }
    private void OnTriggerEnter(Collider other) {
        if (!isTriggered && other.gameObject.IsPlayer()) {
            isTriggered = true;
            StartCoroutine(Trigger());
        }
    }

    private IEnumerator Trigger() {
        entranceDoor.interactable = false;
        entranceDoor.Close();
        GameObject roomPrefab = RoomMasterList.Instance.GetRoomForEnum(nextRoom);
        yield return null;
        GameObject newRoom = Instantiate(roomPrefab);
        newRoom.transform.SetPositionAndRotation(exitDoor.mainT.position, exitDoor.mainT.rotation);
        mainT.SetParent(newRoom.transform, true);
        yield return WaitUtil.GetWait(Door.DOOR_CLOSE_TIME);
        if (roomToDelete != null) {
            Destroy(roomToDelete);
        } else {
            Debug.Log("No Room to Delete!");
        }
    }
}
