using System;
using System.IO;
using UnityEngine;

namespace UnityMugen.Audio
{
    internal enum ALFormat
    {
        Mono8 = 0x1100,
        Mono16 = 0x1101,
        Stereo8 = 0x1102,
        Stereo16 = 0x1103,
        MonoIma4 = 0x1300,
        StereoIma4 = 0x1301,
        MonoMSAdpcm = 0x1302,
        StereoMSAdpcm = 0x1303,
        MonoFloat32 = 0x10010,
        StereoFloat32 = 0x10011,
    }

    public class WavDecode
    {
        internal const int FormatPcm = 1;
        internal const int FormatMsAdpcm = 2;
        internal const int FormatIeee = 3;
        internal const int FormatIma4 = 17;

        static ALFormat GetSoundFormat(int format, int channels, int bits)
        {
            switch (format)
            {
                case FormatPcm:
                    // PCM
                    switch (channels)
                    {
                        case 1: return bits == 8 ? ALFormat.Mono8 : ALFormat.Mono16;
                        case 2: return bits == 8 ? ALFormat.Stereo8 : ALFormat.Stereo16;
                        default: throw new NotSupportedException("The specified channel count is not supported.");
                    }
                case FormatMsAdpcm:
                    // Microsoft ADPCM
                    switch (channels)
                    {
                        case 1: return ALFormat.MonoMSAdpcm;
                        case 2: return ALFormat.StereoMSAdpcm;
                        default: throw new NotSupportedException("The specified channel count is not supported.");
                    }
                case FormatIeee:
                    // IEEE Float
                    switch (channels)
                    {
                        case 1: return ALFormat.MonoFloat32;
                        case 2: return ALFormat.StereoFloat32;
                        default: throw new NotSupportedException("The specified channel count is not supported.");
                    }
                case FormatIma4:
                    // IMA4 ADPCM
                    switch (channels)
                    {
                        case 1: return ALFormat.MonoIma4;
                        case 2: return ALFormat.StereoIma4;
                        default: throw new NotSupportedException("The specified channel count is not supported.");
                    }
                default:
                    throw new NotSupportedException("The specified sound format (" + format.ToString() + ") is not supported.");
            }
        }

        static int SampleAlignment(ALFormat format, int blockAlignment)
        {
            switch (format)
            {
                case ALFormat.MonoIma4:
                    return (blockAlignment - 4) / 4 * 8 + 1;
                case ALFormat.StereoIma4:
                    return (blockAlignment / 2 - 4) / 4 * 8 + 1;
                case ALFormat.MonoMSAdpcm:
                    return (blockAlignment - 7) * 2 + 2;
                case ALFormat.StereoMSAdpcm:
                    return (blockAlignment / 2 - 7) * 2 + 2;
            }
            return 0;
        }

