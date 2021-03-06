﻿using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float minX = -3, maxX = 3, handling = 10f;
    public GameManager gm;
    public Looper looper;
    bool lerpHori = false;
    Vector3 targetPosition, sourcePosition, targetRotation;
    Rigidbody rb;
    Animator anim;
    float left = 0, right = 0, mov = 0;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

	void Start ()
    {

    }
	
	void Update ()
    {
        if (rb.IsSleeping())
            rb.WakeUp();
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            left = 1;
        else if (Input.GetKeyUp(KeyCode.LeftArrow))
            left = 0;
        if (Input.GetKeyDown(KeyCode.RightArrow))
            right = 1;
        else if (Input.GetKeyUp(KeyCode.RightArrow))
            right = 0;
        Move();
        if (Input.GetKeyDown(KeyCode.H))
            Rotate(10);
        if (Input.GetKeyDown(KeyCode.G))
            Rotate(-10);
    }

    void Rotate(float f)
    {
        Debug.Log("Rotating..");
        transform.eulerAngles = Vector3.forward * f;
    }

    public void Reset()
    {
        lerpHori = false;
        transform.position = Vector3.zero;
    }

    void OnTriggerEnter(Collider c)
    {
        if (c.tag == "Obstacle")
            gm.HitObstacle();
        else if (c.tag == "Currency")
            gm.HitCurrency();
        else if (c.tag == "Fuel")
            gm.HitFuel();
        else if (c.tag == "Shield")
            gm.HitShield();
        looper.Reset(c.gameObject);
    }

    void Move()
    {
        if ((right - left) != 0)
        {
            if (mov == 0 || mov == (left-right))
                targetRotation = Vector3.zero;
            mov = right - left;
            targetRotation += Vector3.forward * (left - right) * 100 * Time.deltaTime;
        }
        else
        {
            targetRotation += Vector3.forward * mov * 100 * Time.deltaTime;
            if ((targetRotation.z > 0 && mov > 0) || (targetRotation.z < 0 && mov < 0))
                mov = 0;
        }
        if (targetRotation.z > 30)
            targetRotation.z = 30;
        else if (targetRotation.z < -30)
            targetRotation.z = -30;
        sourcePosition = transform.position;
        targetPosition = transform.position + Vector3.right * (right-left) * Time.deltaTime * handling;
        if (targetPosition.x < minX)
            targetPosition.x = minX;
        else if (targetPosition.x > maxX)
            targetPosition.x = maxX;
        transform.position = Vector3.Lerp(sourcePosition, targetPosition, 0.2f);
        transform.eulerAngles = targetRotation;
    }

    public void LeftDown()
    {
        left = 1;
    }

    public void LeftUp()
    {
        left = 0;
    }

    public void RightDown()
    {
        right = 1;
    }

    public void RightUp()
    {
        right = 0;
    }
}
