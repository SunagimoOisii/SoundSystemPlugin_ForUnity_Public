using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// SE������AudioSource���v�[���ŊǗ�����N���X<para></para>
/// - ���g�p��AudioSource������΂����Ԃ�<para></para>
/// - �S�Ďg�p���ōő�T�C�Y�����Ȃ�V�K�쐬�������̂�Ԃ�<para></para>
/// - �S�Ďg�p���ōő�T�C�Y�Ȃ�null��Ԃ�
/// </summary>
internal sealed class AudioSourcePool_Strict : AudioSourcePool_Base
{
    public AudioSourcePool_Strict(AudioMixerGroup mixerG, int initSize,
        int maxSize)
        : base(mixerG, initSize, maxSize)
    {
    }

    public override AudioSource Retrieve()
    {
        Log.Safe("Retrieve���s");

        //���g�p��AudioSource������΁A�����Ԃ�
        for (int i = 0; i < pool.Count; i++)
        {
            var source = pool.Dequeue();
            if (source.isPlaying == false)
            {
                return source;
            }

            pool.Enqueue(source);
        }

        //�v�[�����ő�T�C�Y�����Ȃ�V�K�쐬�������̂�Ԃ�
        if (pool.Count < maxSize)
        {
            var created = CreateSourceWithOwnerGameObject();
            pool.Enqueue(created);
            return created;
        }

        //�ő�T�C�Y�őS�Ďg�p���Ȃ�null
        return null;
    }
}