using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScriptOffline : MonoBehaviour
{
    public TextMesh playerNameText;
    //public GameObject floatingInfo;

    private Material playerMaterialClone;
    private bool godMode = false;
    private System.DateTime godCount;
    private bool jumping = false;
    private System.DateTime jumpStamp;
    public bool active = false;
    Animator anim;



    public string playerName;

    public int score = 0;

    private SceneScript sceneScript;
    private SetNameButton setNameButton;

    private int selectedWeaponLocal = 1;
    public GameObject[] weaponArray;


    public int activeWeaponSynced = 1;

    private Weapon activeWeapon;
    private float weaponCooldownTime;

    public float lookSpeed = 3;
    private Vector2 rotation = Vector2.zero;
    public void Look() // Look rotation (UP down is Camera) (Left right is Transform rotation)
    {
        rotation.y += Input.GetAxis("Mouse X");
        rotation.x += -Input.GetAxis("Mouse Y");
        rotation.x = Mathf.Clamp(rotation.x, -15f, 15f);
        transform.eulerAngles = new Vector2(0, rotation.y) * lookSpeed;
        Camera.main.transform.localRotation = Quaternion.Euler(rotation.x * lookSpeed, 0, 0);
    }

    public void Start()
    {
        //sceneScript.playerScript = this;
        //setNameButton.playerScript = this;
        Camera.main.transform.SetParent(transform);
        Camera.main.transform.rotation = new Quaternion(0, 0, 0, 0);
        Camera.main.transform.localPosition = new Vector3(0, 1.5f, 0.2f);

        //floatingInfo.transform.localPosition = new Vector3(0, -0.3f, 0.6f);
        //floatingInfo.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        string name = "Player" + Random.Range(100, 999);
        CmdSetupPlayer(name);
        anim = GetComponent<Animator>();        
        
        foreach (var item in weaponArray)
        {
            if (item != null)
            {
                item.SetActive(false);
            }
            if (selectedWeaponLocal < weaponArray.Length && weaponArray[selectedWeaponLocal] != null) { 
                activeWeapon = weaponArray[selectedWeaponLocal].GetComponent<Weapon>(); 
                sceneScript.UIAmmo(activeWeapon.weaponAmmo);
                weaponArray[selectedWeaponLocal].SetActive(true);
            }
        }
    }

    private void Awake()
    {
        //allows all players to run this
        sceneScript = GameObject.Find("SceneReference").GetComponent<SceneReference>().sceneScript;
        setNameButton = GameObject.Find("SceneReference").GetComponent<SceneReference>().setNameButton;

        

        // disable all weapons


    }

    void Update()
    {

        //floatingInfo.transform.LookAt(Camera.main.transform);

            float moveX = Input.GetAxis("Horizontal") * Time.deltaTime * 4f; // trafe left right
            anim.SetFloat("Strafe", moveX);
            float moveZ = Input.GetAxis("Vertical") * Time.deltaTime * 4f; //ahead back 
            //Debug.Log(moveZ);
            anim.SetFloat("Speed", moveZ);

            if (isJumping() == false && (Input.GetButton("Jump") || Input.GetMouseButtonDown(1)))
            {
                jumpStamp = System.DateTime.Now;
                jumpStamp = jumpStamp.AddSeconds(1);
                anim.SetBool("Jump", true);
                gameObject.GetComponent<SmoothPositionOffline>().setJump(2f);
            }

            transform.Translate(moveX, 0, 0);
            transform.Translate(0, 0, moveZ);

            //if (Input.GetMouseButtonDown(1)) //Fire2 is mouse 2nd click and left alt
            //{
            //    weaponArray[selectedWeaponLocal].SetActive(false);
            //    selectedWeaponLocal += 1;

            //    if (selectedWeaponLocal > weaponArray.Length - 1)
            //    {
            //        selectedWeaponLocal = 1;
            //    }

            //    CmdChangeActiveWeapon(selectedWeaponLocal);
            //}

            if (Input.GetMouseButton(0)) //Fire1 is mouse 1st click
            {
                if (activeWeapon && Time.time > weaponCooldownTime && activeWeapon.weaponAmmo > 0)
                {
                    weaponCooldownTime = Time.time + activeWeapon.weaponCooldown;
                    activeWeapon.weaponAmmo -= 1;
                    sceneScript.UIAmmo(activeWeapon.weaponAmmo);
                    CmdShootRay();
                }
            }

            rotation.y += Input.GetAxis("Mouse X");
            rotation.x += -Input.GetAxis("Mouse Y");
            rotation.x = Mathf.Clamp(rotation.x, -15f, 15f);
            transform.eulerAngles = new Vector2(0, rotation.y) * lookSpeed;
            Camera.main.transform.localRotation = Quaternion.Euler(rotation.x * lookSpeed, 0, 0);


    }

    private bool isJumping()
    {
        if(jumpStamp < System.DateTime.Now)
        {
            jumping = false;
            anim.SetBool("Jump", false);
        } else
        {
            jumping = true;
        }
        return jumping;
    }


    void CmdShootRay()
    {
        RpcFireWeapon();
    }


    void RpcFireWeapon()
    {
        //bulletAudio.Play(); muzzleflash etc ///////////////////// SOUND HEREEEEE !!!!
        activeWeapon.shootWeapon.Play();
        //var bullet = (GameObject)Instantiate(activeWeapon.weaponBullet, activeWeapon.weaponFirePosition.position, activeWeapon.weaponFirePosition.rotation);
        var bullet = (GameObject)Instantiate(activeWeapon.weaponBullet, activeWeapon.weaponFirePosition.position, Camera.main.transform.rotation);
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * activeWeapon.weaponSpeed;
        bullet.GetComponent<Bullet>().setParent(this.gameObject);
        if (bullet) { Destroy(bullet, activeWeapon.weaponLife); }


    }

    void OnWeaponChanged(int _Old, int _New)
    {
        // disable old weapon
        // in range and not null
        if (0 < _Old && _Old < weaponArray.Length && weaponArray[_Old] != null)
        {
            weaponArray[_Old].SetActive(false);
        }

        // enable new weapon
        // in range and not null
        if (0 < _New && _New < weaponArray.Length && weaponArray[_New] != null)
        {
            weaponArray[_New].SetActive(true);
            activeWeapon = weaponArray[activeWeaponSynced].GetComponent<Weapon>();
            
                sceneScript.UIAmmo(activeWeapon.weaponAmmo); 
            
        }
    }

    void OnNameChanged(string _Old, string _New)
    {
        playerNameText.text = playerName;
    }



    public void CmdSendPlayerMessage()
    {
        if (sceneScript)
        {
            sceneScript.statusText = $"{playerName} says hello {Random.Range(10, 99)}";
        }
    }


    public void CmdSetupPlayer(string _name)
    {
        // player info sent to server, then server updates sync vars which handles it on all clients
        playerName = _name;
        sceneScript.statusText = $"{playerName} joined.";
    }


    public void CmdChangeActiveWeapon(int newIndex)
    {
        activeWeaponSynced = newIndex;
    }


    public void CmdRespawnPosition(GameObject a, Vector3 b)
    {
        if (godMode == true)
        {
            a.transform.position = b;
            anim.SetBool("Dead", false);
        }
       
    }


    public void godModeCount()
    {
        if (godMode == true)
        {
            if(godCount < System.DateTime.Now)
            {
                godMode = false;

            }
        }
    }


    public void setGodMode()
    {
        if (godMode == false)
        {
            godCount = System.DateTime.Now;
            godCount = godCount.AddSeconds(5);
            anim.SetBool("Dead", true);
            godMode = true;
        }
        
    }

    public bool isGodMode()
    {
        godModeCount();
        return godMode;
    }


    public void LogText(string s)
    {
        if(godMode == false)
        {
            if (sceneScript)
            {
                //Debug.Log("TEXT INPUT");
                sceneScript.logText3 = sceneScript.logText2;
                sceneScript.logText2 = sceneScript.logText1;
                sceneScript.logText1 = s;
            }
        }

    }

}
