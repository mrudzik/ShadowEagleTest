using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{

	private Vector2 movementVector = Vector2.zero;
	public Vector2 MovementVector { get { return movementVector.normalized; } }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		MovementInput();
		
    }




	void MovementInput()
	{
		if (Input.GetKeyDown(KeyCode.W))	movementVector.y += 1;
		if (Input.GetKeyUp(KeyCode.W))		movementVector.y -= 1;

		if (Input.GetKeyDown(KeyCode.S))	movementVector.y -= 1;
		if (Input.GetKeyUp(KeyCode.S))		movementVector.y += 1;

		if (Input.GetKeyDown(KeyCode.D))	movementVector.x += 1;
		if (Input.GetKeyUp(KeyCode.D))		movementVector.x -= 1;

		if (Input.GetKeyDown(KeyCode.A))	movementVector.x -= 1;
		if (Input.GetKeyUp(KeyCode.A))		movementVector.x += 1;

	}



}
