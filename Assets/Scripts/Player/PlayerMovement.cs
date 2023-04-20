using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	[SerializeField] private float runSpeed = 40f;

	private CharacterController2D controller;
	private Animator animator;

	private float horizontalMove = 0f;
	private bool jump = false;
	private bool crouch = false;


    private void Awake()
    {
		controller = GetComponent<CharacterController2D>();
		animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update () 
	{
		//Walk/Run
		horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
		animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

		if (Input.GetButtonDown("Jump"))
		{
			jump = true;
			animator.SetBool("Jump", true);
		}
	}

	public void OnLanding()
    {
		animator.SetBool("Jump", false);
	}

	private void FixedUpdate ()
	{
		// Move our character
		controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
		jump = false;
	}
}
