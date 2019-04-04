using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public bool bossActive;

    public float timeBetweenBats;
    private float timeBetweenBatsStore;
    private float batCount;

    public Transform leftPoint;
    public Transform rightPoint;
    public Transform rightBatSpawnPoint;
    public Transform leftBatSpawnPoint;

    public GameObject bat;

    public GameObject theBoss;
    public bool bossRight;

    public bool takeDamge;

    public int startingHeath;
    private int currentHeath;

    public GameObject levelExit;

    private CameraController theCamera;

    private LevelManager theLevelManager;
    private SpriteRenderer theSpriteRenderer;


    public bool waitingForRespawn;
    // Start is called before the first frame update
    void Start()
    {
        timeBetweenBatsStore = timeBetweenBats;
        batCount = timeBetweenBats;
        currentHeath = startingHeath;

        theCamera = FindObjectOfType<CameraController>();

        theBoss.transform.position = rightPoint.position;
        bossRight = true;

        theLevelManager = FindObjectOfType<LevelManager>();
        theSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (theLevelManager.respawnCoActive)
        {
            bossActive = false;
            waitingForRespawn = true;
        }

        if (waitingForRespawn && theLevelManager.respawnCoActive)
        {
            theBoss.SetActive(false);

            timeBetweenBats = timeBetweenBatsStore;

            batCount = timeBetweenBats;

            theBoss.transform.position = rightPoint.transform.position;

            currentHeath = startingHeath;

            theCamera.followTarget = true;

            waitingForRespawn = false;
           
           FindObjectOfType<DestroyOnReset>().Destroy();
            
        }

        if (bossActive)
        {
            theCamera.followTarget = false;
            theCamera.transform.position = Vector3.Lerp(theCamera.transform.position, new Vector3(transform.position.x, theCamera.transform.position.y, theCamera.transform.position.z), theCamera.smoothing * Time.deltaTime);

            theBoss.SetActive(true);
            if (batCount > 0)
            {
                batCount -= Time.deltaTime;
            }
            else
            {
                Instantiate(bat, rightBatSpawnPoint.position, rightBatSpawnPoint.rotation);
                Instantiate(bat, leftBatSpawnPoint.position, rightBatSpawnPoint.rotation);
                batCount = timeBetweenBats;
            }
        }

        if (takeDamge)
        {
            currentHeath -= 1;
            if (currentHeath <= 0)
            {
                levelExit.SetActive(true);

                theCamera.followTarget = true;
                gameObject.SetActive(false);
            }

            if (bossRight)
            {
                theBoss.transform.position = leftPoint.position;
            }
            else
            {
                theBoss.transform.position = rightPoint.position;
               
            }

            bossRight = !bossRight;

            timeBetweenBats = timeBetweenBats / 2f;

            takeDamge = false;
        }
    }

     void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            bossActive = true;
        }
    }
}

