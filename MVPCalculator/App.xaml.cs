using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace MVPCalculator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private Controller controller;
        public App()
        {
            controller = new Controller();
        }

        public App(bool contentLoaded):this()
        {
            _contentLoaded = contentLoaded;
        }
    }
}
