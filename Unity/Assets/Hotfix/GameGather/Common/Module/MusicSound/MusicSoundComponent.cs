using ETModel;
using UnityEngine;

namespace ETHotfix
{

    [ObjectSystem]
    public class MusicSoundComponentAwakeSystem : AwakeSystem<MusicSoundComponent>
    {
        public override void Awake(MusicSoundComponent self)
        {
            self.Awake();
        }
    }
    public class MusicSoundComponent : Component
    {
        public static MusicSoundComponent Ins { private set; get; }
        private AudioSource mMusicAudioSource;
        private AudioSource mSoundAudioSource;
        public const string MusicVolumePrefsName = "PlayerPrefs_MusicVolume";
        public const string SoundVolumePrefsName = "PlayerPrefs_SoundVolume";
        public void Awake()
        {
            Ins = this;
            mMusicAudioSource=GameObject.Find("Global").AddComponent<AudioSource>();
            mSoundAudioSource = GameObject.Find("Global").AddComponent<AudioSource>();
            mMusicAudioSource.loop = true;
            mSoundAudioSource.loop = false;
            ButtonClickSound.RegisterPlayButtonSound();//注册按钮点击 播放事件
            MusicVolume=PlayerPrefs.GetFloat(MusicVolumePrefsName, 1f);
            SoundVolume=PlayerPrefs.GetFloat(SoundVolumePrefsName, 1f);
        }

        public void PlayMusic(AudioClip clip)
        {
            mMusicAudioSource.clip = clip;
            mMusicAudioSource.Play();
        }
        public void PlayMusicOneShot(AudioClip clip)
        {
            mMusicAudioSource.PlayOneShot(clip);
        }
        public void StopMusic()
        {
            mMusicAudioSource.Stop();
        }
        public void PlaySound(AudioClip clip)
        {
            mSoundAudioSource.PlayOneShot(clip);
        }

        public void PlayVoice(AudioClip clip)
        {
            mSoundAudioSource.PlayOneShot(clip);
        }

        public void MusicMuteSwitch(bool isMute)
        {
            mMusicAudioSource.mute = isMute;
        }
        public void SoundMuteSwitch(bool isMute)
        {
            mSoundAudioSource.mute = isMute;
        }

        public float MusicVolume
        {
            get { return mMusicAudioSource.volume; }
            set
            {
                PlayerPrefs.SetFloat(MusicVolumePrefsName, value);
                mMusicAudioSource.volume = value;
            }
        }

        public float SoundVolume
        {
            get { return mSoundAudioSource.volume; }
            set
            {
                PlayerPrefs.SetFloat(SoundVolumePrefsName, value);
                mSoundAudioSource.volume = value;
            }
        }
    }
}
