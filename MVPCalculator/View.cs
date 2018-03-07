using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVPCalculator
{
    public class View
    {
        public List<CButton> CButtons { get; set; }
        public List<CButton> BasicCButtons { get; set; }
        public View(MainWindow mainWindow)
        {
            MainWindow = mainWindow;
            CButtons = new List<CButton>();
            AddButtonsToList();
            InitialiseButtons();

            BasicCButtons = CButtons.Where(button => new List<string> { "=","C","Ce", "<-" }.All(any => !any.Equals(button.Button.Content))).ToList();

        }

        private void AddButtonsToList()
        {
            IEnumerable<CButton> a = MainWindow.Buttons.Children.Cast<CButton>();
            CButtons.AddRange(a);

        }

        public MainWindow MainWindow { get; set; }

        private void AddContentToButton(CButton cButton)
        {
            string content = cButton.Name.Split("_".ToCharArray()).Last();
            if (Translator.TextToCode.ContainsKey(content))
            {
                content = Translator.TextToCode[content];
            }
            cButton.Button.Content = content;
        }
        private void InitialiseButtons()
        {
            CButtons.ForEach(AddContentToButton);        

        }
    }
}
