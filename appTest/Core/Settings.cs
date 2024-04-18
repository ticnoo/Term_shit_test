using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static appTest.ViewModels.MainViewModel;
using static System.Net.Mime.MediaTypeNames;

namespace appTest.Core.Settings
{
    public struct mainConsole
    {
        public int MaxBufferLenght { get; set; }
        public int HistoryBufferCut { get; set; }
        public int MaxBufferLines {  get; set; }
        public int HistoryBufferLinesCutCount { get; set; }
    }

    public class AzulaSettings
    {
        //AzulaSettings()
        //{
        //    // default settings setup
        //}

        public string nReplacer = "&&";
        public string rReplacer = "";

        public mainConsole MainConsole = new mainConsole
        {
            MaxBufferLenght = 500,
            HistoryBufferCut = 200,
            MaxBufferLines = 1000,
            HistoryBufferLinesCutCount = 200,
        };
    }
}
