using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSourceSfx;
    [SerializeField] private AudioSource _audioSourceMusic;

    [SerializeField] private List<SfxData> _sfxDatas = new List<SfxData>();
    [SerializeField] private List<MusicData> _musicDatas = new List<MusicData>();

    public static AudioManager Instance;

    void Awake()
    {
        if (Instance != null)
            Destroy(Instance.gameObject);

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlayMusic(MusicType type)
    {
        var musicData = GetMusic(type);
        _audioSourceMusic.clip = musicData.Music;
        _audioSourceMusic.Play();
    }

    public void PlaySfx(SfxType type)
    {
        var sfxData = GetSfx(type);
        _audioSourceSfx.PlayOneShot(sfxData.Sfx);
    }

    private SfxData GetSfx(SfxType type)
    {
        var result = _sfxDatas.Find(sfxData => sfxData.Type == type);
        return result;
    }

    private MusicData GetMusic(MusicType type)
    {
        var result = _musicDatas.Find(musicData => musicData.Type == type);
        return result;
    }
}

[System.Serializable]
public class SfxData
{
    [field: SerializeField] public SfxType Type { get; private set; }
    [field: SerializeField] public AudioClip Sfx { get; private set; }
}

[System.Serializable]
public class MusicData
{
    [field: SerializeField] public MusicType Type { get; private set; }
    [field: SerializeField] public AudioClip Music { get; private set; }

}
