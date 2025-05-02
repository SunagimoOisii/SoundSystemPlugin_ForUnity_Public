using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// SE������AudioSource���v�[���ŊǗ�����N���X<para></para>
/// - ���g�p��AudioSource������΂����Ԃ�<para></para>
/// - �S�Ďg�p���ōő�T�C�Y�Ȃ�ŌÂ̂��̂��ė��p<para></para>
/// - �ő�T�C�Y�����Ȃ�V�K�쐬
/// </summary>
internal sealed class AudioSourcePool_OldestReuse : AudioSourcePool_Base
{
    public AudioSourcePool_OldestReuse(AudioMixerGroup mixerG, int initSize,
        int maxSize)
        : base(mixerG, initSize, maxSize)
    {
    }

    public override AudioSource Retrieve()
    {
        Log.Safe("Retrieve���s");

        //���g�p��AudioSource������΁A�����Ԃ�
        foreach (var source in pool)
        {
            if (source.isPlaying == false)
            {
                return source;
            }
        }

        //�v�[�����ő�T�C�Y�̏ꍇ�A�ŌÂ̂��̂��ė��p
        if (pool.Count >= maxSize)
        {
            var oldest = pool.Dequeue();
            pool.Enqueue(oldest);
            return oldest;
        }
        else //�ő�T�C�Y�����Ȃ�V�K�쐬
        {
            var created = CreateSourceWithOwnerGameObject();
            pool.Enqueue(created);
            return created;
        }
    }
}