using UnityEngine;
using System.Collections;

namespace VisualNovel.Core
{
    /// <summary>
    /// Manages background music and sound effects
    /// </summary>
    public class AudioManager : MonoBehaviour
    {
        private static AudioManager instance;
        public static AudioManager Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject go = new GameObject("AudioManager");
                    instance = go.AddComponent<AudioManager>();
                    DontDestroyOnLoad(go);
                }
                return instance;
            }
        }

        [System.Serializable]
        public class AudioTrack
        {
            public string trackName;
            public AudioClip clip;
        }

        [Header("Audio Sources")]
        private AudioSource musicSource;
        private AudioSource sfxSource;
        private AudioSource voiceSource;

        [Header("Audio Tracks")]
        public AudioTrack[] musicTracks;
        public AudioTrack[] soundEffects;

        [Header("Settings")]
        public float musicFadeDuration = 1f;
        public float defaultMusicVolume = 0.7f;
        public float defaultSFXVolume = 1f;
        public float defaultVoiceVolume = 1f;

        private Coroutine musicFadeCoroutine;

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;
            DontDestroyOnLoad(gameObject);

            InitializeAudioSources();
        }

        private void InitializeAudioSources()
        {
            // Create music source
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.loop = true;
            musicSource.volume = defaultMusicVolume;
            musicSource.playOnAwake = false;

            // Create SFX source
            sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.loop = false;
            sfxSource.volume = defaultSFXVolume;
            sfxSource.playOnAwake = false;

            // Create voice source
            voiceSource = gameObject.AddComponent<AudioSource>();
            voiceSource.loop = false;
            voiceSource.volume = defaultVoiceVolume;
            voiceSource.playOnAwake = false;
        }

        #region Music Control
        /// <summary>
        /// Plays background music by name
        /// </summary>
        public void PlayMusic(string trackName, bool fadeIn = true)
        {
            AudioTrack track = System.Array.Find(musicTracks, t => t.trackName == trackName);
            
            if (track == null)
            {
                Debug.LogWarning($"Music track not found: {trackName}");
                return;
            }

            if (musicSource.clip == track.clip && musicSource.isPlaying)
            {
                return; // Already playing this track
            }

            if (fadeIn && musicSource.isPlaying)
            {
                if (musicFadeCoroutine != null)
                {
                    StopCoroutine(musicFadeCoroutine);
                }
                musicFadeCoroutine = StartCoroutine(CrossfadeMusic(track.clip));
            }
            else
            {
                musicSource.clip = track.clip;
                musicSource.Play();
            }
        }

        /// <summary>
        /// Stops the currently playing music
        /// </summary>
        public void StopMusic(bool fadeOut = true)
        {
            if (fadeOut)
            {
                if (musicFadeCoroutine != null)
                {
                    StopCoroutine(musicFadeCoroutine);
                }
                musicFadeCoroutine = StartCoroutine(FadeOutMusic());
            }
            else
            {
                musicSource.Stop();
            }
        }

        private IEnumerator CrossfadeMusic(AudioClip newClip)
        {
            float elapsed = 0f;
            float startVolume = musicSource.volume;

            // Fade out
            while (elapsed < musicFadeDuration / 2)
            {
                elapsed += Time.deltaTime;
                musicSource.volume = Mathf.Lerp(startVolume, 0f, elapsed / (musicFadeDuration / 2));
                yield return null;
            }

            // Switch clips
            musicSource.clip = newClip;
            musicSource.Play();

            // Fade in
            elapsed = 0f;
            while (elapsed < musicFadeDuration / 2)
            {
                elapsed += Time.deltaTime;
                musicSource.volume = Mathf.Lerp(0f, defaultMusicVolume, elapsed / (musicFadeDuration / 2));
                yield return null;
            }

            musicSource.volume = defaultMusicVolume;
        }

        private IEnumerator FadeOutMusic()
        {
            float elapsed = 0f;
            float startVolume = musicSource.volume;

            while (elapsed < musicFadeDuration)
            {
                elapsed += Time.deltaTime;
                musicSource.volume = Mathf.Lerp(startVolume, 0f, elapsed / musicFadeDuration);
                yield return null;
            }

            musicSource.Stop();
            musicSource.volume = defaultMusicVolume;
        }
        #endregion

        #region Sound Effects
        /// <summary>
        /// Plays a sound effect by name
        /// </summary>
        public void PlaySFX(string sfxName)
        {
            AudioTrack sfx = System.Array.Find(soundEffects, s => s.trackName == sfxName);
            
            if (sfx == null)
            {
                Debug.LogWarning($"Sound effect not found: {sfxName}");
                return;
            }

            sfxSource.PlayOneShot(sfx.clip);
        }

        /// <summary>
        /// Plays a sound effect from an AudioClip
        /// </summary>
        public void PlaySFX(AudioClip clip)
        {
            if (clip != null)
            {
                sfxSource.PlayOneShot(clip);
            }
        }
        #endregion

        #region Voice Acting
        /// <summary>
        /// Plays voice acting clip
        /// </summary>
        public void PlayVoice(AudioClip voiceClip)
        {
            if (voiceClip == null)
                return;

            voiceSource.clip = voiceClip;
            voiceSource.Play();
        }

        /// <summary>
        /// Stops current voice acting
        /// </summary>
        public void StopVoice()
        {
            voiceSource.Stop();
        }
        #endregion

        #region Volume Control
        public void SetMusicVolume(float volume)
        {
            musicSource.volume = Mathf.Clamp01(volume);
            defaultMusicVolume = musicSource.volume;
        }

        public void SetSFXVolume(float volume)
        {
            sfxSource.volume = Mathf.Clamp01(volume);
            defaultSFXVolume = sfxSource.volume;
        }

        public void SetVoiceVolume(float volume)
        {
            voiceSource.volume = Mathf.Clamp01(volume);
            defaultVoiceVolume = voiceSource.volume;
        }
        #endregion
    }
}
