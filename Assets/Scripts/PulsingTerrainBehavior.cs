using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulsingTerrainBehavior : MonoBehaviour
{
    Mesh mesh;
    MeshRenderer meshRenderer;

    Vector3[] vertices;
    int[] triangles;
    Vector2[] uvs;

    [Header ("Mesh Parameters")]
    int currentWidth = 0;
    int currentLength = 0;
    float currentSize = 0;
    [SerializeField] int width = 40;
    [SerializeField] int length = 40;
    [SerializeField] float size = 0.25f;
    [SerializeField] bool updateMesh = false;

    [Header("Perlin Noise Parameters")]
    [SerializeField] float amplitude = 10.0f;
    [SerializeField] float timeMultiplication = 10.0f;
    [SerializeField] float perlinDefinition = 1.0f;
    Vector3 perlinOffset = Vector3.zero;

    [SerializeField] float refreshingTime = 0.5f;
    [SerializeField] AnimationCurve animationCurve;
    float timer = 0;

    [Header("Scene")] [SerializeField] GameObject _directionalLight;

    static readonly int WorldSpaceLightDirection = Shader.PropertyToID("_WorldSpaceLightDirection");

    private void Start() {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshCollider>().sharedMesh = mesh;

        CreateMesh();
        UpdateMesh();

        meshRenderer = GetComponent<MeshRenderer>();

        //Set light direction
        meshRenderer.material.SetVector(WorldSpaceLightDirection, _directionalLight.transform.forward * -1);
        //GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 10.0f);
        //FindObjectOfType<Camera>().GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 10.0f);
    }

    private void Update() {
        timer += Time.deltaTime;
        if(timer >= refreshingTime) {
            timer -= refreshingTime;
            perlinOffset = transform.position;
            NewLineOnMesh();
        }

        if(updateMesh) {
            //CreateMesh();
            UpdateMesh();
            updateMesh = false;
        }
    }

    void CreateMesh() {
        currentWidth = width;
        currentLength = length;
        currentSize = size;

        vertices = new Vector3[(currentWidth + 1) * (currentLength + 1)];
        uvs = new Vector2[(currentWidth + 1) * (currentLength + 1)];

        int index = 0;

        Vector3 offset = new Vector3(currentWidth * -0.5f, 0.0f, currentLength * -0.5f);

        for(int z = 0; z < currentLength; z++) {
            for(int x = 0; x < currentWidth; x++) {
                float t = Mathf.Abs((x * 2.0f) / currentWidth - 1);

                vertices[index] =
                    (new Vector3(
                        x,
                        Mathf.PerlinNoise((float)x / (float)currentWidth * perlinDefinition, (float)z / (float)currentLength * perlinDefinition) * amplitude,
                        z
                        )
                    + offset)
                    * currentSize;

                uvs[index] = new Vector2(x, z);
                index++;
            }
        }

        triangles = new int[6 * (currentWidth) * (currentLength)];

        int vert = 0;
        int tris = 0;
        for(int z = 0; z < currentLength - 1; z++) {
            for(int x = 0; x < currentWidth - 1; x++) {
                //Create a quad
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + currentWidth;
                triangles[tris + 2] = vert + 1;

                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + currentWidth;
                triangles[tris + 5] = vert + currentWidth + 1;

                vert++;
                tris += 6;
            }
            vert++;
        }
    }

    void UpdateMesh() {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
    }

    void NewLineOnMesh() {
        int index = 0;

        ////Offset of tile
        //Vector3 offset = new Vector3(-width * 0.5f, 0, -length * 0.5f);

        ////Move vertices inside mesh's array
        //for(int z = 0; z <= length - 1; z++) {
        //    for(int x = 0; x <= width; x++) {
        //        vertices[index] = (new Vector3(x, 0, z) + offset) * size;
        //        vertices[index] =
        //            new Vector3(
        //                vertices[index].x,
        //                vertices[index + width + 1].y,
        //                vertices[index].z);
        //        index++;
        //    }
        //}

        ////Create new vertices for new line
        //for(int x = 0; x <= width; x++) {
        //    float t = Mathf.Abs((x * 2.0f) / width - 1);

        //    vertices[index] =
        //        (new Vector3(
        //                x,
        //                Random.Range(0f, 1f) * amplitude,
        //                length) +
        //         offset) * size;
        //    index++;
        //}

        Vector3 offset = new Vector3(currentWidth * -0.5f, 0.0f, currentLength * -0.5f);

        for(float z = 0; z < currentLength; z++) {
            for(float x = 0; x < currentWidth; x++) {
                float t = Mathf.Abs((x * 2.0f) / currentWidth - 1);

                vertices[index] =
                    (new Vector3(
                        x,
                        Mathf.PerlinNoise(
                            (x + perlinOffset.x / size + Time.time * timeMultiplication) / (float)currentWidth * perlinDefinition,
                            (z + perlinOffset.z / size + Time.time * timeMultiplication) / (float)currentLength * perlinDefinition
                            ) * amplitude,
                        z
                        )
                    + offset)
                    * currentSize;

                uvs[index] = new Vector2(x, z);
                index++;
            }
        }

        //Update mesh vertices
        mesh.vertices = vertices;
    }
}
