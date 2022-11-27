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
    private ApiController api;
    private List<GameObject> options;

    public PhotonView pv;

    private string jsonURL = "https://drive.google.com/uc?export=download&id=18kr9WL90RinLJdXq22QsYAuS-QWCM6et";

    private bool loaded = false;

    private void Start()
    {
        api = ApiController.GetInstance();
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
                GameObject option = Instantiate(api.presentationOptionPrefab, api.presentationSelectMenu.transform);
                option.transform.GetChild(1).GetComponent<TextMeshPro>().text = p["name"];
                option.GetComponent<ButtonController>().buttonChanges = false;
                option.GetComponent<ButtonController>().invokeMethodOn.AddListener(delegate { SelectPresentationOption(p); });
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

        api.presentButton.SetActive(true);
        api.presentationSelectMenu.SetActive(false);
    }

    [PunRPC]
    public void AddSlideIdRPC(string slideId) {
        api.presentationSlidesIds.Add(slideId);
    }

    [PunRPC]
    public void EndAndClearPresentationRPC()
    {
        loaded = true;
        api.presentationSlidesIds.Clear();
        api.EndPresentation();
    }
}
