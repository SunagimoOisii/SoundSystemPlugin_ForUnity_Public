using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// SE������AudioSource���v�[���ŊǗ�����N���X�̊��N���X<para></para>
/// - �h���N���X�̕��j���Ƃ�Retrieve�֐����I�[�o�[���C�h������
/// </summary>
internal abstract class AudioSourcePool_Base : IAudioSourcePool
{
    protected readonly GameObject sourceRoot;
    protected readonly AudioMixerGroup mixerGroup;

    protected Queue<AudioSource> pool;
    protected readonly int maxSize;
    protected readonly int initSize;
    public IEnumerable<AudioSource> GetAllResources() => pool;

    public AudioSourcePool_Base(AudioMixerGroup mixerG, int initSize, int maxSize)
    {
        pool         = new();
        sourceRoot   = new("SE_AudioSources");
        this.maxSize = maxSize;
        this.initSize = initSize;
        this.mixerGroup = mixerG;

        //�v�[��������
        for (int i = 0; i < initSize; i++)
        {
            var source = CreateSourceWithOwnerGameObject();
            pool.Enqueue(source);
        }
    }

    public void Reinitialize()
    {
        Log.Safe("Reinitialize���s");

        //�v�[�����̗v�f��S�Ė��g�p�ɂ���
        foreach (var source in pool)
        {
            source.Stop();
            source.clip = null;
        }

        //�v�[���T�C�Y�����������̒l�ɖ߂�
        while (pool.Count > initSize) //���ߎ�
        {
            var source = pool.Dequeue();
            Object.Destroy(source.gameObject);
        }
        while (pool.Count < initSize) //�s����
        {
            pool.Enqueue(CreateSourceWithOwnerGameObject());
        }
    }

    public abstract AudioSource Retrieve();

    protected AudioSource CreateSourceWithOwnerGameObject()
    {
        var obj = new GameObject("SESource");
        obj.transform.parent = sourceRoot.transform;

        var source = obj.AddComponent<AudioSource>();
        source.playOnAwake = false;
        source.outputAudioMixerGroup = mixerGroup;
        return source;
    }
}