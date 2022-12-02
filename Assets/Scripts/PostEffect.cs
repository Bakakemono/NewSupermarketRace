using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostEffect : MonoBehaviour
{
    [SerializeField] Material mat;
    void OnRenderImage(RenderTexture source, RenderTexture destination) {

        Debug.Log("OnRenderImage is call");
        Graphics.Blit(source, destination, mat);
    }
}
