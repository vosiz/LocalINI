using LocalIni.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vosiz.Extends;

namespace LocalIni.Files
{
    public class LangFile : Base.IniFile
    {
        public class LanguageException : Exception {

            public LanguageException(string msg) : base(msg) {}
        }

        public class Header {

            public string Id;
            public string NativeName;
            public string Name;
        }

        public static readonly string HEADER_SECTION = "_Header";
        public static readonly string PROPERTIES_SECTION = "_Props";
        public static readonly string TRANSLATION_SECTION = "_Translations";

        public Header LangInfo { get; private set; }


        public static Language ToLanguage(string path) {

            try {

                var langfile = new LangFile(path);
                var header = new Header();
                header.Id = langfile.ReadData(HEADER_SECTION, "Id");
                header.Name = langfile.ReadData(HEADER_SECTION, "Name");
                
                var props = new Language.Setup();
                var propkeys = langfile.FetchSectionKeys(PROPERTIES_SECTION);
                foreach (var key in propkeys) {

                    props.AddProp(
                        key, 
                        langfile.ReadData(
                            PROPERTIES_SECTION, 
                            key, 
                            Language.Setup.Error.InternalError.GetDescription()));
                }

                var lang = new Language(
                    header.Id,
                    header.Name,
                    props
                );

                var trkeys = langfile.FetchSectionKeys(TRANSLATION_SECTION);
                foreach (var key in trkeys) { 
                
                    lang.AddTranslation(key,
                        langfile.ReadData(
                            TRANSLATION_SECTION,
                            key,
                            Language.Setup.Error.InternalError.GetDescription()));
                }

                return lang;

            } catch (Exception exc) {

                throw new LanguageException("Failed to load and convert to Language, becuase " + exc.Message);
            }
            
        }

        public LangFile(string path)
            : base(path) { }

        public string ReadData(string section, string key, object def = null) {

            return (string)base.ReadData(section, key, def);
        }
    }
}
