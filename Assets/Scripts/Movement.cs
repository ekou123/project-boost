using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float thrustStrength = 1f;
    [SerializeField] float rotationThrust = 1f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] ParticleSystem thrustParticles;
    Rigidbody rb;

    AudioSource audioData;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioData = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }

    void ProcessThrust()
    {
         if (Input.GetKey(KeyCode.Space))
         {
            rb.AddRelativeForce(0,thrustStrength,0);
         }

         if (Input.GetKeyDown(KeyCode.Space))
         {
            thrustParticles.Play();
            audioData.PlayOneShot(mainEngine);
         }

         if (Input.GetKeyUp(KeyCode.Space))
         {
            thrustParticles.Stop();
            audioData.Stop();
         }
    }

    void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.A))
        {
            ApplyRotation(rotationThrust);
        }
        else if (Input.GetKey(KeyCode.D))
         {  
            ApplyRotation(-rotationThrust);
         }
    }

    void ApplyRotation(float rotationThisFrame)
    {
        rb.freezeRotation = true; // Freezing rotation so we can manually rotate
        transform.Rotate(0, 0, rotationThrust * rotationThisFrame * Time.deltaTime);
        rb.freezeRotation = false; // Unfreezing rotation so physics sys tem can take over
    }
}
