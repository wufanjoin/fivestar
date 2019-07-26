using UnityEngine;

namespace Assets.Scripts.Module.Common.ViSpeak.Compress
{
    public class ViSpeexCompress
    {
        static NSpeex.SpeexEncoder speexEnc = new NSpeex.SpeexEncoder(NSpeex.BandMode.Narrow);

        public static byte[] SpeexCompress(float[] input, out int length)
        {
            short[] shortBuffer = new short[input.Length];
            byte[] encoded = new byte[input.Length];
            input.ToShortArray(shortBuffer);
            length = speexEnc.Encode(shortBuffer, 0, shortBuffer.Length, encoded, 0, encoded.Length);
            return encoded;
        }

        public static float[] DeCompress(NSpeex.SpeexDecoder speexDec, byte[] data, int dataLength)
        {
            float[] decoded = new float[data.Length];
            short[] shortBuffer = new short[data.Length];
            speexDec.Decode(data, 0, dataLength, shortBuffer, 0, false);
            shortBuffer.ToFloatArray(decoded, shortBuffer.Length);
            return decoded;
        }
    }

    public static class ViSpeakChatUtils
    {
        public static void ToShortArray(this float[] input, short[] output)
        {
            if (output.Length < input.Length)
            {
                return;
            }

            for (int i = 0; i < input.Length; ++i)
            {
                output[i] = (short)Mathf.Clamp((int)(input[i] * 32767.0f), short.MinValue, short.MaxValue);
            }
        }


        public static void ToFloatArray(this short[] input, float[] output, int length)
        {
            if (output.Length < length || input.Length < length)
            {
                return;
            }

            for (int i = 0; i < length; ++i)
            {
                output[i] = input[i] / (float)short.MaxValue;
            }
        }

    }
}