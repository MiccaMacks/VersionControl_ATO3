using UnityEngine;

public class Movement : MonoBehaviour
{
    private float moveSpeed = 3f, sprintSpeed = 2.2f, crouchSpeed = 0.7f, jumpHeight = 3f;
    private CharacterController player;
    private Vector3 velocity;
    private float gravity = 9.8f, currentSpeed, speedVel;

    //smooth variables
    private Vector3 smoothVelocity, currentPos = Vector3.zero;
    [SerializeField] private float smoothTime = 0.1f;

    private void Start()
    {
        player = GetComponent<CharacterController>();
    }
    private void Update()
    {
        //create a new movement vector
        Vector3 movement = Vector3.zero;

        //add the x and z axis movement to it
        movement += transform.forward * Input.GetAxisRaw("Vertical");
        movement += transform.right * Input.GetAxisRaw("Horizontal");

        //smooth the movement by a small amount, to keep it responsive but remove jittery controls.
        currentPos = Vector3.SmoothDamp(currentPos, movement.normalized, ref smoothVelocity, smoothTime);

        //initialise a temporary speed value to the base move speed.
        float tempSpeed = moveSpeed;

        //if the player is moving then check for crouch and sprint.
        if(movement != Vector3.zero)
        {
            if (Input.GetKey(KeyCode.LeftShift)) { tempSpeed *= sprintSpeed; }
            else if (Input.GetKey(KeyCode.LeftControl)) { tempSpeed *= crouchSpeed; }
        }

        //smooth the speed, as with the movement smoothing, an immediate increase/decrease to speed feels unnatural.
        currentSpeed = Mathf.SmoothDamp(currentSpeed, tempSpeed, ref speedVel, smoothTime);

        //check if player is grounded
        if (player.isGrounded)
        {
            //set velocity to zero if grounded
            velocity = Vector3.zero;

            //check if jumping
            if (Input.GetKey(KeyCode.Space))
            {
                //Debug.Log("Player Jumped!");
                velocity.y += jumpHeight - gravity * Time.deltaTime;
            }
            //gravity is still applied if they didnt jump, we just dont want to apply gravity to them as well as gravity to their jump.
            else
            {
                velocity.y -= gravity * Time.deltaTime;
            }
        }
        else
        {
            //apply gravity if not grounded too (because they will be falling.)
            velocity.y -= gravity * Time.deltaTime;
        }

        //move the player, multiply smoothed movement by smoothed speed, then add the y movement to it, then multiply it all by Time.deltaTime.
        player.Move(((currentPos * currentSpeed) + velocity)* Time.deltaTime);
    }


}
