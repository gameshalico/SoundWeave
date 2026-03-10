#nullable enable

using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace SoundWeave.Editor
{
    internal static class SoundProfileDrawerHelper
    {
        public static void AddSlider(VisualElement container, SerializedProperty property,
            string label, float min, float max)
        {
            var slider = new Slider(min, max)
            {
                showInputField = true,
                label = label
            };
            slider.BindProperty(property);
            container.Add(slider);
        }

        public static void AddSliderFields(VisualElement container, SerializedProperty property)
        {
            var volumeProperty = property.FindPropertyRelative("_volume");
            var pitchProperty = property.FindPropertyRelative("_pitch");
            var priorityProperty = property.FindPropertyRelative("_priority");
            var panStereoProperty = property.FindPropertyRelative("_panStereo");

            var prioritySlider = new SliderInt(0, 256)
            {
                showInputField = true,
                label = "Priority"
            };
            prioritySlider.BindProperty(priorityProperty);
            container.Add(prioritySlider);

            AddSlider(container, volumeProperty, "Volume", 0, 1);
            AddSlider(container, pitchProperty, "Pitch", -3, 3);
            AddSlider(container, panStereoProperty, "Stereo Pan", -1, 1);
        }

        public static void AddSamplesFields(VisualElement container, SerializedProperty property)
        {
            var startSampleProperty = property.FindPropertyRelative("_startSample");
            var loopProperty = property.FindPropertyRelative("_loop");

            container.Add(new PropertyField(loopProperty));
            container.Add(new PropertyField(startSampleProperty));
        }

        public static void AddTimingFields(VisualElement container, SerializedProperty property)
        {
            var delayProperty = property.FindPropertyRelative("_delay");
            container.Add(new PropertyField(delayProperty));
        }
    }
}
