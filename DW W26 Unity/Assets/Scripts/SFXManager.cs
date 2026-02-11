using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager instance;


    private void Awake()
    {
        if (instance == null ) instance = this; 
    }

    public void playSFX(AudioClip audio, Transform transform, float volume)
    {
        AudioSource source = Instantiate()




    }






}
