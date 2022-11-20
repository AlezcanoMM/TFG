using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureLoader : Loader
{
    public string presentationName;

    private void Start()
    {
        Load();
    }

    public override void Load() {
        foreach (Object slide in Resources.LoadAll("Presentations/"+ presentationName)) {
            loadedSlides.Add(slide);
        }
    }

    public override void Clear()
    {
        loadedSlides.Clear();
    }
}
