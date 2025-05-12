using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �T�E���h���\�[�X�̃L���b�V���Ǘ���S���N���X<para></para>
/// - �o�^��������w�莞�Ԃ𒴂������\�[�X���폜�ΏۂƂ���
/// </summary>
internal sealed class SoundCache_TTL : SoundCache_Base
{
    private readonly float ttlSeconds;
    private readonly Dictionary<string, float> registerTime = new();

    public SoundCache_TTL(float ttlSeconds)
    {
        this.ttlSeconds = ttlSeconds;
    }

    public override void Add(string resourceAddress, AudioClip clip)
    {
        base.Add(resourceAddress, clip);
        registerTime[resourceAddress] = Time.time;
    }

    public override void Remove(string resourceAddress)
    {
        base.Remove(resourceAddress);
        registerTime.Remove(resourceAddress);
    }

    public override void ClearAll()
    {
        base.ClearAll();
        registerTime.Clear();
    }

    public override void Evict()
    {
        var currentTime = Time.time;
        var toRemove = new List<string>();

        Log.Safe($"Evict���s:{toRemove.Count}���폜,ttl = {ttlSeconds}");
        foreach (var entry in registerTime)
        {
            if (currentTime - entry.Value > ttlSeconds)
            {
                toRemove.Add(entry.Key);
            }
        }

        foreach (var key in toRemove)
        {
            Remove(key);
        }
    }
}