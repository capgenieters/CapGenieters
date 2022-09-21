using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class raycast : MonoBehaviour
{
    //Declare a LineRenderer to store the component attached to the GameObject.
    [SerializeField] LineRenderer rend;
    //Settings for the LineRenderer are stored as a Vector3 array of points. Set up a V3 array to //initialize in Start.
    Vector3[] points;
    public Vector2 turn;
    //Start is called before the first frame update
    public LayerMask layermask;

    public float speed = 2f;

    void Start() { 

        Cursor.lockState = CursorLockMode.Locked;

        //get the LineRenderer attached to the gameobject.
        rend = gameObject.GetComponent<LineRenderer>();
        //initialize the LineRenderer
        points = new Vector3[2];
        //set the start point of the linerenderer to the position of the gameObject.
        points[0] = Vector3.zero;
        //set the end point 20 units away from the GO on the Z axis (pointing forward)
        points[1] = transform.position + new Vector3(0, 0, 20); 
        //finally set the positions array on the LineRenderer to our new values
        rend.SetPositions(points);
        rend.enabled = true;
    }


    private void Update()
    {
        turn.x += Input.GetAxis("Mouse X");
        turn.y += Input.GetAxis("Mouse Y");
        transform.localRotation = Quaternion.Euler(-turn.y * speed, turn.x * speed, 0);
        AlignLineRenderer(rend);
    }

   
    public void AlignLineRenderer(LineRenderer rend)
    {
        Ray ray;
        ray =new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if(Physics.Raycast(ray,out hit, layermask))
        {
            points[1] = transform.forward + new Vector3(0,0,hit.distance);
        }
        else
        {
            points[1] = transform.forward + new Vector3(0,0,20);
        }
        rend.SetPositions(points);
    }

}
