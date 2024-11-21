using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu( fileName = "AudioDataList", menuName = "Audio" )]
public class AudioDataList : ScriptableObject {
    public List<AudioData> audio;
}

[Serializable]
public class AudioData {
    public SoundEffect soundEffect;
    public AssetReferenceT<AudioClip> audioClipRef;
}