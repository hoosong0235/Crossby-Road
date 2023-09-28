using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndController : MonoBehaviour
{
    public GameObject score, bestScore, pauseButton, title, player, ParticleGenerator, background;
    GameObject particleGenerator;
    float titleStartPosX, titleEndPosX, titleNewPosX;
    float backgroundTimer, backgroundTime, backgroundStartTime, newAlpha;
    Image backgroundImage;
    float timer, time;

    // Start is called before the first frame update
    void Start()
    {
        backgroundTimer = 0f;
        backgroundStartTime = 1f / 2f;
        backgroundTime = 1f / 2f;
        backgroundImage = background.GetComponent<Image>();

        timer = 0f;
        time = 1f;

        titleStartPosX = -1280f;
        titleEndPosX = titleStartPosX + 1280f;

        score.SetActive(false);
        bestScore.SetActive(false);
        pauseButton.SetActive(false);

        title.SetActive(true);

        particleGenerator = Instantiate(ParticleGenerator) as GameObject;
        particleGenerator.transform.position = player.transform.position;
        Destroy(player);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        titleNewPosX = Mathf.Lerp(titleEndPosX, titleStartPosX, Mathf.Pow((timer / time - 1), 2));
        title.transform.localPosition = new Vector3(titleNewPosX, title.transform.localPosition.y, title.transform.localPosition.z);

        if (backgroundStartTime <= timer && backgroundTimer <= backgroundTime)
        {
            backgroundTimer += Time.deltaTime;
            newAlpha = backgroundTimer / backgroundTime;
            backgroundImage.color = new Color(0f, 0f, 0f, newAlpha);
        }

        if (timer >= time)
        {
            SceneManager.LoadScene(0);
        }
    }
}
