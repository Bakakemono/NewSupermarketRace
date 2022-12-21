using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WiggleEffectSetup : MonoBehaviour
{
    public Camera cameraPostEffect;
    public Material PortalMaterial;

    private void Start() {
        if(cameraPostEffect.targetTexture != null) {
            cameraPostEffect.targetTexture.Release();
        }
        cameraPostEffect.targetTexture =
            new RenderTexture(Screen.width, Screen.height, 24);

        PortalMaterial.mainTexture = cameraPostEffect.targetTexture;
    }
}
