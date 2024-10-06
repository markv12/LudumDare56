using UnityEngine;

[CreateAssetMenu(fileName = "RoomMasterList", menuName = "MasterLists/Room Master List")]
public class RoomMasterList : ScriptableObject {
    public GameObject startRoom;
    public GameObject kitchenRoom;
    public GameObject bedRoom;
    public GameObject hallwayRoom;

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
            case Rooms.Bedroom:
                return bedRoom;
            case Rooms.Hallway:
                return hallwayRoom;
            default:
                return startRoom;
        }
    }
}
