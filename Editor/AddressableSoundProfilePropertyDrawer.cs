#nullable enable
#if SOUNDWEAVE_ADDRESSABLES_SUPPORT

using SoundWeave.Impl;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace SoundWeave.Editor
{
    [CustomPropertyDrawer(typeof(AddressableSoundProfile))]
    public sealed class AddressableSoundProfilePropertyDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var container = new VisualElement();
            var foldout = new Foldout { text = property.displayName };

            var clipReferenceProperty = property.FindPropertyRelative("_clipReference");
            var outputAudioMixerGroupProperty = property.FindPropertyRelative("_outputAudioMixerGroup");
            var muteProperty = property.FindPropertyRelative("_mute");

            container.Add(new PropertyField(clipReferenceProperty) { label = "AudioClip (Addressable)" });
            container.Add(new PropertyField(outputAudioMixerGroupProperty) { label = "Output" });
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
#endif
