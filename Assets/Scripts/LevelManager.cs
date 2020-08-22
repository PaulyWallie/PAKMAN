using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public float waitToRespawn;

    public float timeInLevel;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        timeInLevel = 0f;
    }
    private void Update()
    {
        timeInLevel += Time.deltaTime;
    }

    public void RespawnPlayer()
    {
        StartCoroutine(RespawnCo());
    }

     private IEnumerator RespawnCo()
    {
        PlayerController.instance.gameObject.SetActive(false);

        yield return new WaitForSeconds(2f);

        PlayerController.instance.transform.position = CheckpointController.instance.spawnPoint;

        PlayerController.instance.gameObject.SetActive(true);
        //AudioManager.instance.PlaySFX()
    }

    public void EndLevel()
    {
        StartCoroutine(EndLevelCo());
    }

    IEnumerator EndLevelCo()
    {
        //AudioManager.instance.PlayVictoryMusic()
        PlayerController.instance.stopInput = true;

        yield return new WaitForSeconds(2f);

    }

}



