using UnityEngine;

public static class SplatManager {
    public static int paintingLayer;

    private const int SPLAT_POOL_SIZE = 20000;
    private static SplatData[] splatPool = new SplatData[SPLAT_POOL_SIZE];
    private static int splatPoolIndex;
    public static void CreateSplat(RaycastHit hit, Color color, float scale) {
        ref SplatData sd = ref splatPool[splatPoolIndex];
        splatPoolIndex = (splatPoolIndex + 1) % SPLAT_POOL_SIZE;
        if (!sd.inUse) {
            sd.splatGO = new GameObject("Splat");
            sd.splatGO.layer = paintingLayer;
            Object.DontDestroyOnLoad(sd.splatGO);
            sd.splatT = sd.splatGO.transform;
            sd.splatRenderer = sd.splatGO.AddComponent<SpriteRenderer>();
            sd.splatRenderer.sortingOrder = 1;
            sd.inUse = true;
        } else {
            sd.splatGO.SetActive(true);
        }
        Vector3 pos = hit.point + (hit.normal * 0.02f);
        sd.splatT.SetPositionAndRotation(pos, Quaternion.LookRotation(hit.normal));
        sd.splatT.RotateAround(pos, sd.splatT.forward, Random.Range(0f, 360f));
        sd.splatT.localScale = new Vector3(scale, scale, scale);
        Sprite[] sprites = SplatSprites.Instance.sprites;
        sd.splatRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
        sd.splatRenderer.color = color;
    }

    public static void Reset() {
        for (int i = 0; i < splatPool.Length; i++) {
            ref SplatData sd = ref splatPool[i];
            if (sd.inUse) {
                sd.splatGO.SetActive(false);
            }
        }
    }

    private struct SplatData {
        public bool inUse;
        public GameObject splatGO;
        public Transform splatT;
        public SpriteRenderer splatRenderer;
    }
}
