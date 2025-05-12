using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

/// <summary>
/// �T�E���h���\�[�X�̃��[�h,�A�����[�h��S���N���X<para></para>
/// - Addressable�����AudioClip��񓯊��Ƀ��[�h<para></para>
/// - ���[�h���ʂ��L���b�V���Ǘ��N���X(ISoundCache)�ɈϏ�
/// </summary>
public class SoundLoader : ISoundLoader
{
    private readonly ISoundCache cache;

    public SoundLoader(ISoundCache cache)
    {
        this.cache = cache;
    }

    public async UniTask<(bool success, AudioClip clip)> TryLoadClip(string resourceAddress)
    {
        Log.Safe($"TryLoadClip���s:{resourceAddress}");
        var handle = Addressables.LoadAssetAsync<AudioClip>(resourceAddress);
        var clip   = await handle.Task;

        if (clip != null &&
            handle.Status == AsyncOperationStatus.Succeeded)
        {
            cache.Add(resourceAddress, clip);
            Log.Safe($"TryLoadClip����:{resourceAddress}");
            return (success: true, clip);
        }
        else
        {
            Log.Error($"TryLoadClip���s:{resourceAddress},Status = {handle.Status}");
            cache.Remove(resourceAddress);
            Addressables.Release(handle);
            return (success: false, null);
        }
    }

    public void UnloadClip(string resourceAddress)
    {
        Log.Safe($"UnloadClip���s:{resourceAddress}");
        cache.Remove(resourceAddress);
    }
}