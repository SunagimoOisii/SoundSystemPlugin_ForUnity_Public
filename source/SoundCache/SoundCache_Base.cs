using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

/// <summary>
/// �T�E���h���\�[�X�̃L���b�V���Ǘ���S���N���X�̊��N���X<para></para>
/// - �h���N���X�̍폜���j���Ƃ�Evict�֐����I�[�o�[���C�h������
/// </summary>
internal abstract class SoundCache_Base : ISoundCache
{
    protected readonly Dictionary<string, AudioClip> cache = new();

    /// <summary>
    /// �w�胊�\�[�X���L���b�V������擾����<para></para>
    /// �擾���ɍŏI�A�N�Z�X���Ԃ��X�V����
    /// </summary>
    public virtual AudioClip Retrieve(string resourceAddress)
    {
        if (cache.TryGetValue(resourceAddress, out var clip))
        {
            return clip;
        }
        return null;
    }

    /// <summary>
    /// �w�胊�\�[�X���L���b�V���ɒǉ�����<para></para>
    /// </summary>
    public virtual void Add(string resourceAddress, AudioClip clip)
    {
        cache[resourceAddress] = clip;
    }

    public virtual void Remove(string resourceAddress)
    {
        if (cache.TryGetValue(resourceAddress, out var clip))
        {
            Log.Safe($"Remove���s:{resourceAddress}");
            Addressables.Release(clip);
            cache.Remove(resourceAddress);
        }
    }

    /// <summary>
    /// �L���b�V������AudioSource��S�Ĕj������
    /// </summary>
    public virtual void ClearAll()
    {
        Log.Safe("ClearAll���s");
        foreach (var clip in cache.Values)
        {
            Addressables.Release(clip);
        }
        cache.Clear();
    }

    public abstract void Evict();
}