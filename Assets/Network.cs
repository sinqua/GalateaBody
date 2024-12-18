using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class Network : MonoBehaviour
{
    public TMP_InputField inputField;
    public TextToSpeech textToSpeech;

    public void OnSendButtonClicked()
    {
        StartCoroutine(SendMessageToServer(inputField.text));
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
}
