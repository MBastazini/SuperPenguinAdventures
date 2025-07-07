using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager _instance;

    [SerializeField] private AudioSource audioSourcePrefab;

    private AudioSource thisObjectAudioSource;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            //DontDestroyOnLoad(gameObject); // Keep this instance across scenes
        }
        else
        {
            Destroy(gameObject); // Ensure only one instance exists
        }

        thisObjectAudioSource = GetComponent<AudioSource>();
        if (thisObjectAudioSource == null)
        {
            Debug.LogError("AudioSource component is missing on SoundFXManager.");
        }
    }
    public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        //print("Playing sound effect: " + audioClip.name);
        //Spawn audioSource object
        //if(audioSourcePrefab == null)
        //{
        //    Debug.LogError("AudioSource prefab is not assigned in SoundFXManager.");
        //    return;
        //}
        //AudioSource audioSource = Instantiate(audioSourcePrefab, spawnTransform.position, Quaternion.identity);
        //audioSource.clip = audioClip;
        //audioSource.volume = volume;
        //audioSource.Play(); 

        //float clipLength = audioClip.length;

        //Destroy(audioClip, clipLength); // Destroy the audio source after the clip has finished playing

        thisObjectAudioSource.PlayOneShot(audioClip, volume);

    }
}
