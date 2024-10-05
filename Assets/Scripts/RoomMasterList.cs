using UnityEngine;

[CreateAssetMenu(fileName = "RoomMasterList", menuName = "MasterLists/Room Master List")]
public class RoomMasterList : ScriptableObject {
    public GameObject startRoom;
    public GameObject kitchenRoom;

    private static RoomMasterList instance;
    public static RoomMasterList Instance {
        get {
            if (instance == null) {
                instance = Resources.Load<RoomMasterList>("RoomMasterList");
            }
            return instance;
        }
    }

    public GameObject GetRoomForEnum(Rooms room) {
        switch (room) {
            case Rooms.Kitchen:
                return kitchenRoom;
            default:
                return startRoom;
        }
    }
}
