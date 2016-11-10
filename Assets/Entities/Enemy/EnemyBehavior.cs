using UnityEngine;
using System.Collections;

public class EnemyBehavior : MonoBehaviour {

	public GameObject projectile;
	public float health = 150.0f;
	public float projectileSpeed = 15f;
	public float shotsPerSecond = 0.9f;
	public int scoreValue = 100;
	public AudioClip fireSound;
	public AudioClip deathSound;

	private ScoreKeeper scoreKeeper;

	void Start() {
		scoreKeeper = GameObject.Find("Score").GetComponent<ScoreKeeper>();
	}


	void Update ()
	{
		float probability = shotsPerSecond * Time.deltaTime;
		if (Random.value < probability) {
			Fire();
		}
	}

	void Fire() {
		GameObject missile = Instantiate(projectile, transform.position, Quaternion.identity) as GameObject;
		missile.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);
		AudioSource.PlayClipAtPoint(fireSound, transform.position);
	}

	void OnTriggerEnter2D (Collider2D collider)
	{
		Projectile missile = collider.gameObject.GetComponent<Projectile> ();
		if (missile) {
			health -= missile.getDamage ();
			missile.hit();
			if (health <= 0) {
				die();
			}
		}
	}

	void die() {
		scoreKeeper.addPoints(scoreValue);
		AudioSource.PlayClipAtPoint(deathSound, transform.position);
		Destroy(gameObject);
	}
}
