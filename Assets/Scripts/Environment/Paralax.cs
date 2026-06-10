using UnityEngine;

public class Paralax : MonoBehaviour
{
    Transform cam;
    public Transform sky, hills;
    [Range(0f,1f)]public float parallaxSpeed;
   
    private Vector3 skyStartPos, hillsStartPos;
   
    void Start()
    {
        cam = Camera.main.transform;
        if (sky != null) skyStartPos = sky.position;
        if (hills != null) hillsStartPos = hills.position;
    }
   
    void Update()
    {
        if (sky != null)
            sky.position = new Vector3(cam.position.x, skyStartPos.y, sky.position.z);

        if (hills != null)
            hills.position = new Vector3(cam.position.x * parallaxSpeed, hillsStartPos.y, hills.position.z);
    }
}
