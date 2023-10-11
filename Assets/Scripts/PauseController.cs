using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseController : MonoBehaviour
{
    public GameObject pauseButton, resumeButton, background;
    float newAlpha;
    Image backgroundImage;

    // Start is called before the first frame update
    void Start()
    {
        backgroundImage = background.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Pause()
    {
        Time.timeScale = 0f;
        pauseButton.SetActive(false);
        resumeButton.SetActive(true);

        newAlpha = 0.5f;
        backgroundImage.color = new Color(0f, 0f, 0f, newAlpha);
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        pauseButton.SetActive(true);
        resumeButton.SetActive(false);

        newAlpha = 0f;
        backgroundImage.color = new Color(0f, 0f, 0f, newAlpha);
    }
}
