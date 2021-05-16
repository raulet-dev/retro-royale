using Mirror;
using Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneScript : NetworkBehaviour
{
    public GameObject resp;
    private Vector3 respawnPoint;
    private Transform[] spw;

    public Text canvasStatusText;
    public PlayerScript playerScript;

    [SyncVar(hook = nameof(OnStatusTextChanged))]
    public string statusText;

    [SyncVar(hook = nameof(OnLogText1Changed))]
    public string logText1;
    [SyncVar(hook = nameof(OnLogText2Changed))]
    public string logText2;
    [SyncVar(hook = nameof(OnLogText3Changed))]
    public string logText3;
    [SyncVar(hook = nameof(OnLogScoreChanged))]
    public int logScore = 0;

    public SceneReference sceneReference;

    public TMPro.TMP_Text canvasAmmoText;
    public TMPro.TMP_Text log1;
    public TMPro.TMP_Text log2;
    public TMPro.TMP_Text log3;
    public TMPro.TMP_Text score;

    void OnLogScoreChanged(int _Old, int _New)
    {
        //called from sync var hook, to update info on screen for all players
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        score.text = "";
        foreach (GameObject player in players)
            {
            score.text += ("\n" + player.GetComponent<PlayerScript>().playerName + ", "
                + player.GetComponent<PlayerScript>().score.ToString() + ".");
            };
    }

    void OnStatusTextChanged(string _Old, string _New)
    {
        //called from sync var hook, to update info on screen for all players
        canvasStatusText.text = statusText;
    }

    void OnLogText1Changed(string _Old, string _New)
    {
        //called from sync var hook, to update info on screen for all players
        log1.text = logText1;
    }
    void OnLogText2Changed(string _Old, string _New)
    {
        //called from sync var hook, to update info on screen for all players
        log2.text = logText2;
    }
    void OnLogText3Changed(string _Old, string _New)
    {
        //called from sync var hook, to update info on screen for all players
        log3.text = logText3;
    }

    public void ButtonSendMessage()
    {
        if (playerScript != null)
        {
            playerScript.CmdSendPlayerMessage();
        }
    }

    public void ButtonChangeScene()
    {
        if (isServer)
        {
            Scene scene = SceneManager.GetActiveScene();
            if (scene.name == "SampleScene") {
                NetworkManager.singleton.ServerChangeScene("OtherScene");
            } else {
                NetworkManager.singleton.ServerChangeScene("SampleScene");
            }


        }
        else { Debug.Log("You are not Host."); }
    }

    public void UIAmmo(int _value)
    {
        canvasAmmoText.text = "Ammo: " + _value;
    }

    public void CmdKillPlayer(GameObject player, GameObject parent)
    {

        if (isServer)
        {
            
            if (player.GetComponent<PlayerScript>().isGodMode() == false){
                //Debug.Log("SEND TEXT INPUT");
                playerScript.LogText(player.GetComponent<PlayerScript>().playerName + " was killed by " + parent.GetComponent<PlayerScript>().playerName);
                parent.GetComponent<PlayerScript>().score += 1;
                logScore++;
                player.GetComponent<PlayerScript>().setGodMode();
                spw = GameObject.Find("SpawnPoints").GetComponentsInChildren<Transform>();
                respawnPoint = spw[Random.Range(0, spw.Length)].position;
                StartCoroutine(RespawnTime(player));
            }

        }
    }

    IEnumerator RespawnTime(GameObject player)
    {
        yield return new WaitForSeconds(3f);
        player.GetComponent<PlayerScript>().CmdRespawnPosition(player, respawnPoint);
    }

}
