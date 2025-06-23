using LocalIni.Files;
using LocalIni.Model;
using LocalIni;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibTester.Tests
{
    public static class T01_FileManipulation
    {

        // creates language file
        public static LangFile CreateLangFile(Localization loc, Language lang) {

            return loc.FileManager.CreateLang(lang);
        }

    }
}
