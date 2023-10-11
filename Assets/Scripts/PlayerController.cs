using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

enum Drag
{
    None,
    Touch,
    Left,
    Right,
    Top,
    Bottom,
}

enum Terrain
{
    Grass,
    Road,
    None,
}

public class PlayerController : MonoBehaviour
{
    public GameObject endController;
    Touch touch;
    Drag drag;
    Vector2 touchPos, dragDir, dragVector;
    float dragAngle;
    float dragThreshold = 100f;
    float startPosX, endPosX, newPosX;
    float startPosY, midPosY, endPosY, newPosY;
    float startPosZ, endPosZ, newPosZ;
    float startAngX, endAngX, newAngX;
    float startAngZ, endAngZ, newAngZ;
    float jumpTimer, jumpTime;
    float idleHeight, idlePosition, idleScale;
    float idleTimer, idleTime;
    bool isJumping = false;
    public GameObject ParticleGenerator;
    GameObject particleGenerator;
    public HashSet<float> destroyed;
    AudioSource audioSource;
    public AudioClip carDestroyAudio, jumpAudio, scoreAudio;

    // Start is called before the first frame update
    void Start()
    {
        jumpTime = 1f / 4f;
        idleTime = 1f;

        destroyed = new HashSet<float>();

        dragVector = new Vector2(Mathf.Cos(25 * Mathf.Deg2Rad), Mathf.Sin(25 * Mathf.Deg2Rad));
        
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    void animateIdle()
    {
        float idleHeight = getIdleHeight(getTerrain(transform.position.x));
        idlePosition = Mathf.Lerp(idleHeight, idleHeight - 0.15f, 1f - Mathf.Abs(idleTimer / idleTime - 0.5f) * 2f);
        idleScale = Mathf.Lerp(3f, 2.7f, 1f - Mathf.Abs(idleTimer / idleTime - 0.5f) * 2f);

        transform.position = new Vector3(transform.position.x, idlePosition, transform.position.z);
        transform.localScale = new Vector3(transform.localScale.x, idleScale, transform.localScale.z);
    }

    void animateJump()
    {
        newPosX = Mathf.Lerp(startPosX, endPosX, jumpTimer / jumpTime);
        if (jumpTimer / jumpTime < 0.5f)
            newPosY = Mathf.Lerp(startPosY, midPosY, (jumpTimer / jumpTime) * 2f);
        else
            newPosY = Mathf.Lerp(midPosY, endPosY, (jumpTimer / jumpTime) * 2f - 1f);

        newPosZ = Mathf.Lerp(startPosZ, endPosZ, jumpTimer / jumpTime);
        newAngX = Mathf.Lerp(startAngX, endAngX, jumpTimer / jumpTime);
        newAngZ = Mathf.Lerp(startAngZ, endAngZ, jumpTimer / jumpTime);

        transform.position = new Vector3(newPosX, newPosY, newPosZ);
        transform.rotation = Quaternion.Euler(new Vector3(newAngX, 0f, newAngZ));
    }

    void normalize()
    {
        transform.position = new Vector3(transform.position.x, getIdleHeight(getTerrain(transform.position.x)), transform.position.z);
        transform.localScale = new Vector3(transform.localScale.x, 3f, transform.localScale.z);
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));

