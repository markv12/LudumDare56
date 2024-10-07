using UnityEngine;

public class Room : MonoBehaviour {
    public Rooms nextRoom;
    [UnityEngine.Serialization.FormerlySerializedAs("spawnLocations")]
    public RoomSpawnLocation[] spawnLocationsLCR; //Left Center Right
    public RoomAnomaly[] anomalies; //Order doesn't matter
    private int correctDoorIndex;
    private void Awake() {
        correctDoorIndex = Random.Range(0, spawnLocationsLCR.Length);
        for (int i = 0; i < spawnLocationsLCR.Length; i++) {
            RoomSpawnLocation rsl = spawnLocationsLCR[i];
            Rooms room = i == correctDoorIndex ? nextRoom : Rooms.StartRoom;
            rsl.Setup(room, gameObject);
        }
    }

    private void Start() {
        if (anomalies.Length > 0) {
            anomalies[Random.Range(0, anomalies.Length)].Activate(correctDoorIndex);
        }
    }

    public int CorrectDoorIndex => correctDoorIndex;
    public Vector3 CorrectDoorPos => spawnLocationsLCR[correctDoorIndex].mainT.position;
}
