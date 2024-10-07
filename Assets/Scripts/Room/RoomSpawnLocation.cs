using System;
using UnityEngine;

public class RoomSpawnLocation : MonoBehaviour {
    public Transform mainT;
    [NonSerialized] public AirlockPassage airlock;
    private static AirlockPassage airlockPassagePrefab;
    private static void EnsurePrefab() {
        if(airlockPassagePrefab == null) {
            airlockPassagePrefab = Resources.Load<AirlockPassage>("AirlockPassage");
        }
    }

    public void Setup(Rooms nextRoom, GameObject roomToDelete) {
        EnsurePrefab();
        airlock = Instantiate(airlockPassagePrefab, mainT);
        //newAirlock.mainT.SetPositionAndRotation(mainT.position, mainT.rotation);
        airlock.Setup(nextRoom, roomToDelete);
    }
}

public enum Rooms {
    StartRoom,
    Kitchen,
    Bedroom,
    Hallway,
    EndRoom,
}
