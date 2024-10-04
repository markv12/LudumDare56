using System.Collections.Generic;
using UnityEngine;

//https://mmems.gitbook.io/calepin/Algorithms/Polygon%20triangulation%20-%20tessellation/Triangulator%20-%20Unity
//https://github.com/mems/calepin/blob/main/Algorithms/Polygon%20triangulation%20-%20tessellation/Triangulator%20-%20Unity.md
public class Triangulator {
    public static Mesh VerticesToMesh(List<Vector2> pathSource) {
        List<Vector2[]> splitLines = LineSplitter.SplitSelfIntersectingLine(pathSource.ToArray());
        int[][] splitTris = new int[splitLines.Count][];
        int totalVerts = 0;
        int totalTris = 0;
        for (int i = 0; i < splitLines.Count; i++) {
            Vector2[] line = splitLines[i];
            Triangulator triangulator = new Triangulator(line);
            int[] tris = triangulator.Triangulate();
            splitTris[i] = tris;

            totalVerts += line.Length;
            totalTris += tris.Length;
        }
        Vector3[] threeDVerts = new Vector3[totalVerts];
        int[] allTris = new int[totalTris];
        int vertIndex = 0;
        int triIndex = 0;
        int triAdjust = 0;
        for (int i = 0; i < splitLines.Count; i++) {
            Vector2[] line = splitLines[i];
            for (int j = 0; j < line.Length; j++) {
                threeDVerts[vertIndex] = line[j];
                vertIndex++;
            }
            int[] tris = splitTris[i];
            for (int j = 0; j < tris.Length; j++) {
                allTris[triIndex] = tris[j] + triAdjust;
                triIndex++;
            }
            triAdjust += line.Length;
        }

        Mesh result = new Mesh {
            vertices = threeDVerts,
            triangles = allTris,
            uv = CalculateUVs(threeDVerts)
        };
        return result;
    }

    private Vector2[] m_points;
    public Triangulator(Vector2[] points) {
        m_points = points;
    }

    public int[] Triangulate() {
        List<int> indices = new List<int>();

        int n = m_points.Length;
        if (n < 3)
            return indices.ToArray();

        int[] V = new int[n];
        if (Area() > 0) {
            for (int v = 0; v < n; v++)
                V[v] = v;
        } else {
            for (int v = 0; v < n; v++)
                V[v] = (n - 1) - v;
        }

        int nv = n;
        int count = 2 * nv;
        for (int m = 0, v = nv - 1; nv > 2;) {
            if ((count--) <= 0)
                return indices.ToArray();

            int u = v;
            if (nv <= u)
                u = 0;
            v = u + 1;
            if (nv <= v)
                v = 0;
            int w = v + 1;
            if (nv <= w)
                w = 0;

            if (Snip(u, v, w, nv, V)) {
                int a, b, c, s, t;
                a = V[u];
                b = V[v];
                c = V[w];
                indices.Add(a);
                indices.Add(b);
                indices.Add(c);
                m++;
                for (s = v, t = v + 1; t < nv; s++, t++)
                    V[s] = V[t];
                nv--;
                count = 2 * nv;
            }
        }

        indices.Reverse();
        return indices.ToArray();
    }

    private float Area() {
        int n = m_points.Length;
        float A = 0.0f;
        for (int p = n - 1, q = 0; q < n; p = q++) {
            Vector2 pval = m_points[p];
            Vector2 qval = m_points[q];
            A += pval.x * qval.y - qval.x * pval.y;
        }
        return (A * 0.5f);
    }

    private bool Snip(int u, int v, int w, int n, int[] V) {
        int p;
        Vector2 A = m_points[V[u]];
        Vector2 B = m_points[V[v]];
        Vector2 C = m_points[V[w]];
        if (Mathf.Epsilon > (((B.x - A.x) * (C.y - A.y)) - ((B.y - A.y) * (C.x - A.x))))
            return false;
        float ax, ay, bx, by, cx, cy, apx, apy, bpx, bpy, cpx, cpy;
        float cCROSSap, bCROSScp, aCROSSbp;
        for (p = 0; p < n; p++) {
            if ((p == u) || (p == v) || (p == w))
                continue;
            Vector2 P = m_points[V[p]];

            ax = C.x - B.x; ay = C.y - B.y;
            bx = A.x - C.x; by = A.y - C.y;
            cx = B.x - A.x; cy = B.y - A.y;
            apx = P.x - A.x; apy = P.y - A.y;
            bpx = P.x - B.x; bpy = P.y - B.y;
            cpx = P.x - C.x; cpy = P.y - C.y;

            aCROSSbp = ax * bpy - ay * bpx;
            cCROSSap = cx * apy - cy * apx;
            bCROSScp = bx * cpy - by * cpx;

            if ((aCROSSbp >= 0.0f) && (bCROSScp >= 0.0f) && (cCROSSap >= 0.0f))
                return false;
        }
        return true;
    }

    //private bool InsideTriangle(Vector2 A, Vector2 B, Vector2 C, Vector2 P) {
    //    float ax, ay, bx, by, cx, cy, apx, apy, bpx, bpy, cpx, cpy;
    //    float cCROSSap, bCROSScp, aCROSSbp;

    //    ax = C.x - B.x; ay = C.y - B.y;
    //    bx = A.x - C.x; by = A.y - C.y;
    //    cx = B.x - A.x; cy = B.y - A.y;
    //    apx = P.x - A.x; apy = P.y - A.y;
    //    bpx = P.x - B.x; bpy = P.y - B.y;
    //    cpx = P.x - C.x; cpy = P.y - C.y;

    //    aCROSSbp = ax * bpy - ay * bpx;
    //    cCROSSap = cx * apy - cy * apx;
    //    bCROSScp = bx * cpy - by * cpx;

    //    return ((aCROSSbp >= 0.0f) && (bCROSScp >= 0.0f) && (cCROSSap >= 0.0f));
    //}
    private static Vector2[] CalculateUVs(Vector3[] points) {
        Vector2[] result = new Vector2[points.Length];
        for (int i = 0; i < points.Length; i++) {
            Vector2 point = points[i];
            point.x += 0.5f;
            point.y += 0.5f;
            result[i] = point;
        }
        return result;
    }
}
