using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
    Text text;
    Transform playerTransform;
    int score, newScore, destroyed = 0;

    // Start is called before the first frame update
    void Start()
    {
        text = gameObject.GetComponent<Text>();
        playerTransform = GameObject.Find("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        destroyed = GameObject.Find("Player").GetComponent<PlayerController>().destroyed.Count;

        newScore = (int)playerTransform.position.x / 5 + destroyed;
        if (newScore > score)
            score = (int)playerTransform.position.x / 5 + destroyed;

        text.text = score.ToString();
    }
}
