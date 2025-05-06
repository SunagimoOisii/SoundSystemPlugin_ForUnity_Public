using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// SoundPresetProperty�Ŏg�p�����SE�v���Z�b�g�Q��ێ�,���삷��N���X<para/>
/// - �C���X�y�N�^�[�ł͕ҏW�\��List�Ƃ��ĊǗ������<para/>
/// - ���s���ɂ́ApresetName���L�[�Ƃ���Dictionary�֕ϊ����A�����ȎQ�Ƃ��\<para/>
/// - Dictionary�ւ̕ϊ���ISerializationCallbackReceiver.OnAfterDeserialize()���ōs��
/// </summary>
[System.Serializable]
public class SerializedSESettingDictionary : ISerializationCallbackReceiver
{
    [SerializeField]
    private List<SoundPresetProperty.SEPreset> presetList = new();

    private Dictionary<string, SoundPresetProperty.SEPreset> presetDict = new();

    public bool TryGetValue(string key, out SoundPresetProperty.SEPreset value)
    {
        return presetDict.TryGetValue(key, out value);
    }

    public void OnAfterDeserialize()
    {
        presetDict.Clear();
        foreach (var preset in presetList)
        {
            if (string.IsNullOrEmpty(preset.presetName))
            {
                continue;
            }

            if (presetDict.ContainsKey(preset.presetName))
            {
                Debug.LogWarning($"�L�[�̏d��:key = {preset.presetName}");
                continue;
            }

            presetDict.Add(preset.presetName, preset);
        }
    }

    //�����i�V
    public void OnBeforeSerialize() { }
}