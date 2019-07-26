using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Assets.Scripts.Module.Common.ViSpeak.Compress;
using ETModel;

namespace ETHotfix
{



    public class SpeexRecordMgr : Single<SpeexRecordMgr>
    {
        public int SpeakMaxTime = 6; //录音最长的时间 到了时间会自动截断
        private const int RecordSamplingLength = 640; //取样长度 好像是固定640搞不懂
        private const int RecordFrequency = 4000; //取样率
        private const float ByteInLengthPropotion = RecordFrequency / 4.00f; //音频byte数组和时间长度的比例

        bool _isRecording = false; //是否录音中
        int _sampleIndex = 0; //当前取样的起点下标

        float[] _sampleBuffer; //记录当前帧的 声音录制flaot数组
        string _device; //录音设备名字
        private AudioClip _clip; //录音数据存放


        private AudioClip _byteConvetorClip; //byte转的clip这样 长度就是实际长度

        //录音的clip本地可以直接播放
        public AudioClip RecordClip //获取录音Clip
        {
            get
            {
                if (_byteConvetorClip == null)
                {
                    _byteConvetorClip = BytesToAudioClip(_sampleDataList.ToArray());
                }
                return _byteConvetorClip;
            }
        }

        public override void Init()
        {
            base.Init();
            //麦克风权限请求
            if (!Application.HasUserAuthorization(UserAuthorization.Microphone))
            {
                Application.RequestUserAuthorization(UserAuthorization.Microphone);
            }
            //初始化设备名字
            if (Microphone.devices.Length > 0)
            {
                _device = Microphone.devices[0];
            }
        }

        private bool isRecordDataIn = false;

        private async void RecordData()
        {
            if (isRecordDataIn)
            {
                return;
            }
            isRecordDataIn = true;
            while (_isRecording)
            {
                await ETModel.Game.Scene.GetComponent<TimerComponent>().WaitAsync(40); //等待40毫秒
                int currentRecodingPos = Microphone.GetPosition(null);
                while (_sampleIndex + RecordSamplingLength <= currentRecodingPos)
                {
                    _OnResample();
                }
            }
            isRecordDataIn = false;
        }


        //开始录音
        public void StartRecording()
        {
            //防止某些手机崩溃
            try
            {
                _byteConvetorClip = null;
                _sampleDataList.Clear();
                _sampleBuffer = new float[RecordSamplingLength];
                //                                       最长时间      取样率
                _clip = Microphone.Start(_device, true, SpeakMaxTime, RecordFrequency);
                _isRecording = true;
                RecordData();
            }
            catch (Exception e)
            {
                Debug.LogError("语音StartRecording()错误：" + e);
                throw;
            }
        }



        void _OnResample()
        {
            //用取样数据填充clip
            _clip.GetData(_sampleBuffer, _sampleIndex);

            float[] targetBuffer = new float[RecordSamplingLength];
            Resample(_sampleBuffer, targetBuffer); //声音去噪

            //更新取样数据下标
            _sampleIndex += RecordSamplingLength;

            //处理取样缓冲数据
            TransmitBuffer(targetBuffer);
        }

        //每帧记录声音float[]转byte[]
        void TransmitBuffer(float[] sampleBuffer)
        {
            //压缩语音：
            //得到声音包长度
            int length = 0;
            byte[] buffer = ViSpeexCompress.SpeexCompress(sampleBuffer, out length);

            //创建声音包
            ViChatPacket packet = new ViChatPacket();

            //拷贝压缩数据
            byte[] availableSampleBuffer = new byte[length];
            Buffer.BlockCopy(buffer, 0, availableSampleBuffer, 0, length);

            //赋值声音包
            packet.Data = availableSampleBuffer;
            packet.Length = length;
            packet.DataLength = buffer.Length;

            //记录缓存
            OnRecordSample(packet);
        }

        private List<byte> _sampleDataList = new List<byte>();

        /// 采样数据
        //录音过程中,保存采样包
        void OnRecordSample(ViChatPacket packet)
        {
            _sampleDataList.AddRange(ViChatPacketSerializer.Serializer(packet));
        }

        //结束录音
        public byte[] EndRecording()
        {
            if (!_isRecording)
            {
                return null; //如果不在录音中 返回空
            }
            Microphone.End(null);
            _isRecording = false;
            _sampleIndex = 0;
            return _sampleDataList.ToArray();
        }

        ///音频去噪
        void Resample(float[] src, float[] dst)
        {
            if (src.Length == dst.Length)
            {
                Array.Copy(src, 0, dst, 0, src.Length);
            }
            else
            {
                //目标数组频率
                float rec = 1.0f / (float) dst.Length; //eg: 目标数组1000长度，频率0.001     源数组长度999次  遍历1000次，最后一个数据就被合并了

                //遍历目标数组长度次
                for (int i = 0; i < dst.Length; ++i)
                {
                    //注意：这里是rec * src.Length不一定是1，因为两个数组长度可能不一样
                    float interp = rec * (float) i * (float) src.Length;
                    dst[i] = src[
                        (int) interp]; //如果源数组长度更长呢，会不会数组越界？  目标取样长度640   源数组取样长度_recordSampleSize 经过计算：不会的，即使你修改了ViSpeakSetting类中参数
                }
            }
        }

        //根据byte数组，得到一个声音片段
        public AudioClip BytesToAudioClip(byte[] buffer)
        {

            //byte数组和声音长度的换算 这里暂时不知道稳不稳定 
            float _nAudioTime = buffer.Length / ByteInLengthPropotion;

            //长度不难为0否则会报错
            if (_nAudioTime <= 0.1f)
            {
                _nAudioTime = 0.1f;
            }

            //创建声音片段，取样大小==声音长度*频率
            AudioClip clip = AudioClip.Create("AA", (int) (_nAudioTime * RecordFrequency), 1, RecordFrequency, false);

            //将_list反序列成完整的声音包列表
            List<ViChatPacket> list = new List<ViChatPacket>();
            int offest = 0;

            while (offest < buffer.Length)
            {
                ViChatPacket packet = ViChatPacketSerializer.DeSerializer(buffer, offest);
                offest = offest + 8 + packet.Data.Length; //8和包长度可能是这个声音包的包头
                list.Add(packet);
            }

            int sampleIndex = 0;
            NSpeex.SpeexDecoder _speexDec = new NSpeex.SpeexDecoder(NSpeex.BandMode.Narrow); //解析器
            for (int i = 0; i < list.Count; ++i)
            {
                //取反序列化的声音包
                ViChatPacket iterPacket = list[i];
                //包内数据拷贝到数组
                byte[] sampleData = new byte[iterPacket.DataLength];
                Buffer.BlockCopy(iterPacket.Data, 0, sampleData, 0, iterPacket.Length);
                //解压
                float[] sample = ViSpeexCompress.DeCompress(_speexDec, sampleData, iterPacket.Length);
                //将数据添加到声音片段
                clip.SetData(sample, sampleIndex);
                sampleIndex += sample.Length;
            }
            return clip;
        }
    }
}
