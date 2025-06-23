using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IniParser.Model;
using IniParser;
using IniParser.Exceptions;
using System.IO;
using Vosiz.Helpers;


namespace LocalIni.Files.Base
{
    public class IniFile
    {
        public string Filepath { get; private set; }

        protected FileIniDataParser Parser;
        protected IniData Data;
        

        // create
        public static T Create<T>(string filepath, bool overwrite = false, Dictionary<string, Dictionary<string, string>> InitParams = null) where T : IniFile {

            if (!File.Exists(filepath) || overwrite) {

                var file = File.Create(filepath);
                file.Close();
                file.Dispose();
            }

            var ini = (T)Activator.CreateInstance(typeof(T), filepath);

            if (InitParams != null)
            {

                foreach (var kv in InitParams)
                {

                    var section = kv.Key;
                    var values = kv.Value;

                    foreach (var kv2 in values) {

                        ini.WriteData(section, kv2.Key, kv2.Value);
                    }

                    ini.WriteData("_TimeStamp", "Created", DateTime.Now.ToString("yyyy-MM-dd HH:MM:ss"), true);
                }
            }

            return ini;
        }

        // construct
        public IniFile(string path) {

            Filepath = path;

            try {

                Parser = new FileIniDataParser();

            } catch (Exception exc) {

                throw exc;
            }

        }

        // read
        public virtual object ReadData(string section, string key, object def = null) {

            try
            {
                if(Data == null)
                    Data = Parser.ReadFile(Filepath);

                return Data[section][key];
            }
            catch (ParsingException parse_exc)
            {
                throw parse_exc; 
            }
            catch (Exception exc) {

                throw new IOException($"Cannot read data [{section}].{key}", exc);
            }
        }

        // read section, return keys
        public string[] FetchSectionKeys(string section) {

            try
            {
                var section_data = Data[section];
                return section_data.Select(kvp => kvp.KeyName).ToArray();
            }
            catch (Exception exc) {

                throw exc;
            }
            
        }

        // write
        public void WriteData(string section, string key, string value, bool save = false) {

            try
            {
                if (Data == null)
                    Data = Parser.ReadFile(Filepath);

                Data[section][key] = value;

                if (save)
                    Parser.WriteFile(Filepath, Data);
            }
            catch (Exception exc)
            {
                throw new IOException($"Cannot write data [{section}].{key} <- {value}", exc);
            }
        }

    }
}
