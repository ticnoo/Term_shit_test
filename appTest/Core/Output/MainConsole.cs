using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace appTest.Core.Output
{
    public class MainConsole
    {
        public string Buffer { get; set; } = "";

        public void Add (string text)
        {
            this.Buffer += text;
        }
    }
}
