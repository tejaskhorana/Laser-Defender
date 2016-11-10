using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float speed = 5.0f;
	public float padding = 1.0f;
	public GameObject projectile;
	public float projectileSpeed = 15.0f;
	public float firingRate = 0.4f;
	public float health = 250f;
	public AudioClip fireSound;

	private float xMin;
	private float xMax;

	// Use this for initialization
	void Start () {
		//works with a single camera
		float distance = transform.position.z - Camera.main.transform.position.z;

		Vector3 leftMost = Camera.main.ViewportToWorldPoint(new Vector3(0,0,distance));
		Vector3 rightMost = Camera.main.ViewportToWorldPoint(new Vector3(1,0,distance));
		xMin = leftMost.x + padding;
		xMax = rightMost.x - padding;

	}

	void Fire ()
	{
		Vector3 offset = new Vector3(0, 1, 0);
		GameObject beam = Instantiate(projectile, transform.position+offset, Quaternion.identity) as GameObject;
		beam.GetComponent<Rigidbody2D>().velocity = new Vector3(0, projectileSpeed,0);
		AudioSource.PlayClipAtPoint(fireSound, transform.position);
	}

	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Space)) {
			//time before first invocation is second arg, do a little over to prevent bug)
			InvokeRepeating ("Fire", 0.000001f, firingRate);
		}
		if (Input.GetKeyUp (KeyCode.Space)) {
			CancelInvoke("Fire");
		}

		Vector2 newPosition = this.transform.position;
		if (Input.GetKey(KeyCode.A)) {
			//alternatively transform.position += new Vector3(-speed, 0, 0)
			//alternatively transform.position += Vector3.left * speed * Time.deltaTime
			//deltaTime allows for frame rate independent. longer time to render, faster speed, and v.v.
			newPosition.x -= speed*Time.deltaTime;
		} else if (Input.GetKey(KeyCode.D)) {
			newPosition.x += speed*Time.deltaTime;
		}
		this.transform.position = newPosition;

		//restrict player to game space 
		float newX = Mathf.Clamp(transform.position.x, xMin, xMax);
		this.transform.position = new Vector2(newX, transform.position.y);
	}

	void OnTriggerEnter2D (Collider2D collider)
	{
		Projectile missile = collider.gameObject.GetComponent<Projectile> ();
		if (missile) {
			health -= missile.getDamage ();
			missile.hit();
			if (health <= 0) {
				LevelManager levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
				levelManager.LoadLevel("Win Screen");
				Destroy(gameObject);
			}
		}
	}

}
