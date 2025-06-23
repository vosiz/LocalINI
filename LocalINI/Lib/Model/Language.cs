using System.Collections.Generic;
using Vosiz.Extends;
using Vosiz.Commons;
using Vosiz.Helpers;
using System;
using System.ComponentModel;
using LocalIni.Files;
using System.Text;

namespace LocalIni.Model
{
    public class Language
    {

        public class Setup
        {

            public enum Error { 
            
                [Description("_UndefinedError")]
                Undefined               = 0x00,

                [Description("_NotAnError")]
                NotError                = 0x01,
                [Description("_InternalError")]
                InternalError           = 0x02,

                [Description("_TranslationNotFound")]
                TranslationNotFound     = 0x10,
                [Description("_---")]
                TranslationStock        = 0x11,

                [Description("_PropertyNotDefined")]
                PropertyNotFound        = 0x20,
            }
            
            public Dictionary<string, string> Props { get; private set; }

            public Setup() {

                Props = new Dictionary<string, string>();

                // setup basic common properties
                // - error handlers
                Props.Add(
                    "TranslationNotFound", ErrorToString(Error.TranslationNotFound));
                Props.Add(
                    "Untranslated", ErrorToString(Error.TranslationStock));
            }

            // add recognizable property
            public bool AddProp(string key, string value) {

                return Props.TryAdd(key, value, true);
            }

            // get property
            public string GetProp(string key, string def = "", bool use_predef = true) {

                try {

                    return Props.TryGet(key);

                } catch (IndexOutOfRangeException) {

                    if (use_predef)
                    {
                        return ErrorToString(Error.PropertyNotFound);
                    }
                    else {

                        return def;
                    }

                } catch (Exception exc) {

                    throw exc;
                }
                
            }

            // error enum to string
            private string ErrorToString(Error err) {

                return err == Error.NotError 
                    ? string.Empty
                    : err.GetDescription();
            }
        }


        public string Name { get; private set; } // english name
        public string Id { get; private set; } // for file and id
        public Encoding Encoding { get; set; }
        public Setup LanguageProperties { get; private set; }
        public Dictionary<string, string> Translations { get; private set; }

        // copies language with default value
        public static string[] Copy(Language from, ref Language to) {

            return to.AddStockTranslations(from.Translations);
        }


        public Language(string id, string name, Setup props) {

            Name = name;
            Id = id;
            LanguageProperties = props;
            Translations = new Dictionary<string, string>();

            Encoding = Encoding.UTF8;
        }

        // adds translations
        public void AddTranslations(Dictionary<string, string> translations) {

            foreach (var kv in translations) {

                AddTranslation(kv);
            }
        }

        // add single translation - key, value
        public void AddTranslation(string key, string value, bool update = true) {

            Translations.TryAdd(key, value, update);
        }

        // add translation - keyval pair
        public void AddTranslation(KeyValuePair<string, string> kv)
        {
            AddTranslation(kv.Key, kv.Value);
        }

        // adds non-translated translations (keys with default values)
        public string[] AddStockTranslations(Dictionary<string, string> translations) {

            List<string> list = new List<string>();
            foreach (var kv in translations)
            {
                AddTranslation(kv.Key, Setup.Error.TranslationStock.GetDescription());
                list.Add(kv.Key);
            }

            return list.ToArray();
        }

        // tries to translate, letpass does not trigger exception, def is when not found
        public string TryTranslate(string key, bool let_pass = true, string def = null) {

            if (Translations.Count == 0)
                throw new TranslationException("Translation dictionary is empty");

            try
            {
                var translation = Translations.TryGet(key);
                if (translation == Setup.Error.TranslationStock.GetDescription())
                    return LanguageProperties.GetProp("Untranslated");

                return translation;
            }
            catch (TranslationException) {

                return TranslationMissing("TranslationNotFound");
            }
            catch (Exception exc)
            {

                if (let_pass)
                {
                    return def != null ? def : TranslationMissing("TranslationNotFound");
                }
                else
                {
                    throw exc;
                }
            }
        }

        // when translation is missing
        public string TranslationMissing(string key)
        {
            try
            {
                return LanguageProperties.GetProp(key);
            }
            catch (LanguagePropertyException lp_exc) {

                throw lp_exc;
            }
            catch (Exception exc)
            {

                throw exc;
            }
            
        }

    }
}
