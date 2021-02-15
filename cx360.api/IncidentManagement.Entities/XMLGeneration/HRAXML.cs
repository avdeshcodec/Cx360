using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using IncidentManagement.Entities.Common;
namespace IncidentManagement.Entities.XMLGeneration
{
   public class HRAXML
    {
        public static string GenerateHeaderTrailer(DataSet dataset, string importedXML)
        {
            DataTable dataTableHeader = new DataTable();
            DataTable dataTableTrailer = new DataTable();
            try
            {
                dataTableHeader = (dataset.Tables[2].DefaultView).ToTable();
                dataTableTrailer = (dataset.Tables[3].DefaultView).ToTable();
                XDocument hfsXML = new XDocument();
                hfsXML.Add(new XComment("HFS XML docmumet"));
                XElement XRoot = new XElement("root");
                XRoot.Add(new XElement("header",
                   from row in dataTableHeader.AsEnumerable()
                   from column in dataTableHeader.Columns.Cast<DataColumn>()
                   select new XElement(column.ColumnName.ToLower(), row[column])

                    ));
                XRoot.Add(new XElement("trailer",
                   from row in dataTableTrailer.AsEnumerable()
                   from column in dataTableTrailer.Columns.Cast<DataColumn>()
                   select new XElement(column.ColumnName.ToLower(), row[column])

                    ));
                hfsXML.Add(XRoot);
                importedXML = hfsXML.ToString();



            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return importedXML;
        }
        public static string GenerateHRAClientRow(DataSet dataset, string xmlClientRow)
        {

            try
            {
                XDocument xDoc = new XDocument();
                xmlClientRow = GenerateGeneralXML(dataset, xmlClientRow);
                xmlClientRow = GenerateHRAInfoXML(dataset, xmlClientRow);
               
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return xmlClientRow;
        }
        public static string GenerateGeneralXML(DataSet dataset, string xmlClientRow)
        {
            DataTable dataTableGeneral = new DataTable();
            try
            {

                XDocument hfsXML = new XDocument();
                hfsXML.Add(new XComment("HRA XML docmumet"));
                XElement XRoot = new XElement("row");
                dataTableGeneral = (dataset.Tables[0].DefaultView).ToTable();
                XRoot.Add(new XElement("header",
                  from row in dataTableGeneral.AsEnumerable()
                  from column in dataTableGeneral.Columns.Cast<DataColumn>()
                  where column!= dataTableGeneral.Columns[0]
                  select new XElement(column.ColumnName.ToLower().Replace("$", ""), row[column])

                   ));
                hfsXML.Add(XRoot);
                xmlClientRow = hfsXML.ToString();
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return xmlClientRow;
        }
        public static string GenerateHRAInfoXML(DataSet dataset, string xmlClientRow)
        {
            DataTable dataTableCansType = new DataTable();
            try
            {
                dataTableCansType = (dataset.Tables[0].DefaultView).ToTable();
                XDocument cansInfo = XDocument.Parse(xmlClientRow);
                cansInfo.Root.Add(new XElement("hrainfo",
                     from row in dataTableCansType.AsEnumerable()
                     from column in dataTableCansType.Columns.Cast<DataColumn>()
                     where column!= dataTableCansType.Columns[1] && column != dataTableCansType.Columns[2]
                     select new XElement(column.ColumnName.ToLower().Replace("$", ""), row[column], new XAttribute("type", XMLCommonFunctions.GetColumnDataType(column))),

                    new XComment("<geninfo>"),
                    XMLCommonFunctions.GenerateTablesXML((dataset.Tables[1].DefaultView).ToTable()),
                     new XComment("</geninfo>"),
                    new XComment("<medication>"),
                    XMLCommonFunctions.GenerateTablesXML((dataset.Tables[2].DefaultView).ToTable()),
                     XMLCommonFunctions.GenerateChildTablesXML((dataset.Tables[3].DefaultView).ToTable()),
                    new XComment("</medication>"),
                    new XComment("<healthstatus>"),
                    XMLCommonFunctions.GenerateTablesXML((dataset.Tables[4].DefaultView).ToTable()),
                    new XComment("</healthstatus>"),
                     new XComment("<developmentstatus>"),
                    XMLCommonFunctions.GenerateTablesXML((dataset.Tables[5].DefaultView).ToTable()),
                    new XComment("</developmentstatus>"),
                    new XComment("<medicalhistory>"),
                    XMLCommonFunctions.GenerateTablesXML((dataset.Tables[6].DefaultView).ToTable()),
                      XMLCommonFunctions.GenerateChildTablesXML((dataset.Tables[7].DefaultView).ToTable()),
                       XMLCommonFunctions.GenerateChildTablesXML((dataset.Tables[8].DefaultView).ToTable()),
                       XMLCommonFunctions.GenerateChildTablesXML((dataset.Tables[9].DefaultView).ToTable()),

                    new XComment("</medicalhistory>")
                   ));
                xmlClientRow = cansInfo.ToString();
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return xmlClientRow;
        }

    }
}
