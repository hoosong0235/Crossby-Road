using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
    private Transform playerTransform;
    float newX, newZ;

    // Start is called before the first frame update
    void Start()
    {
        this.playerTransform = GameObject.Find("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        newX = playerTransform.position.x - 16f;
        if (newX >= transform.position.x)
            transform.position = new Vector3(newX, transform.position.y, transform.position.z);

        newZ = playerTransform.position.z - 12f;
        transform.position = new Vector3(transform.position.x, transform.position.y, newZ);

        if (15f <= transform.position.x - newX || -35f >= playerTransform.position.z || 55f <= playerTransform.position.z)
            SceneManager.LoadScene(0);
    }
}
