using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {

	public GameObject enemyPrefab;
	public float width = 10f;
	public float height = 5f;
	public float speed = 5f;
	public float spawnDelay = 0.5f;

	private bool movingRight = true;
	private float xMax;
	private float xMin;

	// Use this for initialization
	void Start ()
	{
		float distanceToCamera = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftBoundary = Camera.main.ViewportToWorldPoint(new Vector3(0,0,distanceToCamera));
		Vector3 rightBoundary = Camera.main.ViewportToWorldPoint(new Vector3(1,0,distanceToCamera));
		xMin = leftBoundary.x;
		xMax = rightBoundary.x;

		spawnEnemiesUntilFull();

	}

	void OnDrawGizmos()
	{
		Gizmos.DrawWireCube(transform.position, new Vector2(width, height));
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (movingRight) {
			transform.position += new Vector3 (speed * Time.deltaTime, 0, 0);
		} else {
			transform.position += new Vector3 (-speed * Time.deltaTime, 0, 0);
		}

		float rightEdgeOfFormation = transform.position.x + 0.5f * width;
		float leftEdgeOfFormation = transform.position.x - 0.5f * width;

		if (leftEdgeOfFormation < xMin) {
			movingRight = true;
		} else if (rightEdgeOfFormation > xMax) {
			movingRight = false;
		}

		if (allMembersDead ()) {
			spawnEnemiesUntilFull();
		}

	}

	Transform nextFreePosition() {

		foreach (Transform childPositionGameObject in transform) {
			if (childPositionGameObject.childCount == 0) {
				return childPositionGameObject;
			}
		}
		return null;
	}

	bool allMembersDead ()
	{
		foreach (Transform childPositionGameObject in transform) {
			if (childPositionGameObject.childCount > 0) {
				return false;
			}
		}
		return true;
	}

	void spawnEnemiesUntilFull ()
	{
		Transform freePosition = nextFreePosition();
		if (freePosition) {
			GameObject enemy = Instantiate (enemyPrefab, freePosition.position, Quaternion.identity) as GameObject;
			enemy.transform.parent = freePosition;	
		}
		if (nextFreePosition ()) {
			Invoke("spawnEnemiesUntilFull", spawnDelay);
		}
	}

}
