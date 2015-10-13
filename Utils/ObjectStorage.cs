using System;
using System.IO;
using System.Diagnostics;
using System.Xml.Serialization;

namespace GazeLaser.Utils
{
    public sealed class ObjectStorage<T>
    {
        private static string sOptionsFileExtension = ".xml";

        public static void save(object aObject)
        {
            if (aObject == null)
            {
                return;
            }

            Type type = typeof(T);
            string fileName = GetShortName(type.ToString()) + sOptionsFileExtension;
            
            try
            {
                XmlSerializer serializer = new XmlSerializer(type);
                TextWriter writer = new StreamWriter(fileName);
                serializer.Serialize(writer, aObject);
                writer.Close();
            }
            catch (Exception e)
            {
                Debug.WriteLine(String.Format("Error on saving the object of {0} type: {1}", type.ToString(), e.Message), "ObjectStorage");
            }
        }

        public static T load(string aFileName = "")
        {
            T result = default(T);
            Type type = typeof(T);
            string fileName = string.IsNullOrEmpty(aFileName) ? GetShortName(type.ToString()) + sOptionsFileExtension : aFileName;

            try
            {
                XmlSerializer serializer = new XmlSerializer(type);
                FileStream fs = new FileStream(fileName, FileMode.Open);
                result = (T)serializer.Deserialize(fs);
                fs.Close();
            }
            catch (Exception e)
            {
                Debug.WriteLine(String.Format("Error on loading the object of {0} type: {1}", type.ToString(), e.Message), "ObjectStorage");
                result = (T)Activator.CreateInstance(typeof(T));
            }

            return result;
        }

        private ObjectStorage() { }

        private static string GetShortName(string aTypeName)
        {
            string[] parts = aTypeName.ToString().Split('.', '+');
            return parts[parts.Length - 1];
        }
    }
}
