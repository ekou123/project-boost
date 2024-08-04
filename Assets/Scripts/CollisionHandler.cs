using UnityEngine.SceneManagement;
using UnityEngine;
using Unity.VisualScripting;
using System;

public class CollisionHandler : MonoBehaviour
{
    AudioSource audioData;
    [SerializeField] AudioClip winClip;
    [SerializeField] AudioClip loseClip;

    [SerializeField] ParticleSystem winParticles;
    [SerializeField] ParticleSystem loseParticles;

    bool isTransitioning = false;
    bool collisionDisabled = false;

    private void Start() {
        audioData = GetComponent<AudioSource>();

    }

    private void Update() {
        RespondToDebugKeys();
    }

    private void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            collisionDisabled = !collisionDisabled; // Toggle collision

        }
    }

    void OnCollisionEnter(Collision other) 
    {
        if (isTransitioning || collisionDisabled) {return;}

        switch (other.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("This thing is Friendly");
                break;
            case "Finish":
                winParticles.Play();
                audioData.PlayOneShot(winClip);
                StopPlayerControls();
                Invoke("LoadNextLevel", 1f);
                break;
            case "Fuel":
                Debug.Log("Fueling up!");
                break;
            default:
                loseParticles.Play();
                StopPlayerControls();
                StartCrashSequence();
                break;
        }
    }

    void StartCrashSequence()
    {
        audioData.Stop();
        audioData.PlayOneShot(loseClip);
        Invoke("ReloadLevel", 1f);
    }

    void StopPlayerControls()
    {
        GetComponent<Movement>().enabled = false;
    }

    void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    void LoadNextLevel()
    {
        audioData.Stop();
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(currentSceneIndex + 1);


    }
}
