using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Loader: MonoBehaviour
{
    public static List<Object> loadedSlidesTextures = new List<Object>();
    public static List<string> loadedSlidesUrls = new List<string>();

    public virtual void Load(string slideId) { 
        
    }

    public virtual void Clear() { 
        
    }

    public virtual List<Object> GetLoadedSlidesTextures() {
        return loadedSlidesTextures;
    }

    public virtual List<string> GetLoadedSlidesUrls()
    {
        return loadedSlidesUrls;
    }
}
