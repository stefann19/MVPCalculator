using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Expression = org.mariuszgromada.math.mxparser.Expression;

namespace MVPCalculator
{
    public class Controller
    {
        public List<double> Operands { get; set; }
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
            AddKeyboardFunctionality();
            AddLoadFunctionality();
            Operands = new List<double>();
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
                {
                    MainWindow.LabelOutput.Text = MainWindow.LabelOutput.Text.Substring(0, length - 1);
                }
            }

            void DeleteAllCharacters(object sender, MouseButtonEventArgs e)
            {
                MainWindow.LabelOutput.Text = string.Empty;
            }
        }


        private void AddAssesFunctionality()
        {

            MainWindow.Button_Equals.PreviewMouseDown += EqualsButtonPressed;


            void EqualsButtonPressed(object sender, MouseButtonEventArgs e)
            {
                AssesAndClearExpresion();
            }

            void AssesAndClearExpresion()
            {
                string result = AssesExpresionAndAddToList();

                MainWindow.LabelOutput.Text = result;

            }
        }

        private void AddKeyboardFunctionality()
        {
            MainWindow.PreviewTextInput += TextInput;
            void TextInput(object sender, TextCompositionEventArgs e)
            {
/*
                if(new List<string>{"\b","\r"}.Any(s=> s ==e.Text))return;//Ignora asta \b e backspace \r e enter
*/
                if (e.Text.Length != 1)
                {
                    return;
                }

                char c = e.Text[0];
                AddNewCharacter(c);
            }
        }


        private void AddBasicButtonsFunctionlity()
        {
            View.BasicCButtons.ForEach(button => button.Button.PreviewMouseDown += AddButtonTextToLabel);

            void AddButtonTextToLabel(object sender, MouseButtonEventArgs e)
            {
                if (!((sender as Button)?.Parent is CButton button))
                {
                    return;
                }

                AddNewCharacter(button.Button.Content.ToString()[0]);
            }
        }

        private string AssesExpresion()
        {
            string result = new Expression(MainWindow.LabelOutput.Text).calculate().ToString();
            if (result == "NaN") return "NaN";
            MainWindow.IntermediaryResults.Text = $"{MainWindow.LabelOutput.Text} =  {result}";
            return result;
/*
            MainWindow.LabelOutput.CaretIndex = MainWindow.LabelOutput.Text.Length;
*/
        }

         string AssesExpresionAndAddToList()
        {
            string result = new Expression(MainWindow.LabelOutput.Text).calculate().ToString();
            if (result == "NaN") return "NaN";
            Label label = new Label
            {
                Content = $"{MainWindow.LabelOutput.Text} = {result}",
                HorizontalAlignment = HorizontalAlignment.Right,
                Foreground = Brushes.CadetBlue
            };
            MainWindow.ListView.Items.Add(label);
            MainWindow.ListView.ScrollIntoView(label);
            MainWindow.IntermediaryResults.Text = "";
            return result;
        }

        private Cases AssesCase(char native, char input)
        {
            bool IsSymbol(char c)
            {
                return new List<char> {'+', '-', '/', '*'}.Any(a => a == c);
            }

            bool IsDot(char c)
            {
                return c == '.' || c == ',';
            }

            bool IsLetter(char c)
            {
                return char.IsLetter(c);
            }

            bool IsPlusOrMinus(char c)
            {
                return new List<char> {'+', '-'}.Any(a => a == c);
            }

            bool IsNumber(char c)
            {
                return char.IsNumber(c);
            }

            bool IsBackSpace(char c)
            {
                return c == '\b';
            }

            bool IsEnter(char c)
            {
                return c == '\r';
            }

            bool IsParanthesis(char c)
            {
                return c == '(' || c == ')';
            }

            if (IsBackSpace(input))
            {
                return Cases.Backspace;
            }

            if (IsEnter(input))
            {
                return Cases.EnterAndEquals;
            }

            if (native == '\u0000')
            {
                return Cases.EmptySpace;
            }

            if (IsLetter(native))
            {
                return Cases.Letter;
            }

            if (IsSymbol(native))
            {
                if (IsSymbol(input))
                {
                    return Cases.SymbolAndSymbol;
                }

                if (IsNumber(input))
                {
                    return Cases.SymbolAndNumber;
                }

                if (IsDot(input))
                {
                    return Cases.SymbolAndPeriod;
                }

                if (IsParanthesis(input))
                {
                    return Cases.SymbolAndParanthesis;
                }
            }
            else if (IsNumber(native))
            {
                if (IsSymbol(input))
                {
                    return Cases.NumberAndSymbol;
                }

                if (IsNumber(input))
                {
                    return Cases.NumberAndNumber;
                }

                if (IsDot(input))
                {
                    return Cases.NumberAndPeriod;
                }

                if (IsParanthesis(input))
                {
                    return Cases.NumberAndParanthesis;
                }
            }
            else if (IsDot(native))
            {
                if (IsSymbol(input))
                {
                    return Cases.PeriodAndSymbol;
                }

                if (IsNumber(input))
                {
                    return Cases.PeriodAndNumber;
                }

                if (IsDot(input))
                {
                    return Cases.PeriodAndPeriod;
                }

                if (IsParanthesis(input))
                {
                    return Cases.PeriodAndParanthesis;
                }
            }
            else if (IsParanthesis(native))
            {
                if (IsSymbol(input))
                {
                    return Cases.ParanthesisAndSymbol;
                }

                if (IsNumber(input))
                {
                    return Cases.ParanthesisAndNumber;
                }

                if (IsDot(input))
                {
                    return Cases.ParanthesisAndPeriod;
                }

                if (IsParanthesis(input))
                {
                    return Cases.ParanthesisAndParanthesis;
                }
            }

            return Cases.Unidentified;
        }

        private void AddNewCharacter(char input)
        {
            string nativeString = MainWindow.LabelOutput.Text;
            char nativeChar = nativeString.LastOrDefault();

            Cases @case = AssesCase(nativeChar, input);

            void Replace()
            {
                MainWindow.LabelOutput.Text = nativeString.Substring(0, nativeString.Length - 1) + input;
            }

            void Add()
            {
                MainWindow.LabelOutput.Text += input.ToString();
            }

            void ClearAndAdd()
            {
                MainWindow.LabelOutput.Text = input.ToString();
            }

            void NewNumber()
            {
                Add();
            }

            void EndNumber()
            {
                /*double newOperand;
                string newOperandAsString = nativeString.Split(new[] {'+', '-', '/', '*'}).LastOrDefault();
                if (Double.TryParse(newOperandAsString, out newOperand))
                {
                    Operands.Add(newOperand);
                }*/
                if (nativeString.Split('+', '-', '/', '*').Length > 1)
                {
                    AssesExpresion();
                }

                Add();
            }

            void AddPeriod()
            {
                string operand = nativeString.Split('+', '-', '/', '*').LastOrDefault();
                if (operand == null)
                {
                    Add();
                    return;
                }

                int appatritions = operand.Count(c => c == '.');
                if (appatritions == 0)
                {
                    Add();
                }
            }

            void DeleteLast()
            {
                string txt = MainWindow.LabelOutput.Text;
                if (txt.Length > 1)
                {
                    txt = txt.TrimEnd().Substring(0, txt.Length - 1);
                    MainWindow.LabelOutput.Text = txt.Replace("\b", "");
                }
                else
                {
                    MainWindow.LabelOutput.Text = "";
                }
            }

            void EnterPressed()
            {
                MainWindow.LabelOutput.Text = AssesExpresionAndAddToList();
            }

            void AddParanthesis()
            {
                if (input == '(')
                {
                    Add();
                }
                else
                {
                    if(nativeString.Count(c=> c=='(') > nativeString.Count(c => c == ')'))Add();
                }
            }

            switch (@case)
            {
                case Cases.EmptySpace:
                case Cases.Letter:ClearAndAdd();
                    break;
                case Cases.SymbolAndSymbol:Replace();
                    break;
                case Cases.SymbolAndNumber:NewNumber();
                    break;
                case Cases.SymbolAndPeriod: //do nothing
                    break;
                case Cases.SymbolAndParanthesis: AddParanthesis();
                    break;
                case Cases.NumberAndSymbol:EndNumber();
                    break;
                case Cases.NumberAndNumber:Add();
                    break;
                case Cases.NumberAndPeriod:AddPeriod();
                    break;
                case Cases.NumberAndParanthesis: if(input==')')AddParanthesis();
                    break;
                case Cases.PeriodAndSymbol: //do nothing
                    break;
                case Cases.PeriodAndNumber:Add();
                    break;
                case Cases.PeriodAndPeriod: //do nothing
                    break;
                case Cases.PeriodAndParanthesis: //do nothing
                    break;
                case Cases.ParanthesisAndSymbol:Add();
                    break;
                case Cases.ParanthesisAndNumber: Add();
                    break;
                case Cases.ParanthesisAndPeriod: //do nothing
                    break;
                case Cases.ParanthesisAndParanthesis:
                    break;//do nothing
                case Cases.Backspace:DeleteLast();
                    break;
                case Cases.EnterAndEquals:EnterPressed();
                    break;
                case Cases.Unidentified:
                    Debug.Print($"{nativeString} ---- {input}");
                    /*ClearAndAdd();*/
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void AddLoadFunctionality()
        {
            /*
                        MainWindow.ListView.select
            */
            MainWindow.ListView.PreviewMouseDoubleClick += SelectItem;

            void SelectItem(object sender, MouseButtonEventArgs e)
            {
                Label item = (sender as ListView).SelectedItem as Label ;
                string[] split = item.Content.ToString().Split('=');

                MainWindow.IntermediaryResults.Text = item.Content.ToString().Trim();
                MainWindow.LabelOutput.Text = split[0].Trim();
            }
        }

       


    }
}