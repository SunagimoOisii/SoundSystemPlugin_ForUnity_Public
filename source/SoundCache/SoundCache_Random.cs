using System;
using System.Collections.Generic;

/// <summary>
/// �T�E���h���\�[�X�̃L���b�V���Ǘ���S���N���X<para></para>
/// - �L���b�V�����̏���𒴂����ꍇ�A�����_���Ƀ��\�[�X��I�����č폜���s��
/// </summary>
internal sealed class SoundCache_Random : SoundCache_Base
{
    private readonly int maxCacheCount;
    private readonly Random random = new();

    public SoundCache_Random(int maxCacheCount)
    {
        this.maxCacheCount = maxCacheCount;
    }

    public override void Evict()
    {
        if (cache.Count <= maxCacheCount)
        {
            return;
        }

        int excessCount = cache.Count - maxCacheCount;
        var keys        = new List<string>(cache.Keys);

        Log.Safe($"Evict���s:{excessCount}���폜,max = {maxCacheCount}");
        for (int i = 0; i < excessCount; i++)
        {
            if (keys.Count == 0) break;

            int randomIndex  = random.Next(keys.Count);
            string randomKey = keys[randomIndex];

            Remove(randomKey);
            keys.RemoveAt(randomIndex);
        }
    }
}