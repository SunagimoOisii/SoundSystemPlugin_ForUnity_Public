using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// SE������AudioSource���v�[���ŊǗ�����N���X<para></para>
/// - ���g�p��AudioSource������΂����Ԃ�<para></para>
/// - �S�Ďg�p���ōő�T�C�Y�Ȃ�ŌÂ̂��̂��ė��p<para></para>
/// - �S�Ďg�p���ōő�T�C�Y�����Ȃ�V�K�쐬�������̂�Ԃ�
/// </summary>
internal sealed class AudioSourcePool_FIFO : AudioSourcePool_Base
{
    public AudioSourcePool_FIFO(AudioMixerGroup mixerG, int initSize,
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