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
            menuCubeRigid.freezeRotation = false;
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

            var desiredQ = Quaternion.Euler(0, 0, 0);
            Debug.Log(Quaternion.Angle(menuCube.transform.rotation, desiredQ));
            menuCube.transform.rotation = Quaternion.Lerp(menuCube.transform.rotation, desiredQ, speed * Time.deltaTime);
            if (Quaternion.Angle(menuCube.transform.rotation  ,desiredQ) < 3f)
            {
            
                Debug.Log("GOT IT");
                shooted = false;
                toggleMenu(true);
                menuCubeRigid.freezeRotation = true;
            }
        }

       

    }
    private void ShootMenu()
    {
        menuCube.SetActive(true);
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
            menuCube.SetActive(false);

        }
    }
    private void toggleMenu(bool open)
    {
    
            foreach (Transform child in menuCube.transform)
            {
                Debug.Log(open);
                child.gameObject.SetActive(open);
            }
        }
    
}
