using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyAudioManager : MonoBehaviour
{
    public AudioClip[] punchSounds;
    public AudioClip[] hitSounds;
    public AudioClip[] chaseSounds;
    public AudioClip[] deathSounds;

    private AudioSource audioSource;
    private EnemyStateMachine enemyStateMachine;

    private void Awake()
    {
        // Initialize and configure AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();
        enemyStateMachine = GetComponent<EnemyStateMachine>();
    }

    // Methods to play random sound from each category, called from the state machine

    public void PlayRandomPunchSound()
    {
        PlayRandomSound(punchSounds);
    }

    public void PlayRandomHitSound()
    {
        PlayRandomSound(hitSounds);
    }

    public void PlayRandomChaseSound()
    {
        PlayRandomSound(chaseSounds);
    }

    public void PlayRandomDeathSound()
    {
        // Stop any currently playing audio and give priority to the death sound
        audioSource.Stop();
        PlayRandomSound(deathSounds, isPriority: true);
    }

    // General method to play a random clip from the given array
    private void PlayRandomSound(AudioClip[] clips, bool isPriority = false)
    {
        if (clips.Length == 0 || (!isPriority && audioSource.isPlaying)) return;
        
        audioSource.clip = clips[Random.Range(0, clips.Length)];
        audioSource.Play();
    }
}
