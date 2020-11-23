using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBuilder : MonoBehaviour {
    public GameObject wallPrefab;
    public float buildingRate = 3.5f;
    private float timer = 3.5f;
    private bool stop = false;
    
	
	// Update is called once per frame
	void Update () {
        if (stop)
        {
            return;
        }

        timer += Time.deltaTime;

        if (timer >= buildingRate)
        {
            BuildWall();
            timer = 0;
        }
    }
    void BuildWall()
    {
        GameObject wall = Instantiate(wallPrefab
            , new Vector3(12, Random.Range(-2.8f, 2.6f), 0)
            , new Quaternion(0, 0, 0, 0));
        GameManager.gameManager.AddWall(wall);
    }
    public void ResumeBuilding()
    {
        stop = false;
        timer = buildingRate;
    }
    public void StopBuilding()
    {
        stop = true;
    }
}
