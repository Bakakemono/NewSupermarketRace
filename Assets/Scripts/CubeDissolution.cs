using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeDissolution : MonoBehaviour
{
    [SerializeField] Material _material;
    [SerializeField] Transform _dissolutionObjectTransform;
    [SerializeField, Range(0.0f, 10.0f)] float _dissolutionDistance = 0.0f;

    private void Start() {
        _material = GetComponent<MeshRenderer>().material;
    }
    private void Update() {
        _material.SetVector("_StartPos", _dissolutionObjectTransform.position);
        _material.SetFloat("_DisolutionDistance", _dissolutionDistance);
    }
}
