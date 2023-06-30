using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CharacterController2D))]
public class PlayerMovement : MonoBehaviour 
{
	[SerializeField] private float _moveSpeed = 40f;
	[SerializeField] private float _jumpTime = 0.5f;
	[Range(0,100)][SerializeField] private float _jumpBoost = 20f;

	private float _currentJumpTime = 0f;

	private Rigidbody2D _rb2D;
	private CharacterController2D _controller;
	private Animator _animator;

	private float _horizontalMove = 0f;
	private bool _jump = false;
	private bool _crouch = false;

	private bool _isJumping = false;
	private bool _isGrounded = false;


	private const float _COYOTE_TIME = 0.2f;
	private float _coyoteTimeCounter = _COYOTE_TIME;

	public void OnLanding()
	{
		_isGrounded = true;

		Debug.Log("Player Landed");
	}

	public void OnFall()
    {
		_animator.SetBool("Jump", false);
		_isGrounded = false;

		Debug.Log("Player Falling");
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

		CoyoteTime();

		if (_coyoteTimeCounter < _COYOTE_TIME && Input.GetButtonDown("Jump"))
			Jump();

		if (Input.GetButton("Jump") && _isJumping)
			JumpBoost();

		//Cancel Jump Boost if Jump Button is released
		if(!Input.GetButton("Jump"))
        {
			_isJumping = false; //Disable Jump Boost
			_currentJumpTime = 0f; //Reset Jump Boost Timer
		}

		//Coyote Time Jump Spam Prevention
		if (Input.GetButtonUp("Jump"))
        {		
			_coyoteTimeCounter = _COYOTE_TIME; 
		}
	}

	private void FixedUpdate ()
	{
		// Move our character
		_controller.Move(_horizontalMove * Time.fixedDeltaTime, _crouch, _jump);

		//Apply Jump Boost
		if (_isJumping)
			_rb2D.AddForce(new Vector2(0, _jumpBoost));

		_jump = false;
	}

	private void Jump()
    {
		_jump = true;
		_isJumping = true;
		_isGrounded = false;
		_animator.SetBool("Jump", true);
	}

	private void JumpBoost()
    {
		_currentJumpTime += Time.deltaTime;
		if (_currentJumpTime < _jumpTime) return;

		_isJumping = false;
		_currentJumpTime = 0f;
    }

	private void CoyoteTime()
    {
		if (_isGrounded)
			_coyoteTimeCounter = 0f;
		else
			_coyoteTimeCounter += Time.deltaTime;
    }
}
