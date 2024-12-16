using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.Networking;

public class Network : MonoBehaviour
{
    public TMP_InputField inputField;
    public TextToSpeech textToSpeech;


    // Start is called before the first frame update
    void Start()
    {

    }

    public void OnSendButtonClicked()
    {
        // StartCoroutine(GetDataFromServer());
        //StartCoroutine(SendDataToServer(inputField.text));
        StartCoroutine(SendMessageToServer(inputField.text));
    }
    [System.Serializable]
    public class Message
    {
        public string message;
    }

    IEnumerator SendMessageToServer(string text)
    {
        string url = "http://localhost:8080/voice";
        WWWForm form = new WWWForm();
        form.AddField("voice", text);

        UnityWebRequest request = UnityWebRequest.Post(url, form);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(request.error);
        }
        else
        {
            Debug.Log(request.downloadHandler.text);
            textToSpeech.ButtonClick(request.downloadHandler.text);
        }
    }


    IEnumerator SendDataToServer(string text)
    {
        string url = "http://localhost:8080/voice";
        Message message = new Message{ message = text };
        string jsonData = JsonUtility.ToJson(message);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(request.error);
        }
        else
        {
            Debug.Log("Data sent successfully");
        }
    }

    IEnumerator GetDataFromServer()
    {
        string url = "http://localhost:8000";
        UnityWebRequest request = UnityWebRequest.Get(url);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(request.error);
        }
        else
        {
            Debug.Log(request.downloadHandler.text);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
