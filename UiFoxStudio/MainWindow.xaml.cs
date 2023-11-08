using MouseHookLibrary;
using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using UiFoxStudio.ViewModel;

namespace UiFoxStudio
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MouseHookViewModel();
        }       


    }
}