        jumpTimer = 0f;
        idleTimer = 0f;
    }

    Drag getDrag()
    {
        touch = Input.GetTouch(0);

        switch (touch.phase)
        {
            case TouchPhase.Began:
                touchPos = touch.position;
                break;

            case TouchPhase.Moved:
                break;

            case TouchPhase.Ended:
                dragDir = touch.position - touchPos;

                if (dragDir.magnitude < dragThreshold) return Drag.Touch;

                dragAngle = Vector2.SignedAngle(dragVector, dragDir);

                if (dragAngle < -90) return Drag.Bottom;
                else if (dragAngle < 0) return Drag.Right;
                else if (dragAngle < 90) return Drag.Top;
                else return Drag.Left;
        }

        return Drag.None;
    }

    bool setPosAng(Drag drag)
    {
        startPosX = transform.position.x;
        startPosY = transform.position.y;
        startPosZ = transform.position.z;
        startAngX = transform.eulerAngles.x;
        startAngZ = transform.eulerAngles.z;

        switch (drag)
        {
            case Drag.Left:
                endPosX = startPosX;
                endPosZ = startPosZ + 5f;
                endAngX = startAngX + 90f;
                endAngZ = startAngZ;
                break;

            case Drag.Top:
            case Drag.Touch:
                endPosX = startPosX + 5f;
                endPosZ = startPosZ;
                endAngX = startAngX;
                endAngZ = startAngZ - 90f;
                break;

            case Drag.Right:
                endPosX = startPosX;
                endPosZ = startPosZ - 5f;
                endAngX = startAngX - 90f;
                endAngZ = startAngZ;
                break;

            case Drag.Bottom:
                endPosX = startPosX - 5f;
                endPosZ = startPosZ;
                endAngX = startAngX;
                endAngZ = startAngZ + 90f;
                break;
        }

        midPosY = startPosY + 3.0f;
        endPosY = getIdleHeight(getTerrain(endPosX));

        return !isDecoration(endPosX, endPosZ);
    }

    bool isDecoration(float x, float z)
    {
        GameObject[] decorations = GameObject.FindGameObjectsWithTag("Decoration");
        foreach (GameObject decoration in decorations)
        {
            if (decoration.transform.position.x == x && decoration.transform.position.z == z)
            {
                return true;
            }
        }
        return false;
    }

    Terrain getTerrain(float x)
    {
        GameObject[] grasses = GameObject.FindGameObjectsWithTag("Grass");
        foreach (GameObject grass in grasses)
        {
            if (grass.transform.position.x == x)
            {
                return Terrain.Grass;
            }
        }

        GameObject[] roads = GameObject.FindGameObjectsWithTag("Road");
        foreach (GameObject road in roads)
        {
            if (road.transform.position.x == x)
            {
                return Terrain.Road;
            }
        }

        return Terrain.None;
    }

    float getIdleHeight(Terrain terrain)
    {
        switch (terrain)
        {
            case Terrain.Grass:
                return 2.5f;
            case Terrain.Road:
                return 1.5f;
        }

        return 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isJumping)
        {
            jumpTimer += Time.deltaTime;
            if (jumpTimer > jumpTime) jumpTimer = jumpTime;

            animateJump();

            if (jumpTimer == jumpTime)
            {
                isJumping = false;
                normalize();
            }
        }
        else
        {
            idleTimer += Time.deltaTime;
            while (idleTimer > idleTime) idleTimer -= idleTime;

            animateIdle();

            drag = getDrag();
            if (drag != Drag.None)
            {
                isJumping = setPosAng(drag);
                normalize();

                if (isJumping) audioSource.PlayOneShot(jumpAudio);
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "SmallVehicle")
        {
            bool isException = transform.position.x > other.gameObject.transform.position.x && jumpTimer < jumpTime * 0.5f;
            if (transform.position.y > endPosY && !isException)
            {
                particleGenerator = Instantiate(ParticleGenerator) as GameObject;
                particleGenerator.transform.position = other.transform.position;

                if (!destroyed.Contains(other.gameObject.transform.position.x))
                {
                    ParticleSystem.MainModule main = particleGenerator.GetComponent<ParticleSystem>().main;
                    main.startColor = Color.yellow;

                    audioSource.PlayOneShot(scoreAudio);
                }
                else 
                {
                    audioSource.PlayOneShot(carDestroyAudio);
                }

                destroyed.Add(other.gameObject.transform.position.x);
                Destroy(other.gameObject);
            }
            else
            {
                endController.SetActive(true);
            }
        }
        else if (other.gameObject.tag == "LargeVehicle")
        {
            endController.SetActive(true);
        }
        else if (other.gameObject.tag == "Decoration")
        {
            endController.SetActive(true);
        }
        else
        {

        }
    }

}
