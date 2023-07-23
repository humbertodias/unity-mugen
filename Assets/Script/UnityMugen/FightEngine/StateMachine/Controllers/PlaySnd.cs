﻿using UnityEngine;
using UnityMugen.Audio;
using UnityMugen.Combat;
using UnityMugen.Evaluation;
using UnityMugen.IO;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("PlaySnd")]
    public class PlaySnd : StateController
    {

        private PrefixedExpression m_soundID;
        private Expression m_volume;

        /// <summary>
        /// 0 = 0%, ... 100 = 100%
        /// </summary>
        private Expression m_volumeScale;
        private Expression m_channel;
        private Expression m_channelPriority;
        private Expression m_freqMul;
        private Expression m_loop;
        private Expression m_pan;
        private Expression m_absPan;

        public PlaySnd(StateSystem statesystem, string label, TextSection textsection)
                : base(statesystem, label, textsection)
        {
            m_soundID = textsection.GetAttribute<PrefixedExpression>("value", null);
            m_pan = textsection.GetAttribute<Expression>("pan", null);
            m_absPan = textsection.GetAttribute<Expression>("abspan", null);
        }

        public override void Load()
        {
            if (isLoaded == false)
            {
                base.Load();

                m_volumeScale = textSection.GetAttribute<Expression>("volumescale", null);
                m_volume = textSection.GetAttribute<Expression>("volume", null);
                m_channel = textSection.GetAttribute<Expression>("channel", null);
                m_channelPriority = textSection.GetAttribute<Expression>("lowpriority", null);
                m_freqMul = textSection.GetAttribute<Expression>("freqmul", null);
                m_loop = textSection.GetAttribute<Expression>("loop", null);
            }
        }

        public override void Run(Character character)
        {
            Load();

            var soundid = EvaluationHelper.AsSoundId(character, m_soundID, null);

            if (soundid == null)
            {
                Debug.Log("PlaySnd : value Required");
                return;
            }


            var volume = EvaluationHelper.AsInt32(character, m_volume, 100);
            int? volumeScale = EvaluationHelper.AsInt32(character, m_volumeScale, null);
            var channelindex = EvaluationHelper.AsInt32(character, m_channel, -1);
            var priority = EvaluationHelper.AsBoolean(character, m_channelPriority, false);
            var frequencymultiplier = EvaluationHelper.AsSingle(character, m_freqMul, 1.0f);
            var loop = EvaluationHelper.AsBoolean(character, m_loop, false);
            var pan = EvaluationHelper.AsInt32(character, m_pan, null);
            var abspan = EvaluationHelper.AsInt32(character, m_absPan, null);

            SoundManager soundmanager = m_soundID.IsCommon(false) ? character.Engine.CommonSounds : character.SoundManager;

            //Channel channel = soundmanager.Play(channelindex, soundid.Value, priority, volume, frequencymultiplier, loop);
            //if (channel != null && pan != null) channel.RelativePan(pan.Value);
            //if (channel != null && abspan != null) channel.AbsolutePan(abspan.Value);

            float VolumeF = volumeScale ?? (volume * (25.0f / 64.0f));
            volume = (int)(VolumeF < 0 ? -VolumeF : VolumeF);
            soundmanager.Play(channelindex, soundid.Value, priority, volume, frequencymultiplier, loop);
        }

        public override bool IsValid()
        {
            if (base.IsValid() == false)
                return false;

            if (m_soundID == null)
                return false;
            if (m_pan != null && m_absPan != null)
                return false;

            return true;
        }
    }
}