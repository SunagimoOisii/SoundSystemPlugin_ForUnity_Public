using System;

/// <summary>
/// ISoundCache�C���X�^���X�𐶐�����t�@�N�g���[�N���X
/// </summary>
public static class SoundCacheFactory
{
    public enum SoundCacheType
    {
        LRU,
        TTL,
        Random
    }

    /// <summary>
    /// �w��L���b�V�������ɉ�����ISoundCache�C���X�^���X�𐶐�����
    /// </summary>
    /// <param name="type">�L���b�V������</param>
    /// <param name="param">�����ɉ������p�����[�^(�b���܂��͍ő吔)</param>
    public static ISoundCache Create(SoundCacheType type, float param)
    {
        return type switch
        {
            SoundCacheType.LRU    => new SoundCache_LRU(idleTimeThreshold: param),
            SoundCacheType.TTL    => new SoundCache_TTL(ttlSeconds: param),
            SoundCacheType.Random => new SoundCache_Random(maxCacheCount: (int)param),
            _ => throw new ArgumentOutOfRangeException(nameof(type), $"���Ή���SoundCacheType: {type}")
        };
    }
}