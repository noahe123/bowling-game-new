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

		current_score = showScore ();
		manager.gaming (current_score);

		if (current_score == 10) {
			pinsReplayText.GetComponent<Text>().text = "Strike!!!";
			manager.gaming (0);
		} 
		 else
        {
			pinsReplayText.GetComponent<Text>().text = showScore().ToString();
		}
		ball.Resets ();
		ball_is_out = false;
		ball.increase_round();
	}
	public int showScore(){

		int score = 0;

		foreach (Pins pin in GameObject.FindObjectsOfType<Pins>()) {
			if (pin.pin_has_fallen()) {
				score++;
			}
		}

		return score;
	}

	public void SetScoreText()
	{
		if (current_score == 10) 
		{
			pinsReplayText.GetComponent<Text>().text = "Strike!!!";
		}
        else
        {
			pinsReplayText.GetComponent<Text>().text = showScore().ToString();
		}
	}
}
