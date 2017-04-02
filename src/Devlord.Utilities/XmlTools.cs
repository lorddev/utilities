using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace Devlord.Utilities
{
    public static class XmlTools
    {
        public static string ToXmlString<T>(this T input)
        {
            using (var writer = new StringWriter())
            {
                input.ToXml(writer);
                return writer.ToString();
            }
        }

        private static bool IsDataContract(Type t)
        {
            bool isDataContract = t.GetTypeInfo().GetCustomAttributes(typeof(DataContractAttribute), true).Any();
            return isDataContract;
        }

        public static void ToXml<T>(this T objectToSerialize, Stream stream)
        {
            if (IsDataContract(typeof(T)))
            {
                objectToSerialize.ToXml(new DataContractSerializer(typeof(T)), stream);
            }
            else
            {
                objectToSerialize.ToXml(new XmlSerializer(typeof(T)), stream);
            }
        }
        
        private static void ToXml<T>(this T objectToSerialize, XmlObjectSerializer serializer, Stream stream)
        {
             serializer.WriteObject(stream, objectToSerialize);
        }

        private static void ToXml<T>(this T objectToSerialize, XmlSerializer serializer, Stream stream)
        {
            serializer.Serialize(stream, objectToSerialize);
        }

        public static void ToXml<T>(this T objectToSerialize, StringWriter writer)
        {
            var settings = new XmlWriterSettings {OmitXmlDeclaration = true};
            using (var xwriter = XmlWriter.Create(writer, settings))
            {
                if (IsDataContract(typeof (T)))
                {
                    objectToSerialize.ToXml(new DataContractSerializer(typeof (T)), xwriter);
                }
                else
                {
                    objectToSerialize.ToXml(new XmlSerializer(typeof (T)), xwriter);
                }
            }
        }

        private static void ToXml<T>(this T objectToSerialize, XmlSerializer serializer, XmlWriter writer)
        {
            serializer.Serialize(writer, objectToSerialize);
        }

        private static void ToXml<T>(this T objectToSerialize, XmlObjectSerializer serializer, XmlWriter writer)
        {
            serializer.WriteObject(writer, objectToSerialize);
        }

        public static T DeserializeAs<T>(this string objectToDeserialize)
            where T : class
        {
            using (var xreader = XmlReader.Create(new StringReader(objectToDeserialize)))
            {
                if (IsDataContract(typeof (T)))
                {
                    return Deserialize<T>(new DataContractSerializer(typeof(T)), xreader);
                }

                return Deserialize<T>(new XmlSerializer(typeof(T)), xreader);
            }
        }

        private static T Deserialize<T>(XmlSerializer serializer, XmlReader reader)
            where T : class
        {
            return serializer.Deserialize(reader) as T;
        }

        private static T Deserialize<T>(XmlObjectSerializer serializer, XmlReader reader)
            where T : class
        {
            return serializer.ReadObject(reader) as T;
        }
    }
}