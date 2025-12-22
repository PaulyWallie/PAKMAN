using UnityEngine;

public class HurtPlayer : MonoBehaviour
{
    private LevelManager theLevelManager;
    private AudioManager audioManager;

    public int damageToGive;

    // Start is called before the first frame update
    void Start()
    {
        theLevelManager = FindObjectOfType<LevelManager>();
        audioManager = FindObjectOfType<AudioManager>();
    }

     void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            //theLevelManager.HurtPlayer(damageToGive);
            //audioManager.PlayHitHurtAudio();
        }
    }
}
