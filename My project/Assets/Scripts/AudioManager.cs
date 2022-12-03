using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Audio
{
    public class AudioManager : MonoBehaviour
    {
        private static AudioManager instance;
        public AudioSource bgm;
        public AudioSource hit;
        public AudioSource clickBtn;
        public static AudioManager Instance;

        public void Awake()
        {
            Instance = this;
            bgm.volume = 0.9F;
        }


        public void ChangeVolume(float volume)
        {
            bgm.volume = volume;
            hit.volume = volume;
            clickBtn.volume = volume;
        }

        public void Stop(int type)
        {
            switch (type)
            {
                case 0: bgm.Stop();
                    break;
                case 1: hit.Stop();
                    break;
                case 2: clickBtn.Stop();
                    break;
                default:
                    throw new Exception("No such audio");
            }
        }

        public void Play(int type)
        {
            switch (type)
            {
                case 0: bgm.Play();
                    break;
                case 1: hit.Play();
                    break;
                case 2: clickBtn.Play();
                    break;
                default:
                    throw new Exception("No such audio");
            }
        }

    }
}