using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuShooter : MonoBehaviour
{
    public static bool MenuOpen = false;
    [SerializeField] LineRenderer rend;
    public GameObject menuCube;
    public InputActionProperty leftHand;
    [SerializeField] Rigidbody menuCubeRigid;
    public float speed = 10f;
    private bool lerpped = false;
    private bool shooted = false;
    void Start()
    {
        rend = gameObject.GetComponent<LineRenderer>();
        menuCubeRigid = menuCube.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (lerpped)
        {
            retract();
        }

        if (leftHand.action.triggered)
        {
            if (MenuOpen)
            {
                lerpped = true;
            }
            else
            {
                lerpped = false;
                ShootMenu();

            }
        }
      
        if(shooted && menuCubeRigid.velocity.magnitude < 1f)
        {
            Debug.Log(menuCube.transform.rotation);
            var desiredQ = Quaternion.Euler(0, 0, 0);
      
            menuCube.transform.rotation = Quaternion.Lerp(menuCube.transform.rotation, desiredQ, speed * Time.deltaTime);
            if (menuCube.transform.rotation == desiredQ)
            {
                shooted = false;
                toggleMenu(true);
            }
        }

       

    }
    private void ShootMenu()
    {
        menuCube.transform.position = transform.position;
        menuCubeRigid.velocity = (transform.forward * 10) + (transform.up * 3);
        MenuOpen = true;
        shooted = true;

    }

    private void retract()
    {
        shooted = false;
        MenuOpen = false;
        float journeyLength = Vector3.Distance(menuCube.transform.position, transform.position);

        menuCube.transform.position = Vector3.Lerp(menuCube.transform.position, transform.position, speed * Time.deltaTime);
        if (journeyLength < 0.5f)
        {
            lerpped = false;
            menuCubeRigid.velocity = Vector3.zero;
            toggleMenu(false);

        }
    }
    private void toggleMenu(bool open)
    {
        if (menuCubeRigid.velocity == Vector3.zero)
        {
            foreach (Transform child in menuCube.transform)
            {
                Debug.Log(open);
                child.gameObject.SetActive(open);
            }
        }
    }
}
