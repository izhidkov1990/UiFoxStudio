using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace UiFoxStudio.View
{
    public partial class OverlayWindow : Window
    {
        public OverlayWindow()
        {
            // Делаем окно прозрачным для событий мыши и видимым поверх других окон
            this.Topmost = true;
            this.WindowStyle = WindowStyle.None;
            this.AllowsTransparency = true;
            this.Background = Brushes.Transparent;
            this.IsHitTestVisible = false; // окно не будет получать события мыши

            // Располагаем окно поверх всех окон
            this.Left = SystemParameters.VirtualScreenLeft;
            this.Top = SystemParameters.VirtualScreenTop;
            this.Width = SystemParameters.VirtualScreenWidth;
            this.Height = SystemParameters.VirtualScreenHeight;

            // Убираем рамку окна и делаем его полностью прозрачным
            this.Opacity = 0.0;
        }
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            // Здесь будет рисоваться UI, когда он будет обновляться
        }
        public void Update(Rect bounds)
        {
            // Очистка предыдущего UI
            this.InvalidateVisual();

            // Рисуем новый UI
            var drawingVisual = new DrawingVisual();
            using (var drawingContext = drawingVisual.RenderOpen())
            {
                drawingContext.DrawRectangle(Brushes.Transparent, new Pen(Brushes.Red, 2), bounds);
            }

            // Обновляем окно
            this.Content = new VisualBrush(drawingVisual);
            this.Opacity = 1.0; // делаем содержимое окна видимым
        }
    }
}
