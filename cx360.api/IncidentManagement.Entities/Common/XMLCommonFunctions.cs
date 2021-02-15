using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace IncidentManagement.Entities.Common
{
    public class XMLCommonFunctions
    {

        public static int goalSortKey = 0;
        public static int objectiveSortKey = 0;
        public static int serviceSortKey = 0;
        public static string extendedSortKey = string.Empty;
        public static IEnumerable<XElement> GenerateTablesXML(DataTable dataTableCANS)
        {
            try
            {
                return from row in dataTableCANS.AsEnumerable()
                       from column in dataTableCANS.Columns.Cast<DataColumn>()
                       where row[column] != DBNull.Value
                       select new XElement(column.ColumnName.ToLower().Replace("$", ""), row[column], new XAttribute("type", GetColumnDataType(column)));
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }
        public static IEnumerable<XElement> GenerateChildTablesXML(DataTable dataTableCANS)
        {
            try
            {
                return from row in dataTableCANS.AsEnumerable()
                       from column in dataTableCANS.Columns.Cast<DataColumn>()
                       where (column != dataTableCANS.Columns[0] && row[column] != DBNull.Value)
                       select new XElement(column.ColumnName.ToLower().Replace("$", "") + row.Field<Int64>(0), row[column], new XAttribute("type", GetColumnDataType(column)));
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }
        public static string GetColumnDataType(DataColumn column)
        {
            string dataType = string.Empty;
            try
            {
                if (column.ColumnName.ToString().Split(new[] { "$" }, StringSplitOptions.None).Length - 1 == 1)
                {
                    dataType = "list";
                }
                else if (column.ColumnName.ToString().Split(new[] { "$" }, StringSplitOptions.None).Length - 1 == 2)
                {
                    dataType = "bool";
                }
                else if (column.ColumnName.ToString().Split(new[] { "$" }, StringSplitOptions.None).Length - 1 == 3)
                {
                    dataType = "str";
                }
                else if (column.ColumnName.ToString().Split(new[] { "$" }, StringSplitOptions.None).Length - 1 == 4)
                {
                    dataType = "date";
                }
                else if (column.ColumnName.ToString().Split(new[] { "$" }, StringSplitOptions.None).Length - 1 == 5)
                {
                    dataType = "int";
                }
                else
                {
                    if (column.DataType == typeof(string))
                    {
                        dataType = "str";
                    }
                    else if (column.DataType == typeof(DateTime))
                    {
                        dataType = "date";
                    }
                    else if (column.DataType == typeof(Int32))
                    {
                        dataType = "int";
                    }
                }
                return dataType;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public static string GetTreatmentPlanSortKey(string planpart,int itemKey,string goalkey,string objectivekey,string servicekey)
        {
            string addIntialZero = "0";
            string sortKeyInitial = string.Empty;
            try
            {
                switch (planpart)
                {
                    case "Goal":
                        sortKeyInitial = addIntialZero + ++goalSortKey;
                        extendedSortKey = (addIntialZero + itemKey+ sortKeyInitial).ToString();
                        break;
                    case "Objective":
                        sortKeyInitial = addIntialZero + ++objectiveSortKey;
                        extendedSortKey =goalkey + sortKeyInitial;
                        break;
                    case "Service":
                        sortKeyInitial = addIntialZero + ++serviceSortKey;
                        extendedSortKey = objectivekey+ sortKeyInitial;
                        break;
                }
                return extendedSortKey;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }
    }
}
