using UnityEngine;

public class CameraCorrector : MonoBehaviour
{
    [SerializeField] private Transform CameraEndPos;

    [SerializeField] private Vector3 smoothedPos;
    [SerializeField] private Vector3 currentVel;
    [SerializeField] private float smoothSpeed = 0.2f;

    private void Update()
    {
        transform.localPosition = Vector3.Lerp(CameraEndPos.localPosition, transform.localPosition, 0.75f);
    }
}
