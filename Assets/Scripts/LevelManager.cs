using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public float waitToRespawn;
    public PlayerController thePlayer;

    public GameObject deathSposion;

    public int coinCount;
    private int coinBonusLifeCount;
    public int bonsusLifeThresehold;

    public Text coinText;

    public Image heart1;
    public Image heart2;
    public Image heart3;

    public Sprite heartFull;
    public Sprite heartHalf;
    public Sprite heartEmpty;

    public int maxHealth;
    public int healthCount;

    private bool respawning;

    private ResetOnRespawn[] objectsToReset;

    public bool invinciable;
    public Text livesText;

    public int startingLives;
    public int currentLives;

    public GameObject gameOverScreen;

    public bool respawnCoActive;

    // Start is called before the first frame update
    void Start()
    {
        thePlayer = FindObjectOfType<PlayerController>();
        healthCount = maxHealth;

        objectsToReset = FindObjectsOfType<ResetOnRespawn>();
        
        if (PlayerPrefs.HasKey("CoinCount"))
        {
            coinCount = PlayerPrefs.GetInt("CoinCount");
        }

        if (PlayerPrefs.HasKey("PlayerLives"))
        {
            currentLives = PlayerPrefs.GetInt("PlayerLives");
        }
        else
        {
            currentLives = startingLives;
        }

        coinText.text = "Coins: " + coinCount;
        livesText.text = "Lives x " + currentLives;

    }

    // Update is called once per frame
    void Update()
    {
        if (healthCount <= 0)
        {
            Respawn();
        }
           

        if (coinBonusLifeCount >= bonsusLifeThresehold)
        {
            currentLives += 1;
            livesText.text = "Lives x " + currentLives;
            coinBonusLifeCount -= bonsusLifeThresehold;
        }
    }

    public void Respawn()
    {
        if (!respawning)
        {
            currentLives -= 1;
            livesText.text = "Lives x " + currentLives;
            if (currentLives > 0)
            {
                respawning = true;
                StartCoroutine("RespawnCO");
            }
            else
            {
                thePlayer.gameObject.SetActive(false);
                gameOverScreen.SetActive(true);
                AudioManager.current.PlayGameoverMusic();
            }
        }   
    }
    
    public IEnumerator RespawnCO()
    {
        respawnCoActive = true;
        thePlayer.gameObject.SetActive(false);
        Instantiate(deathSposion, thePlayer.transform.position, thePlayer.transform.rotation);

        yield return new WaitForSeconds(waitToRespawn);


        respawnCoActive = false;
        healthCount = maxHealth;
        respawning = false;
        UpdateHeartMeter();

        coinCount = 0;
        coinText.text = "Coins: " + coinCount;
        coinBonusLifeCount = 0;

        thePlayer.transform.position = thePlayer.respawnPosition;
        thePlayer.gameObject.SetActive(true);

        for (int i = 0; i < objectsToReset.Length ; i++)
        {
            objectsToReset[i].gameObject.SetActive(true);
            objectsToReset[i].ResetObect();
        }
    }

    public void AddCoins(int coinsToAdd)
    {
        coinCount += coinsToAdd;
        coinBonusLifeCount += coinsToAdd;

        coinText.text = "Coins: " + coinCount;

        AudioManager.current.PlayCoinAudio();
    }
  

    public void HurtPlayer(int damageToTake)
    {
        if (!invinciable)
        {
            healthCount -= damageToTake;
            UpdateHeartMeter();

            thePlayer.Knockback();
        }
    }
        public void GiveHeath(int healthToGive)
        {
            healthCount += healthToGive;

        if (healthCount > maxHealth)
        {
            healthCount = maxHealth;
        }

        

        UpdateHeartMeter();
    }

    public void UpdateHeartMeter()
    {
        switch (healthCount)
        {
            case 6:
                heart1.sprite = heartFull;
                heart2.sprite = heartFull;
                heart3.sprite = heartFull;
                return;
            case 5:
                heart1.sprite = heartFull;
                heart2.sprite = heartFull;
                heart3.sprite = heartHalf;
                return;
            case 4:
                heart1.sprite = heartFull;
                heart2.sprite = heartFull;
                heart3.sprite = heartEmpty;
                return;
            case 3:
                heart1.sprite = heartFull;
                heart2.sprite = heartHalf;
                heart3.sprite = heartEmpty;
                return;
            case 2:
                heart1.sprite = heartFull;
                heart2.sprite = heartEmpty;
                heart3.sprite = heartEmpty;
                return;
            case 1:
                heart1.sprite = heartHalf;
                heart2.sprite = heartEmpty;
                heart3.sprite = heartEmpty;
                return;
            case 0:
                heart1.sprite = heartEmpty;
                heart2.sprite = heartEmpty;
                heart3.sprite = heartEmpty;
                return;
            default:
                heart1.sprite = heartEmpty;
                heart2.sprite = heartEmpty;
                heart3.sprite = heartEmpty;
                return;


            }
        }
    public void AddLives(int LivesToGive)
    {
        currentLives += LivesToGive;
        livesText.text = "Lives x " + currentLives;
    }
 }


    

