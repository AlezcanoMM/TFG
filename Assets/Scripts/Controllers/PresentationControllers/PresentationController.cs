using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using Photon.Pun;
using UnityEngine.Networking;
using SimpleJSON;

public class PresentationController : MonoBehaviour
{
    private AppController app;
    private List<GameObject> options;

    public PhotonView pv;

    private string jsonURL = "https://drive.google.com/uc?export=download&id=1NGDrZg9HM9T83Fgos_XO0_-UdNvnj1HF";

    private bool loaded = false;

    private void Start()
    {
        app = AppController.GetInstance();
        options = new List<GameObject>();
    }

    public void CreatePresentationOptionFromServer() {
        if (!loaded)
        {
            loaded = true;
            StartCoroutine(GetDataFromJSON(jsonURL));
        }
        return;
    }

    IEnumerator GetDataFromJSON(string url) {
        UnityWebRequest request = UnityWebRequest.Get(url);

        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.LogError(request.error);
        }
        else 
        {
            var wc = new System.Net.WebClient();
            string data = wc.DownloadString(url);

            JSONNode jsonData = JSONNode.Parse(data);

            foreach (JSONNode p in jsonData["presentations"])
            {
                Debug.Log("Getting presentation: "+p["name"]);
                GameObject option = Instantiate(app.presentationOptionPrefab, app.presentationSelectMenu.transform);
                option.transform.GetChild(1).GetComponent<TextMeshPro>().text = p["name"];
                option.GetComponent<InterfaceController>().buttonChanges = false;
                option.GetComponent<InterfaceController>().invokeMethodOn.AddListener(delegate { SelectPresentationOption(p); });
                options.Add(option);
            }
        }
    }

    public void SelectPresentationOption(JSONNode p) {
        pv.RPC("EndAndClearPresentationRPC", RpcTarget.All);

        foreach (JSONNode slide in p["slides"])
        {
            string slideId = slide["type"] + "|" + slide["slideUrl"];
            pv.RPC("AddSlideIdRPC", RpcTarget.All, slideId);
        }

        app.presentButton.SetActive(true);
        app.presentationSelectMenu.SetActive(false);
    }

    [PunRPC]
    public void AddSlideIdRPC(string slideId) {
        app.presentationSlidesIds.Add(slideId);
    }

    [PunRPC]
    public void EndAndClearPresentationRPC()
    {
        loaded = true;
        app.presentationSlidesIds.Clear();
        app.EndPresentation();
    }
}
