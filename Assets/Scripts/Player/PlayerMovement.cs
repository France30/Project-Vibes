using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController), typeof(CharacterController2D))]
public class PlayerMovement : MonoBehaviour {

	private CharacterController2D _controller;
	private Animator _animator;

	private float _horizontalMove = 0f;
	private bool _jump = false;
	private bool _crouch = false;

	public float MoveSpeed { get; set; }


	private void Start()
    {
		_controller = GetComponent<CharacterController2D>();
		_animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update () 
	{
		//Walk/Run
		_horizontalMove = Input.GetAxisRaw("Horizontal") * MoveSpeed;
		_animator.SetFloat("Speed", Mathf.Abs(_horizontalMove));

		if (Input.GetButtonDown("Jump"))
		{
			_jump = true;
			_animator.SetBool("Jump", true);
		}
	}

	public void OnLanding()
    {
		_animator.SetBool("Jump", false);
	}

	private void FixedUpdate ()
	{
		// Move our character
		_controller.Move(_horizontalMove * Time.fixedDeltaTime, _crouch, _jump);
		_jump = false;
	}
}
