using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class SetNameButton : NetworkBehaviour
{
    public TMPro.TMP_Text txtBox;
    public PlayerScript playerScript;
    public TMPro.TMP_InputField iField;
    private bool active = true;

    private void Start()
    {
        iField.ActivateInputField();
    }

    public void Update()
    {
        if (active)
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                setName();
                active = false;
                Debug.Log("Enter Key");
            }
        }
    }

    public void setName()
    {
        string txt = txtBox.text;
        if (txt == "")
        {
            txt = "Player" + Random.Range(100, 999);
        }
        playerScript.CmdSetupPlayer(txt);
        this.gameObject.SetActive(false);
        playerScript.active = true;
    }

}
