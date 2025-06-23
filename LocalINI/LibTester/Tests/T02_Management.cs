using LocalIni;
using LocalIni.Files;
using LocalIni.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibTester.Tests
{
    public static class T02_Management
    {


        public static Master CreateMaster(Localization loc, Language def) {

            return loc.FileManager.CreateMaster(def);
        }

        // load languages
        public static string[] LoadLanguages(Localization loc) {

            return loc.FileManager.LoadAll();
        }

        // change language
        public static Language ChangeToLanguage(Localization loc, string id) {

            return loc.ChangeLang(id);
        }


        // translate selected language
        //public static string TranslateWithSelected(Localization loc) { 
        
            
        //}
    }
}
