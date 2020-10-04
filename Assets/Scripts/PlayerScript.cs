using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    GameManager gameManager;
    int posX = 0;
    int posY = 0;
    public float coolDown = 0f;
    public AudioSource pickUpSound;
    public DashSound dashSound;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.isGameOver)
        {
            return;
        }
        if (coolDown > 0f)
        {
            coolDown -= Time.deltaTime;
            return;
        }
        int boardSize = Mathf.FloorToInt(gameManager.boardSize * 0.5f);
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            if (posY < boardSize)
            {
                transform.Translate(new Vector2(0, 1));
                posY++;
                dashSound.Play();
            }
            
        } else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            if (posY > -boardSize)
            {
                transform.Translate(new Vector2(0, -1));
                posY--;
                dashSound.Play();
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            if (posX > -boardSize)
            {
                transform.Translate(new Vector2(-1, 0));
                posX--;
                dashSound.Play();
            }

        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            if (posX < boardSize)
            {
                transform.Translate(new Vector2(1, 0));
                posX++;
                dashSound.Play();
            }
        }
    }

    public void ResetPosition()
    {
        posX = 0;
        posY = 0;
        transform.localPosition = Vector2.zero;
        coolDown = 0.5f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Diamond")
        {
            gameManager.CollectDiamond();
            pickUpSound.Play();
            Destroy(collision.gameObject);
            gameManager.CreateDiamondAtNewLocation();
        }
    }
}
