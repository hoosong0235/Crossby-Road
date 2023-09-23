using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderController : MonoBehaviour
{

    Transform playerTransform;
    float newX;

    // Start is called before the first frame update
    void Start()
    {
        this.playerTransform = GameObject.Find("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        newX = playerTransform.position.x + 25f;
        if (newX >= transform.position.x)
            gameObject.transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }
}
