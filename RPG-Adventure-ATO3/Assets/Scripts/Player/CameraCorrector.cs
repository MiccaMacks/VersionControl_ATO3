using UnityEngine;

public class CameraCorrector : MonoBehaviour
{
    //CameraEndPos is the position CameraController is attempting to put the camera.
    public Transform CameraEndPos;

    //variables for smoothing
    private Vector3 currentVel;
    [SerializeField] private float smoothSpeed = 0.2f;

    private void Awake()
    {
        if (CameraEndPos == null)
        {
            Debug.LogError("CameraEndPos needs to be given to the CameraCorrector script!!!");
        }
    }

    private void Update()
    {
        /* Older attempts
        //transform.localPosition = Vector3.Lerp(CameraEndPos.localPosition, transform.localPosition, 0.75f);
        //currentPos = Vector3.SmoothDamp(currentPos, CameraEndPos.localPosition - transform.localPosition, ref currentVel, smoothSpeed);*/

        //smooths the movement betwen current pos and target pos by smoothSpeed.
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, CameraEndPos.localPosition, ref currentVel, smoothSpeed);
    }
}
