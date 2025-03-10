using UnityEngine;

public interface IAudioService
{
    void PlaySound(string clipName);
}

public class AudioService : IAudioService
{
    public void PlaySound(string clipName)
    {
        Debug.Log($"Playing sound: {clipName}");
        // Aquí podrías integrar AudioSource o un AudioManager real
    }
}
