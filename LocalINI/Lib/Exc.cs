using System;

namespace LocalIni
{
    public class TranslationException : Exception {

        public TranslationException(string message, Exception inner)
            : base(message, inner) { }

        public TranslationException(string key, string section = "*", Exception inner = null)
            : this($"Translation error ({section}.{key})", inner) { }
    }

    public class LanguagePropertyException : Exception {

        public LanguagePropertyException(string message)
            : base(message) { }
    }
}
