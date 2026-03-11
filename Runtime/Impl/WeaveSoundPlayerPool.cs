#nullable enable

using System;
using System.Collections.Generic;
using UnityEngine;

namespace SoundWeave.Impl
{
    public sealed class WeaveSoundPlayerPool : MonoBehaviour, ISoundPlayerPool
    {
        [SerializeField] private AudioSource? _audioSourcePrefab;
        [SerializeField] private int _initialCount;
        [SerializeField] private int _maxCount = -1;
        [SerializeField] private bool _warnOnMaxExceeded = true;
        [SerializeField] private bool _warnOnNullMixerGroup = true;
        [SerializeField] private bool _dontDestroyOnLoad = true;

        private List<WeaveSoundPlayer> _players = null!;
        private int _freeCount;

        public int ActiveCount => _players.Count - _freeCount;
        public int FreeCount => _freeCount;

        public SoundHandle Play(in SoundData data)
        {
            var player = RentPlayer();
            if (player == null)
                return SoundHandle.Invalid;

            if (_warnOnNullMixerGroup && data.OutputAudioMixerGroup == null)
                Debug.LogWarning(
                    $"SoundWeave: OutputAudioMixerGroup is null. AudioGenerator: {data.AudioGenerator.definition}",
                    player);

            return player.Play(data);
        }

        public void Dispose()
        {
            if (_players == null)
                return;

            for (var i = _players.Count - 1; i >= 0; i--)
            {
                if (_players[i] != null)
                    Destroy(_players[i].gameObject);
            }

            _players.Clear();
            _freeCount = 0;
        }

        internal void ReturnToPool(WeaveSoundPlayer player)
        {
            if (!player.gameObject.activeSelf)
                return;

            _freeCount++;
            player.gameObject.SetActive(false);
        }

        public IEnumerable<SoundHandle> EnumerateActiveHandles()
        {
            foreach (var player in _players)
            {
                if (!player.IsFree)
                    yield return player.Handle;
            }
        }

        public static WeaveSoundPlayerPool Create(
            Transform? parent = null,
            int initialCount = 4,
            int maxCount = -1,
            AudioSource? prefab = null,
            bool dontDestroyOnLoad = true)
        {
            var go = new GameObject(nameof(WeaveSoundPlayerPool));

            if (parent != null)
                go.transform.SetParent(parent);

            var pool = go.AddComponent<WeaveSoundPlayerPool>();
            pool._audioSourcePrefab = prefab;
            pool._initialCount = initialCount;
            pool._maxCount = maxCount;
            pool._dontDestroyOnLoad = dontDestroyOnLoad;
            pool.Initialize();

            return pool;
        }

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            if (_players != null)
                return;

            _players = new List<WeaveSoundPlayer>(_initialCount);

            for (var i = 0; i < _initialCount; i++)
                CreatePlayer();

            if (_dontDestroyOnLoad)
                DontDestroyOnLoad(gameObject);
        }

        private WeaveSoundPlayer? RentPlayer()
        {
            var player = GetOrCreatePlayer();
            if (player == null)
                return null;

            _freeCount--;
            player.gameObject.SetActive(true);
            return player;
        }

        private WeaveSoundPlayer? GetFreePlayer()
        {
            foreach (var player in _players)
            {
                if (player.IsFree)
                    return player;
            }

            return null;
        }

        private WeaveSoundPlayer? GetOrCreatePlayer()
        {
            if (_freeCount > 0)
                return GetFreePlayer();

            if (_maxCount < 0 || _players.Count < _maxCount)
                return CreatePlayer();

            if (_warnOnMaxExceeded)
                Debug.LogWarning(
                    "SoundWeave: Max player count exceeded. Increase maxCount or check for leaks.");

            return null;
        }

        private WeaveSoundPlayer CreatePlayer()
        {
            GameObject playerGo;

            if (_audioSourcePrefab != null)
            {
                playerGo = Instantiate(_audioSourcePrefab, transform).gameObject;
            }
            else
            {
                playerGo = new GameObject("WeaveSoundPlayer", typeof(AudioSource));
                playerGo.transform.SetParent(transform);
            }

            var player = playerGo.AddComponent<WeaveSoundPlayer>();
            player.SetPool(this);
            playerGo.SetActive(false);

            _players.Add(player);
            _freeCount++;
            return player;
        }
    }
}
