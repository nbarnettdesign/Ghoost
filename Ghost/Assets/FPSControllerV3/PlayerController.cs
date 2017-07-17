using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody))]
[RequireComponent (typeof (CapsuleCollider))]

public class PlayerController : MonoBehaviour {

	//Head Movement
	public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
	public RotationAxes axes = RotationAxes.MouseXAndY;
	public Transform head;

	public float minimumX = 0f;
	public float maximumX = 360f;
	public float minimumY = -30f;
	public float maximumY = 45f;

	private float sensitivityX = 5f;
	private float sensitivityY = 5f;
	private float rotationY = 0f;

	//Body Movement
	public float walkSpeed = 6;
	public float runSpeed = 10;
	public float strafeSpeed = 5;
	public float gravity = 20;
	public float jumpHeight = 2;
	public bool canJump = true;
	private bool isRunning = false;
	public bool isGrounded = false;
	public float charSkin = 1f;

    public bool IsRunning
    {
        get { return isRunning; }
    }

	void Awake () 
	{
		GetComponent<Rigidbody>().freezeRotation = true;
		GetComponent<Rigidbody>().useGravity = false;

		// Make the rigid body not change rotation
		if (GetComponent<Rigidbody> ()) {
			GetComponent<Rigidbody> ().freezeRotation = true;
		}

		UnityEngine.Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}
	
	void FixedUpdate () 
	{
		// get correct speed
		float forwardAndBackSpeed = walkSpeed;

        // if running, set run speed
		if (isRunning) {
			forwardAndBackSpeed = runSpeed;
		}

		// calculate how fast it should be moving
		Vector3 targetVelocity = new Vector3(CheckMoveDirection(Input.GetAxis("Horizontal"),transform.right) * strafeSpeed, 0,CheckMoveDirection(Input.GetAxis ("Vertical"),transform.forward)* forwardAndBackSpeed);

		targetVelocity = transform.TransformDirection(targetVelocity);
		
		// apply a force that attempts to reach our target velocity
		Vector3 velocity = GetComponent<Rigidbody>().velocity;
		Vector3 velocityChange = (targetVelocity - velocity);
		velocityChange.y = 0;
		GetComponent<Rigidbody>().AddForce(velocityChange, ForceMode.VelocityChange);
		
		// jump
		if (canJump && isGrounded && Input.GetButton("Jump")) {
			GetComponent<Rigidbody>().velocity = new Vector3(velocity.x, Mathf.Sqrt(2 * jumpHeight * gravity), velocity.z);
			isGrounded = false;
		}
		
		// apply gravity
		GetComponent<Rigidbody>().AddForce(new Vector3 (0, -gravity * GetComponent<Rigidbody>().mass, 0));
	}

	void Update() 
	{
		MoveHead ();
		MoveBody ();

		if(Input.GetKey(KeyCode.Escape))
		{
			UnityEngine.Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
		}
	}

	private void MoveHead()
	{
		if (axes == RotationAxes.MouseXAndY)
		{
			float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;

			rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
			rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);

			transform.localEulerAngles = new Vector3(0, rotationX, 0);
			head.localEulerAngles = new Vector3(-rotationY,0,0);
		}
		else if (axes == RotationAxes.MouseX)
		{
			transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);
		}
		else
		{
			rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
			rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);

			transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
		}
	}

	private void MoveBody()
	{
		// check if the player is touching a surface below them
		CheckGrounded();

		// check if the player is running
		if (isGrounded && Input.GetKeyDown(KeyCode.LeftShift)) {
			isRunning = true;
		}

		// check if the player stops running
		if (Input.GetKeyUp(KeyCode.LeftShift)) {
			isRunning = false;
		}
	}


	private float CheckMoveDirection(float speed,Vector3 dir){

		float direction = speed;
		int dirModifier = 1;
		if (direction < 0) {
			dirModifier = -1;
		}

		//for (int cntX = 0; cntX < 2; cntX++) {
			for (int cntY = 0; cntY < 3; cntY++) {
				RaycastHit hit;
     
            Ray ray = new Ray (transform.position + (Vector3.up * (cntY + 1.5f) * 0.3f), dir * dirModifier * 0.5f );
            Debug.DrawRay (ray.origin,  ray.direction * charSkin, Color.magenta);
				// if there is something directly below the player
				if (Physics.Raycast (ray, out hit, charSkin)) {
                    if (!hit.transform.GetComponent<Collider>().isTrigger){
					    return 0;
                    }
				} 
			}
		//}
		return direction;

	}


	private void CheckGrounded() 
	{
        /* ==============
         * REMEMBER
         * ==============
         * If you change the size of the prefab, you may have
         * to change the length of the ray to ensure it hits
         * the ground.
         * 
         * All obstacles/walls/floors must have rigidbodies
         * attached to them. If not, Unity physics may get
         * confused and the player can jump really high
         * when in a corner between 2 walls for example.
         */
        float rayLength = 1f;
        RaycastHit hit;
        Ray ray = new Ray(transform.position, -transform.up);
 		Debug.DrawRay(ray.origin, ray.direction * rayLength);
        // if there is something directly below the player
        if (Physics.Raycast(ray, out hit, rayLength)) {
            isGrounded = true;
        }
	}


}
