﻿using UnityEngine;
using Cysharp.Threading.Tasks;

/// <summary>
/// SoundSystemが操作するクラスの1つ<para></para>
/// - AudioSourcePoolを用いたSE再生<para></para>
/// </summary>
internal sealed class SEManager
{
    private readonly IAudioSourcePool sourcePool;
    private readonly ISoundLoader loader;

    public SEManager(IAudioSourcePool sourcePool, ISoundLoader loader)
    {
        this.sourcePool = sourcePool;
        this.loader     = loader;
    }

    /// <param name="volume">音量(0〜1)</param>
    /// <param name="pitch">ピッチ(0〜1)</param>
    /// <param name="spatialBlend">サラウンド度(0〜1)</param>
    /// <param name="position">再生座標</param>
    public async UniTask Play(string resourceAddress,
        float volume, float pitch, float spatialBlend, Vector3 position)
    {
        //サウンドリソースのロード
        var (success, clip) = await loader.TryLoadClip(resourceAddress);
        if (success == false)
        {
            Log.Error($"Play失敗:リソース読込に失敗,{resourceAddress}");
            return;
        }

        //指定の音声設定で再生
        var source = sourcePool.Retrieve();
        source.pitch              = pitch;
        source.volume             = volume;
        source.spatialBlend       = spatialBlend;
        source.transform.position = position;
        source.PlayOneShot(clip);
        Log.Safe($"Play成功:{resourceAddress},vol = {volume},pitch = {pitch}," +
            $"blend = {spatialBlend}");
    }

    public void StopAll()
    {
        Log.Safe("StopAll実行");
        var sources = sourcePool.GetAllResources();
        foreach (var source in sources)
        {
            if (source == null) continue;
            source.Stop();
        }
    }

    public void ResumeAll()
    {
        Log.Safe("ResumeAll実行");
        var sources = sourcePool.GetAllResources();
        foreach (var source in sources)
        {
            if (source == null) continue;
            source.UnPause();
        }
    }

    public void PauseAll()
    {
        Log.Safe("PauseAll実行");
        var sources = sourcePool.GetAllResources();
        foreach (var source in sources)
        {
            if (source == null) continue;
            source.Pause();
        }
    }
}