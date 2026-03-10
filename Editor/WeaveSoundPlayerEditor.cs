#nullable enable

using SoundWeave.Impl;
using UnityEditor;
using UnityEngine.UIElements;

namespace SoundWeave.Editor
{
    [CustomEditor(typeof(WeaveSoundPlayer))]
    public sealed class WeaveSoundPlayerEditor : UnityEditor.Editor
    {
        private EnumField _stateField = null!;
        private IntegerField _versionField = null!;
        private DoubleField _playDspTimeField = null!;
        private Toggle _isPlayingField = null!;
        private FloatField _timeField = null!;
        private IntegerField _timeSamplesField = null!;

        private void OnEnable()
        {
            EditorApplication.update += UpdateValues;
        }

        private void OnDisable()
        {
            EditorApplication.update -= UpdateValues;
        }

        private void UpdateValues()
        {
            var player = (WeaveSoundPlayer)target;
            _stateField.SetValueWithoutNotify(player.State);
            _versionField.SetValueWithoutNotify(player.Version);
            _playDspTimeField.SetValueWithoutNotify(player.PlayDspTime);
            _isPlayingField.SetValueWithoutNotify(player.IsPlaying);
            _timeField.SetValueWithoutNotify(player.Time);
            _timeSamplesField.SetValueWithoutNotify(player.TimeSamples);
        }

        public override VisualElement CreateInspectorGUI()
        {
            var container = new VisualElement();

            _stateField = new EnumField("Playback State", PlaybackState.Free);
            _versionField = new IntegerField("Version");
            _playDspTimeField = new DoubleField("Dsp Time");
            _isPlayingField = new Toggle("Is Playing");
            _timeField = new FloatField("Time");
            _timeSamplesField = new IntegerField("Time Samples");

            _stateField.SetEnabled(false);
            _versionField.SetEnabled(false);
            _playDspTimeField.SetEnabled(false);
            _isPlayingField.SetEnabled(false);
            _timeField.SetEnabled(false);
            _timeSamplesField.SetEnabled(false);

            UpdateValues();

            container.Add(_stateField);
            container.Add(_versionField);
            container.Add(_playDspTimeField);
            container.Add(_isPlayingField);
            container.Add(_timeField);
            container.Add(_timeSamplesField);

            return container;
        }
    }
}
