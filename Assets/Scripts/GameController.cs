using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	public GameObject[] hazards;
	public Vector3 spawnValues;
	public int hazardCount;
	public float spawnWait;
	public float startWait;
	public float waveWait;

	public Text scoreText;
	public Text restartText;
	public Text gameOverText;

	private int score;
	private bool restart;
	private bool gameOver;

	void Start() {
		gameOver = false;
		restart = false;
		restartText.text = "";
		gameOverText.text = "";
		score = 0;
		UpdateScore();
		StartCoroutine(SpawnWaves());
	}

	void Update() {
		if (restart) {
			if (Input.GetKeyDown(KeyCode.R)) {
				Application.LoadLevel(Application.loadedLevel);
				//SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // 新寫法
			}
		}
	}

	IEnumerator SpawnWaves() {
		yield return new WaitForSeconds(startWait);
		while (true) {
			for (int i = 0; i < hazardCount; i++) {
				GameObject hazard = hazards[Random.Range(0, hazards.Length)];
				/*bool flag = (Random.value > 0.5f)
				  if (flag) {
					  spawnValues.z (16 or -16)
				  } */
				Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
				Quaternion spawnRotation = Quaternion.identity;
				GameObject clone = Instantiate(hazard, spawnPosition, spawnRotation);//创建一个障碍物，赋值给clone
				ReverseDirection (clone);
				yield return new WaitForSeconds(spawnWait);
			}
			yield return new WaitForSeconds(waveWait);
			if (gameOver) {
				restartText.text = "Press 'R' for Restart";
				restart = true;
				break;
			}
		}
	}
//保证一个石头面向上方，飞向下方
	void ReverseDirection (GameObject clone) {
		Quaternion r = clone.transform.rotation;
		r.y = 0;
		clone.transform.rotation = r;
		clone.GetComponent<Mover>().speed = -5;
	}

	public void AddScore(int newScoreValue) {
		score += newScoreValue;
		UpdateScore();
	}

	void UpdateScore() {
		scoreText.text = "Score: " + score;
	}

	public void GameOver() {
		gameOverText.text = "Game Over";
		gameOver = true;
	}
}
