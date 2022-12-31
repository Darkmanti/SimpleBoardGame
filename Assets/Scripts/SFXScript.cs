using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXScript : MonoBehaviour
{
    [SerializeField] List<AudioClip> steps = new List<AudioClip>();

    private AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        source = this.GetComponentInParent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayOneOfStepSound()
    {
        int count = steps.Count;

        int number = Random.Range(0, count);

        source.PlayOneShot(steps[number]);
    }
}
