using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// SoundPresetProperty�Ŏg�p�����BGM�v���Z�b�g�Q��ێ�,���삷��N���X<para/>
/// - �C���X�y�N�^�[�ł͕ҏW�\��List�Ƃ��ĊǗ������<para/>
/// - ���s���ɂ́ApresetName���L�[�Ƃ���Dictionary�֕ϊ����A�����ȎQ�Ƃ��\<para/>
/// - Dictionary�ւ̕ϊ���ISerializationCallbackReceiver.OnAfterDeserialize()���ōs��
/// </summary>
[System.Serializable]
public class SerializedBGMSettingDictionary : ISerializationCallbackReceiver
{
	[SerializeField]
	private List<SoundPresetProperty.BGMPreset> presetList = new();

	private Dictionary<string, SoundPresetProperty.BGMPreset> presetDict = new();

	public bool TryGetValue(string key, out SoundPresetProperty.BGMPreset value)
	{
		return presetDict.TryGetValue(key, out value);
	}

	void ISerializationCallbackReceiver.OnAfterDeserialize()
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
				Debug.LogWarning($"�L�[�̏d��:presetName = {preset.presetName}");
				continue;
			}

			presetDict.Add(preset.presetName, preset);
		}
	}

	//�����i�V
	void ISerializationCallbackReceiver.OnBeforeSerialize() { }
}