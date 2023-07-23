using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Audio;
using UnityMugen.Collections;

namespace UnityMugen.Audio
{

    public class SoundSystem : MonoBehaviour
    {
        private LauncherEngine Launcher => LauncherEngine.Inst;

        [Header("Audio Mixer Groups")]
        public AudioMixerGroup master;
        public AudioMixerGroup music;
        public AudioMixerGroup sfx;
        public AudioMixerGroup voice;

        [Header("Audio Clips")]
        public AudioClip select;
        public AudioClip selected;
        public AudioClip cancel;

        public AudioSource audioSourceMusic { get; set; }
        AudioSource audioSourcePrimary;
        AudioSource audioSourceVoiceTest;

        public SoundSystem Initialize()
        {
            m_soundcache = new Dictionary<string, ReadOnlyDictionary<SoundId, AudioClip>>(StringComparer.OrdinalIgnoreCase);
            m_volume = 0;
            m_channels = new List<Channel>();

            CreateMusicChannel();
            CreateSoundChannelPrimary();
            CreateSoundChannelVoiceTest();
            CreateSoundChannels();
            master.audioMixer.GetFloat("MasterVolume", out volumeMaster);
            return this;
        }

        float volumeMaster;
        void OnApplicationFocus(bool hasFocus)
        {
            int count = Launcher.initializationSettings.SoundChannels;
            if (!hasFocus)
            {
                master.audioMixer.SetFloat("MasterVolume", -80f);
                foreach (Channel channel in m_channels)
                {
                    channel.Mute(true);
                }
            }
            else
            {
                master.audioMixer.SetFloat("MasterVolume", volumeMaster);
                foreach (Channel channel in m_channels)
                {
                    channel.Mute(false);
                }
            }
        }

        public void CreateMusicChannel()
        {
            audioSourceMusic = gameObject.AddComponent<AudioSource>();
            audioSourceMusic.playOnAwake = false;
            audioSourceMusic.outputAudioMixerGroup = music;
        }

        private void CreateSoundChannelVoiceTest()
        {
            audioSourceVoiceTest = gameObject.AddComponent<AudioSource>();
            audioSourceVoiceTest.playOnAwake = false;
            audioSourceVoiceTest.outputAudioMixerGroup = voice;
        }

        private void CreateSoundChannelPrimary()
        {
            audioSourcePrimary = gameObject.AddComponent<AudioSource>();
            audioSourcePrimary.playOnAwake = false;
            audioSourcePrimary.outputAudioMixerGroup = sfx;
        }

        public void PlayMusic(AudioClip audioClip, bool loop)
        {
            audioSourceMusic.clip = audioClip;
            audioSourceMusic.loop = loop;
            audioSourceMusic.volume = 1;
            audioSourceMusic.Play();
        }

        public void StopMusic()
        {
            audioSourceMusic.Stop();
        }

        public void ControlVolumeMusic(float volume)
        {
            audioSourceMusic.volume = volume;
        }

        public IEnumerator FadeOutMusic(float fadeTime)
        {
            //float startVolume = audioSourceMusic.volume;

            while (audioSourceMusic.volume > 0)
            {
                audioSourceMusic.volume -= /*startVolume **/ Time.deltaTime / fadeTime;

                yield return null;
            }

            audioSourceMusic.Stop();
            //audioSourceMusic.volume = startVolume;
        }

        public void PlayVoiceTest(AudioClip audioClip)
        {
            audioSourceVoiceTest.clip = audioClip;
            audioSourceVoiceTest.Play();
        }

        public void PlayChannelPrimary(TypeSound typeSound)
        {
            if (typeSound == TypeSound.SELECT)
                audioSourcePrimary.clip = select;
            else if (typeSound == TypeSound.SELECTED)
                audioSourcePrimary.clip = selected;
            else if (typeSound == TypeSound.CANCEL)
                audioSourcePrimary.clip = cancel;

            audioSourcePrimary.Play();
        }

