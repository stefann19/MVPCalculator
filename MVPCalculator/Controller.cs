using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MVPCalculator
{
    public class Controller
    {
        public MainWindow MainWindow { get; set; }
        public View View { get; set; }
        public Controller()
        {
            MainWindow = new MainWindow();
            MainWindow.Show();
            View = new View(MainWindow);

            AddAssesFunctionality();
            AddBasicButtonsFunctionlity();
            AddDeleteFunctionality();


        }

        private void AddDeleteFunctionality()
        {
            MainWindow.Button_Delete.PreviewMouseDown += DeleteSingleCharacter;
            MainWindow.Button_C.PreviewMouseDown += DeleteAllCharacters;
            MainWindow.Button_Ce.PreviewMouseDown += DeleteAllCharacters;

            void DeleteSingleCharacter(object sender, MouseButtonEventArgs e)
            {
                int length = MainWindow.LabelOutput.Text.Length;
                if (length >= 1)
                    MainWindow.LabelOutput.Text = MainWindow.LabelOutput.Text.Substring(0, length - 1);
            }

            void DeleteAllCharacters(object sender, MouseButtonEventArgs e)
            {
                MainWindow.LabelOutput.Text = String.Empty;

            }
        }

        

        private void AddAssesFunctionality()
        {
            EventManager.RegisterClassHandler(typeof(Window),
                Keyboard.KeyUpEvent, new KeyEventHandler(KeyDown), true);

            MainWindow.Button_Equals.PreviewMouseDown += EqualsButtonPressed; ;

            void KeyDown(object sender, KeyEventArgs keyEventArgs)
            {
                if (keyEventArgs.Key == Key.Enter)
                {

                    AssesExpresion();
                }
            }
            void EqualsButtonPressed(object sender, MouseButtonEventArgs e)
            {
                AssesExpresion();
            }

            void AssesExpresion()
            {
                MainWindow.LabelOutput.Text = new org.mariuszgromada.math.mxparser.Expression(MainWindow.LabelOutput.Text).calculate().ToString();
                MainWindow.LabelOutput.CaretIndex = MainWindow.LabelOutput.Text.Length;
            }
        }
        

        

        private void AddBasicButtonsFunctionlity()
        {
            View.BasicCButtons.ForEach(button=> button.Button.PreviewMouseDown += AddButtonTextToLabel);

            void AddButtonTextToLabel(object sender, MouseButtonEventArgs e)
            {
                if(!(((sender as Button)?.Parent as Grid)?.Parent is CButton button))return;

                MainWindow.LabelOutput.Text += button.Button.Content.ToString();
            }
        }

       
    }
}
