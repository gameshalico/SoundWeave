#nullable enable

using SoundWeave.Impl;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace SoundWeave.Editor
{
    [CustomEditor(typeof(SoundProfileAsset))]
    public sealed class SoundProfileAssetEditor : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var container = new VisualElement();

            var clipSourceProperty = serializedObject.FindProperty("_clipSource");
            ClipSourceDrawerHelper.AddClipSourceFields(container, clipSourceProperty);

            container.Add(new PropertyField(serializedObject.FindProperty("_outputAudioMixerGroup")) { label = "Output" });
            container.Add(new PropertyField(serializedObject.FindProperty("_mute")));

            AddSlider(container, serializedObject.FindProperty("_volume"), "Volume", 0, 1);
            AddSlider(container, serializedObject.FindProperty("_pitch"), "Pitch", -3, 3);

            var prioritySlider = new SliderInt(0, 256) { showInputField = true, label = "Priority" };
            prioritySlider.BindProperty(serializedObject.FindProperty("_priority"));
            container.Add(prioritySlider);

            AddSlider(container, serializedObject.FindProperty("_panStereo"), "Stereo Pan", -1, 1);

            container.Add(new PropertyField(serializedObject.FindProperty("_loop")));
            container.Add(new PropertyField(serializedObject.FindProperty("_startSample")));
            container.Add(new PropertyField(serializedObject.FindProperty("_delay")));

            return container;
        }

        private static void AddSlider(VisualElement container, SerializedProperty property,
            string label, float min, float max)
        {
            var slider = new Slider(min, max) { showInputField = true, label = label };
            slider.BindProperty(property);
            container.Add(slider);
        }
    }
}
