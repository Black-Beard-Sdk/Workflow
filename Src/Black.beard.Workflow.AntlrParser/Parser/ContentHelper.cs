using Bb.Workflows.Parser;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Bb.Workflows.Parser
{

    public class ContentHelper
    {

        /// <summary>
        /// Loads the content of the file.
        /// </summary>
        /// <param name="rootSource">The root source.</param>
        /// <returns></returns>
        public static string LoadContentFromFile(params string[] rootSource)
        {
            string _path = System.IO.Path.Combine(rootSource);
            return LoadContentFromFile(_path);
        }

        public static string LoadContentFromFile(string _path)
        {

            string fileContents = string.Empty;
            System.Text.Encoding encoding = null;
            FileInfo _file = new FileInfo(_path);

            using (FileStream fs = _file.OpenRead())
            {

                Ude.CharsetDetector cdet = new Ude.CharsetDetector();
                cdet.Feed(fs);
                cdet.DataEnd();
                if (cdet.Charset != null)
                    encoding = System.Text.Encoding.GetEncoding(cdet.Charset);
                else
                    encoding = System.Text.Encoding.UTF8;

                fs.Position = 0;

                byte[] ar = new byte[_file.Length];
                fs.Read(ar, 0, ar.Length);
                fileContents = encoding.GetString(ar);
            }

            if (fileContents.StartsWith("ï»¿"))
                fileContents = fileContents.Substring(3);

            if (encoding != System.Text.Encoding.UTF8)
            {
                var datas = System.Text.Encoding.UTF8.GetBytes(fileContents);
                fileContents = System.Text.Encoding.UTF8.GetString(datas);
            }

            return fileContents;

        }

        //public static string[] GetKeywords()
        //{

        //    if (_keywords.Length == 0)
        //        _keywords = System.Text.Encoding.Default.GetString(Resources.Tokens)
        //            .Split('\r', '\n', '=')
        //            .Where(c => c.StartsWith("'") && c.Length > 2)
        //            .Select(c => c.Substring(1, c.Length - 2))
        //            .Where(c => char.IsLetter(c[0]))
        //            .ToArray()
        //            ;

        //    return _keywords;

        //}



        static string[] _keywords = new string[] { };

    }

}