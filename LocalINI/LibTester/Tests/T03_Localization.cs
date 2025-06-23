using LocalIni;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibTester.Tests
{
    public static class T03_Localization
    {
        // mast have files
        public static void FromCradleToGrave() {

            var loc = new Localization();
            var lang = loc.Load();
            var lang_id = lang.Id;
            Console.WriteLine("Selected = " + lang_id);
            Console.WriteLine($"Ean is {loc.TranslateStrict("Ean")} in " + lang_id);
            lang = loc.ChangeLang("cs_cz");
            lang_id = lang.Id;
            Console.WriteLine("Selected = " + lang_id);
            Console.WriteLine($"Ean is {loc.TranslateStrict("Ean")} in " + lang_id);

        }
    }
}