        private void CreateSoundChannels()
        {
            int count = Launcher.initializationSettings.SoundChannels;

            if (count <= 0) throw new ArgumentOutOfRangeException(nameof(count));

            if (m_channels.Count != count)
            {
                for (var i = 0; i != count; ++i)
                {
                    AudioSource audioSource = gameObject.AddComponent<AudioSource>();
                    audioSource.playOnAwake = false;
                    m_channels.Add(new Channel(audioSource));
                }
            }
        }

        /// <summary>
        /// Creates a new SoundManager from a SND file.
        /// </summary>
        /// <param name="filepath">Relative filepath of the SND file.</param>
        /// <returns>A new SoundManager instance capable of playing the sounds contained in a SND file.</returns>
        public SoundManager CreateManager(string filepath)
        {
            ReadOnlyDictionary<SoundId, AudioClip> sounds;

            if (m_soundcache.TryGetValue(filepath.GetHashCode().ToString(), out sounds) == true)
                return new SoundManager(this, filepath.GetHashCode().ToString(), sounds);

            try
            {
                return new SoundManager(this, filepath.GetHashCode().ToString(), GetSounds2(filepath));
            }
            catch (System.IO.FileNotFoundException)
            {
                return new SoundManager(this, filepath.GetHashCode().ToString(), new ReadOnlyDictionary<SoundId, AudioClip>(new Dictionary<SoundId, AudioClip>()));
            }
        }

        /// <summary>
        /// Creates a duplicate of a sound buffer.
        /// </summary>
        /// <param name="buffer">The buffer to be cloned.</param>
        /// <returns>A duplicate instance of supplied sound buffer.</returns>
        public byte[] CloneBuffer(byte[] buffer)
        {
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));

            var newbuffer = new byte[buffer.Length];
            Buffer.BlockCopy(buffer, 0, newbuffer, 0, buffer.Length);
            //newbuffer.Volume = 0;

