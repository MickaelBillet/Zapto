using Connect.Mobile.ViewModel;
using Connect.Model;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Xamarin.Forms;

namespace Connect.Mobile.View.Controls
{
    public class OperationRangesCanvasView : SKCanvasView
    {
        #region Property

        public static readonly BindableProperty OperationRangesProperty = BindableProperty.Create(nameof(OperationRanges), typeof(ObservableCollection<OperationRange>), typeof(OperationRangesCanvasView), null,
            propertyChanged: (bindable, oldValue, newValue) =>
            {
                if (newValue != null)
                {
                    (newValue as ObservableCollection<OperationRange>).CollectionChanged += (coll, arg) =>
                    {
                        SKCanvasView view = (bindable as SKCanvasView);

                        switch (arg.Action)
                        {
                            case NotifyCollectionChangedAction.Add:
                                view.InvalidateSurface();
                                break;

                            case NotifyCollectionChangedAction.Remove:
                                view.InvalidateSurface();

                                break;

                            case NotifyCollectionChangedAction.Move:
                            //Do your stuff
                            break;

                            case NotifyCollectionChangedAction.Replace:
                                view.InvalidateSurface();
                                break;

                            case NotifyCollectionChangedAction.Reset:
                                view.InvalidateSurface();
                                break;
                        }
                    };
                }
            });

        public ObservableCollection<OperationRange> OperationRanges
        {
            get { return (ObservableCollection<OperationRange>)GetValue(OperationRangesProperty); }
            set { SetValue(OperationRangesProperty, value); }
        }

        private List<CalendarEvent> CalendarEvents
        {
            get; set;
        }

        #endregion

        #region Constructor

        public OperationRangesCanvasView()
        {
            this.CalendarEvents = new List<CalendarEvent>();
        }

        #endregion

        #region Method

        /// <summary>
        /// Draw the specified args.
        /// </summary>
        /// <param name="args">Arguments.</param>
        public void Draw(SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            this.CalendarEvents.Clear();

            int width = info.Width;

            if (this.OperationRanges != null)
            {
                foreach (OperationRange operationRange in this.OperationRanges)
                {
                    float left = DayTime.Convert(operationRange.Day) * (width / ConnectConstants.DayPerWeek) + ((width / ConnectConstants.DayPerWeek) * 0.15f);
                    float right = DayTime.Convert(operationRange.Day) * (width / ConnectConstants.DayPerWeek) + ((width / ConnectConstants.DayPerWeek) * 0.85f);
                    float top = ((float)operationRange.StartTime.TotalMinutes * info.Height) / ConnectConstants.MinutesPerDay;
                    float bottom = ((float)operationRange.EndTime.TotalMinutes * info.Height) / ConnectConstants.MinutesPerDay;

                    SKRect rect = new SKRect(left, top + 5, right, bottom + 5);
                    SKRoundRect roundRect = new SKRoundRect(rect, 20, 20);

                    CalendarEvent evt = new CalendarEvent(roundRect, operationRange.Id);

                    Color color = (Color)Xamarin.Forms.Application.Current.Resources["StandardPlug"];

                    SKColor col = color.ToSKColor();

                    if (operationRange.Condition != null)
                    {
                       col  = this.GetColor(operationRange.Condition);
                    }

                    evt.Paint(canvas, col);

                    this.CalendarEvents.Add(evt);
                }
            }
        }

        /// <summary>
        /// Gets the color.
        /// </summary>
        /// <returns>The color.</returns>
        /// <param name="conditionType">Condition type.</param>
        private SKColor GetColor(Model.Condition condition)
        {
            Color color = (Color)Xamarin.Forms.Application.Current.Resources["StandardPlug"];

            if ((condition.HumidityOrder != null) && (condition.TemperatureOrder == null))
            {
                color = (Color)Xamarin.Forms.Application.Current.Resources["HumidityPlug"];
            }
            else if ((condition.HumidityOrder == null) && (condition.TemperatureOrder != null))
            {
                color = (Color)Xamarin.Forms.Application.Current.Resources["TemperaturePlug"];
            }
            else if ((condition.HumidityOrder != null) && (condition.TemperatureOrder != null))
            {
                color = (Color)Xamarin.Forms.Application.Current.Resources["HumidityTemperaturePlug"];
            }

            return color.ToSKColor();
        }

        /// <summary>
        /// Processes the touch event.
        /// </summary>
        /// <param name="point">Point.</param>
        public void ProcessTouchEvent(SKPoint point)
        {
            foreach(CalendarEvent evt in this.CalendarEvents)
            {
                if (evt.HitTest(point))
                {
                    SettingsViewModel viewModel = (this.BindingContext as SettingsViewModel);

                    if (viewModel.EditOpRangeCommand.CanExecute(null))
                    {
                        viewModel.EditOpRangeCommand.Execute(this.OperationRanges.FirstOrDefault<OperationRange>((arg) => arg.Id == evt.OperationRangeId));
                    }

                    break;
                }
            }
        }
        #endregion
    }
}

