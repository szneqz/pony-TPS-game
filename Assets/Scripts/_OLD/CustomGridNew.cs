using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class CustomGridNew : MonoBehaviour
{
    public int size;
    public float distance = 3.0f;
    public float rotAngle = 20.0f;
    private Vector3[] vertices;
    private bool[] onSurface;
    private Mesh mesh;
    public LayerMask layer = ~0;
    public GameObject ball;
    public float maxDist = 1.5f;


    private void Start()
    {
        Instantiate(ball, transform.position, Quaternion.identity); //<------------------------------------------------------------------------------debug 2
        transform.rotation = Quaternion.LookRotation(-transform.forward) * Quaternion.Euler(0.0f, 0.0f, transform.rotation.eulerAngles.z);
        Generate();

        gameObject.isStatic = true; //obiekt statyczny
        DecalPoolScript.SharedInstanceDec.addDecal(gameObject);
        this.enabled = false;
    }

    private Vector3 rotatingMesh(Vector3 vertice, Quaternion rot, Quaternion mainRot)
    {
        vertice = transform.InverseTransformPoint(vertice); //wrzucam local space
        transform.rotation = rot * mainRot;   //obracam
        vertice = transform.TransformPoint(vertice);        //wrzucam world space
        return vertice;
    }

    private void Generate()
    {
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Decal";

        RaycastHit h;
        Vector3 normalCenter = Vector3.zero;
        Vector3 tempRot = transform.forward;

        if (Physics.Raycast(transform.position , -transform.forward, out h, distance, layer))  //przod
        {
            //  tempRot *= Quaternion.LookRotation(h.normal, transform.up);
            tempRot += h.normal;
            if (normalCenter == Vector3.zero)
            normalCenter = h.point;
        }
            if (Physics.Raycast(transform.position - transform.up * transform.localScale.x / (size / 2), -transform.forward, out h, distance, layer)) //dol
            {
            //  tempRot *= Quaternion.LookRotation(h.normal, transform.up);
            tempRot += h.normal;
            if (normalCenter == Vector3.zero)
                    normalCenter = h.point;
            }
                if (Physics.Raycast(transform.position + transform.up * transform.localScale.x / (size / 2), -transform.forward, out h, distance, layer)) //gora
                {
            //   tempRot *= Quaternion.LookRotation(h.normal, transform.up);
            tempRot += h.normal;
            if (normalCenter == Vector3.zero)
                        normalCenter = h.point;
                }
                    if (Physics.Raycast(transform.position - transform.right * transform.localScale.x / (size / 2), -transform.forward, out h, distance, layer)) //lewo
                    {
            //   tempRot *= Quaternion.LookRotation(h.normal, transform.up);
            tempRot += h.normal;
            if (normalCenter == Vector3.zero)
                            normalCenter = h.point;
                    }
                        if (Physics.Raycast(transform.position + transform.right * transform.localScale.x / (size/2), -transform.forward, out h, distance, layer))  //prawo
                        {
            //   tempRot *= Quaternion.LookRotation(h.normal, transform.up);
            tempRot += h.normal;
            if (normalCenter == Vector3.zero)
                                normalCenter = h.point;
                        }
                            if (normalCenter == Vector3.zero)
                                normalCenter = transform.position;

        transform.rotation = Quaternion.LookRotation(tempRot.normalized, transform.up);

        if (Physics.Raycast(transform.position + transform.forward * 0.5f * distance, -tempRot.normalized, out h, 2 * distance, layer))  //przod
        {
            normalCenter = h.point;
        }

      //      for (int g = 0; g < 5; g++)
      //  {
      //      Instantiate(ball, transform.position + transform.forward * g, Quaternion.identity);   //taki debuuuuuuuuuuuuuuuug
     //   }

        Quaternion mainRot = transform.rotation;
        Vector3 center = transform.position;
        transform.position = transform.position + (-transform.right * (transform.localScale.x) / 2 - transform.up * (transform.localScale.x) / 2);

        vertices = new Vector3[(size + 1) * (size + 1)];
        onSurface = new bool[(size + 1) * (size + 1)];
        float[] dist = new float[(size + 1) * (size + 1)];
        Vector3[] normals = new Vector3[(size + 1) * (size + 1)];
        Vector2[] uv = new Vector2[vertices.Length];
        Vector4[] tangents = new Vector4[vertices.Length];
        Vector4 tangent = new Vector4(1f, 0f, 0f, -1f);

        for (int i = 0, y = 0; y <= size; y++)
        {
            for (int x = 0; x <= size; x++, i++)
            {
                transform.rotation = mainRot;   //rotation bedzie sie zmieniac z czasem wiec zawsze bedziemy do niego wracac
                vertices[i] = transform.TransformPoint(new Vector3((float)x / size, (float)y / size));
                dist[i] = (vertices[i] - center).magnitude; //odleglosc kazdego punktu od centrum do dostosowania odleglosci

                if(Physics.Raycast(vertices[i], -transform.forward, out h, distance, layer))
                {
                    vertices[i] = h.point;
                    onSurface[i] = true;
                }
                else
                {
                    onSurface[i] = false;
                    vertices[i] = rotatingMesh(vertices[i], mainRot, Quaternion.Euler(rotAngle, 0.0f, 0.0f));   //rotAngle w dol

                    if(Physics.Raycast(vertices[i], -transform.forward, out h, distance, layer))
                    {
                        vertices[i] = h.point;
                        onSurface[i] = true;
                    }
                    else
                    {
                        vertices[i] = rotatingMesh(vertices[i], mainRot, Quaternion.Euler(-rotAngle, 0.0f, 0.0f));   //rotAngle w gore

                        if (Physics.Raycast(vertices[i], -transform.forward, out h, distance, layer))
                        {
                            vertices[i] = h.point;
                            onSurface[i] = true;
                        }
                        else
                        {
                            vertices[i] = rotatingMesh(vertices[i], mainRot, Quaternion.Euler(0.0f, rotAngle, 0.0f));   //rotAngle w prawo

                            if (Physics.Raycast(vertices[i], -transform.forward, out h, distance, layer))
                            {
                                vertices[i] = h.point;
                                onSurface[i] = true;
                            }
                            else
                            {
                                vertices[i] = rotatingMesh(vertices[i], mainRot, Quaternion.Euler(0.0f, -rotAngle, 0.0f));   //rotAngle w lewo

                                if (Physics.Raycast(vertices[i], -transform.forward, out h, distance, layer))
                                {
                                    vertices[i] = h.point;
                                    onSurface[i] = true;
                                }
                                else
                                {
                                    transform.rotation = mainRot;   //resetowanie obrotu
                                    for (float lenght = 1.0f; lenght > 0.0f; lenght -= 0.1f)
                                    {
                                        if (Physics.Raycast(vertices[i], ((-transform.forward * lenght).normalized + ((normalCenter - vertices[i]) * (1.0f - lenght)).normalized).normalized, out h, distance, layer))
                                        {   //lej
                                            vertices[i] = h.point;
                                            onSurface[i] = true;
                                            break;  //wybijam z leja
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (onSurface[i] == false)
                {
                    if (i > 0)
                    {
                        vertices[i] = vertices[i - 1];
                    }
                    else
                    vertices[i] = normalCenter;
                }

                if ((vertices[i] - normalCenter).magnitude > dist[i] * maxDist + maxDist)
                {
                    vertices[i] = normalCenter + ((vertices[i] - normalCenter).normalized) * (dist[i] * maxDist + maxDist);    //maksymalne dist to maxDist poczatkowego dist
                }

                transform.rotation = mainRot;
                vertices[i] = transform.InverseTransformPoint(vertices[i]);

                uv[i] = new Vector2((float)x / size, (float)y / size);
                tangents[i] = tangent;

            }
        }
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.tangents = tangents;
        mesh.normals = normals;


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
