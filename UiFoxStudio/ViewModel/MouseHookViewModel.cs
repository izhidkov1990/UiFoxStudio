using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using MouseHookLibrary;
using UICore;
using UICore.interfaces;
using UiFoxStudio.View;

namespace UiFoxStudio.ViewModel
{
    public class MouseHookViewModel : INotifyPropertyChanged
    {
        private MouseHook _mouseHook;
        private string _mousePosition;
        private HighLighter _highLighter;

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand StartCommand { get; private set; }
        public ICommand StopCommand { get; private set; }
        public string Title
        {
            get => "UiFox " + _mousePosition;
        }

        public string MousePosition
        {
            get => _mousePosition;
            set
            {
                if (_mousePosition != value)
                {
                    _mousePosition = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(Title));
                }
            }
        }

        public MouseHookViewModel()
        {
            //MouseHook
            _mouseHook = new MouseHook();
            _mouseHook.MouseMoved += OnMouseMoved;
            //HighLighter
            _highLighter = new HighLighter();

            //Commands
            StartCommand = new RelayCommand(StartTracking);
            StopCommand = new RelayCommand(StopTracking);
        }

        private void StartTracking()
        {
            _mouseHook.Start();
        }

        private void StopTracking()
        {
            _mouseHook.Stop();
            MousePosition = null;
        }

        private void OnMouseMoved(object sender, MouseHookLibrary.Point point)
        {
            MousePosition = $"[Mouse position: {point.X}, {point.Y}]";
            _highLighter.HighLightElement(new System.Windows.Point(point.X, point.Y));
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (Application.Current.Dispatcher.CheckAccess())
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            else
            {
                Application.Current.Dispatcher.Invoke(() => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)));
            }
        }

        public class RelayCommand : ICommand
        {
            private readonly Action _execute;
            private readonly Func<bool> _canExecute;

            public event EventHandler CanExecuteChanged
            {
                add { CommandManager.RequerySuggested += value; }
                remove { CommandManager.RequerySuggested -= value; }
            }

            public RelayCommand(Action execute, Func<bool> canExecute = null)
            {
                _execute = execute ?? throw new ArgumentNullException(nameof(execute));
                _canExecute = canExecute;
            }

            public bool CanExecute(object parameter)
            {
                return _canExecute == null || _canExecute();
            }

            public void Execute(object parameter)
            {
                _execute();
            }
        }
    }
}
