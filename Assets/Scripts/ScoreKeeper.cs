using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreKeeper : MonoBehaviour {

	public static int score;

	private Text myText;

	void Start() {
		myText = GetComponent<Text>();
		resetScore();
	}

	// Use this for initialization
	public void addPoints(int points) {
		score += points;
		myText.text = score.ToString();
	}

	public static void resetScore() {
		score = 0;
	}
}
