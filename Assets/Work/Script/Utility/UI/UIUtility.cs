using System;
namespace Utility
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public static class UIUtility
    {
        public static Vector2 GetCanvasPosition(PositionType _type, RectTransform _rect, Vector2 _canvasSize)
        {

            float currentScreenWidth = Screen.width;
            float currentScreenHeight = Screen.height;

            float referenceWidth = Constants.REFERANCE_WIDTH;
            float referenceHeight = Constants.REFERANCE_HEIGHT;

            float scaleX = referenceWidth / currentScreenWidth;
            float scaleY = referenceHeight / currentScreenHeight;

            // Log.Logger.Log($"{scaleX} / {scaleY}");

            float canvasHeight = currentScreenHeight * scaleX;
            float canvasWidth = currentScreenWidth * scaleY;

            float rectHalfWidth = _rect.rect.width * 0.5f;
            float rectHalfHeight = _rect.rect.height * 0.5f;

            Vector2 _resolutionHalfSize = new Vector2(canvasWidth, canvasHeight) * 0.5f;
            Vector2 _calculatedSize = new Vector2(_resolutionHalfSize.x + rectHalfWidth, _resolutionHalfSize.y + rectHalfHeight);

            switch (_type)
            {
                case PositionType.Top:
                    return new Vector2(0, _calculatedSize.y);
                case PositionType.Bottom:
                    return new Vector2(0, -_calculatedSize.y);
                case PositionType.Right:
                    return new Vector2(_calculatedSize.x, 0);
                case PositionType.Left:
                    return new Vector2(-_calculatedSize.x, 0);
            }

            return default;
        }
    }
}
