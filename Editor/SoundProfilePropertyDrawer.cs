#nullable enable

using SoundWeave.Impl;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace SoundWeave.Editor
{
    [CustomPropertyDrawer(typeof(SoundProfile))]
    public sealed class SoundProfilePropertyDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var foldout = new Foldout { text = property.displayName };
            var container = new VisualElement();

            var clipSourceProperty = property.FindPropertyRelative("_clipSource");
            ClipSourceDrawerHelper.AddClipSourceFields(container, clipSourceProperty);

            var outputProperty = property.FindPropertyRelative("_outputAudioMixerGroup");
            var muteProperty = property.FindPropertyRelative("_mute");
            container.Add(new PropertyField(outputProperty) { label = "Output" });
            container.Add(new PropertyField(muteProperty));

            SoundProfileDrawerHelper.AddSliderFields(container, property);
            SoundProfileDrawerHelper.AddSamplesFields(container, property);
            SoundProfileDrawerHelper.AddTimingFields(container, property);

            foldout.Add(container);
            return foldout;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.PropertyField(position, property, label, true);
            EditorGUI.EndProperty();
        }
    }
}
