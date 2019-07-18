﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopperTrainingProjectileController : MonoBehaviour
{
    public float globalDirection = -1;
    public float shootVelocityDirection = 2.5f;
    public float shootVelocityUp = 10f;
    float revSpeed = 360f;
    float[,] throwingPower = new float[,] {
        { 1.5f, 6 },
        { 2, 5 },
        {2.5f, 5 },
        { 3, 5 },
        { 3.5f, 4.6f },
        { 2, 4.5f }
    };
    // Start is called before the first frame update
    void Start()
    {
        int i = Random.Range(0, throwingPower.GetLength(0));
        GetComponent<Rigidbody2D>().AddForce(globalDirection * Vector2.right * throwingPower[i,0], ForceMode2D.Impulse);
        GetComponent<Rigidbody2D>().AddForce(Vector2.up * throwingPower[i, 1], ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        GetComponent<Rigidbody2D>().MoveRotation(GetComponent<Rigidbody2D>().rotation + revSpeed * Time.fixedDeltaTime);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Ground Collider Top") {
            Destroy(gameObject);
        }
    }
}
