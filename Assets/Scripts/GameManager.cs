using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int level;
    public int baseSeed;

    private int prevRoomPlayerHealth;
    private int prevRoomPlayerCoins;

    private Player player;

    public static GameManager instance;

    void Awake ()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start ()
    {
        level = 1;
        baseSeed = PlayerPrefs.GetInt("Seed");
        Random.InitState(baseSeed);
        Generation.instance.Generate();
        UI.instance.UpdateLevelText(level);

        player = FindObjectOfType<Player>();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void GoToNextLevel ()
    {
        prevRoomPlayerHealth = player.curHp;
        prevRoomPlayerCoins = player.coins;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void OnSceneLoaded (Scene scene, LoadSceneMode mode)
    {
        if(scene.name != "Game")
        {
            Destroy(gameObject);
            return;
        }

        player = FindObjectOfType<Player>();
        level++;
        baseSeed++;

        Generation.instance.Generate();

        player.curHp = prevRoomPlayerHealth;
        player.coins = prevRoomPlayerCoins;

        UI.instance.UpdateHealth(prevRoomPlayerHealth);
        UI.instance.UpdateCoinText(prevRoomPlayerCoins);
        UI.instance.UpdateLevelText(level);
    }
}