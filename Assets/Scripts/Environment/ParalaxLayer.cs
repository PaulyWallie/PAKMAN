using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    [Range(0f, 1f)]
    [SerializeField] private float parallaxAmount = 0.2f;

    private Transform cam;
    private Vector3 startPosition;
    private Vector3 cameraStartPosition;

    private void Awake()
    {
        cam = Camera.main.transform;
        startPosition = transform.position;
        cameraStartPosition = cam.position;
    }

    private void LateUpdate()
    {
        float distance = cam.position.x - cameraStartPosition.x;

        Vector3 pos = startPosition;
        pos.x += distance * parallaxAmount;

        transform.position = pos;
    }
}