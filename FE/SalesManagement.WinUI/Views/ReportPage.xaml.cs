using Microsoft.Extensions.DependencyInjection;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SalesManagement.WinUI.ViewModels;
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
            ProductLineChart?.Invalidate();
        }

        private async void OnLoadClick(object sender, RoutedEventArgs e)
        {
            await ViewModel.LoadReportAsync();
            ProductLineChart?.Invalidate();
        }

        private void ProductLineChart_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            var ds = args.DrawingSession;
            ds.Antialiasing = Microsoft.Graphics.Canvas.CanvasAntialiasing.Antialiased;

            var data = ViewModel.ProductDaily;
            if (!data.Any())
                return;

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
                return;

            // ===== Max Y =====
            int maxY = data.Max(p => p.QuantitySold);
            if (maxY <= 0) maxY = 1;

            // ===== Axes =====
            ds.DrawLine(margin, margin, margin, margin + height, Colors.Black, 2);
            ds.DrawLine(margin, margin + height, margin + width, margin + height, Colors.Black, 2);

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

            // ===== Draw bars =====
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

                    float barHeight = (value / (float)maxY) * height;

                    float x = groupStartX + p * barWidth;
                    float y = margin + height - barHeight;

                    ds.FillRectangle(x, y, barWidth - 2, barHeight, color);
                }
            }

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
        }
    }
}
