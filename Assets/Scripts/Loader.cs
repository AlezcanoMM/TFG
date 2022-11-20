using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Loader: MonoBehaviour
{
    public List<Object> loadedSlides;

    public virtual void Load() { 
        
    }

    public virtual void Clear() { 
        
    }

    public virtual List<Object> GetLoadedSlides() {
        return loadedSlides;
    }
}
