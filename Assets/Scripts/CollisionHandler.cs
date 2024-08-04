using UnityEngine.SceneManagement;
using UnityEngine;
using Unity.VisualScripting;

public class CollisionHandler : MonoBehaviour
{
    AudioSource audioData;
    [SerializeField] AudioClip winClip;
    [SerializeField] AudioClip loseClip;

    bool isTransitioning = false;

    private void Start() {
        audioData = GetComponent<AudioSource>();
    }
    
    void OnCollisionEnter(Collision other) 
    {
        if (isTransitioning) {return;}

        switch (other.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("This thing is Friendly");
                break;
            case "Finish":
                audioData.PlayOneShot(winClip);
                StopPlayerControls();
                Invoke("LoadNextLevel", 1f);
                break;
            case "Fuel":
                Debug.Log("Fueling up!");
                break;
            default:
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
