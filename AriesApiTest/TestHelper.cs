using Aries.Core;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using NUnit.Framework;
using Path = System.IO.Path;

namespace Aries.Test
{
    public class TestHelper
    {
        public static string ReadResult(string type)
        {
            return File.ReadAllText(RetrievePath(type));
        }

        public static void WriteResult(string result, string type)
        {
            File.WriteAllText(RetrievePath(type), result);
        }

        public static string RetrievePath(string type)
        {
            var baseDir = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return System.IO.Path.Combine(baseDir, String.Format("{0}.json", type));
        }

        public static void CleaunUpResult(string type)
        {
            var baseDir = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var testFile = System.IO.Path.Combine(baseDir, String.Format("{0}.json", type));
            if (File.Exists(testFile))
            {
                File.Delete(testFile);
            }
        }

        public static Dictionary<string, State> ReadState()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            string states = ReadResult("state");
            return JsonConvert.DeserializeObject<Dictionary<string, State>>(states, settings);
        }

        public static bool RemoteFileExists(string url)
        {
            HttpWebResponse response = null;
            try
            {
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                request.Method = "GET";
                response = request.GetResponse() as HttpWebResponse;
                return (response.StatusCode == HttpStatusCode.OK);
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
            }
        }

        public static string GetBinPath()
        {
            return System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        }

        public static string GetBinPath(string filepath)
        {
            return Path.Combine(GetBinPath(), filepath);
        }

        public static string GetProjectPath()
        {
            string appRoot = GetBinPath();
            var dir = new DirectoryInfo(appRoot).Parent.Parent.Parent;
            var name = dir.Name;
            return dir.FullName + @"\" + name + @"\";
        }

        public static string GetTestProjectPath()
        {
            string appRoot = GetBinPath();
            var dir = new DirectoryInfo(appRoot).Parent.Parent;
            return dir.FullName + @"\";
        }

        public static string GetMainProjectPath()
        {
            string testProjectPath = GetTestProjectPath();
            // Just hope it ends in the standard .Tests, lop it off, done.
            string path = testProjectPath.Substring(0, testProjectPath.Length - 7) + @"\";
            return path;
        }
    }
    
    public class TestDataClass
    {
        public static IEnumerable TestSetAgentConsumption
        {
            get
            {
                yield return new TestCaseData("AGENT0", ConsumptionLevel.High);
                yield return new TestCaseData("AGENT0", ConsumptionLevel.Low);
                yield return new TestCaseData("AGENT0", ConsumptionLevel.Medium);
            }
        }  
        
        public static IEnumerable ConsumptionLevels
        {
            get
            {
                yield return new TestCaseData(ConsumptionLevel.High);
                yield return new TestCaseData(ConsumptionLevel.Low);
                yield return new TestCaseData(ConsumptionLevel.Medium);
            }
        }  

        public static IEnumerable TestSetDaytime
        {
            get
            {
                yield return new TestCaseData(Daytime.Night);
                yield return new TestCaseData(Daytime.Day);
                yield return new TestCaseData(Daytime.Evening);
            }
        }  
    }
}