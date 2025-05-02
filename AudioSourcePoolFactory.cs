using UnityEngine.Audio;

/// <summary>
/// IAudioSourcePool�C���X�^���X�𐶐�����t�@�N�g���[�N���X
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