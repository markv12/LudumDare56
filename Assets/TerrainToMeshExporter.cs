#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Text;
using Unity.VisualScripting;

public class TerrainToMeshExporter : Editor {
    [MenuItem("Tools/Terrain to OBJ")]
    public static void ExportTerrainToOBJ() {
        Terrain terrain = Selection.activeObject.GetComponent<Terrain>();

        if (terrain == null) {
            Debug.LogError("Please select a Terrain object in the scene.");
            return;
        }

        string path = EditorUtility.SaveFilePanel("Save Terrain Mesh", "", "TerrainMesh.obj", "obj");
        if (string.IsNullOrEmpty(path))
            return;

        Mesh mesh = TerrainToMesh(terrain, 200); // Change the resolution here as needed
        ExportMeshToOBJ(mesh, path);
        Debug.Log("Terrain mesh exported to " + path);
    }

    private static Mesh TerrainToMesh(Terrain terrain, int resolution) {
        TerrainData terrainData = terrain.terrainData;
        int width = terrainData.heightmapResolution;
        int height = terrainData.heightmapResolution;
        float[,] heights = terrainData.GetHeights(0, 0, width, height);
        Vector3 meshScale = terrainData.size;
        Vector3 size = terrainData.size;
        meshScale = new Vector3(meshScale.x / (width - 1) * resolution, meshScale.y, meshScale.z / (height - 1) * resolution);

        Vector3[] vertices = new Vector3[resolution * resolution];
        Vector3[] normals = new Vector3[resolution * resolution];
        Vector2[] uv = new Vector2[resolution * resolution];
        int[] triangles = new int[(resolution - 1) * (resolution - 1) * 6];

        for (int y = 0; y < resolution; y++) {
            for (int x = 0; x < resolution; x++) {
                float y01 = (float)y / (resolution - 1);
                float x01 = (float)x / (resolution - 1);
                int index = y * resolution + x;

                vertices[index] = new Vector3(x01 * size.x, heights[(int)(y01 * (width - 1)), (int)(x01 * (height - 1))] * size.y, y01 * size.z);
                uv[index] = new Vector2(x01, y01);
                normals[index] = Vector3.up;
            }
        }

        int tIndex = 0;
        for (int y = 0; y < resolution - 1; y++) {
            for (int x = 0; x < resolution - 1; x++) {
                int i0 = y * resolution + x;
                int i1 = y * resolution + x + 1;
                int i2 = (y + 1) * resolution + x;
                int i3 = (y + 1) * resolution + x + 1;

                triangles[tIndex++] = i0;
                triangles[tIndex++] = i2;
                triangles[tIndex++] = i1;

                triangles[tIndex++] = i1;
                triangles[tIndex++] = i2;
                triangles[tIndex++] = i3;
            }
        }

        Mesh mesh = new Mesh {
            vertices = vertices,
            uv = uv,
            triangles = triangles,
            normals = normals
        };
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        return mesh;
    }

    private static void ExportMeshToOBJ(Mesh mesh, string path) {
        using (StreamWriter sw = new StreamWriter(path)) {
            sw.WriteLine("# Unity terrain mesh export");

            foreach (Vector3 v in mesh.vertices) {
                sw.WriteLine($"v {v.x} {v.y} {v.z}");
            }
            sw.WriteLine();

            for (int i = 0; i < mesh.triangles.Length; i += 3) {
                int i0 = mesh.triangles[i] + 1;
                int i1 = mesh.triangles[i + 1] + 1;
                int i2 = mesh.triangles[i + 2] + 1;
                sw.WriteLine($"f {i0} {i1} {i2}");
            }
        }
    }
}
#endif