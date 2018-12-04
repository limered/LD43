using Assets.Systems.Sound.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using SystemBase;
using UniRx;
using UnityEngine;

namespace Assets.Systems.Sound
{
    [GameSystem]
    public class SoundSystem : GameSystem<SoundComponent, MusicComponent>
    {
        private class SoundComparer : IEqualityComparer<PlaySoundMessage>
        {
            public bool Equals(PlaySoundMessage x, PlaySoundMessage y)
            {
                return x.Tag != null && y.Tag != null && x.Tag == y.Tag;
            }

            public int GetHashCode(PlaySoundMessage obj)
            {
                throw new NotImplementedException("dont use this");
            }
        }

        public override void Register(MusicComponent component)
        {
            MessageBroker.Default
                .Receive<MuteMusicMessage>()
                .Select(x => x.Mute)
                .Subscribe(mute => component.MusicAudioSource.mute = mute)
                .AddTo(component);
        }

        public override void Register(SoundComponent component)
        {
            MessageBroker.Default
                .Receive<PlaySoundMessage>()
                .DistinctUntilChanged(new SoundComparer())
                .Select(x => x.Name)
                .Subscribe(soundName =>
                {
                    if(component.Sounds.Any(x => x.Name == soundName))
                    {
                        var sound = component.Sounds.First(x => x.Name == soundName);
                        var source = component.gameObject.AddComponent<AudioSource>();
                        source.pitch = 1 + ((UnityEngine.Random.value - 0.5f) * 2f * component.MaxPitchChange);
                        source.PlayOneShot(sound.File, sound.Volume);
                        Observable
                            .Interval(TimeSpan.FromSeconds(1))
                            .TakeWhile(_ => source.isPlaying)
                            .Subscribe(_ => { }, () => GameObject.Destroy(source));

                    }
                    else
                    {
                        Debug.LogWarning("Sound not found: " + soundName);
                    }
                })
                .AddTo(component);
        }
    }

    public static class SoundExtensions
    {
        public static void Play(this string soundName, string tag = null)
        {
            MessageBroker.Default.Publish(new PlaySoundMessage(soundName, tag));
        }
    }
}
