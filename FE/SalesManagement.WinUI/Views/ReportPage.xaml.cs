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

                // Clear background
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

                float marginLeft = 60;
                float marginRight = 40;
                float marginTop = 60; // Tăng lên để có chỗ cho label trục Y ở trên

                // Group data
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

                // Tính toán marginBottom cho legend (dạng danh sách bullet)
                float legendItemHeight = 25;
                float marginBottom = 80 + (productCount * legendItemHeight);

                // Tính chiều cao cần thiết cho canvas
                float requiredHeight = 400 + marginTop + marginBottom;

                // Tự động điều chỉnh chiều cao canvas nếu cần
                if (sender.Height < requiredHeight)
                {
                    sender.Height = requiredHeight;
                    return; // Return để trigger redraw với chiều cao mới
                }

                float canvasWidth = (float)sender.ActualWidth;
                float width = canvasWidth - marginLeft - marginRight;
                float height = (float)sender.ActualHeight - marginTop - marginBottom;

                if (width <= 0 || height <= 0)
                {
                    Debug.WriteLine($"ProductLineChart_Draw: Invalid dimensions - width: {width}, height: {height}");
                    return;
                }

                // Tính Max Y (QuantitySold)
                int maxY = data.Max(p => p.QuantitySold);
                if (maxY <= 0) maxY = 10;

                // Làm tròn maxY lên
                int yAxisMax = maxY + 1; // Thêm 1 để có khoảng trống ở trên

                Debug.WriteLine($"MaxY: {maxY}, YAxisMax: {yAxisMax}");

                // Tọa độ gốc (origin)
                float originX = marginLeft;
                float originY = marginTop + height;

                // Vẽ trục X và Y
                ds.DrawLine(originX, originY, originX, marginTop, Colors.Black, 2); // Trục Y
                ds.DrawLine(originX, originY, originX + width, originY, Colors.Black, 2); // Trục X

                // Vẽ grid lines và Y-axis labels (QuantitySold - mỗi khoảng 1 đơn vị)
                for (int i = 0; i <= yAxisMax; i++)
                {
                    float y = originY - (height / yAxisMax) * i;

                    // Grid line ngang
                    ds.DrawLine(originX, y, originX + width, y, Colors.LightGray, 1);

                    // Y label (QuantitySold)
                    ds.DrawText(
                        i.ToString(),
                        new Vector2(originX - 30, y - 8),
                        Colors.Black,
                        new Microsoft.Graphics.Canvas.Text.CanvasTextFormat { FontSize = 12 });
                }

                // Nhãn trục Y - đặt ở trên cùng
                ds.DrawText(
                    "Số lượng bán",
                    new Vector2(originX - 50, marginTop - 30),
                    Colors.Black,
                    new Microsoft.Graphics.Canvas.Text.CanvasTextFormat
                    {
                        FontSize = 13,
                    });

                // Vẽ X-axis labels (Dates - dd/MM/yyyy)
                float stepX = dateCount > 1 ? width / (dateCount - 1) : width / 2;

                for (int i = 0; i < dateCount; i++)
                {
                    float x = originX + i * stepX;

                    // Vertical grid line
                    if (i > 0)
                    {
                        ds.DrawLine(x, marginTop, x, originY, Colors.LightGray, 1);
                    }

                    // X label (Date)
                    string dateLabel = dates[i].ToString("dd/MM/yyyy");
                    var textFormat = new Microsoft.Graphics.Canvas.Text.CanvasTextFormat
                    {
                        FontSize = 11
                    };

                    ds.DrawText(
                        dateLabel,
                        new Vector2(x - 30, originY + 8),
                        Colors.Black,
                        textFormat);
                }

                // Nhãn trục X
                ds.DrawText(
                    "Ngày",
                    new Vector2(originX + width / 2 - 15, originY + 32),
                    Colors.Black,
                    new Microsoft.Graphics.Canvas.Text.CanvasTextFormat
                    {
                        FontSize = 13,
                    });

                // Vẽ các đường line cho mỗi sản phẩm
                foreach (var product in products)
                {
                    // Lấy màu cho sản phẩm
                    if (!_colorMap.TryGetValue(product, out var color))
                    {
                        color = Color.FromArgb(
                            255,
                            (byte)_rand.Next(50, 230),
                            (byte)_rand.Next(50, 230),
                            (byte)_rand.Next(50, 230));

                        _colorMap[product] = color;
                    }

                    // Thu thập tất cả các điểm cho sản phẩm này
                    var points = new List<Vector2>();

                    for (int d = 0; d < dateCount; d++)
                    {
                        var item = data.FirstOrDefault(x =>
                            x.ProductName == product &&
                            x.Date.Date == dates[d]);

                        int quantitySold = item?.QuantitySold ?? 0;

                        float x = originX + d * stepX;
                        // Y tính từ gốc tọa độ (originY) đi lên
                        float y = originY - (quantitySold / (float)yAxisMax) * height;

                        points.Add(new Vector2(x, y));
                    }

                    // Vẽ đường line
                    if (points.Count > 1)
                    {
                        for (int i = 0; i < points.Count - 1; i++)
                        {
                            ds.DrawLine(points[i], points[i + 1], color, 3);
                        }
                    }

                    // Vẽ các điểm trên đường
                    foreach (var point in points)
                    {
                        ds.FillCircle(point, 5, color);
                        ds.DrawCircle(point, 5, Colors.White, 2);
                    }
                }

                // Vẽ legend (bên dưới đồ thị, dạng danh sách bullet)
                float legendStartX = marginLeft;
                float legendY = originY + 55;

                var legendTextFormat = new Microsoft.Graphics.Canvas.Text.CanvasTextFormat
                {
                    FontSize = 12
                };

                for (int i = 0; i < products.Count; i++)
                {
                    var product = products[i];

                    if (!_colorMap.TryGetValue(product, out var color))
                        continue;

                    float currentLegendY = legendY + i * legendItemHeight;

                    // Vẽ bullet (dấu chấm tròn)
                    ds.FillCircle(new Vector2(legendStartX, currentLegendY + 7), 4, color);

                    // Vẽ line mẫu nhỏ
                    ds.DrawLine(legendStartX + 8, currentLegendY + 7, legendStartX + 28, currentLegendY + 7, color, 3);

                    // Tên sản phẩm
                    ds.DrawText(
                        product,
                        new Vector2(legendStartX + 35, currentLegendY),
                        Colors.Black,
                        legendTextFormat);
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