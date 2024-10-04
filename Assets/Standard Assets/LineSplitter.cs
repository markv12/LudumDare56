using System.Collections.Generic;
using UnityEngine;

public static class LineSplitter {
    public static List<Vector2[]> SplitSelfIntersectingLine(Vector2[] line) {
        List<Vector2[]> result = new List<Vector2[]>();

        List<Vector2> currentLine = new List<Vector2>(32);
        int lengthMinus1 = line.Length - 1;
        int j;
        for (int i = 0; i < lengthMinus1; i++) {
            currentLine.Add(line[i]);
            ref Vector2 start1 = ref line[i];
            ref Vector2 end1 = ref line[i + 1];
            float x1Diff = end1.x - start1.x;
            float y1Diff = end1.y - start1.y;

            for (j = 0; j < lengthMinus1; j++) {
                int diff = i - j;
                if (diff > 1 || diff < -1) {
                    ref Vector2 start2 = ref line[j];
                    ref Vector2 end2 = ref line[j + 1];

                    float d1 = y1Diff * (start2.x - end1.x) - x1Diff * (start2.y - end1.y);
                    float d2 = y1Diff * (end2.x - end1.x) - x1Diff * (end2.y - end1.y);
                    if((d1 > 0 && d2 < 0) || (d1 < 0 && d2 > 0)) {
                        float d3 = (end2.y - start2.y) * (start1.x - end2.x) - (end2.x - start2.x) * (start1.y - end2.y);
                        float d4 = (end2.y - start2.y) * (end1.x - end2.x) - (end2.x - start2.x) * (end1.y - end2.y);
                        if ((d3 > 0 && d4 < 0) || (d3 < 0 && d4 > 0)) {
                            Vector2 intersectionPoint = GetIntersectionPoint(start1, end1, start2, end2);
                            currentLine.Add(intersectionPoint);
                            result.Add(currentLine.ToArray());
                            currentLine.Clear(); //Reuse current line
                            break;
                        }
                    }
                }
            }
        }
        result.Add(currentLine.ToArray());
        result = ReconnectLines(result);

        return result;
    }

    private static List<Vector2[]> ReconnectLines(List<Vector2[]> lines) {
        if (lines.Count >= 3) {
            Vector2[] firstLine = lines[0];
            Vector2[] lastLine = lines[lines.Count - 1];
            lines.Add(CombineLines(lastLine, firstLine, false));
            lines.Remove(firstLine);
            lines.Remove(lastLine);
        }
        if (lines.Count >= 4) {
            int lineCount = lines.Count;
            for (int i = 0; i < lineCount; i++) {
                if(i >= 0) { //Somehow i-- makes i less than zero sometimes
                    Vector2[] line1 = lines[i];
                    for (int j = 0; j < lineCount; j++) {
                        if (i != j) {
                            Vector2[] line2 = lines[j];
                            ref Vector2 start1 = ref line1[0];
                            ref Vector2 end1 = ref line1[line1.Length - 1];
                            ref Vector2 start2 = ref line2[0];
                            ref Vector2 end2 = ref line2[line2.Length - 1];
                            bool startsMatch = start1.x == start2.x && start1.y == start2.y && end1.x == end2.x && end1.y == end2.y;
                            if (startsMatch || (start1.x == end2.x && start1.y == end2.y && end1.x == start2.x && end1.y == start2.y)) {
                                Vector2[] newLine = CombineLines(line1, line2, true);
                                lines.Remove(line1);
                                lines.Remove(line2);
                                lines.Add(newLine);
                                lineCount = lines.Count;
                                i--;
                                j--;
                            }
                        }
                    }
                }
            }
        }

        return lines;
    }

    private static Vector2[] CombineLines(Vector2[] line1, Vector2[] line2, bool trim) {
        int resultLength = line1.Length + line2.Length - (trim ? 2 : 0);
        Vector2[] result = new Vector2[resultLength];
        int index = 0;
        for (int i = 0; i < line1.Length; i++) {
            result[index] = line1[i];
            index++;
        }
        int iStart = trim ? 1 : 0;
        int iEnd = line2.Length - (trim ? 1 : 0);
        for (int i = iStart; i < iEnd; i++) {
            result[index] = line2[i];
            index++;
        }
        return result;
    }

    static Vector2 GetIntersectionPoint(Vector2 p1, Vector2 q1, Vector2 p2, Vector2 q2) {
        float a1 = q1.y - p1.y;
        float b1 = p1.x - q1.x;
        float c1 = a1 * p1.x + b1 * p1.y;

        float a2 = q2.y - p2.y;
        float b2 = p2.x - q2.x;
        float c2 = a2 * p2.x + b2 * p2.y;

        float determinant = a1 * b2 - a2 * b1;

        float x = (b2 * c1 - b1 * c2) / determinant;
        float y = (a1 * c2 - a2 * c1) / determinant;

        return new Vector2(x, y);
    }

    private static float direction(Vector2 p, Vector2 q, Vector2 r) {
        return (q.y - p.y) * (r.x - q.x) - (q.x - p.x) * (r.y - q.y);
    }

    private static bool areCollinearAndOverlapping(Vector2 a1, Vector2 b1, Vector2 a2, Vector2 b2) {
        // Check if the line segments are collinear
        if (direction(a1, b1, a2) == 0) {
            // Check if the line segments overlap
            if (a2.x <= Mathf.Max(a1.x, b1.x) && a2.x >= Mathf.Min(a1.x, b1.x) && a2.y <= Mathf.Max(a1.y, b1.y) && a2.y >= Mathf.Min(a1.y, b1.y)) {
                return true;
            }
        }
        return false;
    }

    private static bool DoIntersect(Vector2 a1, Vector2 b1, Vector2 a2, Vector2 b2) {
        // Compute the directions of the four line segments
        float d1 = direction(a1, b1, a2);
        float d2 = direction(a1, b1, b2);
        float d3 = direction(a2, b2, a1);
        float d4 = direction(a2, b2, b1);

        // Check if the two line segments intersect
        if (((d1 > 0 && d2 < 0) || (d1 < 0 && d2 > 0)) && ((d3 > 0 && d4 < 0) || (d3 < 0 && d4 > 0))) {
            return true;
        }

        // Check if the line segments are collinear and overlapping
        if (areCollinearAndOverlapping(a1, b1, a2, b2) || areCollinearAndOverlapping(a2, b2, a1, b1)) {
            return true;
        }

        return false;
    }
}
