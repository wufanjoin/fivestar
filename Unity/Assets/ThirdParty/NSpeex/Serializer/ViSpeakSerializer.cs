using System;
using System.Text;
using UnityEngine;

public class ViSpeakStruct
{
	public int Channels;
	public int Frequency;
	public int SamplesLength;
	public int DataLength;
	public byte[] Data;

	public void Print()
	{
		StringBuilder sb = new StringBuilder();
		sb.Append("Channels:" + Channels.ToString());
		sb.AppendLine();
		sb.Append("Frequency:" + Frequency.ToString());
		sb.AppendLine();
		sb.Append("SamplesLength:" + SamplesLength.ToString());
		sb.AppendLine();
		sb.Append("DataLength:" + DataLength.ToString());
		sb.AppendLine();
		sb.Append("Data:" + Data.ToString());
		sb.AppendLine();
		//
		Debug.Log(sb.ToString());
	}
}

public class ViSpeakSerializer
{
	public static byte[] Serializer(ViSpeakStruct viSpeakStruct)
	{
		byte[] buffer = new byte[12 + viSpeakStruct.Data.Length];
		byte[] channelsBuffer = BitConverter.GetBytes(viSpeakStruct.Channels);
		Buffer.BlockCopy(channelsBuffer, 0, buffer, 0, 4);
		//
		byte[] frequencyBuffer = BitConverter.GetBytes(viSpeakStruct.Frequency);
		Buffer.BlockCopy(frequencyBuffer, 0, buffer, 4, 4);
		//
		byte[] samplesLengthBuffer = BitConverter.GetBytes(viSpeakStruct.SamplesLength);
		Buffer.BlockCopy(samplesLengthBuffer, 0, buffer, 8, 4);
		//
		byte[] dataLengthBuffer = BitConverter.GetBytes(viSpeakStruct.DataLength);
		Buffer.BlockCopy(dataLengthBuffer, 0, buffer, 12, 4);
		//
		for (int iter = 0; iter < viSpeakStruct.Data.Length; ++iter)
		{
			buffer[16 + iter] = viSpeakStruct.Data[iter];
		}
		//
		return buffer;
	}

	public static ViSpeakStruct DeSerializer(byte[] buffer)
	{
		ViSpeakStruct viSpeakStruct = new ViSpeakStruct();
		viSpeakStruct.Channels = BitConverter.ToInt32(buffer, 0);
		viSpeakStruct.Frequency = BitConverter.ToInt32(buffer, 4);
		viSpeakStruct.SamplesLength = BitConverter.ToInt32(buffer, 8);
		viSpeakStruct.DataLength = BitConverter.ToInt32(buffer, 12);
		viSpeakStruct.Data = new byte[viSpeakStruct.DataLength];
		Buffer.BlockCopy(buffer, 16, viSpeakStruct.Data, 0, viSpeakStruct.DataLength);
		//
		return viSpeakStruct;
	}
}
