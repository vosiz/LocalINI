using LocalIni.Model;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
using Spath = System.IO.Path;

namespace LocalIni.Files
{
    public class FileMan
    {

        public string Path { get; set; }
        public string Ext { get; set; }
        public Master MasterFile { get; private set; }

        public Dictionary<string, Language> Languages { get; private set; }


        // construct, path can be relative, extension needs to include "."
        public FileMan(string path, string ext)
        {
            Path = path;
            Ext = ext;
            Directory.CreateDirectory(path);

            Languages = new Dictionary<string, Language>();
        }

        // creates master file
        public Master CreateMaster(Language def_lang) {

            var master = Master.Create<Master>(
                CreateFilepath(Master.MASTER_NAME));

            master.ChangeLang(def_lang.Id);
            master.Load(this);
            MasterFile = master;

            return master;
        }

        public Master LoadMaster() {

            var master = Master.Create<Master>(
                CreateFilepath(Master.MASTER_NAME));

            master.Load(this);
            MasterFile = master;

            return master;
        }

        public LangFile CreateLang(Language lang) {

            if (lang.Id == Master.MASTER_NAME)
                throw new Exception();

            var InitParams = new Dictionary<string, Dictionary<string, string>>()
            {
                [LangFile.HEADER_SECTION] = new Dictionary<string, string>
                {
                    ["Id"] = lang.Id,
                    ["NativeName"] = lang.Name,
                    ["Name"] = lang.Name,
                },
                [LangFile.PROPERTIES_SECTION] = new Dictionary<string, string>(),
                [LangFile.TRANSLATION_SECTION] = new Dictionary<string, string>()
            };

            // store props - alphabet
            foreach (var kv in lang.LanguageProperties.Props.OrderBy(kv => kv.Key)) {

                InitParams[LangFile.PROPERTIES_SECTION].Add(kv.Key, kv.Value);
            }

            // store translations - alphabet
            foreach (var kv in lang.Translations.OrderBy(kv => kv.Key)) {

                InitParams[LangFile.TRANSLATION_SECTION].Add(kv.Key, kv.Value);
            }

            return LangFile.Create<LangFile>(CreateFilepath(lang.Id), false, InitParams);
        }

        public string[] LoadAll()
        {
            var list = new List<string>();
            Languages.Clear();
            foreach (var file in Directory.GetFiles(Path)) {

                string id = Spath.GetFileNameWithoutExtension(file);
                var lang = Load(id);
                Languages.Add(id, lang);
                list.Add(id);
            }

            return list.ToArray();
        }

        public Language Load(string id)
        {
            var path = CreateFilepath(id);
            return LoadLang(path);
        }

        public Language LoadLang(string path) {

            try
            {
                return LangFile.ToLanguage(path);
            }
            catch (LangFile.LanguageException lexc)
            {
                throw new Exception("Loading language file failed", lexc);
            }
            catch (Exception exc) {

                throw exc;
            }
        }

        // Creates filepath from file name with no extension
        public string CreateFilepath(string filename_no_ext) {

            return Spath.Combine(Path, filename_no_ext + Ext);
        }

    }
}
