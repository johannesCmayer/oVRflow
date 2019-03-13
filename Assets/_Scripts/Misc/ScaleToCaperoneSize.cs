using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ScaleToCaperoneSize : MonoBehaviour {

    void Start()
    {
        SetScaleAndMaterialTiling();
    }

    void SetScaleAndMaterialTiling()
    {
        float chaperoneSizeX = 0;
        float chaperoneSizeZ = 0;
        OpenVR.Chaperone.GetPlayAreaSize(ref chaperoneSizeX, ref chaperoneSizeZ);

        Material mat = GetComponent<Renderer>().material;
        mat.mainTextureScale = new Vector2(chaperoneSizeX * mat.mainTextureScale.x / transform.localScale.x, chaperoneSizeZ * mat.mainTextureScale.y / transform.localScale.z);

        transform.localScale = new Vector3(chaperoneSizeX / 10, 1, chaperoneSizeZ / 10);
    }
}
