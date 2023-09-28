using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
    public GameObject player;
    Text text;
    Transform playerTransform;
    PlayerController playerController;
    int score, newScore, destroyed = 0;

    // Start is called before the first frame update
    void Start()
    {
        text = gameObject.GetComponent<Text>();
        playerTransform = player.GetComponent<Transform>();
        playerController = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        destroyed = playerController.destroyed.Count;

        newScore = (int)playerTransform.position.x / 5 + destroyed;
        if (newScore > score)
            score = (int)playerTransform.position.x / 5 + destroyed;

        text.text = score.ToString();
    }
}
