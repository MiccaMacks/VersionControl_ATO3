using UnityEngine;

public class CameraController : MonoBehaviour
{
    /* Description:
     * This script will be attatched to an empty container on the player.
     * The camera will be a child of the container.
     * 
     * This script handles the rotation of this object and the player, 
     * a seperate script attatched to this object will handle the zoom in and out
     * as well as preventing the camera from clipping through surfaces.
     * 
     */

    private float currentDistance = -6f;

    private float sensitivity = 2.5f;
    private float drag = 1.5f;
    private Vector2 mouseDir, smoothing, result;
    [SerializeField] private float clampLow = 20, clampHigh = 60;
    [SerializeField] private float zoomLow = 3, zoomHigh = 16;


    [SerializeField] private Transform player, camPos;

    public bool CanLook = true;

    private void Start()
    {
        if (player == null)
        {
            player = GetComponentInParent<Movement>().gameObject.transform;
        }
        if (camPos == null)
        {
            camPos = GetComponentInChildren<Camera>().gameObject.transform;
            currentDistance = camPos.localPosition.z;
        }
    }

    private void Update()
    {
        if (CanLook)
        {
            Look();
            if(Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                currentDistance += Input.GetAxis("Mouse ScrollWheel") * 4;
                currentDistance = Mathf.Clamp(currentDistance, -zoomHigh, -zoomLow);
                camPos.localPosition = new Vector3(0, 0, currentDistance);
            }
        }
    }


    private void Look()
    {
        mouseDir = new Vector2(Input.GetAxisRaw("Mouse X") * sensitivity, Input.GetAxisRaw("Mouse Y") * sensitivity);
        smoothing = Vector2.Lerp(smoothing, mouseDir, 1 / drag);
        result += smoothing;
        result.y = Mathf.Clamp(result.y, -clampHigh, clampLow);

        transform.localRotation = Quaternion.AngleAxis(-result.y, Vector3.right);
        player.rotation = Quaternion.AngleAxis(result.x, player.up);
    }
}
