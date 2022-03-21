using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PlayerState
{
    idle,
    walk,
    attack,
    interact,
    stagger
}


public class PlayerMovement : MonoBehaviour
{
    public PlayerState currentState;
    public float speed;
    private Rigidbody2D myRigidBody2D;
    private Vector2 change; // Could be Vector2 but we use Vector3 to 
    private Animator animator;
    public IntValue currentHealth;
    public Signal playerHealthSignal;
    public Vector2Value startingPosition;


    // Start is called before the first frame update
    void Start()
    {
        currentState = PlayerState.walk;
        myRigidBody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        animator.SetFloat("movY", -1);  // Tell the animator we're looking down. Otherwise, if we attack before moving, all 4 hitboxes activates

        transform.position = startingPosition.value;
    }


    void FixedUpdate()
    {
        // Get movement input
        change = Vector2.zero;
        change.x = Input.GetAxisRaw("Horizontal");
        change.y = Input.GetAxisRaw("Vertical");
        change.Normalize(); //Make sure we don't move faster on the diagonals

        if ((currentState == PlayerState.idle) || (currentState == PlayerState.walk))
		{
            UpdateAnimationAndMove();
        }

    }

    void Update()
    {
        // The "attack" input is defined in Edit>Preferences>Input Manager
        if (Input.GetButtonDown("attack")  &&  (currentState != PlayerState.attack)  &&  (currentState != PlayerState.stagger))
        {
            StartCoroutine(AttackCo());
        }
    }


    void UpdateAnimationAndMove()
    {
        // Move player & update animation
        if (change != Vector2.zero)
        {
            transform.Translate(speed * Time.deltaTime * change);
            animator.SetFloat("movX", change.x);
            animator.SetFloat("movY", change.y);
            animator.SetBool("moving", true);

        }
        else
        {
            animator.SetBool("moving", false);
        }
    }


    private IEnumerator AttackCo()
	{
        currentState = PlayerState.attack;

        animator.SetBool("attacking", true);    // Trigger the player's animation transition
        yield return null;                      // We only need 'attacking' to be true for 1 cycle
        animator.SetBool("attacking", false);

        yield return new WaitForSeconds(.33f);  // After some time, update player's internal state machine
        currentState = PlayerState.walk;
    }


    public void Knock(Vector3 attackerPosition, float thrust, float knockTime, int damage)
	{        
        currentHealth.value -= damage;
        playerHealthSignal.Raise();

        if (currentHealth.InitialValue > 0)   StartCoroutine(KnockCo(attackerPosition, thrust, knockTime));
	}



    private IEnumerator KnockCo(Vector3 attackerPosition, float thrust, float knockTime)
    {
        if ((attackerPosition != null)  &&  (currentState != PlayerState.stagger))
        {
            currentState = PlayerState.stagger;
            myRigidBody2D = GetComponent<Rigidbody2D>();

            // Start moving in the specified direction at the set speed
            Vector2 throwDirection = transform.position - attackerPosition;
            Vector2 speedVector = throwDirection.normalized * thrust;
            myRigidBody2D.velocity = speedVector;

            // After some time, stop the movement
            yield return new WaitForSeconds(knockTime);
            myRigidBody2D.velocity = Vector2.zero;

            currentState = PlayerState.idle;
        }
    }


}
