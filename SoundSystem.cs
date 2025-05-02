using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// サウンド管理のエントリーポイントを提供するクラス<para></para>
/// - 各マネージャと機能インターフェースを外部から受け取り統一的に管理<para></para>
/// - 本モジュールに同梱のAudioMixerオブジェクトの使用を前提としている
/// </summary>
public sealed class SoundSystem
{
    private readonly BGMManager bgm;
    private readonly SEManager  se;
    private readonly ListenerEffector effector;

    private readonly AudioMixer mixer;

    private List<SoundSystemPreset.BGMSetting> bgmPresets;
    private List<SoundSystemPreset.SESetting>  sePresets;

    public SoundSystem(ISoundCache cache, IAudioSourcePool sourcePool, AudioMixer mixer,
        AudioMixerGroup bgmGroup, bool canLogging = true)
    {
        if (canLogging)
        {
            Log.Initialize();
            Application.quitting += () => Log.Close();
        }

        var loader = new SoundLoader(cache);
        bgm        = new(bgmGroup, loader);
        se         = new(sourcePool, loader);
        effector   = new();
        this.mixer = mixer;
    }

    public static SoundSystem CreateFromPreset(SoundSystemPreset preset, IAudioSourcePool sourcePool,
        AudioMixer mixer, AudioMixerGroup bgmGroup)
    {
        var cache = SoundCacheFactory.Create(
            preset.cacheType,
            preset.param
        );

        var ss = new SoundSystem(cache, sourcePool, mixer, bgmGroup);
        ss.SetPresets(preset.bgmSettings, preset.seSettings);
        return ss;
    }

    //CreateFromPreset関数のために実装した
    private void SetPresets(List<SoundSystemPreset.BGMSetting> bgmList,
        List<SoundSystemPreset.SESetting> seList)
    {
        bgmPresets = bgmList;
        sePresets  = seList;
    }

    public float? RetrieveMixerParameter(string exposedParamName)
    {
        if (mixer.GetFloat(exposedParamName, out float value))
        {
            return value;
        }
        else
        {
            Debug.LogWarning($"SoundSystem: パラメータ '{exposedParamName}' の取得に失敗");
            return null;
        }
    }

    public void RetrieveMixerParameter(string exposedParamName, float value)
    {
        if (mixer.SetFloat(exposedParamName, value) == false)
        {
            Debug.LogWarning($"SoundSystem: パラメータ '{exposedParamName}' の設定に失敗");
        }
    }

    private bool TryRetrieveBGMPreset(string presetName, out SoundSystemPreset.BGMSetting preset)
    {
        preset = bgmPresets?.Find(b => b.name == presetName);
        if (preset == null)
        {
            Debug.LogWarning($"SoundSystem: presetName '{presetName}' に対応するBGMプリセットが存在しない");
            return false;
        }
        else return true;
    }

    private bool TryRetrieveSEPreset(string presetName, out SoundSystemPreset.SESetting preset)
    {
        preset = sePresets?.Find(s => s.name == presetName);
        if (preset == null)
        {
            Debug.LogWarning($"SoundSystem: presetName '{presetName}' に対応するSEプリセットが存在しない");
            return false;
        }
        else return true;
    }

    public void MuteAllSound()
    {
        bgm.Stop();
        se.StopAll();
    }

    #region BGM
    public async UniTask PlayBGM(string resourceAddress, float volume = 0.5f)
    {
        await bgm.Play(resourceAddress, volume);
    }

    public async UniTask PlayBGMWithPreset(string resourceAddress, string presetName)
    {
        if (TryRetrieveBGMPreset(presetName, out SoundSystemPreset.BGMSetting preset))
        {
            await FadeInBGM(resourceAddress, preset.fadeInDuration, preset.volume);
        }
    }

    public void StopBGM() 
    { 
        bgm.Stop(); 
    }

    public void ResumeBGM()
    {
        bgm.Resume();
    }

    public void PauseBGM()
    {
        bgm.Pause();
    }

    public async UniTask FadeInBGM(string resourceAddress, float duration,
        float volume = 1.0f)
    {
        await bgm.FadeIn(resourceAddress, duration, volume);
    }

    public async UniTask FadeInBGMWithPreset(string resourceAddress, string presetName)
    {
        if (TryRetrieveBGMPreset(presetName, out SoundSystemPreset.BGMSetting preset))
        {
            await bgm.FadeIn(resourceAddress, preset.fadeInDuration, preset.volume);
        }
    }

    public async UniTask FadeOutBGM(float duration)
    {
        await bgm.FadeOut(duration);
    }

    public async UniTask FadeOutBGMWithPreset(string presetName)
    {
        if (TryRetrieveBGMPreset(presetName, out SoundSystemPreset.BGMSetting preset))
        {
            await bgm.FadeOut(preset.fadeOutDuration);
        }
    }

    public async UniTask CrossFadeBGM(string resourceAddress, float duration)
    {
        await bgm.CrossFade(resourceAddress, duration);
    }

    public async UniTask CrossFadeBGMWithPreset(string resourceAddress, string presetName)
    {
        if (TryRetrieveBGMPreset(presetName, out SoundSystemPreset.BGMSetting preset))
        {
            await bgm.CrossFade(resourceAddress, preset.crossFadeDuration);
        }
    }
    #endregion

    #region SE
    public async UniTask PlaySE(string resourceAddress, Vector3 position = default,
        float volume = 0.5f, float pitch = 1.0f, float spatialBlend = 1.0f)
    {
        await se.Play(resourceAddress, volume, pitch, spatialBlend, position);
    }

    public async UniTask PlaySEWithPreset(string resourceAddress, string presetName)
    {
        if (TryRetrieveSEPreset(presetName, out SoundSystemPreset.SESetting preset))
        {
            await se.Play(resourceAddress,
                preset.volume, preset.pitch, preset.spatialBlend, preset.position);
        }
    }

    public void StopAllSE()
    {
        se.StopAll();
    }

    public void ResumeAllSE()
    {
        se.ResumeAll();
    }

    public void PauseAllSE()
    {
        se.PauseAll();
    }
    #endregion

    #region ListenerEffector
    public void ApplyEffectFilter<T>(Action<T> configure) where T : Behaviour
    {
        effector.ApplyFilter(configure);
    }

    public void DisableEffecteFilter<T>() where T : Behaviour
    {
        effector.DisableFilter<T>();
    }

    public void DisableAllEffectFilter()
    {
        effector.DisableAllFilters();
    }
    #endregion
}