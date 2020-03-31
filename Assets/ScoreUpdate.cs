using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScoreUpdate : MonoBehaviour {

	// Use this for initialization

    public Text txtScore;
    public Text BestScore;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if( txtScore != null)
            txtScore.text = GameManager.Instance.Score.ToString();

        if( BestScore != null)
            BestScore.text = GameManager.Instance.BestScore.ToString();
	}
}
