using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour {

    public float flapPower = 200;
    private int currentWallIndex = 0;
    private Dna dna;
    private NeuralNetwork neuralNetwork;

    private void Awake()
    {
        currentWallIndex = 0;
        neuralNetwork = GetComponent<NeuralNetwork>();
    }

    public void SetDna(Dna dna)
    {
        this.dna = dna;
    }
    // Update is called once per frame
    void Update () {
        Vector2 currentWallPos = GetCurrentWallPos();

        float NeuralNetworkResult = neuralNetwork.GetNeuralNetworkResult(dna, currentWallPos);

        if (NeuralNetworkResult > 0.5)
        {
            Flap();
        }
    }
    
    private Vector2 GetCurrentWallPos()
    {
        GameObject currentWall = GameManager.gameManager.GetWallByIndex(currentWallIndex);

        Vector2 currentWallPos = currentWall.transform.position;
        currentWallPos.x += 0.8f;

        if (CheckIfCrossTheCurrentWall(currentWallPos))
        {
            currentWallIndex++;
            currentWallPos = currentWall.transform.position;
            currentWallPos.x += 0.8f;
        }

        return currentWallPos;
    }
    private bool CheckIfCrossTheCurrentWall(Vector2 currentWallPos)
    {
        return currentWallPos.x - transform.position.x <= 0;
    }
    private void Flap()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Rigidbody2D>().AddForce(Vector2.up * flapPower);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            Death();
        }
    }
    private void Death()
    {
        GameManager.gameManager.numberOfBirds--;
        dna.fitness = GameManager.gameManager.currentFitness;
        Destroy(gameObject);
    }
}
