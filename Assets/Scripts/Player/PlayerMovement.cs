using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	[SerializeField] private float runSpeed = 40f;

	private CharacterController2D controller;

	private float horizontalMove = 0f;
	private bool jump = false;
	private bool crouch = false;


    private void Awake()
    {
		controller = GetComponent<CharacterController2D>();
    }

    // Update is called once per frame
    private void Update () 
	{
		horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

		if (Input.GetButtonDown("Jump")) jump = true;
	}

	private void FixedUpdate ()
	{
		// Move our character
		controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
		jump = false;
	}
}
