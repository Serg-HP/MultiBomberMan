using Assets.New_Folder.Basis;
using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerController : SimpleMoving
{
    private AudioSource source;
    public AudioClip setBombSound;
    public AudioClip flySound;
    public AudioClip deathSound;
    private int bombLimit;
    public int explosionArea;
    private float speedLimit = 0.25f;
    private bool wallPass;
    private bool isAlive=true;


    void Start ()
    {
        explosionArea = 1;
        bombLimit = 0;
        speed = 0.1f;
        wallPass = false;
        source = gameObject.GetComponent<AudioSource>();
    }
    private void MakeAction()
    {
        Animator animatorPlayer = gameObject.GetComponent<Animator>();
        if (Input.anyKey)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                animatorPlayer.SetTrigger("SetBomb");
                CmdPutBomb();
            }
            else
            {
                animatorPlayer.SetFloat("Walk", 1);
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    MoveLeft();
                }
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    MoveRight();
                }
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    MoveUp();
                }
                if (Input.GetKey(KeyCode.DownArrow))
                {
                    MoveDown();
                }
            }
        }
        else
        {
            animatorPlayer.SetFloat("Walk", 0);
        }
    }
    [Command]
    private void CmdPutBomb()
    {
        GameObject[] go = GameObject.FindGameObjectsWithTag("Bomb");
        if (go.Length <= bombLimit)
        {
            Vector3 bombposition = new Vector3(Mathf.RoundToInt(transform.position.x), 0, Mathf.RoundToInt(transform.position.z));
            if (wallPass == true)
            {
                Collider[] hitColliders = Physics.OverlapSphere(bombposition, 0.1f);
                if (hitColliders.Length > 1)
                    return;
            }
            GameObject bomb = Instantiate(EnvironmentTools.GetBomb(), bombposition, Quaternion.identity);
            NetworkServer.Spawn(bomb);
        }
    }
	void Update ()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        if (isAlive)
            MakeAction();
        //if (isAlive)
        //{
        //    if (!isLocalPlayer)
        //    {
        //        MakeAction();
        //    }
        //    else
        //        MakeAnotherAction();
        //}
    }
    public void IncreaseBombNumber()
    {
        bombLimit++;
    }
    public void IncreaseSpeed()
    {
        if (speed < speedLimit)
            speed +=0.05f;
    }
    public void SetWallPassActive()
    {
        wallPass = true;
    }
    public void IncreasePower()
    {
        explosionArea++;
    }

    public void SetDead()
    {
        isAlive = false;
        
    }

    private void PlayFlySound()
    {
        source.PlayOneShot(flySound);
    }
    private void PlaySetBombSound()
    {
        source.PlayOneShot(setBombSound);
    }
    private void PlayDeathSound()
    {
        source.PlayOneShot(deathSound);
    }
    void OnCollisionEnter(Collision hit)
    {
        var tag = hit.gameObject.tag;
        if (tag== "BreakWall")
            if (wallPass)
                Physics.IgnoreCollision(hit.collider, gameObject.GetComponent<CapsuleCollider>());
    }

    //private void MakeAnotherAction()
    //{
    //    Animator animatorPlayer = gameObject.GetComponent<Animator>();
    //    if (Input.anyKey)
    //    {
    //        if (Input.GetKeyDown(KeyCode.Tab))
    //        {
    //            animatorPlayer.SetTrigger("SetBomb");
    //            CmdPutBomb();
    //        }
    //        else
    //        {
    //            animatorPlayer.SetFloat("Walk", 1);
    //            if (Input.GetKey(KeyCode.A))
    //            {
    //                MoveLeft();
    //            }
    //            if (Input.GetKey(KeyCode.D))
    //            {
    //                MoveRight();
    //            }
    //            if (Input.GetKey(KeyCode.W))
    //            {
    //                MoveUp();
    //            }
    //            if (Input.GetKey(KeyCode.S))
    //            {
    //                MoveDown();
    //            }
    //        }
    //    }
    //    else
    //    {
    //        animatorPlayer.SetFloat("Walk", 0);
    //    }
    //}

    //private void OnDestroy()
    //{
    //    var resultText = GameObject.Find("ResultText").GetComponent<Text>();
    //    resultText.text = "GameOver!!!";
    //    Application.Quit();
    //}

}
