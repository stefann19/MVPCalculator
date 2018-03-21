using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVPCalculator
{
    public static class Translator
    {
        public static Dictionary<string, string> TextToCode = new Dictionary<string, string>
        {
            {"Minus","-"},{"Addition","+"}
            ,{"Multiply","*"},{"Divide","/"}
            ,{"Dot","."},{"Equals","="}
            ,{"Delete","<-"},{"Module","%"}
            ,{"Root","√"},{"Square","²"}
            ,{"Cube","³"},{"Reverse","<-"}
            ,{"PlusMinus","±"}
        };

    }
}
