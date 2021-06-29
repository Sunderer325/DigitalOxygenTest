using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioPrefab : MonoBehaviour
{
    public AudioClip[] clips;

    AudioSource source;

    void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    public void Play(string name)
    {
        foreach (AudioClip clip in clips)
        {
            if (clip.name == name)
            {
                source.PlayOneShot(clip);
                return;
            }
        }
        Debug.LogError("there is no sound with the name: \"" + name + "\": " + gameObject.name);
    }

    public void Play(string name, float volume)
    {
        source.volume = volume;
        Play(name);
    }

    public void PlayLoop(string name)
    {
        foreach (AudioClip clip in clips)
        {
            if (clip.name == name)
            {
                if (source.clip != clip)
                    source.clip = clip;
                //source.volume = 0.3f;
                source.loop = true;
                source.Play();
                return;
            }
        }
        Debug.LogError("there is no sound with the name: \"" + name + "\": " + gameObject.name);
    }

    public void Stop()
    {
        source.Stop();

        source.loop = false;
    }

    public void Pause()
    {
        source.Pause();
    }

    public void Unpause()
    {
        source.UnPause();
    }
}
