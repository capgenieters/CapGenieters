using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreedomPlayer : MonoBehaviour
{

    [SerializeField] float MovementSpeed = 5f;
    [SerializeField] float RotationSpeed = 100f;
    [SerializeField] float Gravity = 2.0f;
    [Range(0.01f, 0.2f)] [SerializeField] float SurfaceAlignSpeed = 0.05f;

    public LayerMask GroundLayer;

    private GameObject Player;
    private GameObject Camera;

    // Start is called before the first frame update
    void Start()
    {
        Player = this.gameObject;
        Camera = GameObject.Find("Main Camera");

        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        // Camera/Player Rotation
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");      

        Vector3 rotationX = new Vector3(0f, mouseX * RotationSpeed * Time.deltaTime, 0f);
        Vector3 rotationY = new Vector3(-mouseY * RotationSpeed * Time.deltaTime, 0f, 0f);
        
        Player.transform.Rotate(rotationX);
        Camera.transform.Rotate(rotationY);

        // Player Movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Player.transform.position += transform.forward * verticalInput * MovementSpeed * Time.deltaTime;
        Player.transform.position += transform.right * horizontalInput * MovementSpeed * Time.deltaTime;

        // Align player with ground to allow walking on slopes/walls
        RaycastHit hit;


        if (Physics.Raycast(transform.position, -transform.up, out hit))
        {
            Debug.Log(hit.transform.gameObject.layer);
            //if(hit.transform.gameObject.layer != GroundLayer) return;


            Player.transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation, SurfaceAlignSpeed);

            // Gravity 
            if (hit.distance > 1.1f)
            {
                Player.transform.position -= transform.up * Gravity * Time.deltaTime;
            }
        }
    }
}
