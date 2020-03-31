using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit2D : MonoBehaviour {

	private bool canBeDead; //If we can destroy the object
	private Vector3 screen; //Position on the screen
	public GameObject splataster; //The splat prefab of the fruit
    public GameObject splatasterpowerup; //The splat prefab of the fruit
    public GameObject textaster; //The splat prefab of the fruit
    public GameObject splatbomb; //The splat prefab of the fruit
    
    public Sprite[] textures;
    int spidx = 0;
    public float speed;

    int PowerUp = 0;
	// Use this for initialization
	void Start () {
        StartCoroutine(changeSprite(speed));
        if (gameObject.tag == "Powerup")
        {
            PowerUp = (Random.RandomRange(1, 10) % 2) + 1;

            
        }
	}

    IEnumerator changeSprite(float dt)
    {
        GetComponent<SpriteRenderer>().sprite = textures[spidx];

        yield return new WaitForSeconds(dt);
        spidx++;
        if (spidx >= textures.Length ) spidx = 0;

        StartCoroutine(changeSprite(speed));
    }
	
	// Update is called once per frame
	void Update () {

        if (GameManager.Instance.state == GameManager.GameState.GameOver)
        {
            Destroy(gameObject);
            return;
        }
		screen = Camera.main.WorldToScreenPoint (transform.position);
		if (canBeDead && screen.y < -20) {
			//Destroy
			Destroy (gameObject);
		} else if (!canBeDead && screen.y > -10) {
			canBeDead = true;
		}
	}

	public void Hit()
	{
		Destroy(gameObject);

		if (tag == "Fruit") {
			//Spawn splat prefab of the fruit
			Instantiate (splataster, new Vector2 (transform.position.x, transform.position.y), Quaternion.identity);
            if (GameManager.Instance.PowerUP == 1)
            {
                Instantiate(splatasterpowerup, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
            }
            else
            {
                Instantiate(textaster, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
            }
            
            AudioController.SharedInstance().PlaySound("goal");
            GameManager.Instance.Score++;
            if (GameManager.Instance.PowerUP == 1)
            {
                GameManager.Instance.Score++;
            }

        }
        else if (tag == "Enemy")
        {
            
            if (GameManager.Instance.PowerUP == 2)
            {
                return;    
            }
            GameManager.Instance.Lives--;
            Instantiate(splatbomb, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
            AudioController.SharedInstance().PlaySound("bomb");

            if (GameManager.Instance.Lives <= 0)
            {
                AudioController.SharedInstance().PlaySound("GameOver");
                if (PlayerPrefs.HasKey("BestScore") == false)
                {
                    GameManager.Instance.BestScore = GameManager.Instance.Score;
                    GameManager.Instance.OpenPopup(1);
                }
                else if (GameManager.Instance.Score > GameManager.Instance.BestScore)
                {
                    GameManager.Instance.BestScore = GameManager.Instance.Score;
                    GameManager.Instance.OpenPopup(0);
                }
                else
                {
                    GameManager.Instance.BestScore = GameManager.Instance.Score;
                    GameManager.Instance.OpenPopup(2);
                }
            }
        }
        else if (tag == "Powerup")
        {
            GameManager.Instance.StartedPowerup(PowerUp);
            Instantiate(splataster, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
            AudioController.SharedInstance().PlaySound("mystery");
        }
	}
}
