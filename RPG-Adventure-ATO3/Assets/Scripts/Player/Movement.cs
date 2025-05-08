using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 3f, sprintSpeed = 1.8f, crouchSpeed = 0.7f, jumpHeight = 3f;
    private CharacterController player;
    private Vector3 velocity;
    [SerializeField] bool grounded;
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
       
        Vector3 movement = Vector3.zero;
        movement += transform.forward * Input.GetAxisRaw("Vertical");
        movement += transform.right * Input.GetAxisRaw("Horizontal");
        currentPos = Vector3.SmoothDamp(currentPos, movement.normalized, ref smoothVelocity, smoothTime);

        float tempSpeed = moveSpeed;
        if(movement != Vector3.zero)
        {
            if (Input.GetKey(KeyCode.LeftShift)) { tempSpeed *= sprintSpeed; }
            else if (Input.GetKey(KeyCode.LeftControl)) { tempSpeed *= crouchSpeed; }
        }

        currentSpeed = Mathf.SmoothDamp(currentSpeed, tempSpeed, ref speedVel, smoothTime);

        if (player.isGrounded)
        {
            grounded = true;
            velocity = Vector3.zero;

            if (Input.GetKey(KeyCode.Space))
            {
                Debug.Log("Player Jumped!");
                velocity.y += jumpHeight - gravity * Time.deltaTime;
            }
            else
            {
                velocity.y -= gravity * Time.deltaTime;
            }
        }
        else
        {
            grounded = false;
            velocity.y -= gravity * Time.deltaTime;
        }
        player.Move(((currentPos * currentSpeed) + velocity)* Time.deltaTime);
    }


}
