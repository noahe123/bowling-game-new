using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CountPinsSingle : MonoBehaviour {

	public ManagerSinglePlayer manager;
	public Text currentScore;
	public Pokeball ball;
	public bool round_completed;
	private bool ball_is_out;
	private int current_score;
	public GameObject pinsReplayText;
	int totalPins;
	public bool strike;

	void Start () {
		
		currentScore.text = "0";
		ball_is_out = false;
	}

	void OnTriggerEnter (Collider collider) {
		if (collider.gameObject.name == "Ball") {

			//audio.Play ();

		}
	}

	void OnTriggerExit (Collider collider) {
		if (!ball_is_out) 
		{
			if (collider.gameObject.name == "Ball") 
			{
				ball_is_out = true;
				Invoke ("roundCompleted", 3);
			}
		}
	}

	public void roundCompleted (){


		/*
		if (current_score == totalPins) {
			pinsReplayText.GetComponent<Text>().text = "Spare!!!";
			manager.gaming (0);
		} 
		 else
        {
			pinsReplayText.GetComponent<Text>().text = showScore().ToString();
		}*/
		if (strike)
        {
			strike = false;
			roundCompleted();
		}
		current_score = showScore();
		manager.gaming(current_score);
		ball.Resets ();
		ball_is_out = false;
		ball.increase_round();
	}
	public int showScore(){

		int score = 0;
		totalPins = 0;
		foreach (Pins pin in FindObjectsOfType<Pins>()) {
			if (pin.pin_has_fallen()) {
				score++;
			}
			totalPins++;
		}

		return score;
	}

	public void SetScoreText()
	{
		int myScore = showScore();
		if (ball.GetComponent<Pokeball>().round_number() % 2 == 0)
		{
			if (myScore == totalPins)
			{
				strike = true;
				pinsReplayText.GetComponent<Text>().text = "Strike!!!";
			}
			else
			{
				pinsReplayText.GetComponent<Text>().text = myScore.ToString();
			}
		}
        else
        {
			if (myScore == totalPins)
			{
				pinsReplayText.GetComponent<Text>().text = "Spare!!!";
			}
			else
			{
				pinsReplayText.GetComponent<Text>().text = myScore.ToString();
			}
		}
	}
}
