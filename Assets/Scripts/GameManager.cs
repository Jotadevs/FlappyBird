using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public WallBuilder wallBuilder;
    public int numberOfBirds = 0;
    public float bestFitness = 0;
    public float currentFitness = 0;

    private List<GameObject> walls = new List<GameObject>();
    private bool testEnd = true;

    public static GameManager gameManager;

    private void Awake()
    {
        if (gameManager == null)
            gameManager = this;
    }

	// Update is called once per frame
	void Update () {
		if(numberOfBirds <= 0)
        {
            EndTest();
            return;
        }
        currentFitness += Time.deltaTime;
	}
    private void EndTest()
    {
        testEnd = true;
        walls.Clear();
        wallBuilder.StopBuilding();

        if (bestFitness < currentFitness)
        {
            bestFitness = currentFitness;
        }

        GeneticManager.geneticManager.StartRePop();
    }
    public void StartTest(int PopulationLength)
    {
        numberOfBirds = PopulationLength;
        testEnd = false;
        wallBuilder.ResumeBuilding();
        currentFitness = 0;
    }
    public void AddWall(GameObject wall)
    {
        walls.Add(wall);
    }
    public GameObject GetWallByIndex(int index)
    {
        return walls[index];
    }
    
}
