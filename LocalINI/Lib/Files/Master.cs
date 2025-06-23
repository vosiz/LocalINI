using LocalIni.Model;
using System;

namespace LocalIni.Files
{
    public class Master : Base.IniFile
    {
        public static readonly string MASTER_NAME = "master";
        public static readonly string STATE_SECTION = "State";

        public Language LoadedLang { get; private set; }
        public string MasterPath { get; private set; }


        public Master(string path)
            : base(path) {

            MasterPath = path;
        }

        // write language change
        public void ChangeLang(string id) {

            try
            {
                WriteData(STATE_SECTION, "SelectedLang", id, true); 
            }
            catch (Exception exc) {

                throw exc;
            }
        }

        // loads file
        public void Load(FileMan fm) {

            try
            {
                // language selected
                var lang_id = (string)ReadData(STATE_SECTION, "SelectedLang");
                if (lang_id == null)
                    throw new Exception("Language id could not retreived form master file");

                LoadedLang = LangFile.ToLanguage(fm.CreateFilepath(lang_id));

            }
            catch (Exception exc) {

                throw new Exception("Master file load failed", exc);
            }
        }
    }
}
