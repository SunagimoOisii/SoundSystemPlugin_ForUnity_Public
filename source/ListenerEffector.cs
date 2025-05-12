using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// SoundSystem�����삷��N���X�̂P��<para></para>
/// AudioListener�ɃG�t�F�N�g�t�B���^�[�𓮓I�ɒǉ���������s��
/// (�G�t�F�N�g�t�B���^�[�Ƃ�AudioReverbFilter��AudioEchoFilter�ȂǂŁA
/// �{�N���X�ł�Behaviour�N���X�����^�Ƃ��ē���I�Ɉ���)
/// </summary>
internal sealed class ListenerEffector
{
    public AudioListener Listener { private get; set; }

    private readonly Dictionary<Type, Component> filterDict = new();

    public ListenerEffector()
    {
        Listener = UnityEngine.Object.FindObjectOfType<AudioListener>();
        if (Listener == null)
        {
            Log.Warn("AudioListener���V�[�����Ō�����Ȃ�");
        }
    }

    /// <typeparam name="FilterT">�K�p����t�B���^�[�̌^</typeparam>
    /// <param name="configure">�t�B���^�[�̐ݒ���s���A�N�V����</param>
    /// <remarks>�g�p��: effector.ApplyFilter<AudioReverbFilter>(filter => filter.reverbLevel = Mathf.Clamp(reverbLevel, -10000f, 2000f));</remarks>
    public void ApplyFilter<FilterT>(Action<FilterT> configure) where FilterT : Behaviour
    {
        Log.Safe($"ApplyFilter���s:{typeof(FilterT).Name}");
        if (filterDict.TryGetValue(typeof(FilterT), out var component) == false)
        {
            component = Listener.gameObject.AddComponent<FilterT>();
            filterDict[typeof(FilterT)] = component;
        }

        var filter = component as FilterT;
        filter.enabled = true;
        configure?.Invoke(filter);
    }

    public void DisableFilter<FilterT>() where FilterT : Behaviour
    {
        Log.Safe($"DisableFilter���s:{typeof(FilterT).Name}");
        if (filterDict.TryGetValue(typeof(FilterT), out var component))
        {
            var filter = component as FilterT;
            filter.enabled = false;
        }
    }

    public void DisableAllFilters()
    {
        foreach (var filter in filterDict.Values)
        {
            if (filter is Behaviour b) b.enabled = false;
        }
    }
}