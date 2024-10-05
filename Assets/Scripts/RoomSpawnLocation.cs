using System.Collections;
using UnityEngine;

public class RoomSpawnLocation : MonoBehaviour {
    public Transform mainT;
    public GameObject roomToDelete;
    public Rooms room;

    private static AirlockPassage airlockPassagePrefab;
    private static void EnsurePrefab() {
        if(airlockPassagePrefab == null) {
            airlockPassagePrefab = Resources.Load<AirlockPassage>("AirlockPassage");
        }
    }

    private IEnumerator Start() {
        EnsurePrefab();
        yield return null;
        AirlockPassage newAirlock = Instantiate(airlockPassagePrefab, mainT);
        //newAirlock.mainT.SetPositionAndRotation(mainT.position, mainT.rotation);
        newAirlock.Setup(room, roomToDelete);
    }
}

public enum Rooms {
    StartRoom,
    Kitchen
}
