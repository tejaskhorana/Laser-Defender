using UnityEngine;
using System.Collections;

public class Shredder : MonoBehaviour {

	void OnTriggerEnter2D (Collider2D col)
	{
		Destroy(col.gameObject);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
