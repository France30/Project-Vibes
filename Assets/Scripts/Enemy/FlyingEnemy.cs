using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : EnemyController
{
    [SerializeField] private float hoverSpeed = 1f;
    [SerializeField] private float hoverDistance = 1f;
    [Range(0, .3f)] [SerializeField] private float movementSmoothing = 0.05f;

    private Rigidbody2D rb2D;
    private Vector3 velocity = Vector3.zero;

    private float currentHoverDistance;
    private bool isHoveringUp = true;

    private bool isAttacking = false;


    protected override void Awake()
    {
        base.Awake();
        rb2D = GetComponent<Rigidbody2D>();
    }

    protected override void Flip()
    {
        base.Flip();
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    protected override void MoveToTargetDirection(Transform target)
    {
        base.MoveToTargetDirection(target);

        //allows for more free movement
        moveSpeed = Mathf.Abs(moveSpeed) * -1; //moveSpeed value must always be negative
        float move = (moveSpeed * Time.fixedDeltaTime) * 3f;
        Vector2 direction = (transform.position - target.position).normalized;
        Vector3 targetVelocity = direction * move;
        rb2D.velocity = Vector3.SmoothDamp(rb2D.velocity, targetVelocity, ref velocity, movementSmoothing);
    }

    private void FixedUpdate()
    {
        if(!isAttacking)
            Hover();
    }

    private void Hover()
    {       
        float hover = hoverSpeed * Time.fixedDeltaTime;
        currentHoverDistance += hover;
        if (currentHoverDistance >= hoverDistance && isHoveringUp)
            FlipHover();
        else if(currentHoverDistance <= hoverDistance && !isHoveringUp)
            FlipHover();

        Vector3 targetVelocity = new Vector2(rb2D.velocity.x, hover);
        rb2D.velocity = Vector3.SmoothDamp(rb2D.velocity, targetVelocity, ref velocity, movementSmoothing);
    }

    private void FlipHover()
    {
        isHoveringUp = !isHoveringUp;
        hoverDistance *= -1;
        hoverSpeed *= -1;
    }
}
