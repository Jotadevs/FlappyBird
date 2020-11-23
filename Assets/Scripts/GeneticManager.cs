using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticManager : MonoBehaviour {

    public static GeneticManager geneticManager;

    public int PopulationLength = 200;
    public int WeightsLength = 5;
    public float MutationRate = 0.05f;
    public int GenerationNumber = 0;
    public GameObject FlappyBirdPrefab;
    private bool isRePopulation = true;
    private Dna[] Population;
    private Dna[] BestPopulationSelect;

    private void Awake()
    {
        if (geneticManager == null)
        {
            geneticManager = this;
        }
    }

    private void Start()
    {
        InitPop();  
        GameManager.gameManager.numberOfBirds = PopulationLength;
        GameManager.gameManager.currentFitness = 0;
        isRePopulation = false;
    }

    private void InitPop()
    {
        Population = new Dna[PopulationLength];

        for (int i = 0; i < PopulationLength; i++)
        {
            Population[i] = new Dna();
            Population[i].weights = new float[WeightsLength];

            for (int j = 0; j < WeightsLength; j++)
            {
                Population[i].weights[j] = Random.Range(-1.0f, 1.0f);
            }

            Population[i].fitness = 0;

            GameObject birdGameObject = Instantiate(FlappyBirdPrefab, new Vector3(-7, 0, 0), new Quaternion(0, 0, 0, 0));

            Population[i].bird = birdGameObject;

            birdGameObject.GetComponent<Bird>().SetDna(Population[i]);

        }
    }
    public void StartRePop()
    {
        if (isRePopulation)
        {
            return;
        }
        isRePopulation = true;
        StartCoroutine(RePop());
    }
    IEnumerator RePop()
    {
        yield return new WaitForSeconds(2f);

        SortPop();
        SelectBestPop();
        CrossOverTheBestPopSelection();
        FillThePopulationWithTheBestPop();
        FillTheRestPopWithNewDna();
        Mutate();

        GameManager.gameManager.StartTest(PopulationLength);
        yield return new WaitForSeconds(2f);

        InstantiateBirds();

        isRePopulation = false;
        GenerationNumber++;
    }
    
    private void SortPop()
    {
        for (int i = 0; i < Population.Length; i++)
        {
            for (int j = i + 1; j < Population.Length; j++)
            {
                if (Population[i].fitness < Population[j].fitness)
                {
                    Dna Temp = Population[i];
                    Population[i] = Population[j];
                    Population[j] = Temp;
                }
            }
        }
    }
    private void SelectBestPop()
    {
        BestPopulationSelect = new Dna[Population.Length / 4];

        for (int i = 0; i < PopulationLength / 4; i++)
        {
            BestPopulationSelect[i] = new Dna();
            BestPopulationSelect[i] = Population[i];
        }
    }
    private void CrossOverTheBestPopSelection()
    {
        Population = new Dna[PopulationLength];

        for (int i = 0; i < PopulationLength / 4; i++)
        {
            int a = Random.Range(0, BestPopulationSelect.Length);

            Population[i] = new Dna();
            Population[i].weights = new float[WeightsLength];

            int Mid = Random.Range(1, WeightsLength - 1);

            for (int j = 0; j < Mid; j++)
            {
                Population[i].weights[j] = BestPopulationSelect[i].weights[j];
            }
            for (int j = Mid; j < WeightsLength; j++)
            {
                Population[i].weights[j] = BestPopulationSelect[a].weights[j];
            }
        }
    }
    private void FillThePopulationWithTheBestPop()
    {
        int Index = 0;
        
        for (int i = PopulationLength / 4; i < PopulationLength / 2; i++)
        {
            Population[i] = new Dna();

            Population[i].weights = new float[BestPopulationSelect[0].weights.Length];

            for (int j = 0; j < WeightsLength; j++)
            {
                Population[i].weights[j] = BestPopulationSelect[Index].weights[j];
            }

            Index++;
        }
    }
    private void FillTheRestPopWithNewDna()
    {
        
        for (int i = PopulationLength / 2; i < PopulationLength; i++)
        {
            Population[i] = new Dna();

            Population[i].weights = new float[BestPopulationSelect[0].weights.Length];

            for (int j = 0; j < Population[i].weights.Length; j++)
            {
                Population[i].weights[j] = Random.Range(-1.0f, 1.0f);
            }

        }
    }
    private void Mutate()
    {
        for (int i = 0; i < PopulationLength; i++)
        {
            Dna dna = Population[i];

            for (int j = 0; j < WeightsLength; j++)
            {
                if (Random.Range(0.0f, 1.0f) < MutationRate)
                {
                    dna.weights[j] = Random.Range(-1.0f, 1.0f);
                }
            }
        }
    }
    private void InstantiateBirds()
    {
        for (int i = 0; i < PopulationLength; i++)
        {
           
            GameObject prefab = Instantiate(FlappyBirdPrefab, new Vector3(-7, 0, 0), new Quaternion(0, 0, 0, 0));
            Population[i].bird = prefab;
            Population[i].fitness = 0;
            prefab.GetComponent<Bird>().SetDna(Population[i]);
        }
    }
    
}
