using UnityEngine;
using System.Collections;

public class MoveCamera : MonoBehaviour {

    public float speed = 5f;
    public float rotateSpeed = 5f;

    //private Vector3 lastMouse;

	void Start () {
        //lastMouse = Input.mousePosition;
	}
	
	void Update () {
        //handle keyboard
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        if (horizontal != 0)
            transform.Translate(Vector3.right * horizontal * Time.deltaTime * speed, Space.Self);
        if (vertical != 0)
            transform.Translate(Vector3.forward * vertical * Time.deltaTime * speed, Space.Self);

        //handle mouse
        //Vector3 newMouse = Input.mousePosition;
        //if (Input.GetMouseButton(0))
        //{
        //    float deltaX = newMouse.x - lastMouse.x;
        //    float deltaY = newMouse.y - lastMouse.y;
        //    transform.Rotate(-deltaY * rotateSpeed, 0, 0, Space.Self);
        //    transform.Rotate(0, deltaX * rotateSpeed, 0, Space.Self);
        //}
        
        //lastMouse = newMouse;
	}
}
