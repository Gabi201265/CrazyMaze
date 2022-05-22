using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour
{
    public AudioClip sound; //Gestion du son à jouer

    [RangeAttribute(0f,1f)] public float volume; //Gestion du volume
    [RangeAttribute(0.1f, 2.5f)] public float pitch; //Gestion de la vitesse
    private AudioSource source;

    private void Awake()
    {
        gameObject.AddComponent<AudioSource>(); //On ajoute à l'objet le component Audio Source
        source = GetComponent<AudioSource>(); //On affecte à source le component
        volume = 0.5f;
        pitch = 1f;

    }
    // Start is called before the first frame update
    void Start()
    {
        source.clip = sound;
        source.volume = volume;
        source.pitch = pitch;
    }

    // Update is called once per frame
    void Update()
    {
        source.clip = sound;
        source.volume = volume;
        source.pitch = pitch;
        if (Input.GetKeyDown("space"))
        {
            PlayAndPause();
        }
    }
    public void PlayAndPause()
    {
        if (!source.isPlaying)
        {
            source.Play();
        }
        else
        {
            source.Pause();
        }
    }
}
