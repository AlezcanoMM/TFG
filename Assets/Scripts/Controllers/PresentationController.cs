using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using Photon.Pun;

public class PresentationController : MonoBehaviour
{
    private ApiController api;
    private List<GameObject> options;

    public PhotonView pv;

    private void Start()
    {
        api = ApiController.GetInstance();
        options = new List<GameObject>();
    }

    public void CreatePresentationOptions() {
        if (options.Count > 0)
        {
            api.presentationSelectMenu.SetActive(true);
        }
        else 
        {
            //Creates buttons for each presentations under Assets/Resources/Presentations
            foreach (string presentationName in Directory.GetFiles("Assets/Resources/Presentations"))
            {
                string name = presentationName.Replace("Assets/Resources/Presentations\\", "");
                name = name.Replace(".meta", "");
                Debug.Log(name);

                GameObject option = Instantiate(api.presentationOptionPrefab, api.presentationSelectMenu.transform);
                option.transform.GetChild(1).GetComponent<TextMeshPro>().text = name;
                option.GetComponent<ButtonController>().buttonChanges = false;
                option.GetComponent<ButtonController>().invokeMethodOn.AddListener(delegate { SelectPresentationOption(name); });
                options.Add(option);
            }
        }
        
    }

    public void SelectPresentationOption(string name) {
        pv.RPC("SelectPresentationOptionRPC", RpcTarget.All, name);
    }

    [PunRPC]
    public void SelectPresentationOptionRPC(string name)
    {
        api.EndPresentation();
        api.presentationName = name;
        api.presentButton.SetActive(true);
        api.presentationSelectMenu.SetActive(false);
    }

}
