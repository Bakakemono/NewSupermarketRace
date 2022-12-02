using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTextureSetup : MonoBehaviour
{
    public Camera cameraB;
    public Material materialCameraB;

    private void Start() {
        if(cameraB.targetTexture != null) {
            cameraB.targetTexture.Release();
        }
        cameraB.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);

        materialCameraB.mainTexture = cameraB.targetTexture;
    }
}
