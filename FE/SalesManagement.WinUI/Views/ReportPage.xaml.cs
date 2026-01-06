using Microsoft.Extensions.DependencyInjection;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SalesManagement.WinUI.ViewModels;
using System.Diagnostics;
using System.Numerics;
using Windows.UI;

namespace SalesManagement.WinUI.Views
{
    public sealed partial class ReportPage : Page
    {
        public ReportViewModel ViewModel { get; }

        // Cache màu theo ProductName để không bị nhấp nháy
        private readonly Dictionary<string, Color> _colorMap = new();
        private readonly Random _rand = new();

        public ReportPage()
        {
            InitializeComponent();

            ViewModel = App.Services.GetRequiredService<ReportViewModel>();
            DataContext = ViewModel;

            Loaded += ReportPage_Loaded;
        }

        private async void ReportPage_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.LoadReportAsync();
            ForceRedrawAllCharts();
        }

        private async void OnLoadClick(object sender, RoutedEventArgs e)
        {
            await ViewModel.LoadReportAsync();

            // ===== FIX: Đợi UI thread xử lý xong =====
            await Task.Delay(50);

            ForceRedrawAllCharts();
        }

        private void ForceRedrawAllCharts()
        {
            // ===== FIX: Force recreate và invalidate =====
            DispatcherQueue.TryEnqueue(Microsoft.UI.Dispatching.DispatcherQueuePriority.Normal, () =>
            {
                // Đổi visibility để force re-render
                if (ProductLineChart != null)
                {
                    ProductLineChart.Visibility = Visibility.Collapsed;
                    ProductLineChart.Visibility = Visibility.Visible;
                    ProductLineChart.Invalidate();
                }

                if (RevenueDayPie != null)
                {
                    RevenueDayPie.Visibility = Visibility.Collapsed;
                    RevenueDayPie.Visibility = Visibility.Visible;
                    RevenueDayPie.Invalidate();
                }

                if (RevenueMonthPie != null)
                {
                    RevenueMonthPie.Visibility = Visibility.Collapsed;
                    RevenueMonthPie.Visibility = Visibility.Visible;
                    RevenueMonthPie.Invalidate();
                }

                if (RevenueYearPie != null)
                {
                    RevenueYearPie.Visibility = Visibility.Collapsed;
                    RevenueYearPie.Visibility = Visibility.Visible;
                    RevenueYearPie.Invalidate();
                }
            });
        }

