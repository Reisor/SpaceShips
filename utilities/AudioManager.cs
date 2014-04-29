
using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Audio;

namespace SpaceShips
{
	public class AudioManager
	{
		private Bgm _bgm;
		private BgmPlayer _bgmPlayer;
		private Sound _sound;
		private Dictionary<string, SoundPlayer> _soundPlayer;

		private string _musicPath = "/Application/assets/audio/music/";
		private string _audioPath = "/Application/assets/audio/sound/";

		public AudioManager()
		{
			_bgm = new Bgm(_musicPath + "Music1.mp3");
	
			if(_bgmPlayer != null)
				_bgmPlayer.Dispose();
	
			_bgmPlayer = _bgm.CreatePlayer();
			_bgmPlayer.Loop = true;
			_bgmPlayer.Play();

			_soundPlayer = new Dictionary<string, SoundPlayer>();

			_sound = new Sound(_audioPath + "SYS_SE_01.wav");
			_soundPlayer.Add("systemSelect", _sound.CreatePlayer());
			_sound = new Sound(_audioPath + "Bullet.wav");
			_soundPlayer.Add("laser", _sound.CreatePlayer());
			_sound = new Sound(_audioPath + "Explosion.wav");
			_soundPlayer.Add("explosion", _sound.CreatePlayer());
			_sound = new Sound(_audioPath + "Explosion2.wav");
			_soundPlayer.Add("explosion2", _sound.CreatePlayer());

			_bgm.Dispose();
			_sound.Dispose();
		}
	
		public void ChangeSong(string name)
		{
			_bgmPlayer.Dispose();
			_bgm = new Bgm(_musicPath + name);
			_bgmPlayer = _bgm.CreatePlayer();
			_bgmPlayer.Loop = true;
			_bgmPlayer.Play();

			_bgm.Dispose();
		}

		public void PauseSong()
		{
			if (_bgmPlayer.Status == BgmStatus.Playing)
				_bgmPlayer.Pause();
		}

		public void ResumeSong()
		{
			if (_bgmPlayer.Status == BgmStatus.Paused)
				_bgmPlayer.Resume();
		}

		public void StopSong()
		{
			if (_bgmPlayer.Status == BgmStatus.Playing)
				_bgmPlayer.Stop();
		}
	
		public void playSound(string name)
		{
			_soundPlayer[name].Play();
		}

		public void Terminate()
		{
			_bgmPlayer.Stop();
			_bgmPlayer.Dispose();
			_bgm.Dispose();
		}
	}
}