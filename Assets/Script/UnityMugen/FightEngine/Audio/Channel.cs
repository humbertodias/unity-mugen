using System;
using System.Diagnostics;
using UnityEngine;

namespace UnityMugen.Audio
{
    /// <summary>
    /// A location where an instance of a sound may be played.
    /// </summary>
    public class Channel
    {
        /// <summary>
        /// Creates a new instance of this class.
        /// </summary>
        public Channel(AudioSource audioSource)
        {
            m_soundEffect = audioSource;
            m_paused = false;
            m_usingid = new ChannelId();
        }

        /// <summary>
        /// Plays a sound.
        /// </summary>
        /// <param name="id">The ChannelId identifing the SoundManger playing the sound and the channel number.</param>
        /// <param name="buffer">A byte[] containing the sound to be playing.</param>
        /// <param name="frequencymultiplier">The multiplier applied the sound to change its pitch. 1.0f for no change.</param>
        /// <param name="looping">Whether the sound should loop automatically until stopped.</param>
        /// <param name="volume">The volume level of the sound.</param>
        public void Play(ChannelId id, AudioClip audioClip, float frequencymultiplier, bool looping, int volume)
        {
            if (audioClip == null) throw new ArgumentNullException(nameof(audioClip));

            Stop();

            m_usingid = id;

            try
            {
                m_soundEffect.pitch = frequencymultiplier;
                m_soundEffect.volume = volume * 0.01f;
                m_soundEffect.panStereo = m_panning;
                m_soundEffect.clip = audioClip;
                m_soundEffect.loop = looping;
                m_soundEffect.Play();
            }
            catch
            {
                UnityEngine.Debug.Log("Error - Play Audio. ChannelId: " + id);
            }
        }

        /// <summary>
        /// Pans a currently playing sound left or right of it current location, in pixels.
        /// </summary>
        /// <param name="panning">The distance from the current panning location, in pixels, that the sound should be panned.</param>
        public void RelativePan(int panning)
        {
            AbsolutePan(m_panning + panning);
        }

        /// <summary>
        /// Pans a currently playing sound to a new location, in pixels.
        /// </summary>
        /// <param name="panning">The distance from the center of the screen, in pixels, that the sound should be panned.</param>
        public void AbsolutePan(int panning)
        {
            if (IsPlaying == false) return;

            int halfx = Screen.currentResolution.width / 2;
            m_panning = Misc.Clamp(panning, -halfx, halfx);

            float pan_percentage = (m_panning + halfx) / Screen.currentResolution.width;
            float pan_amount = Mathf.LerpUnclamped(/*Pan.Left*/-1f, /*Pan.Right*/1f, pan_percentage);
            m_soundEffect.panStereo = pan_amount;
        }

        /// <summary>
        /// Stops the sounds currently playing in this Channel.
        /// </summary>
        public void Stop()
        {
            if (m_soundEffect != null)
            {
                m_soundEffect.Stop();
            }
        }

        public void Mute(bool mute)
        {
            if (m_soundEffect != null)
            {
                m_soundEffect.mute = mute;
            }
        }

        public void Pause()
        {
            if (IsPlaying == true)
            {
                m_soundEffect.Pause();
                m_paused = true;
            }
        }

        public void UnPause()
        {
            if (m_soundEffect != null && m_paused == true)
            {
                m_paused = false;
                m_soundEffect.UnPause();
            }
        }

        /// <summary>
        /// Disposes of resources managed by this class instance.
        /// </summary>
        /// <param name="disposing">Determined whether to dispose of managed resources.</param>
        public void Dispose(bool disposing)
        {
            if (disposing == true)
            {
                if (m_soundEffect != null)
                {
                    m_soundEffect.Stop();
                }
            }

        }

        /// <summary>
        /// Get whether there is a sound currenly playing in this Channel.
        /// </summary>
        /// <returns>true is a sound is playing; false otherwise.</returns>
        public bool IsPlaying
        {
            get { return (m_soundEffect != null) && m_soundEffect.isPlaying; }
        }

        /// <summary>
        /// A identifier for the SoundManager currently using this channel.
        /// </summary>
        /// <returns>If there is a sound playing in this Channel, then the ChannelId identifing the SoundManager and the channel number. Otherwise, a default constructed ChannelId.</returns>
        public ChannelId UsingId
        {
            get { return (IsPlaying == true) ? m_usingid : new ChannelId(); }
        }

        #region Fields

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_panning;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ChannelId m_usingid;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_paused;

        public AudioSource m_soundEffect;

        #endregion
    }
}