            return newbuffer;
        }

        /// <summary>
        /// Stops all currently playing sound channels.
        /// </summary>
        public void StopAllSounds()
        {
            foreach (Channel channel in m_channels)
            {
                channel.Stop();
            }
        }

        public void PauseSounds()
        {
            foreach (Channel channel in m_channels)
            {
                channel.Pause();
            }
        }

        public void UnPauseSounds()
        {
            foreach (Channel channel in m_channels)
            {
                channel.UnPause();
            }
        }

        /// <summary>
        /// Stops all sounds that a given SoundManager is currently playing.
        /// </summary>
        /// <param name="manager">The SoundManager to silence.</param>
        public void Stop(SoundManager manager)
        {
            if (manager == null) throw new ArgumentNullException(nameof(manager));

            foreach (Channel channel in m_channels)
            {
                if (channel.UsingId.Manager == manager) channel.Stop();
            }
        }

        /// <summary>
        /// Retrieves a speficied Channel for playing a sound.
        /// </summary>
        /// <param name="id">Identifier for the requested Channel.</param>
        /// <returns>The Channel current playing with the supplied identifier. If no Channel is found, than any available Channel is returned. If there are no available channels, then null.</returns>
        public Channel GetChannel(ChannelId id)
        {
            if (id == new ChannelId()) throw new ArgumentException("id");

            if (id.Number == -1)
            {
                foreach (Channel channel in m_channels)
                {
                    if (channel.IsPlaying == false) return channel;
                }
            }
            else
            {
                foreach (Channel channel in m_channels)
                {
                    if (channel.UsingId == id) return channel;
                }

                foreach (Channel channel in m_channels)
                {
                    if (channel.IsPlaying == false) return channel;
                }
            }

            return null;
        }

        /// <summary>
        /// Loads sounds contained in a SND file.
        /// </summary>
        /// <param name="filepath">The filepath of the SND file to load sounds from.</param>
        /// <returns>A cached ReadOnlyDictionary mapping SoundIds to their respective SecondaryBuffers.</returns>
        private ReadOnlyDictionary<SoundId, AudioClip> GetSounds2(string filepath)
        {
            if (filepath == null) throw new ArgumentNullException(nameof(filepath));

            var sounds = new Dictionary<SoundId, AudioClip>();

            var file = LauncherEngine.Inst.fileSystem.OpenFile(filepath);
            {
                var header = new SoundFileHeader(file);

                file.SeekFromBeginning(header.SubheaderOffset);

                for (var i = 0; i != header.NumberOfSounds; ++i)
                {
                    var subheader = new SoundFileSubHeader(file);

                    if (sounds.ContainsKey(subheader.Id) == true)
                    {
                        UnityEngine.Debug.LogWarningFormat("Duplicate sound with ID {0} discarded.", subheader.Id);
                    }
                    else
                    {
                        try
                        {
                            sounds.Add(subheader.Id, CreateSoundBuffer(file, subheader.SoundLength, subheader.Id));
                        }
                        catch
                        {
                            UnityEngine.Debug.LogWarningFormat("Error reading sound with ID {0}.", subheader.Id);
                        }
                    }

                    file.SeekFromBeginning(subheader.NextOffset);
                }
            }

            ReadOnlyDictionary<SoundId, AudioClip> savedsounds = new ReadOnlyDictionary<SoundId, AudioClip>(sounds);
            m_soundcache.Add(filepath.GetHashCode().ToString(), savedsounds);

            return savedsounds;
        }

        private AudioClip CreateSoundBuffer(IO.File file, int length, SoundId soundId)
        {
            if (file == null) throw new ArgumentNullException(nameof(file));
            if (length <= 0) throw new ArgumentOutOfRangeException(nameof(length), "length must be greater than zero.");

            var buffer = file.ReadBytes(length);

            return WavUtility.ToAudioClip(buffer, 0, soundId.ToString());
        }

        /// <summary>
        /// Disposes of resources managed by this class instance.
        /// </summary>
        /// <param name="disposing">Determined whether to dispose of managed resources.</param>
        protected void Dispose(Boolean disposing)
        {
            if (disposing == true)
            {
                if (m_channels != null)
                {
                    foreach (Channel channel in m_channels)
                    {
                        channel.Dispose(true);
                    }
                }
            }
        }

        /// <summary>
        /// Controls volume level for all sounds.
        /// </summary>
        /// <returns>The current volume level.</returns>
        public int GlobalVolume
        {
            get { return m_volume; }

            set
            {
                //if (value > (int)Volume.Max || value < (int)Volume.Min) throw new ArgumentOutOfRangeException("value");

                m_volume = value;
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Dictionary<string, ReadOnlyDictionary<SoundId, AudioClip>> m_soundcache;

        [NonSerialized]
        public List<Channel> m_channels;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        int m_volume;
    }

    internal class SoundFileSubHeader
    {
        public SoundFileSubHeader(UnityMugen.IO.File file)
        {
            if (file == null) throw new ArgumentNullException(nameof(file));

            var data = file.ReadBytes(16);
            if (data.Length != 16) throw new ArgumentException("File is not long enough", nameof(file));

            NextOffset = BitConverter.ToInt32(data, 0);
            SoundLength = BitConverter.ToInt32(data, 4);
            Id = new SoundId(BitConverter.ToInt32(data, 8), BitConverter.ToInt32(data, 12));
        }

        public int NextOffset;
        public int SoundLength;
        public SoundId Id;
    }

    internal class SoundFileHeader
    {
        public SoundFileHeader(UnityMugen.IO.File file)
        {
            if (file == null) throw new ArgumentNullException(nameof(file));

            var data = file.ReadBytes(24);
            if (data.Length != 24) throw new ArgumentException("File is not long enough", nameof(file));

            Signature = System.Text.Encoding.Default.GetString(data, 0, 11);
            Version = BitConverter.ToInt32(data, 12);
            NumberOfSounds = BitConverter.ToInt32(data, 16);
            SubheaderOffset = BitConverter.ToInt32(data, 20);
        }

        public string Signature;
        public int Version;
        public int NumberOfSounds;
        public int SubheaderOffset;

    }
}