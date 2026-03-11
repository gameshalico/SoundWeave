#nullable enable

using SoundWeave.Impl;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace SoundWeave.Editor
{
    internal static class ClipSourceDrawerHelper
    {
        private enum ClipSourceType
        {
            Direct,
#if SOUNDWEAVE_ADDRESSABLES_SUPPORT
            Addressable,
#endif
        }

        public static void AddClipSourceFields(VisualElement container, SerializedProperty clipSourceProperty)
        {
            var sourceContainer = new VisualElement();

            var currentType = DetectClipSourceType(clipSourceProperty);
            var dropdown = new EnumField("Clip Source", currentType);

            dropdown.RegisterValueChangedCallback(evt =>
            {
                if (evt.newValue is not ClipSourceType newType)
                    return;

                SwitchClipSource(clipSourceProperty, newType);
                sourceContainer.Clear();
                AddClipSourceFieldsForType(sourceContainer, clipSourceProperty, newType);
            });

            container.Add(dropdown);
            AddClipSourceFieldsForType(sourceContainer, clipSourceProperty, currentType);
            container.Add(sourceContainer);
        }

        private static ClipSourceType DetectClipSourceType(SerializedProperty clipSourceProperty)
        {
            var managedRef = clipSourceProperty.managedReferenceValue;
#if SOUNDWEAVE_ADDRESSABLES_SUPPORT
            if (managedRef is AddressableClipSource)
                return ClipSourceType.Addressable;
#endif
            return ClipSourceType.Direct;
        }

        private static void SwitchClipSource(SerializedProperty clipSourceProperty, ClipSourceType type)
        {
            IClipSource newSource = type switch
            {
#if SOUNDWEAVE_ADDRESSABLES_SUPPORT
                ClipSourceType.Addressable => new AddressableClipSource(),
#endif
                _ => new DirectClipSource(),
            };

            clipSourceProperty.managedReferenceValue = newSource;
            clipSourceProperty.serializedObject.ApplyModifiedProperties();
        }

        private static void AddClipSourceFieldsForType(
            VisualElement container, SerializedProperty clipSourceProperty, ClipSourceType type)
        {
            switch (type)
            {
                case ClipSourceType.Direct:
                {
                    var clipProperty = clipSourceProperty.FindPropertyRelative("_audioGenerator");
                    if (clipProperty != null)
                        container.Add(new PropertyField(clipProperty) { label = "Audio Resource" });
                    break;
                }
#if SOUNDWEAVE_ADDRESSABLES_SUPPORT
                case ClipSourceType.Addressable:
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
