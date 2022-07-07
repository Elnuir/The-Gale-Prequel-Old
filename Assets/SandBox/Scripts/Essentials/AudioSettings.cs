using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class AudioSettings : MonoBehaviour
{
    private AudioManager _audio;

    public const string MusicVolumeKey = "music-volume";
    public const string EffectsVolumeKey = "effects-volume";
    public const string VoiceVolumeKey = "voice-volume";
    public const string LanguageKey = "language";

    public UnityEvent LanguageChanged;
    public UnityEvent DifficultyChanged;

    public string Language
    {
        get => PlayerPrefs.GetString(LanguageKey);
        set
        {
            PlayerPrefs.SetString(LanguageKey, value);
            LanguageChanged?.Invoke();
        }
    }

    public float MusicVolume
    {
        get => PlayerPrefs.GetFloat(MusicVolumeKey);
        set
        {
            PlayerPrefs.SetFloat(MusicVolumeKey, value);
            ApplyToAudioManager();
        }
    }

    public float EffectsVolume
    {
        get => PlayerPrefs.GetFloat(EffectsVolumeKey);
        set
        {
            PlayerPrefs.SetFloat(EffectsVolumeKey, value);
            ApplyToAudioManager();
        }
    }

    public float VoiceVolume
    {
        get => PlayerPrefs.GetFloat(VoiceVolumeKey);
        set
        {
            PlayerPrefs.SetFloat(VoiceVolumeKey, value);
            ApplyToAudioManager();
        }
    }

    private void Start()
    {
        _audio = FindObjectOfType<AudioManager>();
        SetDefaultsIfRequired();
        ApplyToAudioManager();

        if (FindObjectOfType<SettingsManager>() != this)
            Debug.LogWarning($"Scene can contain only one {nameof(SettingsManager)}");
    }

    private void SetDefaultsIfRequired()
    {
        if (!PlayerPrefs.HasKey(EffectsVolumeKey))
            EffectsVolume = 1;

        if (!PlayerPrefs.HasKey(MusicVolumeKey))
            MusicVolume = 1;

        if (!PlayerPrefs.HasKey(VoiceVolumeKey))
            VoiceVolume = 1;
    }

    private void ApplyToAudioManager()
    {
        if (_audio == null) return;
        foreach (var sound in _audio.Sounds.Where(s => s.Category == Sound.SoundCategory.Music))
            sound.VolumeScale = MusicVolume;

        foreach (var sound in _audio.Sounds.Where(s => s.Category == Sound.SoundCategory.Effects))
            sound.VolumeScale = EffectsVolume;

        foreach (var sound in _audio.Sounds.Where(s => s.Category == Sound.SoundCategory.Voice))
            sound.VolumeScale = VoiceVolume;
    }
}
