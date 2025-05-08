using UnityEngine;

public class CameraController : MonoBehaviour
{
    /* Description:
     * This script will handle the players mouse inputs for moving the camera.
     * 
     * It clamps the cameras vertical angle between 2 values, to prevent the player from flipping upside down,
     * and to prevent the player from clipping the camera through the floor.
     * 
     * It will also prevent the camera from being blocked by the environment, to ensure the player always has a clear view of everything
     */

    //local z axis of the cameras pos and the pos the player set it to.
    private float currentDistance = -6f, desiredDistance = -6f;

    //smoothing
    private float clippingSmoothVel;

    //sensitivity of the players inputs
    private float sensitivity = 2.5f;

    //drag and mouse smoothing
    private float drag = 1.5f;
    private Vector2 mouseDir, smoothing, result;

    //cam min and max angle
    [SerializeField] private float clampLow = 0, clampHigh = 60;

    //closest and farthest pos for cam on local z axis
    [SerializeField] private float zoomLow = 0, zoomHigh = 16;

    //ref to camera
    private Camera playerCamera;

    //player transform and desired cam pos 
    //camPos should NOT be the camera itself, and instead a transform that the camera will be following.
    private Transform player, camPos;

    public bool CanLook = true;

    private void Start()
    {
        //get player transform
        if (player == null)
        {
            player = GetComponentInParent<Movement>().gameObject.transform;
        }
        if (playerCamera == null)
        {
            playerCamera = GetComponentInChildren<Camera>();
        }
        if (camPos == null && playerCamera != null)
        {
            camPos = playerCamera.gameObject.GetComponent<CameraCorrector>().CameraEndPos;
            //check if camPos transform is given.
            //Debug.LogError("camPos needs to have the transform of the transform the camera is tracking attatched.");
        }

        //if we have campos, we set current and desired distance to the local z
        if(camPos != null) 
        { 
            currentDistance = camPos.localPosition.z;
            desiredDistance = camPos.localPosition.z;
        }

        //turn off and lock cursor. will not stay in this script.
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (CanLook)
        {
            Look();
            /*
            if(Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                currentDistance += Input.GetAxis("Mouse ScrollWheel") * 5;
                desiredDistance += Input.GetAxis("Mouse ScrollWheel") * 5;
            }*/

            //if scrolling
            if(Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                float value = Input.GetAxis("Mouse ScrollWheel");

                //check if cam is closer than player wants, and if the player is trying to bring it closer
                if(currentDistance > desiredDistance && value > 0)
                {
                    //if so, set the players desired distance to the current distance.
                    desiredDistance = currentDistance;
                }

                //add the given input
                currentDistance += value * 5;
                desiredDistance += value * 5;
            }

            //clamp the distance values
            currentDistance = Mathf.Clamp(currentDistance, -zoomHigh, -zoomLow);
            desiredDistance = Mathf.Clamp(desiredDistance, -zoomHigh, -zoomLow);

            //set camPos local pos to currentDistance.
            camPos.localPosition = new Vector3(0, 0, currentDistance);

            //set nearClipPlane depending on how close the camera currently is to the player.
            if(desiredDistance < -3 && currentDistance < -3)
            {
                //camera.nearClipPlane = 3;

                playerCamera.nearClipPlane = Mathf.SmoothDamp(playerCamera.nearClipPlane, 3, ref clippingSmoothVel, 0.2f);
            }
            else
            {
                //camera.nearClipPlane = 1;

                playerCamera.nearClipPlane = Mathf.SmoothDamp(playerCamera.nearClipPlane, 0.5f, ref clippingSmoothVel, 0.2f);
            }
        }
    }


    private void Look()
    {
        //get mouse inputs
        mouseDir = new Vector2(Input.GetAxisRaw("Mouse X") * sensitivity, Input.GetAxisRaw("Mouse Y") * sensitivity);

        //smooth mouse inputs
        smoothing = Vector2.Lerp(smoothing, mouseDir, 1 / drag);

        //add smoothing to result vector
        result += smoothing;

        //clamp y axis
        result.y = Mathf.Clamp(result.y, -clampHigh, clampLow);

        //rotate camera by result on the Y, and player on the X
        transform.localRotation = Quaternion.AngleAxis(-result.y, Vector3.right);
        player.rotation = Quaternion.AngleAxis(result.x, player.up);

        //Raycast from player to the max distance the camera can be
        //Debug.DrawLine(transform.position, camPos.position, Color.red, 0.6f);
        if(Physics.Raycast(transform.position, camPos.position - transform.position, out RaycastHit hitInfo, zoomHigh))
        {
            //check if the hit object is in the environment
            //Debug.Log(hitInfo.collider.name);
            if (hitInfo.collider.CompareTag("Environment"))
            {
                if(-hitInfo.distance < desiredDistance)
                {
                    //if distance of object is further than desired distance, ignore it
                    currentDistance = desiredDistance;
                }
                else
                {
                    //else move camera to that spot
                    currentDistance = -hitInfo.distance;
                }
            }
        }
        else
        {
            //if nothing is hit, set cam to desired pos
            currentDistance = desiredDistance;
        }
    }
}