        public static AudioClip LoadWave(byte[] data2, string name = "wav"/*, out ALFormat format, out int frequency, out int channels, out int blockAlignment, out int bitsPerSample, out int samplesPerBlock, out int sampleCount*/)
        {

            byte[] audioData = null;
            float[] data;

            int audioFormat = 0;
            int channels = 0;
            int bitsPerSample = 0;
            ALFormat format = ALFormat.Mono16;
            int frequency = 0;
            int blockAlignment = 0;
            int samplesPerBlock = 0;
            int sampleCount = 0;

            var stream = new MemoryStream(data2);
            using (BinaryReader reader = new BinaryReader(stream))
            {
                string signature = new string(reader.ReadChars(4));
                if (signature != "RIFF")
                    throw new ArgumentException("Specified stream is not a wave file.");
                reader.ReadInt32(); // riff_chunk_size

                string wformat = new string(reader.ReadChars(4));
                if (wformat != "WAVE")
                    throw new ArgumentException("Specified stream is not a wave file.");


                // WAVE header
                while (audioData == null)
                {
                    string chunkType = new string(reader.ReadChars(4));
                    int chunkSize = reader.ReadInt32();
                    switch (chunkType)
                    {
                        case "fmt ":
                            {
                                audioFormat = reader.ReadInt16(); // 2
                                channels = reader.ReadInt16(); // 4
                                frequency = reader.ReadInt32();  // 8
                                int byteRate = reader.ReadInt32();    // 12
                                blockAlignment = (int)reader.ReadInt16();  // 14
                                bitsPerSample = reader.ReadInt16(); // 16

                                // Read extra data if present
                                if (chunkSize > 16)
                                {
                                    int extraDataSize = reader.ReadInt16();
                                    if (audioFormat == FormatIma4)
                                    {
                                        samplesPerBlock = reader.ReadInt16();
                                        extraDataSize -= 2;
                                    }
                                    if (extraDataSize > 0)
                                    {
                                        if (reader.BaseStream.CanSeek)
                                            reader.BaseStream.Seek(extraDataSize, SeekOrigin.Current);
                                        else
                                        {
                                            for (int i = 0; i < extraDataSize; ++i)
                                                reader.ReadByte();
                                        }
                                    }
                                }
                            }
                            break;
                        case "fact":
                            if (audioFormat == FormatIma4)
                            {
                                sampleCount = reader.ReadInt32() * channels;
                                chunkSize -= 4;
                            }
                            // Skip any remaining chunk data
                            if (chunkSize > 0)
                            {
                                if (reader.BaseStream.CanSeek)
                                    reader.BaseStream.Seek(chunkSize, SeekOrigin.Current);
                                else
                                {
                                    for (int i = 0; i < chunkSize; ++i)
                                        reader.ReadByte();
                                }
                            }
                            break;
                        case "data":
                            audioData = reader.ReadBytes(chunkSize);
                            break;
                        default:
                            // Skip this chunk
                            if (reader.BaseStream.CanSeek)
                                reader.BaseStream.Seek(chunkSize, SeekOrigin.Current);
                            else
                            {
                                for (int i = 0; i < chunkSize; ++i)
                                    reader.ReadByte();
                            }
                            break;
                    }
                }

                // Calculate fields we didn't read from the file
                format = GetSoundFormat(audioFormat, channels, bitsPerSample);

                if (samplesPerBlock == 0)
                {
                    samplesPerBlock = SampleAlignment(format, blockAlignment);
                }

                if (sampleCount == 0)
                {
                    switch (audioFormat)
                    {
                        case FormatIma4:
                        case FormatMsAdpcm:
                            sampleCount = ((audioData.Length / blockAlignment) * samplesPerBlock) + SampleAlignment(format, audioData.Length % blockAlignment);
                            break;
                        case FormatPcm:
                        case FormatIeee:
                            sampleCount = audioData.Length / ((channels * bitsPerSample) / 8);
                            break;
                        default:
                            throw new InvalidDataException("Unhandled WAV format " + format.ToString());
                    }
                }

                switch (bitsPerSample)
                {
                    case 8:
                        data = Convert8BitByteArrayToAudioClipData(audioData);
                        break;
                    case 16:
                        data = Convert16BitByteArrayToAudioClipData(audioData);
                        break;
                    case 24:
                        data = Convert24BitByteArrayToAudioClipData(audioData);
                        break;
                    case 32:
                        data = Convert32BitByteArrayToAudioClipData(audioData);
                        break;
                    default:
                        throw new Exception(bitsPerSample + " bit depth is not supported.");
                }
            }

            AudioClip audioClip = AudioClip.Create(name, data.Length, (int)channels, frequency, false);
            audioClip.SetData(data, 0);
            return audioClip;
        }

        private static float[] Convert8BitByteArrayToAudioClipData(byte[] source)
        {
            float[] data = new float[source.Length];

            int i = 0;
            while (i < source.Length)
            {
                data[i] = 1 - ((float)source[i] / sbyte.MaxValue);
                ++i;
            }

            return data;
        }

        private static float[] Convert16BitByteArrayToAudioClipData(byte[] source)
        {
            int x = sizeof(Int16);
            int convertedSize = source.Length / x;

            float[] data = new float[convertedSize];

            Int16 maxValue = Int16.MaxValue;

            int offset = 0;
            int i = 0;
            while (i < convertedSize)
            {
                offset = i * x;
                data[i] = (float)BitConverter.ToInt16(source, offset) / maxValue;
                ++i;
            }

            Debug.AssertFormat(data.Length == convertedSize, "AudioClip .wav data is wrong size: {0} == {1}", data.Length, convertedSize);

            return data;
        }

        private static float[] Convert24BitByteArrayToAudioClipData(byte[] source)
        {
            int x = 3;
            int convertedSize = source.Length / x;

            int maxValue = Int32.MaxValue;

            float[] data = new float[convertedSize];

            byte[] block = new byte[sizeof(int)]; // using a 4 byte block for copying 3 bytes, then copy bytes with 1 offset

            int offset = 0;
            int i = 0;
            while (i < convertedSize)
            {
                offset = i * x;
                Buffer.BlockCopy(source, offset, block, 1, x);
                data[i] = (float)BitConverter.ToInt32(block, 0) / maxValue;
                ++i;
            }

            Debug.AssertFormat(data.Length == convertedSize, "AudioClip .wav data is wrong size: {0} == {1}", data.Length, convertedSize);

            return data;
        }

        private static float[] Convert32BitByteArrayToAudioClipData(byte[] source)
        {
            int x = sizeof(float);
            int convertedSize = source.Length / x;

            Int32 maxValue = Int32.MaxValue;

            float[] data = new float[convertedSize];

            int offset = 0;
            int i = 0;
            while (i < convertedSize)
            {
                offset = i * x;
                data[i] = (float)BitConverter.ToInt32(source, offset) / maxValue;
                ++i;
            }

            Debug.AssertFormat(data.Length == convertedSize, "AudioClip .wav data is wrong size: {0} == {1}", data.Length, convertedSize);

            return data;
        }
    }
}