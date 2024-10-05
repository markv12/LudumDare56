using UnityEngine;

public class Room : MonoBehaviour {
    public Rooms nextRoom;
    public RoomSpawnLocation[] spawnLocations;
    private int correctDoorIndex;
    private void Awake() {
        correctDoorIndex = Random.Range(0, spawnLocations.Length);
        for (int i = 0; i < spawnLocations.Length; i++) {
            RoomSpawnLocation rsl = spawnLocations[i];
            Rooms room = i == correctDoorIndex ? nextRoom : Rooms.StartRoom;
            rsl.Setup(room, gameObject);
        }
    }

    public Vector3 CorrectDoorPos => spawnLocations[correctDoorIndex].mainT.position;
}
