using UnityEngine;

public class WallHitManager : MonoBehaviour {
    public Transform mainT;
    public float yOffset;
    public LayerMask layerMask;

    private static readonly Vector3[] dirs = new Vector3[]{
        Vector3.forward,
        Vector3.Lerp(Vector3.forward, Vector3.right, 0.5f),
        Vector3.right,
        Vector3.Lerp(-Vector3.forward, Vector3.right, 0.5f),
        -Vector3.forward,
        Vector3.Lerp(-Vector3.forward, -Vector3.right, 0.5f),
        -Vector3.right,
        Vector3.Lerp(Vector3.forward, -Vector3.right, 0.5f),
    };

    private RaycastHit wallHit;
    public bool TryGetWallHit(out Vector3 wallHitNormal) {
        Vector3 pos = mainT.position.AddY(yOffset);
        for (int i = 0; i < 8; i++) {
            if(Physics.Raycast(pos, dirs[i], out wallHit, 0.8f, layerMask)) {
                wallHitNormal = wallHit.normal;
                return true;
            }
        }
        wallHitNormal = Vector3.zero;
        return false;
    }
}
