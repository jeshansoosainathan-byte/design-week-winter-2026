using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager instance;

    [SerializeField] private AudioSource soundFXObject;
    private void Awake()
    {
        if (instance == null) { instance = this; }
    }

    public void playSFX(AudioClip clip, Transform transform, float volume)
    {

        AudioSource audiouSource = Instantiate(soundFXObject, transform.position, Quaternion.identity);

        audiouSource.clip = clip;

        audiouSource.volume = volume;

        audiouSource.Play();

        float cliplength = audiouSource.clip.length;

        Destroy(audiouSource.gameObject, cliplength);

    }






}
