using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace Bb.Workflows
{

    
    public class BusinessLog<TContext>
    {

        static BusinessLog()
        {

            BusinessLog<TContext>.MethodLogResult = typeof(BusinessLog<TContext>).GetMethod("LogResult", BindingFlags.Static | BindingFlags.NonPublic);
            BusinessLog<TContext>.MethodLogResultException = typeof(BusinessLog<TContext>).GetMethod("LogResultException", BindingFlags.Static | BindingFlags.NonPublic);

            
        }



        /// <summary>
        /// Attention on y fait bien référence par reflexion dans la methode précédente.
        /// </summary>
        /// <param name="ruleName"></param>
        /// <param name="result"></param>
        /// <param name="context"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        private static void LogResult(string ruleName, bool result, TContext context, string[] names, object[] arguments)
        {

            if (FunctionalLog == null)
            {
                StringBuilder sb = new StringBuilder(1000);
                sb.Append(ruleName);
                sb.Append(" (");
                string comma = string.Empty;
                for (int i = 0; i < names.Length; i++)
                {
                    sb.Append(comma);
                    var a = arguments[i];
                    sb.Append(names[i]);
                    sb.Append(" : ");
                    sb.Append(a == null ? "null" : a.GetType().Name);
                    sb.Append(" = ");
                    sb.Append(a);
                    comma = ", ";
                }
                sb.Append(") =>");
                sb.Append(result ? "'true'" : "'false'");
                Trace.WriteLine(sb.ToString());
            }
            else
                FunctionalLog(ruleName, result, context, names, arguments);

        }

        /// <summary>
        /// Attention on y fait bien référence par reflexion dans la methode précédente.
        /// </summary>
        /// <param name="ruleName"></param>
        /// <param name="result"></param>
        /// <param name="context"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        private static void LogResultException(string ruleName, Exception result, TContext context, string[] names, object[] arguments)
        {

            if (FunctionalLog == null)
            {
                StringBuilder sb = new StringBuilder(1000);
                sb.Append(ruleName);
                sb.Append(" (");
                string comma = string.Empty;
                for (int i = 0; i < names.Length; i++)
                {
                    sb.Append(comma);
                    var a = arguments[i];
                    sb.Append(names[i]);
                    sb.Append(" : ");
                    sb.Append(a == null ? "null" : a.GetType().Name);
                    sb.Append(" = ");
                    sb.Append(a);
                    comma = ", ";
                }
                sb.Append(") =>");
                sb.Append(result.Message);
                Trace.WriteLine(sb.ToString());
            }
            else
                FunctionalLogException(ruleName, result, context, names, arguments);

        }

        public static Func<string, bool, TContext, string[], object[], bool> FunctionalLog;
        public static Func<string, Exception, TContext, string[], object[], bool> FunctionalLogException;

        public static readonly MethodInfo MethodLogResult;
        public static readonly MethodInfo MethodLogResultException;
        
    }

}
