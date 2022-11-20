using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureController : MonoBehaviour
{
    public GameObject plane;

    public void LoadTextureOnPlane(Object slide) {
        Texture2D texture = slide as Texture2D;
        plane.GetComponent<Renderer>().material.mainTexture = texture;
        float scaleFactor = (float)texture.width / (float)texture.height;
        plane.transform.localScale = new Vector3(scaleFactor, 1, 1);
    }
}
