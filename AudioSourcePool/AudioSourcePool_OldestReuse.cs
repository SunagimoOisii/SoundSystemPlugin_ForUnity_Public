using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// SE向けにAudioSourceをプールで管理するクラス<para></para>
/// - 未使用のAudioSourceがあればそれを返す<para></para>
/// - 全て使用中で最大サイズなら最古のものを再利用<para></para>
/// - 最大サイズ未満なら新規作成
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
        Log.Safe("Retrieve実行");

        //未使用のAudioSourceがあれば、それを返す
        foreach (var source in pool)
        {
            if (source.isPlaying == false)
            {
                return source;
            }
        }

        //プールが最大サイズの場合、最古のものを再利用
        if (pool.Count >= maxSize)
        {
            var oldest = pool.Dequeue();
            pool.Enqueue(oldest);
            return oldest;
        }
        else //最大サイズ未満なら新規作成
        {
            var created = CreateSourceWithOwnerGameObject();
            pool.Enqueue(created);
            return created;
        }
    }
}
