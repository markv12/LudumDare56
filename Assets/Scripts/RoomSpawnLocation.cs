using UnityEngine;

public class RoomSpawnLocation : MonoBehaviour {
    public Transform mainT;

    private static AirlockPassage airlockPassagePrefab;
    private static void EnsurePrefab() {
        if(airlockPassagePrefab == null) {
            airlockPassagePrefab = Resources.Load<AirlockPassage>("AirlockPassage");
        }
    }

    public void Setup(Rooms nextRoom, GameObject roomToDelete) {
        EnsurePrefab();
        AirlockPassage newAirlock = Instantiate(airlockPassagePrefab, mainT);
        //newAirlock.mainT.SetPositionAndRotation(mainT.position, mainT.rotation);
        newAirlock.Setup(nextRoom, roomToDelete);
    }
}

public enum Rooms {
    StartRoom,
    Kitchen
}
