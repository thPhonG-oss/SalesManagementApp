using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using SalesManagement.WinUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SalesManagement.WinUI.Views
{
    public sealed partial class RevenueChart : UserControl
    {
        private List<DailyRevenueResponse> _dataSource;

        public RevenueChart()
        {
            this.InitializeComponent();
            this.Loaded += RevenueChart_Loaded;
        }

        public static readonly DependencyProperty DataSourceProperty =
            DependencyProperty.Register(
                nameof(DataSource),
                typeof(List<DailyRevenueResponse>),
                typeof(RevenueChart),
                new PropertyMetadata(null, OnDataSourceChanged));

        public List<DailyRevenueResponse> DataSource
        {
            get => (List<DailyRevenueResponse>)GetValue(DataSourceProperty);
            set => SetValue(DataSourceProperty, value);
        }

        private static void OnDataSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is RevenueChart chart)
            {
                chart._dataSource = (List<DailyRevenueResponse>)e.NewValue;
                chart.DrawChart();
            }
        }

        private void RevenueChart_Loaded(object sender, RoutedEventArgs e)
        {
            DrawChart();
        }

        private void DrawChart()
        {
            if (_dataSource == null || _dataSource.Count == 0)
            {
                DrawEmptyChart();
                return;
            }

            DrawingCanvas.Children.Clear();

            var width = DrawingCanvas.ActualWidth > 0 ? DrawingCanvas.ActualWidth : 800;
            var height = DrawingCanvas.ActualHeight > 0 ? DrawingCanvas.ActualHeight : 350;

            // ?? v?n gi? 30 ngày g?n nh?t
            var data = _dataSource
                .OrderBy(d => d.Date)
                .TakeLast(30)
                .ToList();

            var maxRevenue = data.Max(d => d.Revenue);
            if (maxRevenue == 0)
            {
                DrawEmptyChart();
                return;
            }

            var padding = 60;
            var chartWidth = width - (padding * 2);
            var chartHeight = height - padding - 40;
            var barSpacing = chartWidth / (data.Count + 1);
            var barWidth = Math.Min(barSpacing * 0.7, 25);

            // Y-axis
            DrawingCanvas.Children.Add(new Line
            {
                X1 = padding,
                Y1 = padding,
                X2 = padding,
                Y2 = height - 40,
                Stroke = new SolidColorBrush(Windows.UI.Color.FromArgb(100, 200, 200, 200)),
                StrokeThickness = 2
            });

            // X-axis
            DrawingCanvas.Children.Add(new Line
            {
                X1 = padding,
                Y1 = height - 40,
                X2 = width - padding,
                Y2 = height - 40,
                Stroke = new SolidColorBrush(Windows.UI.Color.FromArgb(100, 200, 200, 200)),
                StrokeThickness = 2
            });

            DrawYAxisLabels(padding, height, maxRevenue, chartHeight);

            // Bars + labels
            for (int i = 0; i < data.Count; i++)
            {
                var item = data[i];

                var barHeight = (item.Revenue / maxRevenue) * chartHeight;
                var barX = padding + (barSpacing * (i + 0.5)) - (barWidth / 2);
                var barY = (height - 40) - barHeight;

                // Bar
                var rect = new Rectangle
                {
                    Width = barWidth,
                    Height = barHeight,
                    Fill = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 120, 212)),
                    RadiusX = 4,
                    RadiusY = 4
                };
                Canvas.SetLeft(rect, barX);
                Canvas.SetTop(rect, barY);
                DrawingCanvas.Children.Add(rect);

                // Value on bar (gi? nh? c?)
                if (i % Math.Max(1, data.Count / 10) == 0)
                {
                    var valueText = new TextBlock
                    {
                        Text = $"${item.Revenue:F0}",
                        FontSize = 10,
                        Foreground = new SolidColorBrush(
                            Windows.UI.Color.FromArgb(200, 0, 0, 0)),
                        TextAlignment = TextAlignment.Center
                    };

                    Canvas.SetLeft(valueText, barX - 15);
                    Canvas.SetTop(valueText, barY - 20);
                    DrawingCanvas.Children.Add(valueText);
                }

                // ? DATE LABEL – HI?N ?? T?T C? NGÀY (XOAY 45 ??)
                var dateBlock = new TextBlock
                {
                    Text = FormatDate(item.Date),
                    FontSize = 9,
                    Foreground = new SolidColorBrush(
                        Windows.UI.Color.FromArgb(150, 100, 100, 100)),
                    RenderTransform = new RotateTransform
                    {
                        Angle = -45
                    },
                    RenderTransformOrigin = new Windows.Foundation.Point(0, 0.5)
                };

                Canvas.SetLeft(dateBlock, barX - 5);
                Canvas.SetTop(dateBlock, height - 20);
                DrawingCanvas.Children.Add(dateBlock);
            }
        }

        private void DrawYAxisLabels(double padding, double height, double maxRevenue, double chartHeight)
        {
            var steps = 5;
            for (int i = 0; i <= steps; i++)
            {
                var value = maxRevenue * i / steps;
                var yPos = (height - 40) - (chartHeight * i / steps);

                DrawingCanvas.Children.Add(new Line
                {
                    X1 = padding,
                    Y1 = yPos,
                    X2 = DrawingCanvas.ActualWidth - padding,
                    Y2 = yPos,
                    Stroke = new SolidColorBrush(
                        Windows.UI.Color.FromArgb(30, 200, 200, 200)),
                    StrokeThickness = 1
                });

                var label = new TextBlock
                {
                    Text = $"${value:F0}",
                    FontSize = 10,
                    Foreground = new SolidColorBrush(
                        Windows.UI.Color.FromArgb(150, 100, 100, 100))
                };

                Canvas.SetLeft(label, 5);
                Canvas.SetTop(label, yPos - 8);
                DrawingCanvas.Children.Add(label);
            }
        }

        private void DrawEmptyChart()
        {
            DrawingCanvas.Children.Clear();

            var textBlock = new TextBlock
            {
                Text = "Ch?a có d? li?u doanh thu",
                FontSize = 16,
                Foreground = new SolidColorBrush(
                    Windows.UI.Color.FromArgb(150, 150, 150, 150)),
                TextAlignment = TextAlignment.Center
            };

            Canvas.SetLeft(textBlock, DrawingCanvas.ActualWidth / 2 - 100);
            Canvas.SetTop(textBlock, DrawingCanvas.ActualHeight / 2 - 20);
            DrawingCanvas.Children.Add(textBlock);
        }

        private string FormatDate(string dateString)
        {
            if (DateTime.TryParse(dateString, out var date))
            {
                return date.ToString("MM-dd");
            }
            return dateString;
        }
    }
}
