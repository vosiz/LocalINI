using LocalIni.Files;
using LocalIni.Model;
using Vosiz.Extends;

namespace LocalIni
{
    public partial class Localization
    {

        public FileMan FileManager { get; private set; }
        public Language SelectedLanguage { get; private set; }


        public Localization(string rel_path = ".\\locales", string ext = ".loc")
        {
            FileManager = new FileMan(rel_path, ext);
        }

        // loads everything
        public Language Load(string def_lang_id = "en_us") {

            FileManager.LoadAll();
            var master = FileManager.LoadMaster();
            SelectedLanguage = master.LoadedLang;

            return SelectedLanguage;
        }

        // changes language
        public Language ChangeLang(string id)
        {
            var lang = FileManager.Languages.TryGet(id);
            SelectedLanguage = lang;
            FileManager.MasterFile.ChangeLang(id);

            return lang;
        }

        // translate
        public string Translate(string key) {

            return SelectedLanguage.TryTranslate(key);
        }

        // translate with exception
        public string TranslateStrict(string key) {

            return SelectedLanguage.TryTranslate(key, false);
        }

    }
}
