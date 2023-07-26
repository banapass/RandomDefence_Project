namespace Utility
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UIElements;

    public static class Utility
    {
        public static float GetCameraOrthSize(float _height, float _pixelPerUnit)
        {
            return _height * (1 / _pixelPerUnit) * 0.5f;
        }
        public static Vector2 GetCenterPosition(this VisualElement value)
        {
            Vector2 _position = value.worldTransform.GetPosition();
            Vector2 _halfSize = value.layout.size * 0.5f;

            return _position + _halfSize;
        }
        public static Vector2 GetLocalPosition(this VisualElement value)
        {
            return new Vector2(value.layout.width * 0.5f, value.layout.height * 0.5f);
        }
        public static void Translate(this VisualElement value, Vector2 _pos)
        {
            value.style.left = _pos.x;
            value.style.top = _pos.y;
        }

        public static float CalculateScaleFromResolution(float referenceResolutionWidth, float referenceResolutionHeight)
        {
            float screenWidth = Screen.width;
            float screenHeight = Screen.height;

            float aspectRatio = screenWidth / screenHeight;
            float referenceAspectRatio = referenceResolutionWidth / referenceResolutionHeight;

            float scale = aspectRatio / referenceAspectRatio;

            return scale;
        }

        public static T CloneVisualElement<T>(this VisualElement value) where T : VisualElement, new()
        {
            T _result = new T();
            var _classes = value.GetClasses();

            if (_classes != null)
            {
                foreach (var _className in _classes)
                    _result.AddToClassList(_className);
            }

            return _result;
        }
    }

}