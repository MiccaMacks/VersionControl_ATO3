using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 3f, sprintSpeed = 1.8f, crouchSpeed = 0.7f;
    private CharacterController player;


    private void Start()
    {
        player = GetComponent<CharacterController>();
    }
    private void Update()
    {
        Vector3 movement = Vector3.zero;
        movement += transform.forward * Input.GetAxisRaw("Vertical");
        movement += transform.right * Input.GetAxisRaw("Horizontal");
        float currentSpeed = moveSpeed;
        if(Input.GetKey(KeyCode.LeftShift)) { currentSpeed *= sprintSpeed; }
        else if(Input.GetKey(KeyCode.LeftControl)) { currentSpeed *= crouchSpeed; }
        player.Move(movement * currentSpeed * Time.deltaTime);
    }


}
