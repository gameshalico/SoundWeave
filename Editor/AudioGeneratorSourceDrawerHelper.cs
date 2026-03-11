#nullable enable

using SoundWeave.Impl;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace SoundWeave.Editor
{
    internal static class AudioGeneratorSourceDrawerHelper
    {
        private enum AudioGeneratorSourceType
        {
            Direct,
#if SOUNDWEAVE_ADDRESSABLES_SUPPORT
            Addressable,
#endif
        }

        public static void AddGeneratorSourceFields(VisualElement container, SerializedProperty clipSourceProperty)
        {
            var sourceContainer = new VisualElement();

            var currentType = DetectAudioGeneratorSourceType(clipSourceProperty);
            var dropdown = new EnumField("Clip Source", currentType);

            dropdown.RegisterValueChangedCallback(evt =>
            {
                if (evt.newValue is not AudioGeneratorSourceType newType)
                    return;

                SwitchGeneratorSource(clipSourceProperty, newType);
                sourceContainer.Clear();
                AddClipSourceFieldsForType(sourceContainer, clipSourceProperty, newType);
            });

            container.Add(dropdown);
            AddClipSourceFieldsForType(sourceContainer, clipSourceProperty, currentType);
            container.Add(sourceContainer);
        }

        private static AudioGeneratorSourceType DetectAudioGeneratorSourceType(SerializedProperty clipSourceProperty)
        {
            var managedRef = clipSourceProperty.managedReferenceValue;
#if SOUNDWEAVE_ADDRESSABLES_SUPPORT
            if (managedRef is AddressableAudioGeneratorSource)
                return AudioGeneratorSourceType.Addressable;
#endif
            return AudioGeneratorSourceType.Direct;
        }

        private static void SwitchGeneratorSource(SerializedProperty clipSourceProperty, AudioGeneratorSourceType type)
        {
            IAudioGeneratorSource newSource = type switch
            {
#if SOUNDWEAVE_ADDRESSABLES_SUPPORT
                AudioGeneratorSourceType.Addressable => new AddressableAudioGeneratorSource(),
#endif
                _ => new DirectAudioGeneratorSource(),
            };

            clipSourceProperty.managedReferenceValue = newSource;
            clipSourceProperty.serializedObject.ApplyModifiedProperties();
        }

        private static void AddClipSourceFieldsForType(
            VisualElement container, SerializedProperty clipSourceProperty, AudioGeneratorSourceType type)
        {
            switch (type)
            {
                case AudioGeneratorSourceType.Direct:
                    {
                        var clipProperty = clipSourceProperty.FindPropertyRelative("_audioGenerator");
                        if (clipProperty != null)
                            container.Add(new PropertyField(clipProperty) { label = "Audio Resource" });
                        break;
                    }
#if SOUNDWEAVE_ADDRESSABLES_SUPPORT
                case AudioGeneratorSourceType.Addressable:
                    {
                        var refProperty = clipSourceProperty.FindPropertyRelative("_clipReference");
                        if (refProperty != null)
                            container.Add(new PropertyField(refProperty) { label = "Audio Resource (Addressable)" });
                        break;
                    }
#endif
            }
        }
    }
}
