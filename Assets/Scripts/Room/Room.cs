using UnityEngine;

public class Room : MonoBehaviour {
    public Rooms nextRoom;
    public RoomSpawnLocation[] spawnLocations; //Left Center Right
    public RoomAnomaly[] anomalies; //Order doesn't matter
    private int correctDoorIndex;
    private void Awake() {
        correctDoorIndex = Random.Range(0, spawnLocations.Length);
        for (int i = 0; i < spawnLocations.Length; i++) {
            RoomSpawnLocation rsl = spawnLocations[i];
            Rooms room = i == correctDoorIndex ? nextRoom : Rooms.StartRoom;
            rsl.Setup(room, gameObject);
        }
        if(anomalies.Length > 0) {
            anomalies[Random.Range(0, anomalies.Length)].Activate();
        }
    }

    public int CorrectDoorIndex => correctDoorIndex;
    public Vector3 CorrectDoorPos => spawnLocations[correctDoorIndex].mainT.position;
}
