using System.IO;
using UnityEngine;

public class JSONCacheController
{
    [SerializeField]
    private string jsonPath = "flightAwareCache.json";
    private StreamWriter currentFile;

    public void CacheFlightAware(string text)
    {
        CreateOrOpenJSON();
        currentFile.Write(text);
        currentFile.Close();
    }

    private void CreateOrOpenJSON()
    {
        currentFile = File.CreateText(Path.Combine(Application.streamingAssetsPath, jsonPath));
    }
}
