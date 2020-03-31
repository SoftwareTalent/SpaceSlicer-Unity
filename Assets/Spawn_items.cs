using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_items : MonoBehaviour {

	public float spawnTime=1; //Spawn Time
    
	public GameObject[] apple; //Apple prefab
	public GameObject[] bomb; 
	public float upForce = 750; //Up force
	public float leftRightForce = 200; //Left and right force
	public float maxX = -7; //Max x spawn position
	public float minX = 7; //Min x spawn position

	// Use this for initialization

    public static Spawn_items Instance;

    int _ct = 0;

    void Awake()
    {
        Instance = this;
    }

	void Start () {
		//Start the spawn update
		
	}

    public void StartSpawn()
    {
        _ct = 1;
        StartCoroutine("Spawn");
    }
    public void StopSpawn()
    {
        StopAllCoroutines();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator Spawn()
	{
        if (GameManager.Instance.state == GameManager.GameState.Playing)
        {
            //Wait spawnTime
            yield return new WaitForSeconds(spawnTime);
            _ct++;

            if (_ct % 10 == 0 )
            {
                if((_ct/10) % 2 == 0)
                    spawnTime = 0.4f;
                else
                    spawnTime = 1.0f;
            }
            
            //Spawn prefab is apple
            GameObject prefab = apple[Random.RandomRange(0, apple.Length)];
            //If random is over 30
            if (Random.Range(0, 100) < 30)
            {
                //Spawn prefab is bomb
                prefab = bomb[Random.RandomRange(0, bomb.Length)];
            }
            //Spawn prefab add randomc position
            GameObject go = Instantiate(prefab, new Vector3(Random.Range(minX, maxX + 1), transform.position.y, 0f), Quaternion.Euler(0, 0, Random.Range(-90F, 90F))) as GameObject;
            //If x position is over 0 go left
            if (go.transform.position.x > 0)
            {
                go.GetComponent<Rigidbody2D>().AddForce(new Vector2(-leftRightForce, upForce));
            }
            //Else go right
            else
            {
                go.GetComponent<Rigidbody2D>().AddForce(new Vector2(leftRightForce, upForce));
            }
            //Start the spawn again
            StartCoroutine("Spawn");
        }
	}
}
