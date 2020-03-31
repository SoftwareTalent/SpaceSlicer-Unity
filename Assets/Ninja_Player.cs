using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ninja_Player : MonoBehaviour {

	private Vector3 pos; //Position
	public int score = 0;


	// Use this for initialization
	void Start () {
		Screen.orientation = ScreenOrientation.Landscape;
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
	}
	
	// Update is called once per frame
	void Update () {
        if (GameManager.Instance.state != GameManager.GameState.Playing)
            return;
        if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
        {
			if (Input.touchCount == 1) {
				pos = Camera.main.ScreenToWorldPoint (new Vector3 (Input.GetTouch (0).position.x, Input.GetTouch (0).position.y, 1));
				transform.position = new Vector3 (pos.x, pos.y, 3);
				GetComponent<Collider2D> ().enabled = true;
				return;
			}
			GetComponent<Collider2D> ().enabled = false;
		} else {
			pos = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 0));
			transform.position = new Vector3 (pos.x, pos.y, 3);
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
        if (GameManager.Instance.state != GameManager.GameState.Playing)
            return;
		if (other.tag == "Fruit") 
		{
			other.GetComponent<Fruit2D>().Hit();
			score++;
			Debug.Log(score);
		}
		if (other.tag == "Enemy") 
		{
			//Run hit function
			other.GetComponent<Fruit2D>().Hit();

			score = score - 2;

			if (score < 0) score = 0;
		}
        if (other.tag == "Powerup")
        {
            other.GetComponent<Fruit2D>().Hit();
        }
	}
}
