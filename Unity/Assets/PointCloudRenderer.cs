using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using RosSharp.RosBridgeClient;

public class PointCloudRenderer : MonoBehaviour
{
    public PointCloudSubscriber pc2sub;
    public OdometrySubscriber odomsub;
    private Vector3[] pc2pos = new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 1, 0) };
    private Color[] pc2col = new Color[] { new Color(1f, 0f, 0f), new Color(0f, 1f, 0f) };
    public float pointSize = 1f;
    List<GameObject> gemList;
    GameObject cube;
    Mesh mesh;
    MeshFilter mf;

    // Use this for initialization
    void Start()
    {
        gemList = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        pc2pos = pc2sub.GetPCL();

        if (pc2pos == null)
        {
            return;
        }

        pc2col = pc2sub.GetPCLColor();

        cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.GetComponent<MeshRenderer>().material = new Material(Shader.Find("Custom/PointCloudShader"));
        mesh = new Mesh { indexFormat = UnityEngine.Rendering.IndexFormat.UInt32 };
        cube.GetComponent<MeshRenderer>().material.SetFloat("_PointSize", pointSize);
        mesh.Clear();
        mesh.vertices = pc2pos;
        mesh.colors = pc2col;
        int[] indices = new int[pc2pos.Length];
        for (int i = 0; i < pc2pos.Length; i++)
        {
            indices[i] = i;
        }

        mesh.SetIndices(indices, MeshTopology.Points, 0);
        //mesh.triangles = indices;
        cube.GetComponent<MeshFilter>().mesh = mesh;
        pc2sub.ClearPCL();

        transform.position = odomsub.GetPos();
        transform.rotation = odomsub.GetRot();
        cube.transform.position = odomsub.GetPos();
        cube.transform.rotation = odomsub.GetRot();
        gemList.Add(cube);
    }

}
