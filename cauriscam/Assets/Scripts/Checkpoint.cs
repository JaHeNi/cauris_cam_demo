using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private bool active;
    public AudioClip activationAudio;
    private AudioSource checkpointAudioSource;

    void Activate()
    {
        active = true;
        checkpointAudioSource.Play();
    }

    public bool isActive()
    {
        return active;
    }

    // Start is called before the first frame update
    void Start()
    {
        checkpointAudioSource = gameObject.AddComponent<AudioSource>();
        checkpointAudioSource.clip = activationAudio;
        checkpointAudioSource.loop = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        Activate();
    }
}
