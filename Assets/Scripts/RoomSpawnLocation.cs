using UnityEngine;

public class RoomSpawnLocation : MonoBehaviour {
    public Transform mainT;
    public Rooms room;

    private static AirlockPassage airlockPassagePrefab;
    private static void EnsurePrefab() {
        if(airlockPassagePrefab == null) {
            airlockPassagePrefab = Resources.Load<AirlockPassage>("AirlockPassage");
        }
    }

    private void Awake() {
        EnsurePrefab();
        AirlockPassage newAirlock = Instantiate(airlockPassagePrefab);
        newAirlock.mainT.SetPositionAndRotation(mainT.position, mainT.rotation);
        newAirlock.Setup(room);
    }
}

public enum Rooms {
    StartRoom,
    Kitchen
}