        private void ProductLineChart_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            try
            {
                var ds = args.DrawingSession;
                ds.Antialiasing = Microsoft.Graphics.Canvas.CanvasAntialiasing.Antialiased;

                // ===== CLEAR BACKGROUND =====
                ds.Clear(Colors.White);

                var data = ViewModel.ProductDaily;

                if (data == null || data.Count == 0)
                {
                    Debug.WriteLine("ProductLineChart_Draw: No data");
                    ds.DrawText(
                        "No data available for selected period",
                        new Vector2((float)sender.ActualWidth / 2 - 120, (float)sender.ActualHeight / 2),
                        Colors.Gray);
                    return;
                }

                Debug.WriteLine($"ProductLineChart_Draw: Drawing {data.Count} items");

                float margin = 60;

                // ===== Group data =====
                var products = data
                    .Select(p => p.ProductName)
                    .Distinct()
                    .OrderBy(p => p)
                    .ToList();

                var dates = data
                    .Select(p => p.Date.Date)
                    .Distinct()
                    .OrderBy(d => d)
                    .ToList();

                int productCount = products.Count;
                int dateCount = dates.Count;

                if (productCount == 0 || dateCount == 0)
                {
                    Debug.WriteLine("ProductLineChart_Draw: Invalid data structure");
                    return;
                }

                Debug.WriteLine($"Products: {productCount}, Dates: {dateCount}");

                // ===== LEGEND HEIGHT (DYNAMIC) =====
                float legendItemHeight = 18;
                float legendPaddingTop = 20;
                float legendPaddingBottom = 10;

                float legendHeight =
                    legendPaddingTop +
                    productCount * legendItemHeight +
                    legendPaddingBottom;

                float width = (float)sender.ActualWidth - margin * 2;
                float height = (float)sender.ActualHeight - margin * 2 - legendHeight;

                if (width <= 0 || height <= 0)
                {
                    Debug.WriteLine($"ProductLineChart_Draw: Invalid dimensions - width: {width}, height: {height}");
                    return;
                }

                // ===== Max Y =====
                int maxY = data.Max(p => p.QuantitySold);
                if (maxY <= 0) maxY = 1;

                Debug.WriteLine($"MaxY: {maxY}, Width: {width}, Height: {height}");

                // ===== Axes =====
                ds.DrawLine(margin, margin, margin, margin + height, Colors.Black, 2);
                ds.DrawLine(margin, margin + height, margin + width, margin + height, Colors.Black, 2);

                // ===== Y-axis labels =====

                // ===== X-axis labels =====
                float stepX = width / dateCount;

                for (int i = 0; i < dateCount; i++)
                {
                    float centerX = margin + i * stepX + stepX / 2;

                    ds.DrawText(
                        dates[i].ToString("dd/MM"),
                        new Vector2(centerX - 18, margin + height + 8),
                        Colors.Black);
                }

                // ===== Bar sizing =====
                float groupWidth = stepX * 0.8f;
                float barWidth = groupWidth / productCount;

                Debug.WriteLine($"StepX: {stepX}, GroupWidth: {groupWidth}, BarWidth: {barWidth}");

                // ===== Draw bars =====
                int barsDrawn = 0;
                for (int d = 0; d < dateCount; d++)
                {
                    float groupStartX = margin + d * stepX + (stepX - groupWidth) / 2;

                    for (int p = 0; p < productCount; p++)
                    {
                        string product = products[p];

                        if (!_colorMap.TryGetValue(product, out var color))
                        {
                            color = Color.FromArgb(
                                255,
                                (byte)_rand.Next(50, 230),
                                (byte)_rand.Next(50, 230),
                                (byte)_rand.Next(50, 230));

                            _colorMap[product] = color;
                        }

                        var item = data.FirstOrDefault(x =>
                            x.ProductName == product &&
                            x.Date.Date == dates[d]);

                        int value = item?.QuantitySold ?? 0;

                        if (value <= 0)
                            continue;

                        float barHeight = (value / (float)maxY) * height;

                        float x = groupStartX + p * barWidth;
                        float y = margin + height - barHeight;

                        ds.FillRectangle(x, y, barWidth - 2, barHeight, color);
                        barsDrawn++;
                    }
                }

                Debug.WriteLine($"Bars drawn: {barsDrawn}");

                // ===== LEGEND (BOTTOM – ONE COLUMN) =====
                float legendX = margin;
                float legendY = margin + height + legendPaddingTop + 30;

                var legendTextFormat = new Microsoft.Graphics.Canvas.Text.CanvasTextFormat
                {
                    FontSize = 12,
                    WordWrapping = Microsoft.Graphics.Canvas.Text.CanvasWordWrapping.NoWrap
                };

                foreach (var product in products)
                {
                    if (!_colorMap.TryGetValue(product, out var color))
                        continue;

                    ds.FillRectangle(legendX, legendY + 3, 10, 10, color);

                    ds.DrawText(
                        product,
                        new Vector2(legendX + 16, legendY),
                        Colors.Black,
                        legendTextFormat);

                    legendY += legendItemHeight;
                }

                Debug.WriteLine("ProductLineChart_Draw: Completed successfully");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ProductLineChart_Draw ERROR: {ex.Message}");
                Debug.WriteLine($"Stack trace: {ex.StackTrace}");
            }
        }

        private void DrawRevenuePie(
            CanvasControl sender,
            CanvasDrawEventArgs args,
            IList<SalesManagement.WinUI.Models.PieItem> data)
        {
            try
            {
                var ds = args.DrawingSession;
                ds.Antialiasing = Microsoft.Graphics.Canvas.CanvasAntialiasing.Antialiased;

                ds.Clear(Colors.White);

                if (data == null || data.Count == 0)
                {
                    ds.DrawText(
                        "No data",
                        new Vector2((float)sender.ActualWidth / 2 - 30, (float)sender.ActualHeight / 2),
                        Colors.Gray);
                    return;
                }

                float width = (float)sender.ActualWidth;
                float height = (float)sender.ActualHeight;

                float cx = width / 2;
                float cy = height / 2;
                float radius = Math.Min(cx, cy) - 40;

                double total = data.Sum(x => x.Value);
                if (total <= 0)
                    return;

                float startAngle = -90f;

                foreach (var item in data)
                {
                    if (!_colorMap.TryGetValue(item.Label, out var color))
                    {
                        color = Color.FromArgb(
                            255,
                            (byte)_rand.Next(60, 220),
                            (byte)_rand.Next(60, 220),
                            (byte)_rand.Next(60, 220));

                        _colorMap[item.Label] = color;
                    }

                    float sweepAngle = (float)(item.Value / total * 360f);

                    using var pathBuilder = new Microsoft.Graphics.Canvas.Geometry.CanvasPathBuilder(ds);
                    pathBuilder.BeginFigure(cx, cy);

                    pathBuilder.AddArc(
                        new Vector2(cx, cy),
                        radius,
                        radius,
                        startAngle * MathF.PI / 180f,
                        sweepAngle * MathF.PI / 180f);

                    pathBuilder.EndFigure(Microsoft.Graphics.Canvas.Geometry.CanvasFigureLoop.Closed);

                    using var geometry = Microsoft.Graphics.Canvas.Geometry.CanvasGeometry.CreatePath(pathBuilder);

                    ds.FillGeometry(geometry, color);

                    startAngle += sweepAngle;
                }

                // ===== LEGEND (RIGHT SIDE) =====
                float legendX = cx + radius + 20;
                float legendY = cy - (data.Count * 18) / 2f; // canh giữa theo chiều dọc

                float legendItemHeight = 18;

                var legendTextFormat = new Microsoft.Graphics.Canvas.Text.CanvasTextFormat
                {
                    FontSize = 12,
                    WordWrapping = Microsoft.Graphics.Canvas.Text.CanvasWordWrapping.NoWrap
                };

                foreach (var item in data)
                {
                    if (!_colorMap.TryGetValue(item.Label, out var color))
                        continue;

                    // Ô màu
                    ds.FillRectangle(legendX, legendY + 4, 12, 12, color);

                    // Text legend
                    string text = $"{item.Label} ({item.Value:N0})";
                    ds.DrawText(
                        text,
                        new Vector2(legendX + 18, legendY),
                        Colors.Black,
                        legendTextFormat);

                    legendY += legendItemHeight;
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"DrawRevenuePie ERROR: {ex.Message}");
            }
        }

        private void RevenueDayPie_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            DrawRevenuePie(sender, args, ViewModel.RevenueDayPie);
        }

        private void RevenueMonthPie_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            DrawRevenuePie(sender, args, ViewModel.RevenueMonthPie);
        }

        private void RevenueYearPie_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            DrawRevenuePie(sender, args, ViewModel.RevenueYearPie);
        }
    }
}