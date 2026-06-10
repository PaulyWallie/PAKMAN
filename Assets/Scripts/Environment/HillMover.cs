using UnityEngine;

public class HillMover : MonoBehaviour
{
    public float maxDistance = 22f;
    void Start()
    {
        
    }

    void Update()
    {
        float distance = transform.position.x - Camera.main.transform.position.x;

        if(distance > maxDistance )
            transform.position -= new Vector3(maxDistance * 2, 0, 0);
        else if(distance < -maxDistance )
            transform.position += new Vector3(maxDistance * 2, 0, 0);
    }
}
