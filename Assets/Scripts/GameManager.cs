using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject diamondObject;
    public GameObject loopObject;
    public int boardSize = 5;
    public PlayerScript player;

    public float speedModifier = 1f;
    public int diamondCount;
    public int strikeCount;
    public bool isGameOver;
    public int diamondLocationX = 0;
    public int diamondLocationY = 0;

    public GameObject diamondTextObject;
    private Text diamondText;

    public Image[] strikes;
    public Image[] speedUpSignals;
    public GameObject gameOverScreen;

    public AudioSource speedUpSound;
    public CameraShake cameraShake;
    // Start is called before the first frame update
    void Start()
    {
        diamondText = diamondTextObject.GetComponent<Text>();
        // Instantiate diamond somewhere not where the player is at
        player = FindObjectOfType<PlayerScript>();
        // Generate board
        InitializeGame();
    }

    private void InitializeGame()
    {
        // Reset game data
        LoopScript[] existingLoops = FindObjectsOfType<LoopScript>();
        foreach (LoopScript loop in existingLoops)
        {
            Destroy(loop.gameObject);
        }
        GameObject[] existingDiamonds = GameObject.FindGameObjectsWithTag("Diamond");
        foreach (GameObject diamond in existingDiamonds)
        {
            Destroy(diamond);
        }
        foreach (Image strike in strikes)
        {
            strike.color = Color.white;
        }
        gameOverScreen.SetActive(false);
        speedModifier = 1f;
        diamondCount = 0;
        strikeCount = 0;
        isGameOver = false;
        diamondLocationX = 0;
        diamondLocationY = 0;
        diamondText.text = "0";
        // Initialize board
        int halfSize = Mathf.FloorToInt(boardSize * 0.5f);
        for (int x = -halfSize; x <= halfSize; x++)
        {
            for (int y = -halfSize; y <= halfSize; y++)
            {
                LoopScript newLoop = Instantiate(loopObject, new Vector2(x, y), Quaternion.identity)
                    .GetComponent<LoopScript>();
                if ((Mathf.Abs(x) == halfSize && Mathf.Abs(y) == halfSize) || (x == 0 && y == 0))
                {
                    newLoop.isStable = true;
                }
                newLoop.Initialize();
            }
        }
        CreateDiamondAtNewLocation();
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameOver && Input.GetKeyDown(KeyCode.Space))
        {
            InitializeGame();
        }
    }

    public void Strike()
    {
        strikeCount++;
        cameraShake.shakeDuration = 0.5f;
        for (int i = 0; i < strikeCount; i++)
        {
            Image strike = strikes[i];
            strike.color = Color.red;
        }
        if (strikeCount >= 3)
        {
            GameOver();
        }
        player.ResetPosition();

    }
    public void GameOver()
    {
        isGameOver = true;
        gameOverScreen.SetActive(true);
    }

    public void CreateDiamondAtNewLocation()
    {
        int diamondX = diamondLocationX;
        int diamondY = diamondLocationY;
        int halfSize = Mathf.FloorToInt(boardSize * 0.5f);
        while (diamondX == diamondLocationX && diamondY == diamondLocationY)
        {
            diamondX = Random.Range(-halfSize, halfSize + 1);
            diamondY = Random.Range(-halfSize, halfSize + 1);
        }
        Instantiate(diamondObject, new Vector2(diamondX, diamondY), Quaternion.identity);
        diamondLocationX = diamondX;
        diamondLocationY = diamondY;
    }

    public void CollectDiamond()
    {
        diamondCount++;
        diamondText.text = diamondCount.ToString();
        if (diamondCount % 10 == 0)
        {
            speedModifier = 1f + diamondCount * 0.05f;
            speedUpSound.Play();
            StartCoroutine("FlashSpeedUpSignal");
        }
        
    }

    private IEnumerator FlashSpeedUpSignal()
    {
        speedUpSignals[0].color = Color.white;
        speedUpSignals[1].color = Color.white;
        yield return new WaitForSeconds(1f);
        float currentAlpha = speedUpSignals[0].color.a;
        while (currentAlpha >= 0f)
        {
            Color newColor = new Color(1f, 1f, 1f, currentAlpha - Time.deltaTime);
            speedUpSignals[0].color = newColor;
            speedUpSignals[1].color = newColor;
            currentAlpha = newColor.a;
            yield return null;
        }
    }
}
