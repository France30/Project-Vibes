using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CharacterController2D))]
public class PlayerMovement : MonoBehaviour 
{
	[SerializeField] private float _moveSpeed = 40f;

	private Rigidbody2D _rb2D;
	private CharacterController2D _controller;
	private Animator _animator;

	private float _horizontalMove = 0f;
	private bool _jump = false;
	private bool _crouch = false;


	public void OnLanding()
	{
		_animator.SetBool("Jump", false);
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

		if (Input.GetButtonDown("Jump"))
		{
			_jump = true;
			_animator.SetBool("Jump", true);
		}
	}

	private void FixedUpdate ()
	{
		// Move our character
		_controller.Move(_horizontalMove * Time.fixedDeltaTime, _crouch, _jump);
		_jump = false;
	}
}
