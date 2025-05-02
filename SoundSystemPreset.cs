using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SoundSystemPreset", menuName = "SoundSystem/SoundSystemPreset", order = 0)]
public class SoundSystemPreset : ScriptableObject
{
    [System.Serializable]
    public class BGMSetting
    {
        public string name = null;
        public float volume            = 0.5f;
        public float fadeInDuration    = 1.0f;
        public float fadeOutDuration   = 1.0f;
        public float crossFadeDuration = 1.0f;
    }

    [System.Serializable]
    public class SESetting
    {
        public string name = null;
        public float volume       = 0.5f;
        public float pitch        = 1.0f;
        public float spatialBlend = 1.0f; //0 = 2D, 1 = 3D
        public Vector3 position   = Vector3.zero;
    }

    [Header("BGM設定リスト")]
    public List<BGMSetting> bgmSettings = new();

    [Header("SE設定リスト")]
    public List<SESetting> seSettings = new();

    [Header("SoundCache設定")]
    public SoundCacheFactory.SoundCacheType cacheType = SoundCacheFactory.SoundCacheType.LRU;
    public float param = 30f; //idleTimeThresholdやttlSecondsに使う

#if UNITY_EDITOR
    private void OnValidate()
    {
        foreach (var bgm in bgmSettings)
        {
            if (string.IsNullOrEmpty(bgm.name))
            {
                Debug.LogWarning("BGMSettingに名前が設定されていない項目あり");
            }
        }
        foreach (var se in seSettings)
        {
            if (string.IsNullOrEmpty(se.name))
            {
                Debug.LogWarning("SESettingに名前が設定されていない項目あり");
            }
        }
    }
#endif
}