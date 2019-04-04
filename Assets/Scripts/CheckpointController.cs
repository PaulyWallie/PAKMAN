using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    public Sprite TorchUnlit;
    public Sprite TorchLit;

    private SpriteRenderer theSpriteRenderer;

    public bool checkpointActive;

    // Start is called before the first frame update
    void Start()
    {
        theSpriteRenderer = GetComponent<SpriteRenderer>(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

     void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            theSpriteRenderer.sprite = TorchLit;
            checkpointActive = true;
        }        
    }
}
