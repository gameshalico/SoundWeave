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
            var container = new VisualElement();
            var foldout = new Foldout { text = property.displayName };

            var clipProperty = property.FindPropertyRelative("_clip");
            var outputAudioMixerGroupProperty = property.FindPropertyRelative("_outputAudioMixerGroup");
            var muteProperty = property.FindPropertyRelative("_mute");
            var startSampleProperty = property.FindPropertyRelative("_startSample");

            container.Add(new PropertyField(clipProperty) { label = "AudioClip" });
            container.Add(new PropertyField(outputAudioMixerGroupProperty) { label = "Output" });
            container.Add(new PropertyField(muteProperty));

            SoundProfileDrawerHelper.AddSliderFields(container, property);
            SoundProfileDrawerHelper.AddSamplesFields(container, property);
            SoundProfileDrawerHelper.AddTimingFields(container, property);

            container.Add(new Button(() => DetectSamples(clipProperty, startSampleProperty))
            {
                text = "Auto Detect Start Sample"
            });

            foldout.Add(container);
            return foldout;
        }

        private static void DetectSamples(SerializedProperty clipProperty, SerializedProperty startSampleProperty)
        {
            var audioClip = clipProperty.objectReferenceValue as AudioClip;
            if (audioClip == null)
            {
                Debug.LogError("No audio clip assigned.");
                return;
            }

            var samples = AudioClipUtility.DetectSilenceSamples(audioClip);

            Undo.RecordObject(clipProperty.serializedObject.targetObject, "Detect Samples");
            startSampleProperty.intValue = samples.startSample;
            startSampleProperty.serializedObject.ApplyModifiedProperties();
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.PropertyField(position, property, label, true);
            EditorGUI.EndProperty();
        }
    }
}
