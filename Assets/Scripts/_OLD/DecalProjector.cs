/*
 * Created by: Ethan Currah
 * Last Updated: 2015/09/11
 * Notes: Attach to empty gameobject. Will detect and mark vertices on mesh of all 
 *        objects set as objectsToDecal in inspector that are within the specified Bounds.
 */

#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class DecalProjector : MonoBehaviour
{


    public GameObject[] objectsToDecal;

    public Bounds bounds = new Bounds(Vector3.zero, new Vector3(2.2f, 1.0f, 2.2f));
    public Color boundsColor = Color.green;

    public bool debugVertices = true;
    public Color vertexColor = Color.blue;

    public bool drawVertGizmos = false;
    public float vertGizmoRadius = 0.05f;

    private List<Vector3> inboundsVertices = new List<Vector3>();

    private Vector3 v3FrontTopLeft;
    private Vector3 v3FrontTopRight;
    private Vector3 v3FrontBottomLeft;
    private Vector3 v3FrontBottomRight;
    private Vector3 v3BackTopLeft;
    private Vector3 v3BackTopRight;
    private Vector3 v3BackBottomLeft;
    private Vector3 v3BackBottomRight;


    // Unity Lifecycle calls
    void Update()
    {
        CalcPositions();
        DrawBounds();
        CheckVertices(objectsToDecal);
        if (debugVertices)
            DrawVertices();
    }


    void OnDrawGizmos()
    {
        if (drawVertGizmos)
            DrawVertexGizmos();
    }


    // Workhorse Methods
    void CalcPositions()
    {
        bounds.center = transform.position;
        //transform.localPosition = transform.InverseTransformPoint(transform.position);

        Vector3 v3Center = bounds.center;
        Vector3 v3Extents = bounds.extents;

        // calculate positions based on bounds
        v3FrontTopLeft = new Vector3(v3Center.x - v3Extents.x, v3Center.y + v3Extents.y, v3Center.z - v3Extents.z);  // Front top left corner
        v3FrontTopRight = new Vector3(v3Center.x + v3Extents.x, v3Center.y + v3Extents.y, v3Center.z - v3Extents.z);  // Front top right corner
        v3FrontBottomLeft = new Vector3(v3Center.x - v3Extents.x, v3Center.y - v3Extents.y, v3Center.z - v3Extents.z);  // Front bottom left corner
        v3FrontBottomRight = new Vector3(v3Center.x + v3Extents.x, v3Center.y - v3Extents.y, v3Center.z - v3Extents.z);  // Front bottom right corner
        v3BackTopLeft = new Vector3(v3Center.x - v3Extents.x, v3Center.y + v3Extents.y, v3Center.z + v3Extents.z);  // Back top left corner
        v3BackTopRight = new Vector3(v3Center.x + v3Extents.x, v3Center.y + v3Extents.y, v3Center.z + v3Extents.z);  // Back top right corner
        v3BackBottomLeft = new Vector3(v3Center.x - v3Extents.x, v3Center.y - v3Extents.y, v3Center.z + v3Extents.z);  // Back bottom left corner
        v3BackBottomRight = new Vector3(v3Center.x + v3Extents.x, v3Center.y - v3Extents.y, v3Center.z + v3Extents.z);  // Back bottom right corner

        // translate to world coordinates
        /* removed as this is already in world coordinates
        v3FrontTopLeft = transform.TransformPoint(v3FrontTopLeft);
        v3FrontTopRight = transform.TransformPoint(v3FrontTopRight);
        v3FrontBottomLeft = transform.TransformPoint(v3FrontBottomLeft);
        v3FrontBottomRight = transform.TransformPoint(v3FrontBottomRight);
        v3BackTopLeft = transform.TransformPoint(v3BackTopLeft);
        v3BackTopRight = transform.TransformPoint(v3BackTopRight);
        v3BackBottomLeft = transform.TransformPoint(v3BackBottomLeft);
        v3BackBottomRight = transform.TransformPoint(v3BackBottomRight);
         * */
    }

    void CheckVertices(GameObject[] objs)
    {
        inboundsVertices.Clear();
        foreach (GameObject obj in objs)
        {
            if (obj == null)
                continue;

            if (obj.activeSelf == false)
                continue;

            MeshFilter mf = obj.GetComponent<MeshFilter>();
            if (mf == null)
                continue;

            Vector3[] verticesToCheck = obj.GetComponent<MeshFilter>().sharedMesh.vertices;
            foreach (Vector3 vertex in verticesToCheck)
            {
                Vector3 pos = obj.transform.TransformPoint(vertex);
                if (bounds.Contains(pos))
                {
                    inboundsVertices.Add(pos);
                }
            }

        }
    }


    // Methods for Drawing Debug-lines and Gizmos
    void DrawBounds()
    {
        Debug.DrawLine(v3FrontTopLeft, v3FrontTopRight, boundsColor);
        Debug.DrawLine(v3FrontTopRight, v3FrontBottomRight, boundsColor);
        Debug.DrawLine(v3FrontBottomRight, v3FrontBottomLeft, boundsColor);
        Debug.DrawLine(v3FrontBottomLeft, v3FrontTopLeft, boundsColor);

        Debug.DrawLine(v3BackTopLeft, v3BackTopRight, boundsColor);
        Debug.DrawLine(v3BackTopRight, v3BackBottomRight, boundsColor);
        Debug.DrawLine(v3BackBottomRight, v3BackBottomLeft, boundsColor);
        Debug.DrawLine(v3BackBottomLeft, v3BackTopLeft, boundsColor);

        Debug.DrawLine(v3FrontTopLeft, v3BackTopLeft, boundsColor);
        Debug.DrawLine(v3FrontTopRight, v3BackTopRight, boundsColor);
        Debug.DrawLine(v3FrontBottomRight, v3BackBottomRight, boundsColor);
        Debug.DrawLine(v3FrontBottomLeft, v3BackBottomLeft, boundsColor);
    }

    void DrawVertices()
    {
        foreach (Vector3 vertex in inboundsVertices)
        {
            Debug.DrawLine(vertex + Vector3.down * 0.1f, vertex + Vector3.up * 0.1f, vertexColor);
            Debug.DrawLine(vertex + Vector3.left * 0.1f, vertex + Vector3.right * 0.1f, vertexColor);
            Debug.DrawLine(vertex + Vector3.back * 0.1f, vertex + Vector3.forward * 0.1f, vertexColor);
        }
    }


    void DrawVertexGizmos()
    {
        foreach (Vector3 vertex in inboundsVertices)
        {
            Gizmos.DrawSphere(vertex, vertGizmoRadius);
        }
    }

}

#endif