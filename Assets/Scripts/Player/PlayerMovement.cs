using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController2D))]
public class PlayerMovement : MonoBehaviour {

	[SerializeField] private float moveSpeed = 40f;

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
		animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
		horizontalMove = Input.GetAxisRaw("Horizontal") * moveSpeed;

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
