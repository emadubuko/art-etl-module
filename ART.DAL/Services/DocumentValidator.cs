using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Schema;

namespace ART.DAL.Services
{
   public class DocumentValidator
    {
        XmlSchemaSet schemaSet;
        List<string> errorList = new List<string>();
        public DocumentValidator(string xmlSchemaFilePath)
        {
            schemaSet = new XmlSchemaSet();
            schemaSet.Add(null, xmlSchemaFilePath);
        }

        public List<string> ValidateXMLMessage(string xmlFile, out string patientId)
        {
            XmlSchema compiledSchema = null;
            foreach (XmlSchema schema in schemaSet.Schemas())
            {
                compiledSchema = schema;
            }

            XmlReaderSettings settings = new XmlReaderSettings
            {
                ValidationType = ValidationType.Schema
            };
            settings.Schemas.Add(compiledSchema);

            settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
             settings.ValidationEventHandler += (sender, e) =>
            {
                if (e.Severity == XmlSeverityType.Error)
                {
                    string nodeName = ((XmlReader)sender).Name;

                    errorList.Add(nodeName + "|" + e.Message);
                    //Console.WriteLine("\tValidation error: " + e.Message);
                }
            };

            patientId = "";
            XmlReader reader = XmlReader.Create(xmlFile, settings);
            while (reader.Read())
            {

                if (reader.Name == "PatientIdentifier")
                {
                    patientId = reader.Value;
                }
            }

            reader.Close();

            return errorList;
        }
    }
}
