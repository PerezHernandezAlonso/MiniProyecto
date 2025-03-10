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
        // Aqu� podr�as integrar AudioSource o un AudioManager real
    }
}
