using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallPrefab : MonoBehaviour {

    public float Speed = 3f;

    private GameManager gameManager;

    // Use this for initialization
    void Start () {
		gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }
	
	// Update is called once per frame
	void Update () {
        transform.Translate(Vector2.left * Time.deltaTime * Speed);
        if (gameManager.numberOfBirds <= 0)
            Destroy(gameObject);
    }
}
