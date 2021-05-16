using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

[DisallowMultipleComponent]
[AddComponentMenu("Network/NetworkManagerHUD")]
[RequireComponent(typeof(NetworkManager))]
public class MatchMaker : MonoBehaviour
{
    public NetworkManager manager;
    public TMPro.TMP_Text ip;
    // Start is called before the first frame update

    public void startHost()
    {
        manager.StartHost();
    }

    public void startClient()
    {
        ClientScene.Ready(NetworkClient.connection);

        if (ClientScene.localPlayer == null)
        {
            ClientScene.AddPlayer(NetworkClient.connection);
        }

        manager.networkAddress = ip.text;
        manager.StartClient();
    }
}
