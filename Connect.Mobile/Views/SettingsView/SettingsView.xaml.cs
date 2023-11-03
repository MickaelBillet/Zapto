using Connect.Mobile.View.Controls;
using Connect.Mobile.ViewModel;
using Connect.Model;
using Framework.Core.Base;
using Microsoft.Extensions.DependencyInjection;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;


namespace Connect.Mobile.View
{
    public partial class SettingsView : BasePage
    {
        private bool isItemVisible = true;

        #region Property

        private SettingsViewModel ViewModel { get; }

        public bool IsItemVisible
        {
            get { return isItemVisible; }
            set
            {
                isItemVisible = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Constructor 

        public SettingsView()
        {
            InitializeComponent();

            this.BindingContext = this.ViewModel = Host.Current.GetService<SettingsViewModel>();

            this.DrawGrid();           
        }

        #endregion

        #region Method

        /// <summary>
        /// Draws the grid.
        /// </summary>
        private void DrawGrid()
        {
            //Create the times list
            DayTime.Times = new List<String>()
                {
                    "0:00", "0:30", "1:00", "1:30", "2:00", "2:30", "3:00", "3:30", "4:00", "4:30", "5:00", "5:30", "6:00", "6:30", "7:00", "7:30", "8:00",
                    "8:30", "9:00", "9:30", "10:00", "10:30", "11:00", "11:30", "12:00", "12:30", "13:00", "13:30", "14:00", "14:30", "15:00", "15:30", "16:00",
                    "16:30", "17:00", "17:30", "18:00", "18:30", "19:00", "19:30", "20:00", "20:30",  "21:00", "21:30", "22:00", "22:30", "23:00", "23:30"
                };

            //Create the days list
            DayTime.Days = new List<DayOfWeek>(7);

            for (int i = 0; i < 7; i++)
            {
                DayTime.Days.Add((DayOfWeek)((i + 1) % 7));
            }

            for (int day = 0; day < DayTime.Days.Count; day++)
            {
                //Display the days
                DayGrid.Children.Add(new Label()
                {
                    Text = DayTime.Days[day].ToString().Substring(0, 3) + ".",
                    Style = AppStyles.DaysLabelStyle,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    VerticalOptions = LayoutOptions.StartAndExpand
                }, day, 0);
            }

            //Create the rows for TimeGrid
            Enumerable.Range(1, DayTime.Times.Count).ToList().ForEach(x =>
                 TimeGrid.RowDefinitions.Add(new RowDefinition
                 {
                 }));

            for (int time = 0; time < DayTime.Times.Count; time++)
            {
                //Display the hours
                if (time % 2 == 0)
                {
                    Label label = new Label()
                    {
                        Text = DayTime.Times[time],
                        Style = AppStyles.HoursLabelStyle,
                        VerticalOptions = LayoutOptions.StartAndExpand,
                        VerticalTextAlignment = TextAlignment.Start,
                    };

                    TimeGrid.Children.Add(label, 0, time);

                    Grid.SetRowSpan(label, 2);
                }
            }
        }

        /// <summary>
        /// Programmings the button clicked.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        private void ProgrammingButtonClicked(object sender, EventArgs e)
        {
            VisualStateManager.GoToState(this.ManualState, "Normal");
            VisualStateManager.GoToState(this.ProgrammingState, "Selected");

            VisualStateManager.GoToState(this.ManualLayout, "Hidden");
            VisualStateManager.GoToState(this.ProgrammingLayout, "Visible");

            this.IsItemVisible = true;
        }

        /// <summary>
        /// Manuals the button clicked.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        private void ManualButtonClicked(object sender, EventArgs e)
        {
            VisualStateManager.GoToState(this.ManualState, "Selected");
            VisualStateManager.GoToState(this.ProgrammingState, "Normal");

            VisualStateManager.GoToState(this.ManualLayout, "Visible");
            VisualStateManager.GoToState(this.ProgrammingLayout, "Hidden");

            this.IsItemVisible = false;
        }

        /// <summary>
        /// Ons the hsv canvas view paint surface.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="args">Arguments.</param>
        public void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            this.OperationRangesCanvasView.Draw(args);
        }

        /// <summary>
        /// Ons the touch effect action.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="args">Arguments.</param>
        void OnTouchEffectAction(object sender, TouchActionEventArgs args)
        {
            Point pt = args.Location;

            SKPoint point = new SKPoint((float)(this.OperationRangesCanvasView.CanvasSize.Width * pt.X / this.OperationRangesCanvasView.Width),
                                        (float)(this.OperationRangesCanvasView.CanvasSize.Height * pt.Y / this.OperationRangesCanvasView.Height));

            switch (args.Type)
            {
                case TouchActionType.Pressed:
                    this.OperationRangesCanvasView.ProcessTouchEvent(point);
                    break;

                case TouchActionType.Moved:

                    break;

                case TouchActionType.Released:

                    break;

                case TouchActionType.Cancelled:

                    break;
            }
        }

        protected override async void OnDisappearing()
        {
            base.OnDisappearing();

            IPageLifeCylceEvents lifecycleHandler = (IPageLifeCylceEvents)this.BindingContext;
            await lifecycleHandler.OnDisappearing();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            VisualStateManager.GoToState(this.ManualState, "Normal");
            VisualStateManager.GoToState(this.ProgrammingState, "Selected");

            VisualStateManager.GoToState(this.ManualLayout, "Hidden");
            VisualStateManager.GoToState(this.ProgrammingLayout, "Visible");
        }

        #endregion
    }
}