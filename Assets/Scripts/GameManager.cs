using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}

    public float initialGameSpeed = 5f;
    public float gameSpeedIncrease = 0.1f;
    public float gameSpeed {get; private set;}

    public Canvas startMenu;
    public Canvas gameOverMenu;
    public Canvas scoreUI;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI hiscoreText;
    public AudioSource gameOverSound;
    public AudioSource bgMusic;

    private Player player;
    private Spawner spawner;
    private float score;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }

    private void OnDestroy()
    {
        Application.targetFrameRate = 60;
        if (Instance == this)
        {
            Instance = null;
        }   
    }

    private void Start()
    {
        player = FindObjectOfType<Player>();
        spawner = FindObjectOfType<Spawner>();
        gameSpeed = 0f;
        enabled = false;
        player.gameObject.SetActive(false);
        spawner.gameObject.SetActive(false);
    }

    public void NewGame()
    {
        Obstacle[] obstacles = FindObjectsOfType<Obstacle>();

        foreach (var obstacle in obstacles)
        {
            Destroy(obstacle.gameObject);
        }

        bgMusic.Play();
        gameSpeed = initialGameSpeed;
        enabled = true; 

        score = 0f;
        player.gameObject.SetActive(true);
        spawner.gameObject.SetActive(true);
        startMenu.gameObject.SetActive(false);
        gameOverMenu.gameObject.SetActive(false);
        scoreUI.gameObject.SetActive(true);

        UpdateHiscore();
    }

    public void GameOver()
    {
        gameOverSound.Play();
        bgMusic.Stop();
        gameSpeed = 0f;
        enabled = false;

        player.gameObject.SetActive(false);
        spawner.gameObject.SetActive(false);
        gameOverMenu.gameObject.SetActive(true);

        UpdateHiscore();
    }

    private void Update()
    {
        gameSpeed += gameSpeedIncrease * Time.deltaTime;
        score += gameSpeed * Time.deltaTime;

        scoreText.text = Mathf.FloorToInt(score).ToString("D5");
    }

    private void UpdateHiscore()
    {
        float hiscore = PlayerPrefs.GetFloat("hiscore", 0);

        if (score > hiscore)
        {
            hiscore = score;
            PlayerPrefs.SetFloat("hiscore", hiscore);
        }

        hiscoreText.text = Mathf.FloorToInt(hiscore).ToString("D5");
    }
}
