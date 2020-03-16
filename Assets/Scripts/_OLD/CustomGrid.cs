using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class CustomGrid : MonoBehaviour
{
    public int size;
    public bool ifToNormal = false;
    private Vector3[] vertices;
    private float[] dist;
    private Mesh mesh;
    public LayerMask layer = ~0;

    private int notGood = 0;
    private int lastNotGood = -1;

    private void Start()
    {
        transform.rotation = Quaternion.LookRotation(-transform.forward) * Quaternion.Euler(0.0f, 0.0f, transform.rotation.eulerAngles.z);
        Generate();
        if (notGood > ((size + 1) * (size + 1) - 1) / 2 && !ifToNormal)
        {
            ifToNormal = true;
            notGood = 0;
            transform.position = transform.position - (-transform.right * size / 2 - transform.up * size / 2) * transform.localScale.x;
            Generate();
        }

        gameObject.isStatic = true; //obiekt statyczny
        DecalPoolScript.SharedInstanceDec.addDecal(gameObject);
        this.enabled = false;
    }

    private void Generate()
    {
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Decal";

        RaycastHit h;

        if (Physics.Raycast(transform.position + 5 * transform.forward, -transform.forward, out h, 10.0f, layer))   //przod
        {
            if (ifToNormal || Vector3.Angle(h.normal, transform.forward) > 80.0f)
            {
                transform.rotation = Quaternion.LookRotation(h.normal, Vector3.up);
            }
            else
            {
                transform.rotation = Quaternion.LookRotation((transform.forward + h.normal).normalized, transform.up);
            }
        }
        else
        {
            if (Physics.Raycast(transform.position + 5 * transform.forward, -transform.up, out h, 10.0f, layer)) //dol
            {
                if (ifToNormal || Vector3.Angle(h.normal, transform.forward) > 80.0f)
                {
                    transform.rotation = Quaternion.LookRotation(h.normal, Vector3.up);
                }
                else
                {
                    transform.rotation = Quaternion.LookRotation((transform.forward + h.normal).normalized, transform.up);
                }
            }
            else
            {
                if (Physics.Raycast(transform.position + 5 * transform.forward, transform.up, out h, 10.0f, layer)) //gora
                {
                    if (ifToNormal || Vector3.Angle(h.normal, transform.forward) > 80.0f)
                    {
                        transform.rotation = Quaternion.LookRotation(h.normal, Vector3.up);
                    }
                    else
                    {
                        transform.rotation = Quaternion.LookRotation((transform.forward + h.normal).normalized, transform.up);
                    }
                }
                else
                {
                    if (Physics.Raycast(transform.position + 5 * transform.forward, -transform.right, out h, 10.0f, layer)) //lewo
                    {
                        if (ifToNormal || Vector3.Angle(h.normal, transform.forward) > 80.0f)
                        {
                            transform.rotation = Quaternion.LookRotation(h.normal, Vector3.up);
                        }
                        else
                        {
                            transform.rotation = Quaternion.LookRotation((transform.forward + h.normal).normalized, transform.up);
                        }
                    }
                    else
                    {
                        if (Physics.Raycast(transform.position + 5 * transform.forward, transform.right, out h, 10.0f, layer))  //prawo
                        {
                            if (ifToNormal || Vector3.Angle(h.normal, transform.forward) > 80.0f)
                            {
                                transform.rotation = Quaternion.LookRotation(h.normal, Vector3.up);
                            }
                            else
                            {
                                transform.rotation = Quaternion.LookRotation((transform.forward + h.normal).normalized, transform.up);
                            }
                        }
                    }
                }
            }
        }

        Vector3 center = transform.position;
        transform.position = transform.position + (-transform.right * size / 2 - transform.up * size / 2) * transform.localScale.x;

        vertices = new Vector3[(size + 1) * (size + 1)];
        dist = new float[(size + 1) * (size + 1)];
        Vector3[] normals = new Vector3[(size + 1) * (size + 1)];
        Vector2[] uv = new Vector2[vertices.Length];
        Vector4[] tangents = new Vector4[vertices.Length];
        Vector4 tangent = new Vector4(1f, 0f, 0f, -1f);

        for (int i = 0, y = 0; y <= size; y++)
        {
            for (int x = 0; x <= size; x++, i++)
            {
                vertices[i] = transform.TransformPoint(new Vector3(x, y));
                dist[i] = (new Vector3(x, y, 4.0f) - new Vector3((size / 2), (size / 2)) * transform.localScale.x).magnitude;
                if (Physics.Raycast(vertices[i] + transform.forward, -transform.forward, out h, 4.0f, layer))
                {
                    vertices[i] = h.point; // h.point - transform.position;  
                    normals[i] = h.normal;
                }
                else
                {
                    for (float lenght = 1.0f; lenght > 0.0f; lenght -= 0.1f)
                    {
                        if (Physics.Raycast(vertices[i] + transform.forward, ((center - transform.forward * 4.0f) * lenght - vertices[i]).normalized, out h, (((center - transform.forward * 4.0f) * lenght - vertices[i])).magnitude, layer))
                        {
                            vertices[i] = h.point; // h.point - transform.position;  
                            normals[i] = h.normal;
                            break;
                        }
                        else
                        {
                            if (Physics.Raycast(transform.InverseTransformPoint(center) + transform.forward, -transform.forward, out h, 4.0f, layer))
                            {
                                vertices[i] = h.point; // h.point - transform.position;  
                                normals[i] = h.normal;
                                break;
                            }
                            else
                            {
                                if (i == 0)
                                {
                                    vertices[i] = center;
                                    if (lastNotGood != i)
                                        notGood++;
                                    lastNotGood = i;
                                }
                                else
                                {
                                    vertices[i] = transform.TransformPoint(vertices[i - 1] * transform.localScale.x);
                                    if (lastNotGood != i)
                                        notGood++;
                                    lastNotGood = i;
                                }
                            }
                        }
                    }
                }

                vertices[i] = transform.InverseTransformPoint(vertices[i]);
                if (((vertices[i] - new Vector3((size / 2), (size / 2)) * transform.localScale.x).magnitude) > dist[i] / transform.localScale.x)
                {
                    vertices[i] = (vertices[i] - new Vector3((size / 2), (size / 2)) * transform.localScale.x).normalized * 0.5f + new Vector3((size / 2), (size / 2)) * transform.localScale.x;
                }

                uv[i] = new Vector2((float)x / size, (float)y / size);
                tangents[i] = tangent;
            }
        }
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.tangents = tangents;
        mesh.normals = normals;

        transform.localScale *= 0.99f;

        int[] triangles = new int[size * size * 6];
        for (int ti = 0, vi = 0, y = 0; y < size; y++, vi++)
        {
            for (int x = 0; x < size; x++, ti += 6, vi++)
            {
                triangles[ti] = vi;
                triangles[ti + 3] = triangles[ti + 2] = vi + size + 1;
                triangles[ti + 4] = triangles[ti + 1] = vi + 1;
                triangles[ti + 5] = vi + size + 2;
            }
        }
        mesh.triangles = triangles;
    }
}
