using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CharacterController2D))]
public class PlayerMovement : MonoBehaviour 
{
	[SerializeField] private float _moveSpeed = 40f;
	[SerializeField] private float _jumpTime = 0.5f;
	[Range(0,100)][SerializeField] private float _jumpBoost = 20f;

	private Rigidbody2D _rb2D;
	private CharacterController2D _controller;
	private Animator _animator;

	private float _horizontalMove = 0f;
	private bool _jump = false;
	private bool _isJumping = false;
	private bool _isGrounded = false;
	private bool _crouch = false;

	private float _currentJumpTime = 0f;

	public void OnLanding()
	{
		_animator.SetBool("Jump", false);
		_isGrounded = true;
	}

	private void Awake()
    {
		_rb2D = GetComponent<Rigidbody2D>();
		_controller = GetComponent<CharacterController2D>();
		_animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update () 
	{
		//Walk/Run
		_horizontalMove = Input.GetAxisRaw("Horizontal") * _moveSpeed;
		_animator.SetFloat("Speed", Mathf.Abs(_horizontalMove));

		if (Input.GetButtonDown("Jump") && _isGrounded)
			Jump();
	}

	private void FixedUpdate ()
	{
		// Move our character
		_controller.Move(_horizontalMove * Time.fixedDeltaTime, _crouch, _jump);
		_jump = false;
	}

	private void Jump()
    {
		_jump = true;
		_isGrounded = false;
		_animator.SetBool("Jump", true);
	}
}
