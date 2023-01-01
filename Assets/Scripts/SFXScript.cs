using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXScript : MonoBehaviour
{
    [SerializeField] List<AudioClip> steps = new List<AudioClip>();

    private AudioSource source;

    void Start()
    {
        source = this.GetComponentInParent<AudioSource>();
    }


    public void PlayOneOfStepSound()
    {
        int count = steps.Count;

        int number = Random.Range(0, count);

        source.PlayOneShot(steps[number]);
    }
}
