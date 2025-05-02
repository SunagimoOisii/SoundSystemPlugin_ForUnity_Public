using Cysharp.Threading.Tasks;
using UnityEngine;

public interface ISoundLoader
{
    UniTask<(bool success, AudioClip clip)> TryLoadClip(string resourceAddress);

    void UnloadClip(string resourceAddress);
}