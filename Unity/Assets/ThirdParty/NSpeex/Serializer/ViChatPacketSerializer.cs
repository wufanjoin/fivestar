using System;
using System.Text;
using UnityEngine;

public class ViChatPacket
{
	public int Length;
	public int DataLength;
	public byte[] Data;

	public void Print()
	{
		StringBuilder sb = new StringBuilder();
		sb.Append("Length:" + Length.ToString());
		sb.AppendLine();
		sb.Append("DataLength:" + DataLength.ToString());
		sb.AppendLine();
		sb.Append("Data:" + Data.ToString());
		sb.AppendLine();
		//
		Debug.Log(sb.ToString());
	}
}

public class ViChatPacketSerializer
{
	public static byte[] Serializer(ViChatPacket packet)
	{
		byte[] buffer = new byte[8 + packet.Data.Length];
		byte[] lengthBuffer = BitConverter.GetBytes(packet.Length);
		Buffer.BlockCopy(lengthBuffer, 0, buffer, 0, lengthBuffer.Length);
		//
		byte[] dataLengtBuffer = BitConverter.GetBytes(packet.DataLength);
		Buffer.BlockCopy(dataLengtBuffer, 0, buffer, 4, dataLengtBuffer.Length);
		//
		for (int iter = 0; iter < packet.Data.Length; ++iter)
		{
			buffer[8 + iter] = packet.Data[iter];
		}
		//
		return buffer;
	}

	public static ViChatPacket DeSerializer(byte[] buffer, int offest)
	{
		ViChatPacket packet = new ViChatPacket();
		packet.Length = BitConverter.ToInt32(buffer, offest);
		packet.DataLength = BitConverter.ToInt32(buffer, offest + 4);
		packet.Data = new byte[packet.Length];
		Buffer.BlockCopy(buffer, offest + 8, packet.Data, 0, packet.Length);
		//
		return packet;
	}
}
