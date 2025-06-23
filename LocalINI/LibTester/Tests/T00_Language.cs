using LocalIni.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibTester.Tests
{
    class T00_Language
    {
        // creates language - english
        public static Language CreateEnglish()
        {

            var en = new Language("en_us", "English", new Language.Setup());
            return en;
        }

        // add various tranlsation
        public static Language AddTranslations(Language lang)
        {
            lang.AddTranslation("Menu", "Menu");
            lang.AddTranslation("Ean", "John");
            lang.AddTranslation("PickOrder", "Pick order");

            return lang;
        }

        // translate
        public static void Translate(Language lang, string key)
        {

            Console.WriteLine($"Translation[{lang.Id}.{key}]: " + lang.TryTranslate(key));
        }

        // translate
        public static void TranslateStrict(Language lang, string key)
        {

            Console.WriteLine($"Translation[{lang.Id}.{key}]: " + lang.TryTranslate(key, false));
        }

        // copy file and setup translations
        public static Language CopyToCzech() {

            var en = CreateEnglish();
            AddTranslations(en);
            var setup = new Language.Setup();
            setup.AddProp("Untranslated", "_Nepřeloženo");
            setup.AddProp("TranslationNotFound", "_Nemám páru co to znamená");
            var cz = new Language("cs_cz", "Czech", setup);
            cz.Encoding = Encoding.GetEncoding("Windows-1250");
            var missing_translates = Language.Copy(en, ref cz);
            cz.AddTranslation("Ean", "Honza");

            return cz;
        }
    }
}
