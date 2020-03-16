using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class CustomGrid3 : MonoBehaviour
{

    public int complexity;
    private float distance = 3.0f;
    private Vector3[] vertices;
    private Mesh mesh;
    public LayerMask layer = ~0;
    public float checkAngle = 20.0f;
    public float offset = 0.05f;


    private void Start()
    {
        transform.rotation = Quaternion.LookRotation(-transform.forward) * Quaternion.Euler(0.0f, 0.0f, transform.rotation.eulerAngles.z);
        Generate();

        gameObject.isStatic = true; //obiekt statyczny
        DecalPoolScript.SharedInstanceDec.addDecal(gameObject);
        this.enabled = false;
    }



    private void Generate()
    {
        distance = transform.localScale.x / 2;

        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Decal";

        RaycastHit h;

        if (Physics.Raycast(transform.position + transform.forward * distance * 0.2f, -transform.forward, out h, distance, layer))
        {
            transform.position = h.point;
            // centerDist = (h.point - transform.position - transform.forward * distance).magnitude;
            transform.rotation = Quaternion.LookRotation(h.normal, transform.up);
        }
        else
        {
            if (Physics.Raycast(transform.position + transform.forward * distance * 0.2f, Quaternion.Euler(checkAngle, 0, 0) * (-transform.forward), out h, distance, layer))
            {
                transform.position = h.point;
                //  centerDist = (h.point - transform.position - transform.forward * distance).magnitude;
                transform.rotation = Quaternion.LookRotation(h.normal, transform.up);
            }
            else
            {
                if (Physics.Raycast(transform.position + transform.forward * distance * 0.2f, Quaternion.Euler(-checkAngle, 0, 0) * (-transform.forward), out h, distance, layer))
                {
                    transform.position = h.point;
                    //  centerDist = (h.point - transform.position - transform.forward * distance).magnitude;
                    transform.rotation = Quaternion.LookRotation(h.normal, transform.up);
                }
                else
                {
                    if (Physics.Raycast(transform.position + transform.forward * distance * 0.2f, Quaternion.Euler(0, checkAngle, 0) * (-transform.forward), out h, distance, layer))
                    {
                        transform.position = h.point;
                        //  centerDist = (h.point - transform.position - transform.forward * distance).magnitude;
                        transform.rotation = Quaternion.LookRotation(h.normal, transform.up);
                    }
                    else
                    {
                        if (Physics.Raycast(transform.position + transform.forward * distance * 0.2f, Quaternion.Euler(0, -checkAngle, 0) * (-transform.forward), out h, distance, layer))
                        {
                            transform.position = h.point;
                            //  centerDist = (h.point - transform.position - transform.forward * distance).magnitude;
                            transform.rotation = Quaternion.LookRotation(h.normal, transform.up);
                        }
                        else
                        {
                            Destroy(this.gameObject);
                        }
                    }
                }
            }
        }

        Vector3 centerplus = transform.position + h.normal * distance;
        Vector3 centerminus = transform.position - h.normal * distance;

        vertices = new Vector3[(complexity + 1) * (complexity + 1)];
        bool[] notGood = new bool[(complexity + 1) * (complexity + 1)];
        Vector3[] normals = new Vector3[(complexity + 1) * (complexity + 1)];
        Vector2[] uv = new Vector2[vertices.Length];
        Vector4[] tangents = new Vector4[vertices.Length];
        Vector4 tangent = new Vector4(1f, 0f, 0f, -1f);

        for (int i = 0, y = 0; y <= complexity; y++)
        {
            for (int x = 0; x <= complexity; x++, i++)
            {
                vertices[i] = transform.TransformPoint(new Vector3(((float)x - 0.5f * complexity) / complexity, ((float)y - 0.5f * complexity) / complexity, 0.0f));

                for (float g = 0.0f; g < 0.5f; g = g + 0.1f / distance)
                {
                    if(Physics.Raycast(centerplus - transform.forward * distance * 0.1f, (Vector3.Lerp(vertices[i], transform.position, g) - centerplus).normalized, out h, (Vector3.Lerp(vertices[i], transform.position, g) - centerplus).magnitude, layer))
                    {
                        vertices[i] = h.point + h.normal * offset;
                        notGood[i] = false;
                        break;
                    }
                    else
                    {
                        if (Physics.Raycast(Vector3.Lerp(vertices[i], transform.position, g) + transform.forward * distance * 0.1f, (centerminus - Vector3.Lerp(vertices[i], transform.position, g)).normalized, out h, (centerminus - Vector3.Lerp(vertices[i], transform.position, g)).magnitude, layer))
                        {
                            vertices[i] = h.point + h.normal * offset;
                            notGood[i] = false;
                            break;
                        }
                        else
                        {
                            notGood[i] = true;
                        }
                    }
                }
            }
        }

        for (int i = 0, y = 0; y <= complexity; y++)
        {
            for (int x = 0; x <= complexity; x++, i++)
            {
                if (notGood[i])
                {
                    Vector3 sum = Vector3.zero;
                    int many = 0;

                    if ((x - 1) >= 0 && !notGood[i - 1])
                    {
                        sum += vertices[i - 1];
                        many++;
                    }
                    if ((x + 1) < complexity && !notGood[i + 1])
                    {
                        sum += vertices[i + 1];
                        many++;
                    }
                    if ((y - 1) >= 0 && !notGood[i - complexity])
                    {
                        sum += vertices[i - complexity];
                        many++;
                    }
                    if ((y + 1) < complexity && !notGood[i + complexity])
                    {
                        sum += vertices[i + complexity];
                        many++;
                    }
                    if (many > 0)
                    {
                        vertices[i] = sum / many;
                    }
                    else
                        vertices[i] = transform.position;
                }
            }
        }

        for (int i = 0, y = 0; y <= complexity; y++)
        {
            for (int x = 0; x <= complexity; x++, i++)
            {
                vertices[i] = transform.InverseTransformPoint(vertices[i]);

                uv[i] = new Vector2((float)x / complexity, (float)y / complexity);
                tangents[i] = tangent;
            }
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.tangents = tangents;
        mesh.normals = normals;


        int[] triangles = new int[complexity * complexity * 6];
        for (int ti = 0, vi = 0, y = 0; y < complexity; y++, vi++)
        {
            for (int x = 0; x < complexity; x++, ti += 6, vi++)
            {
                triangles[ti] = vi;
                triangles[ti + 3] = triangles[ti + 2] = vi + complexity + 1;
                triangles[ti + 4] = triangles[ti + 1] = vi + 1;
                triangles[ti + 5] = vi + complexity + 2;
            }
        }
        mesh.triangles = triangles;
    }
}
