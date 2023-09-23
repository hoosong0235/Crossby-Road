using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script : MonoBehaviour
{
    public GameObject DarkGrassPrefab, GrassPrefab, RoadPrefab, SplitPrefab, TreePrefab, RockPrefab, VehicleGenerator;
    GameObject terrain, split, decoration, vehicleGenerator;
    float x, z, random;
    int numGrass = 5;
    bool currRoad, nextRoad;
    Transform playerTransform;

    void generate()
    {
        random = Random.Range(0f, 3f);
        if (random < 1f)
        {
            if (++numGrass % 2 == 1) terrain = Instantiate(GrassPrefab) as GameObject;
            else terrain = Instantiate(DarkGrassPrefab) as GameObject;
            terrain.transform.position = new Vector3(x, 1f, 0f);
            nextRoad = false;
        }
        else
        {
            terrain = Instantiate(RoadPrefab) as GameObject;
            terrain.transform.position = new Vector3(x, 0f, 0f);
            nextRoad = true;
        }


        if (!nextRoad)
        {
            for (z = -65f; z < 110f; z += 5f)
            {
                random = Random.Range(0f, 10f);
                if (random < 1f)
                {
                    decoration = Instantiate(RockPrefab) as GameObject;
                    decoration.transform.position = new Vector3(x, 1f, z);
                }
                else if (random < 4f)
                {
                    decoration = Instantiate(TreePrefab) as GameObject;
                    decoration.transform.position = new Vector3(x, 1f, z);
                }
                else
                {

                }

            }
        }
        else
        {
            if (currRoad)
            {
                split = Instantiate(SplitPrefab) as GameObject;
                split.transform.position = new Vector3(x - 2.5f, 0f, -77.5f);
            }

            vehicleGenerator = Instantiate(VehicleGenerator) as GameObject;

            random = Random.Range(0f, 2f);
            if (random < 1f)
            {
                vehicleGenerator.transform.position = new Vector3(x, 0f, -65f);
            }
            else
            {
                vehicleGenerator.transform.position = new Vector3(x, 0f, 105f);
                vehicleGenerator.transform.rotation = Quaternion.Euler(new Vector3(180f, 0f, 180f));
            }

        }

        currRoad = nextRoad;
    }

    // Start is called before the first frame update
    void Start()
    {
        currRoad = false;
        playerTransform = GameObject.Find("Player").transform;

        for (x = 5f; x < 75f; x += 5f) generate();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTransform.position.x + 75f > x)
        {
            generate();
            x += 5f;
        }
    }
}
