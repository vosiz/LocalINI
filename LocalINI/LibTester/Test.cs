using LocalIni;
using LocalIni.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibTester
{

    class Test
    {

        public static class Helper {

            public static Localization CreateLocInst(string dir) {

                return new Localization(dir);
            }

            public static void Clear(string dir) {

                if (System.IO.Directory.Exists(dir))
                    System.IO.Directory.Delete(dir, true);
            }

        }

        static void Main(string[] args)
        {
            string dir = ".\\locales";
            try
            {

                //TestScenarios(dir);
                RealityCheck();

            }
            catch (Exception exc) {

                Console.WriteLine(
                    string.Format("[EXC] {0}: {1}",
                    exc.GetType().ToString(),
                    exc.Message)
               );
            }

        }

        static void TestScenarios(string dir) {

            // reset
            Helper.Clear(dir);

            // try make instance
            var loc = Helper.CreateLocInst(dir);

            // Tests
            var en = Tests.T00_Language.CreateEnglish();
            en = Tests.T00_Language.AddTranslations(en);
            //Tests.T00_Language.Translate(en, "Menu");
            //Tests.T00_Language.Translate(en, "Ean");
            //Tests.T00_Language.Translate(en, "Wrong", false);
            //Tests.T00_Language.Translate(en, "PickOrder");

            var cz = Tests.T00_Language.CopyToCzech();
            //Tests.T00_Language.Translate(cz, "Menu");
            //Tests.T00_Language.Translate(cz, "Ean");
            //Tests.T00_Language.Translate(cz, "Wrong");
            //Tests.T00_Language.TranslateStrict(cz, "Ean");
            //Tests.T00_Language.TranslateStrict(cz, "Wrong");

            var en_file = Tests.T01_FileManipulation.CreateLangFile(loc, en);
            var cz_file = Tests.T01_FileManipulation.CreateLangFile(loc, cz);

            var master = Tests.T02_Management.CreateMaster(loc, en);
            var langs = Tests.T02_Management.LoadLanguages(loc);
            var selected = Tests.T02_Management.ChangeToLanguage(loc, cz.Id);
        }

        static void RealityCheck() {

            Tests.T03_Localization.FromCradleToGrave();
        }
    }
}
