using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopScript : MonoBehaviour
{
    public bool isStable;
    public float speed;
    public bool isGrowing;
    public SpriteRenderer sprite;
    public GameManager gameManager;
    public AudioSource contractSound;
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void Initialize()
    {
        speed = Random.Range(0.15f, 0.3f);
        sprite = GetComponent<SpriteRenderer>();
        if (isStable)
        {
            sprite.color = Color.green;
            transform.localScale = new Vector2(0.8f, 0.8f);
        }
        else
        {
            sprite.color = Color.yellow;
            float initialSize = Random.Range(0.25f, 1f);
            transform.localScale = new Vector2(initialSize, initialSize);
            float isGrowingRand = Random.Range(0f, 1f);
            if (isGrowingRand > 0.5f)
            {
                isGrowing = true;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isStable)
        {
            return;
        }
        // If size <= 0.25, grow; if size >= 1, shrink
        float size = transform.localScale.x;
        float deltaTime = Time.deltaTime;
        float newSize = size;
        if (size <= 0.25f)
        {
            isGrowing = true;
        }
        if (size >= 1f)
        {
            isGrowing = false;
        }
        if (isGrowing)
        {
            newSize = size + deltaTime * (speed * gameManager.speedModifier);
        } else
        {
            newSize = size - deltaTime * (speed * gameManager.speedModifier);
        }
        transform.localScale = new Vector2(newSize, newSize);
        if (newSize < 0.45)
        {
            sprite.color = Color.red;
        } else
        {
            sprite.color = Color.yellow;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && transform.localScale.x < 0.45)
        {
            contractSound.Play();
            gameManager.Strike();
        }
    }
}
