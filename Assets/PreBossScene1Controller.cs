﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PreBossScene1Controller : MonoBehaviour
{
    public GameObject scientist1;
    public List<GameObject> scientist1points;
    float scientistSpeed = 2f;
    public GameObject player;
    public List<GameObject> playerpoints;
    public GameObject copilot;
    public List<GameObject> copilotpoints;
    float sceneNumber = 1;
    public float movementSpeed = 4f;
    public TextMeshPro DialogText;
    public TextMeshPro skipBox;
    string alpha = "<alpha=#00>";
    List<string> scientistTalk = new List<string>() {
        "Oh, hello.",
        "My colleague got all crazy and I think he needs some space, so I'm going home.",
        //Explosion - turns - waits a few - turns back
        "Anyway...",
        "See you tomorrow."
    };
    bool turnOnce = false;
    bool explosion = false;
    bool exploded = false;
    float explosionTime;
    float explosionDelay = 2f;
    int explosionCounter = 0;

    List<string> pilotsTalk = new List<string>() {
        "We should go help him.",
        //Bigger Explosion
        "Uhh, boss.",
        "My bus is coming in a few minutes.",
        "And I was thinking if I could leave early today?",
        //Even Bigger Explosion
        "OKAY, BYE!"
    };
    float newLineDelay = 1f;
    float newLineTime;
    bool newLine = false;
    float typingSpeed = 0.05f;
    float typingTime;
    int iterator = 0;
    int iterator2 = 0;
    float waitForExplosion;
    Vector2 move = Vector2.zero;
    // Start is called before the first frame update
    void Start()
    {
        skipBox.text = "Press " + GetComponent<Keybindings>().attack1.ToString() + " to skip.";
        scientist1.transform.position = scientist1points[0].transform.position;
        player.transform.position = playerpoints[0].transform.position;
        copilot.transform.position = copilotpoints[0].transform.position;

        turnObject(scientist1);
        Scientist1standing();

        DialogText.text = "";
        DialogText.gameObject.SetActive(false);
        skipBox.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        switch (sceneNumber)
        {
            case 1:
                Scientist1waving();
                move = Vector2.zero;
                bool arrived1 = false;
                bool arrived2 = false;
                if (player.transform.position.x < playerpoints[1].transform.position.x)
                {
                    player.GetComponent<Rigidbody2D>().velocity = new Vector2(1 * movementSpeed, player.GetComponent<Rigidbody2D>().velocity.y);
                    // walking animation
                    playerwalking();
                    //
                }
                else
                {
                    player.GetComponent<Rigidbody2D>().velocity = move;
                    //still animation
                    playerstanding();
                    arrived1 = true;
                    //
                }
                if (copilot.transform.position.x < copilotpoints[1].transform.position.x)
                {
                    copilot.GetComponent<Rigidbody2D>().velocity = new Vector2(1 * movementSpeed, copilot.GetComponent<Rigidbody2D>().velocity.y);

                    // walking animation
                    copilotwalking();
                    //
                }
                else
                {
                    copilot.GetComponent<Rigidbody2D>().velocity = move;
                    //still animation
                    copilotstanding();
                    arrived2 = true;
                    //
                }
                if (arrived1 && arrived2)
                {
                    sceneNumber++;
                    typingTime = Time.time + typingSpeed;
                    DialogText.text = alpha + scientistTalk[0];
                }
                break;
            case 2:
                if (Input.GetKeyDown(GetComponent<Keybindings>().attack1) && !explosion)
                {
                    iterator = scientistTalk[iterator2].Length;
                }
                if (explosion)
                {

                    if (Time.time > waitForExplosion)
                    {
                        if (!turnOnce)
                        {
                            turnObject(scientist1);
                            DialogText.gameObject.SetActive(false);
                            skipBox.gameObject.SetActive(false);
                            explosionTime = Time.time + explosionDelay;
                            turnOnce = true;
                        }
                        //camera shake
                        //explosions
                        Explosion();
                        if (Time.time > explosionTime)
                        {
                            explosion = false;
                            exploded = true;
                            turnObject(scientist1);
                        }
                    }
                }
                else
                {
                   
                    DialogText.gameObject.SetActive(true);
                    skipBox.gameObject.SetActive(true);
                }
                if (Time.time > typingTime && !explosion)
                {
                    if (newLine && Time.time > newLineTime)
                    {
                        DialogText.text = alpha + scientistTalk[iterator2];
                        newLine = false;
                    }
                    if (!newLine)
                    {
                        DialogText.text = DialogText.text.Replace(alpha, "");
                        DialogText.text = DialogText.text.Insert(iterator, alpha);
                        iterator++;
                        typingTime = Time.time + typingSpeed;
                    }

                    if (iterator > scientistTalk[iterator2].Length)
                    {
                        iterator = 0;
                        iterator2++;
                        if (iterator2 == scientistTalk.Count)
                        {
                            sceneNumber++;
                            iterator = 0;
                            iterator2 = 0;
                        }
                        else
                        {//novi stavek
                            if (!newLine)
                            {
                                newLineTime = Time.time + newLineDelay;
                                newLine = true;
                            }

                        }
                    }
                    if (iterator2 >= 2 && exploded)
                    {
                        scientist1.transform.position =
                            Vector3.MoveTowards(scientist1.transform.position,
                                        scientist1points[1].transform.position, scientistSpeed * Time.deltaTime);
                        // walking animation
                        Scientist1walking();
                    }
                    else {
                        Scientist1standing();
                    }
                    if (iterator2 == 2 && !explosion && !exploded)
                    {
                        waitForExplosion = Time.time + newLineDelay;
                        explosion = true;
                    }
                }
                break;
            case 3:
                scientist1.transform.position =
                    Vector3.MoveTowards(scientist1.transform.position,
                                scientist1points[1].transform.position, scientistSpeed * Time.deltaTime);
                // walking animation
                Scientist1walking();
                if (scientist1.transform.position == scientist1points[1].transform.position) {
                    sceneNumber++;
                    DialogText.text = alpha + pilotsTalk[iterator2];
                    exploded = false;
                    explosion = false;
                    turnOnce = false;

                }
                break;
            case 4:
                if (Input.GetKeyDown(GetComponent<Keybindings>().attack1))
                {
                    iterator = pilotsTalk[iterator2].Length;
                }
                if (explosion)
                {
                    if (Time.time > waitForExplosion)
                    {
                        if (!turnOnce)
                        {
                            DialogText.gameObject.SetActive(false);
                            skipBox.gameObject.SetActive(false);
                            explosionTime = Time.time + explosionDelay;
                            turnOnce = true;
                        }
                        //camera shake
                        //explosions
                        Explosion();
                        if (Time.time > explosionTime)
                        {
                            explosion = false;
                            exploded = true;
                            turnOnce = false;
                        }
                    }
                }
                else
                {

                    DialogText.gameObject.SetActive(true);
                    skipBox.gameObject.SetActive(true);
                }
                if (Time.time > typingTime && !explosion)
                {
                    if (newLine && Time.time > newLineTime)
                    {
                        DialogText.text = alpha + pilotsTalk[iterator2];
                        newLine = false;
                    }
                    if (!newLine)
                    {
                        DialogText.text = DialogText.text.Replace(alpha, "");
                        DialogText.text = DialogText.text.Insert(iterator, alpha);
                        iterator++;
                        typingTime = Time.time + typingSpeed;
                    }
                    if (iterator > pilotsTalk[iterator2].Length)
                    {
                        iterator = 0;
                        iterator2++;
                        if (iterator2 == 2) {
                            exploded = false;
                        }
                        if (iterator2 == pilotsTalk.Count)
                        {
                            sceneNumber ++;
                        }
                        else
                        {//novi stavek
                            if (!newLine)
                            {
                                newLineTime = Time.time + newLineDelay;
                                newLine = true;
                            }

                        }
                        if (iterator2 == 1 && !explosion && !exploded)
                        {
                            explosionCounter++;
                            waitForExplosion = Time.time + newLineDelay;
                            explosion = true;
                        }

                        if (iterator2 == 4 && !explosion && !exploded)
                        {
                            explosionCounter++;
                            waitForExplosion = Time.time + newLineDelay;
                            explosion = true;
                            turnOnce = false;
                        }
                        if (iterator2 >= 4 && exploded)
                        {
                            if (!turnOnce) {

                                turnObject(copilot);
                                turnOnce = true;
                            }

                            copilot.GetComponent<Rigidbody2D>().velocity = new Vector2(-movementSpeed, copilot.GetComponent<Rigidbody2D>().velocity.y);
                            // walking animation
                            copilotwalking();
                        }
                    }
                }
                break;
            case 5:
                if (copilot.transform.position.x > copilotpoints[0].transform.position.x)
                {
                    copilot.GetComponent<Rigidbody2D>().velocity = new Vector2(-movementSpeed, copilot.GetComponent<Rigidbody2D>().velocity.y);

                    // walking animation
                    copilotwalking();
                    //
                }
                else
                {
                    sceneNumber++;
                    DialogText.gameObject.SetActive(false);
                    skipBox.gameObject.SetActive(false);
                }
                break;
            case 6:
                Debug.Log("player: " + player.transform.position.x + " point: " + playerpoints[2].transform.position.x);
                if (player.transform.position.x < playerpoints[2].transform.position.x)
                {
                    player.GetComponent<Rigidbody2D>().velocity = new Vector2(movementSpeed, player.GetComponent<Rigidbody2D>().velocity.y);
                    // walking animation
                    playerwalking();
                    //
                }
                else
                {
                    player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    playerstanding();
                    //Load new scene here. 
                }
                break;

        }
    }
    void Explosion()
    {
        Debug.Log(explosionCounter);
    }
    void Scientist1walking()
    {
        scientist1.GetComponent<Animator>().SetBool("walking", true);
        scientist1.GetComponent<Animator>().SetBool("standing", false);
        scientist1.GetComponent<Animator>().SetBool("waving", false);
    }
    void Scientist1standing()
    {
        scientist1.GetComponent<Animator>().SetBool("walking", false);
        scientist1.GetComponent<Animator>().SetBool("standing", true);
        scientist1.GetComponent<Animator>().SetBool("waving", false);
    }
    void Scientist1waving()
    {
        scientist1.GetComponent<Animator>().SetBool("walking", false);
        scientist1.GetComponent<Animator>().SetBool("standing", false);
        scientist1.GetComponent<Animator>().SetBool("waving", true);
    }
    void playerwalking()
    {
        player.GetComponent<Animator>().SetBool("walking", true);
        player.GetComponent<Animator>().SetBool("standing", false);
    }
    void playerstanding()
    {
        player.GetComponent<Animator>().SetBool("walking", false);
        player.GetComponent<Animator>().SetBool("standing", true);
    }
    void copilotwalking()
    {
        copilot.GetComponent<Animator>().SetBool("walking", true);
        copilot.GetComponent<Animator>().SetBool("standing", false);
    }
    void copilotstanding()
    {
        copilot.GetComponent<Animator>().SetBool("walking", false);
        copilot.GetComponent<Animator>().SetBool("standing", true);
    }
    void turnObject(GameObject go)
    {
        go.transform.Rotate(0, 180, 0, Space.World);
    }
}
