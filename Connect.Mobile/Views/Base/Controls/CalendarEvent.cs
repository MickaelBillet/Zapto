using SkiaSharp;

namespace Connect.Mobile.View.Controls
{
    public class CalendarEvent
    {
        #region Property

        private SKRoundRect SKRoundRect = null;

        private static readonly int OffsetX = -95;

        private static readonly int OffsetY = -50;

        public string OperationRangeId 
        {
            get; set;
        }

        #endregion

        #region Constructor

        public CalendarEvent(SKRoundRect rect, string id)
        {
            this.SKRoundRect = rect;
            this.OperationRangeId = id;
        }

        #endregion

        #region Method

        /// <summary>
        /// Paint the specified canvas and color.
        /// </summary>
        /// <param name="canvas">Canvas.</param>
        /// <param name="color">Color.</param>
        public void Paint(SKCanvas canvas, SKColor color)
        {
            SKPaint paint = new SKPaint
            {
                Style = SKPaintStyle.StrokeAndFill,
                StrokeWidth = 6,
                FakeBoldText = true,
                Color = color,
            };

            canvas.DrawRoundRect(SKRoundRect, paint);
        }

        /// <summary>
        /// Hits the test.
        /// </summary>
        /// <returns><c>true</c>, if test was hit, <c>false</c> otherwise.</returns>
        /// <param name="location">Location.</param>
        public bool HitTest(SKPoint location)
        {
            location.Offset(CalendarEvent.OffsetX, CalendarEvent.OffsetY);

            // Check if it's in the untransformed bitmap rectangle
            SKRect rect = new SKRect(SKRoundRect.Rect.Left-20, SKRoundRect.Rect.Top, SKRoundRect.Rect.Right+20, SKRoundRect.Rect.Bottom);

            return rect.Contains(location);
        }

        #endregion
    }
}
