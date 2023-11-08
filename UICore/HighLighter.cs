using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using UICore.interfaces;

namespace UICore
{
    public class HighLighter : IHighLighter
    {
        private Window _overlayWindow;
        private Rectangle _highlightRectangle;
        public HighLighter()
        {
            CreateOverlayWindow();
        }

        public Task HighLightElement(Point screenPoint)
        {
            AutomationElement element = AutomationElement.FromPoint(screenPoint);
            while (element != null && element.Current.ControlType != ControlType.Window)
            {
                var walker = TreeWalker.RawViewWalker;
                AutomationElement child = walker.GetFirstChild(element);

                if (child == null) break;

                element = child;
            }
            // Выделить найденный элемент, если он не null
            if (element != null && !element.Current.IsOffscreen)
            {
                // Получение границ элемента
                Rect boundingRect = element.Current.BoundingRectangle;

                if (boundingRect != Rect.Empty && boundingRect.Width > 0 && boundingRect.Height > 0)
                {
                    _highlightRectangle.Width = boundingRect.Width;
                    _highlightRectangle.Height = boundingRect.Height;

                    _overlayWindow.Left = boundingRect.Left;
                    _overlayWindow.Top = boundingRect.Top;
                    _overlayWindow.Width = boundingRect.Width;
                    _overlayWindow.Height = boundingRect.Height;

                    _overlayWindow.Show();
                }
            }
            return Task.CompletedTask;
        }

        private void CreateOverlayWindow()
        {
            // Инициализация окна для выделения
            _overlayWindow = new Window
            {
                WindowStyle = WindowStyle.None,
                Background = Brushes.Transparent,
                AllowsTransparency = true,
                Topmost = true,
                ShowInTaskbar = false
            };
            _highlightRectangle = new Rectangle
            {
                Stroke = Brushes.Red,
                StrokeThickness = 2
            };
            _overlayWindow.Content = _highlightRectangle;
        }
    }
}
