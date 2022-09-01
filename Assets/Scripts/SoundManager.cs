using System.Linq;
using UnityEngine;

namespace Cars
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Singleton;
        [SerializeField]
        private AudioClip[] _tracks;
        [SerializeField]
        private AudioSource _source;
        private AudioClip _currentClip;

        private void Start()
        {
            if (!Singleton)
            {
                Singleton = this;
                DontDestroyOnLoad(gameObject);
            }
            else Destroy(gameObject);
            _currentClip = _tracks[UnityEngine.Random.Range(0, _tracks.Length)];
            _source.Play();
        }

        private void Update()
        {
            if (_source.isPlaying) return;
            _source.clip = GetClip(_currentClip);
            _currentClip = _source.clip;
            _source.Play();
        }

        private AudioClip GetClip(Object exception)
        {
            AudioClip[] clips = _tracks.Where(z => z != exception).ToArray();
            return clips[UnityEngine.Random.Range(0, clips.Length)];
        }
    }
}