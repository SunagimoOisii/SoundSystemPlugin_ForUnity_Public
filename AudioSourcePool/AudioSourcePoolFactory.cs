using UnityEngine.Audio;

/// <summary>
/// IAudioSourcePoolインスタンスを生成するファクトリークラス
/// </summary>
public static class AudioSourcePoolFactory
{
    public static IAudioSourcePool CreateOldestReuse(AudioMixerGroup mixerGroup,
        int initSize, int maxSize)
    {
        return new AudioSourcePool_OldestReuse(
            mixerGroup, initSize, maxSize);
    }
}
