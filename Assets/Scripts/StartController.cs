using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartController : MonoBehaviour
{
    public GameObject score, bestScore, pauseButton, template, title, background;
    Touch touch;
    bool isTouched = false;
    float titleStartPosX, titleEndPosX, titleNewPosX;
    float backgroundTimer, backgroundTime, backgroundStartTime, newAlpha;
    Image backgroundImage;
    float timer, time;

    // Start is called before the first frame update
    void Start()
    {
        backgroundTimer = 0f;
        backgroundStartTime = 0f;
        backgroundTime = 1f / 2f;
        backgroundImage = background.GetComponent<Image>();

        timer = 0f;
        time = 1f;

        titleStartPosX = 0f;
        titleEndPosX = titleStartPosX + 1280f;

        template.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (backgroundStartTime <= timer && backgroundTimer <= backgroundTime)
        {
            backgroundTimer += Time.deltaTime;
            newAlpha = 1 - backgroundTimer / backgroundTime;
            backgroundImage.color = new Color(0f, 0f, 0f, newAlpha);
        }

        if (isTouched)
        {
            timer += Time.deltaTime;
            titleNewPosX = Mathf.Lerp(titleStartPosX, titleEndPosX, Mathf.Pow(timer / time, 2));
            title.transform.localPosition = new Vector3(titleNewPosX, title.transform.localPosition.y, title.transform.localPosition.z);

            if (timer >= time)
            {
                title.SetActive(false);
                gameObject.SetActive(false);

                score.SetActive(true);
                bestScore.SetActive(true);
                pauseButton.SetActive(true);
            }
        }
        else
        {
            touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began) isTouched = true;
        }

    }
}
