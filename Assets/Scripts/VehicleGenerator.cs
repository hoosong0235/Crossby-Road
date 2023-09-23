using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleGenerator : MonoBehaviour
{
    public GameObject[] prefabs;
    int index;
    float timer, period, speed;
    GameObject vehicle;

    void generate()
    {
        vehicle = Instantiate(prefabs[index]) as GameObject;

        vehicle.transform.position = transform.position;
        vehicle.transform.rotation = transform.rotation;
        vehicle.GetComponent<VehicleController>().speed = speed;
    }

    // Start is called before the first frame update
    void Start()
    {
        index = Random.Range(0, prefabs.Length);
        timer = 0f;
        period = Random.Range(2f, 4f);
        speed = Random.Range(8f, 16f);

        generate();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > period)
        {
            timer = 0f;
            generate();
        }
    }
}
