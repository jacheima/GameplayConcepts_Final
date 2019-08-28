using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public List<Transform> PlayerSpawnPositions;
    public List<Transform> EnemySpawnPositions;
    public List<Transform> PowerUpSpawnPositions;
    public List<Transform> availableforPUSpawn;
    public List<Transform> availableEnemySpawns;

    public List<GameObject> Enemies;
    public List<GameObject> Players;
    public List<GameObject> PowerUps;

    //index: 0 = speed, 1 = damage, 2 = health
    public List<GameObject> powerUpType;

    public int powerUpAmount = 1;
    public int enemiesAmount = 4;
    

    public GameObject enemyPrefab;
    public GameObject player1Prefab;

    public GameObject player2Prefab;

    public GameObject enemyParent;
    public GameObject powerUpParent;

    public bool singlePlayer = false;
    public bool multiplayer = false;

    public bool verticalSplit = true;
    public bool horizontalSplit = false;

    public bool levelOneLoaded = false;

    public GameTimer timer;
    public FacePlayer facePlayer;

    public Camera playerOneCam;
    public Camera playerTwoCam;

    public GameObject[] enemyHealthBarP1;
    public GameObject[] enemyHealthBarP2;

    public MapGenerator map;

    public Toggle randomMap;
    public Toggle levelOfTheDay;

    public AudioManager vol;

    public Slider musicVol;

    public AudioClip levelMusic;

    public Slider fxVol;



    //Static instance of GameManager which allows it to be accessed by any other script.
    public static GameManager instance = null;

    //Awake is always called before any Start functions
    void Awake()
    {
        //Check if instance already exists
        if (instance == null)
        {

            //if not, set instance to this
            instance = this;
        }
        //If instance already exists and it's not this:
        else if (instance != this)
        {
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
        }

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        
        
        powerUpAmount = Random.Range(1, PowerUpSpawnPositions.Count);

        //find the map generator game object and component on it
        map = GameObject.Find("MapGenerator").GetComponent<MapGenerator>();

    }

    void Update()
    {
        if (levelOneLoaded)
        {
            timer.CountDown();

        }

        if (PowerUps.Count < powerUpAmount)
        {
            SpawnPowerUps();
        }

        if (Enemies.Count < enemiesAmount)
        {
            SpawnEnemies();
        }
    }

    void OnLevelWasLoaded(int level)
    {
        if (level == 0)
        {
            
            Button options = GameObject.Find("Options").GetComponent<Button>();
            Button singlePlayer = GameObject.Find("SinglePlayer").GetComponent<Button>();
            Button multiplayer = GameObject.Find("Split Screen").GetComponent<Button>();

            options.onClick.AddListener(delegate {OptionsMenu();});
            singlePlayer.onClick.AddListener(delegate {SinglePlayerGame();});
            multiplayer.onClick.AddListener(delegate {SplitScreenGame();});
        }
        if (level == 1)
        {
            //Call the generate map function
            map.CreateMap();

            vol.music.clip = levelMusic;

            vol.music.Play();



            playerOneCam = GameObject.Find("Player1Camera").GetComponent<Camera>();

            //get all the player 1 world space canvas on the AI
            enemyHealthBarP1 = GameObject.FindGameObjectsWithTag("EnemyHealthP1");

            for (int i = 0; i < enemyHealthBarP1.Length; i++)
            {
                Canvas canvas = enemyHealthBarP1[i].GetComponent<Canvas>();

                canvas.worldCamera = playerOneCam;
            }

            if (singlePlayer)
            {
                //Set the camera rect to full screen
                playerOneCam.rect = new Rect(0f, 0f, 1f, 1f);
            }

            if (multiplayer)
            {
                //Find player 2's camera
                playerTwoCam = GameObject.Find("Player2Camera").GetComponent<Camera>();

                if (verticalSplit)
                {
                    //set the camera rects
                    playerOneCam.rect = new Rect(0f, 0f, .5f, 1f);
                    playerTwoCam.rect = new Rect(.5f, 0f, .5f, 1f); 
                }

                if (horizontalSplit)
                {
                    playerOneCam.rect = new Rect(0f, .5f, 1f, .5f);
                    playerTwoCam.rect = new Rect(0f, 0f, 1f, .5f);
                }
            }

            timer = GameObject.Find("GameTimer").GetComponent<GameTimer>();


            levelOneLoaded = true;
        }

        if (level == 3)
        {
            musicVol = GameObject.Find("BackgroundMusicControl").GetComponent<Slider>();
            fxVol = GameObject.Find("SoundEffectsControl").GetComponent<Slider>();
            vol.GetSlider();

            //find the button and set its event listener
            Button mainMenu = GameObject.Find("Main Menu").GetComponent<Button>();
            mainMenu.onClick.AddListener(delegate {MainMenu();});
            mainMenu.onClick.AddListener(delegate { SaveOptions();});

            //find the toggles and set the toggle event listeners
            randomMap = GameObject.Find("RandomMap").GetComponent<Toggle>();
            levelOfTheDay = GameObject.Find("LevelOfTheDay").GetComponent<Toggle>();
            randomMap.onValueChanged.AddListener(delegate { RandomMap(); });
            levelOfTheDay.onValueChanged.AddListener(delegate {LevelOfTheDay();});

        }
    }

    
    public void SpawnEnemies()
    {
        int amountNeeded = enemiesAmount - Enemies.Count;

        //for all the enemy spawn positions
        for (int i = amountNeeded; i > 0; i--)
        {
            int location = Random.Range(0, availableEnemySpawns.Count);

            //get the tile script attached to the parent of the enemy spawn position
            Tile tileScript = availableEnemySpawns[location].GetComponentInParent<Tile>();

            //Spawn Enmey
            GameObject enemy = Instantiate(enemyPrefab, availableEnemySpawns[location].position,
                availableEnemySpawns[location].rotation);

            //Set the enemy health bar
            enemy.GetComponent<PawnData>().healthBar = enemy.GetComponentInChildren<HealthBar>();

            //set the enemies name
            enemy.name = "DanTheCroc";

            //parent the enemies to the enemy gameObject
            enemy.GetComponent<Transform>().parent = enemyParent.GetComponent<Transform>();

            //Add the enemy to the list of enemies currently in the world
            Enemies.Add(enemy);

            //call the get waypoints function in the Tile script to assign each enemy all the waypoints of the tile they spawned on
            tileScript.GetWaypoints(enemy);

            availableEnemySpawns.Remove(enemy.transform);
        }
    }

    public void SpawnPlayers()
    {

        //choose a random spawn position from the player spawn point list
        int randomPlayer1Spawn = Random.Range(0, PlayerSpawnPositions.Count);

        //Instantiate player 1 at that location
        GameObject player1 = Instantiate(player1Prefab, PlayerSpawnPositions[randomPlayer1Spawn].position,
            PlayerSpawnPositions[randomPlayer1Spawn].rotation);

        //assign health bar
        player1.GetComponent<PawnData>().healthBar =  GameObject.Find("Player1HUD").GetComponentInChildren<HealthBar>();

        player1.name = "Player One";

        Players.Add(player1);

        //if the game is multiplayer
        if (multiplayer)
        {
            int randomPlayer2Spawn = Random.Range(0, PlayerSpawnPositions.Count);

            GameObject player2 = Instantiate(player2Prefab, PlayerSpawnPositions[randomPlayer2Spawn].position, PlayerSpawnPositions[randomPlayer2Spawn].rotation);

            player2.GetComponent<PawnData>().healthBar =
                GameObject.Find("Player2HUD").GetComponentInChildren<HealthBar>();

            player2.name = "Player Two";

            Players.Add(player2);
        }
        
    }

    public void RespawnPlayer(GameObject player)
    {
        int RandomLocation = Random.Range(0, PlayerSpawnPositions.Count);

        player.transform.position = PlayerSpawnPositions[RandomLocation].position;

        player.GetComponent<PawnData>().health = 100;

        player.SetActive(true);
    }

    public void SpawnPowerUps()
    {
        int amountNeeded = powerUpAmount - PowerUps.Count;

        for (int i = amountNeeded; i > 0; i--)
        {
            int type = Random.Range(0, powerUpType.Count);
            int location = Random.Range(0, availableforPUSpawn.Count);

            GameObject powerup = Instantiate(powerUpType[type], availableforPUSpawn[location].position,
                availableforPUSpawn[location].rotation);

            powerup.transform.parent = powerUpParent.transform;

            availableforPUSpawn.Remove(powerup.transform);

            PowerUps.Add(powerup);
        }
    }

    public void SinglePlayerGame()
    {
        singlePlayer = true;
        multiplayer = false;
        SceneManager.LoadScene(1);
    }

    public void SplitScreenGame()
    {
        singlePlayer = false;
        multiplayer = true;
        SceneManager.LoadScene(1);
    }

    public void GameOver()
    {
        SceneManager.LoadScene(2);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void SaveOptions()
    {
        Debug.Log("Saving");
        vol.VolumePrefs();
    }

    public void OptionsMenu()
    {
        SceneManager.LoadScene(3);
    }

    public void RandomMap()
    {
        if (randomMap.isOn)
        {
            map.randomMap = true;
        }

        if (!randomMap.isOn)
        {
            map.randomMap = false;
        }
    }

    public void LevelOfTheDay()
    {
        if (levelOfTheDay.isOn)
        {
            map.levelOfTheDay = true;
        }

        if (!levelOfTheDay.isOn)
        {
            map.levelOfTheDay = false;
        }
    }
}
