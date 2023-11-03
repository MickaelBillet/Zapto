using Android.Graphics;
using Android.Views;
using Connect.Mobile.Droid.Renderers;
using Connect.Mobile.View.Controls;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ImageCircle), typeof(ImageCircleRenderer))]

namespace Connect.Mobile.Droid.Renderers
{
    class ImageCircleRenderer : ImageRenderer
    {
        public ImageCircleRenderer(Android.Content.Context context) : base(context)
		{
            AutoPackage = false;
        }

        protected override bool DrawChild(Canvas canvas, global::Android.Views.View child, long drawingTime)
        {
            try
            {
                int radius = Math.Min(Width, Height) / 2;
                int strokeWidth = 10;
                radius -= strokeWidth / 2;

                //Create path to clip
                Path path = new Path();
                path.AddCircle(Width / 2, Height / 2, radius +10, Path.Direction.Ccw);
                canvas.Save();
                canvas.ClipPath(path);

                bool result = base.DrawChild(canvas, child, drawingTime);

                canvas.Restore();

                // Create path for circle border
                path = new Path();
                path.AddCircle((Width) / 2, (Height)/ 2, radius + 10 , Path.Direction.Ccw);

                Paint paint = new Paint();
                paint.AntiAlias = true;
                paint.StrokeWidth = 5;
                paint.SetStyle(Paint.Style.Stroke);
                paint.Color = global::Android.Graphics.Color.White;

                canvas.DrawPath(path, paint);

                //Properly dispose
                paint.Dispose();
                path.Dispose();
                return result;
            }
            catch (Exception)
            {
            }

            return base.DrawChild(canvas, child, drawingTime);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement == null)
            {
                if ((int)Android.OS.Build.VERSION.SdkInt < 18)
                    SetLayerType(LayerType.Software, null);
            }
        }
    }
}