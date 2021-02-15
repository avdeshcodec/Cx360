using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using IncidentManagement.Entities.Common;
namespace IncidentManagement.Entities.XMLGeneration
{
    public class HFSXML
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
        public static string GenerateHFSClientRow(DataSet dataset, string xmlClientRow)
        {

            try
            {


                XDocument xDoc = new XDocument();

                xmlClientRow = GenerateGeneralXML(dataset, xmlClientRow);
                xmlClientRow = GenerateClientInfoXML(dataset, xmlClientRow);
                xmlClientRow = GenerateCANSInfoXML(dataset, xmlClientRow);
                xmlClientRow = GenerateCareGiverAddendumXML(dataset, xmlClientRow);
                xmlClientRow = GenerateDCFSAddendumXML(dataset, xmlClientRow);
                xmlClientRow = GenerateDiagnosisXML(dataset, xmlClientRow);
                xmlClientRow = GenerateTreatmentPlanXML(dataset, xmlClientRow);


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
                hfsXML.Add(new XComment("HFS XML docmumet"));
                XElement XRoot = new XElement("row");
                dataTableGeneral = (dataset.Tables[0].DefaultView).ToTable();
                XRoot.Add(new XElement("header",
                  from row in dataTableGeneral.AsEnumerable()
                  from column in dataTableGeneral.Columns.Cast<DataColumn>()
                  select new XElement(column.ColumnName.ToLower().Replace("$", ""), row[column], column.ColumnName.ToLower().Replace("$", "") == "canstype" ? new XAttribute("type", XMLCommonFunctions.GetColumnDataType(column)) : null)

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
        public static string GenerateClientInfoXML(DataSet dataset, string xmlClientRow)
        {

            DataTable dataTableFamilyEsSupport = new DataTable();
            try
            {
                dataTableFamilyEsSupport = (dataset.Tables[3].DefaultView).ToTable();
                XDocument generalClientInfo = XDocument.Parse(xmlClientRow);
                generalClientInfo.Root.Add(new XElement("clientinfo",
                  XMLCommonFunctions.GenerateTablesXML(dataset.Tables[1]),
                  //FamilyMembers
                  XMLCommonFunctions.GenerateChildTablesXML(dataset.Tables[2]),
                    //EstablishedSupport
                    from row in dataTableFamilyEsSupport.AsEnumerable()
                    from column in dataTableFamilyEsSupport.Columns.Cast<DataColumn>()
                    where column != dataTableFamilyEsSupport.Columns[0]
                    select new XElement(row.Field<string>(0).ToLower() + column.ColumnName.ToLower().Replace("$", ""), row[column], new XAttribute("type", XMLCommonFunctions.GetColumnDataType(column)))
                   ));
                xmlClientRow = generalClientInfo.ToString();
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return xmlClientRow;
        }
        public static string GenerateCANSInfoXML(DataSet dataset, string xmlClientRow)
        {
            DataTable dataTableTraumaExposure = new DataTable();
            try
            {
                dataTableTraumaExposure = (dataset.Tables[4].DefaultView).ToTable();
                XDocument cansInfo = XDocument.Parse(xmlClientRow);
                cansInfo.Root.Add(new XElement("cansinfo",
                    new XComment("trauma"),
                    XMLCommonFunctions.GenerateTablesXML((dataset.Tables[4].DefaultView).ToTable()),
                     new XComment("/trauma"),
                    new XComment("lifefunc"),
                    XMLCommonFunctions.GenerateTablesXML((dataset.Tables[5].DefaultView).ToTable()),
                    new XComment("/lifefunc"),
                    new XComment("safety"),
                    XMLCommonFunctions.GenerateTablesXML((dataset.Tables[6].DefaultView).ToTable()),
                    new XComment("/safety"),
                    new XComment("subuse"),
                    XMLCommonFunctions.GenerateTablesXML((dataset.Tables[7].DefaultView).ToTable()),
                    new XComment("subuse"),
                    new XComment("placementhist"),
                    XMLCommonFunctions.GenerateTablesXML((dataset.Tables[8].DefaultView).ToTable()),
                     new XComment("/placementhist"),
                    new XComment("psychinfo"),
                     XMLCommonFunctions.GenerateTablesXML((dataset.Tables[9].DefaultView).ToTable()),
                    XMLCommonFunctions.GenerateChildTablesXML((dataset.Tables[10].DefaultView).ToTable()),
                    new XComment("psychinfo"),
                     new XComment("clientstrengths"),
                     XMLCommonFunctions.GenerateTablesXML((dataset.Tables[11].DefaultView).ToTable()),
                    new XComment("clientstrengths"),
                     new XComment("/familyinfo"),
                     XMLCommonFunctions.GenerateTablesXML((dataset.Tables[12].DefaultView).ToTable()),
                    new XComment("/familyinfo"),
                     new XComment("/needs"),
                     XMLCommonFunctions.GenerateTablesXML((dataset.Tables[13].DefaultView).ToTable()),
                      XMLCommonFunctions.GenerateTablesXML((dataset.Tables[14].DefaultView).ToTable()),
                      XMLCommonFunctions.GenerateTablesXML((dataset.Tables[15].DefaultView).ToTable()),
                    new XComment("needs"),
                     new XComment("/individtreatment"),
                     XMLCommonFunctions.GenerateTablesXML((dataset.Tables[16].DefaultView).ToTable()),
                    new XComment("/individtreatment"),
                    new XComment("/IMCANS Signatures"),
                     XMLCommonFunctions.GenerateTablesXML((dataset.Tables[17].DefaultView).ToTable()),
                    new XComment("/IMCANS Signatures")
                   ));
                xmlClientRow = cansInfo.ToString();
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return xmlClientRow;
        }
        public static string GenerateCareGiverAddendumXML(DataSet dataset, string xmlClientRow)
        {
            try
            {
                XDocument generalCareGiverAddendum = XDocument.Parse(xmlClientRow);
                generalCareGiverAddendum.Root.Add(new XElement("caregiveraddendum",
                                      XMLCommonFunctions.GenerateTablesXML((dataset.Tables[18].DefaultView).ToTable())
                   ));
                xmlClientRow = generalCareGiverAddendum.ToString();
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return xmlClientRow;
        }
        public static string GenerateDCFSAddendumXML(DataSet dataset, string xmlClientRow)
        {

            try
            {
                XDocument cansInfo = XDocument.Parse(xmlClientRow);
                cansInfo.Root.Add(new XElement("dcfsaddendum",
                    new XComment("geninfo"),
                    XMLCommonFunctions.GenerateTablesXML((dataset.Tables[19].DefaultView).ToTable()),
                     new XComment("/geninfo"),
                    new XComment("sexagg"),
                    XMLCommonFunctions.GenerateTablesXML((dataset.Tables[20].DefaultView).ToTable()),
                    new XComment("/sexagg"),
                    new XComment("pgsafe"),
                    XMLCommonFunctions.GenerateTablesXML((dataset.Tables[21].DefaultView).ToTable()),
                    new XComment("/pgsafe"),
                    new XComment("pgwell"),
                    XMLCommonFunctions.GenerateTablesXML((dataset.Tables[22].DefaultView).ToTable()),
                    new XComment("/pgwell"),
                    new XComment("pgperm"),
                    XMLCommonFunctions.GenerateTablesXML((dataset.Tables[23].DefaultView).ToTable()),
                    new XComment("/pgperm"),
                    new XComment("subcomitperm"),
                     XMLCommonFunctions.GenerateTablesXML((dataset.Tables[24].DefaultView).ToTable()),
                    new XComment("/subcomitperm"),
                     new XComment("intact"),
                     XMLCommonFunctions.GenerateTablesXML((dataset.Tables[25].DefaultView).ToTable()),
                    new XComment("/intact"),
                     new XComment("ips"),
                     XMLCommonFunctions.GenerateTablesXML((dataset.Tables[26].DefaultView).ToTable()),
                    new XComment("/ips")

                   ));
                xmlClientRow = cansInfo.ToString();
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return xmlClientRow;
        }
        public static string GenerateDiagnosisXML(DataSet dataset, string xmlClientRow)
        {
            DataTable dataTableDiagnosis = new DataTable();
            try
            {


                dataTableDiagnosis = (dataset.Tables[27].DefaultView).ToTable();
                XDocument docDiagnosis = XDocument.Parse(xmlClientRow);
                foreach (DataRow row in dataTableDiagnosis.AsEnumerable())
                {
                    XElement rd = new XElement("diagnosis", new object[] {
                     new XElement("icd10code", row["icd10code"],new XAttribute("type", "str")),
                    new XElement("icd10desccode", row["icd10desccode"],new XAttribute("type", "str")),
                    new XElement("preventivediagnosis", row["preventivediagnosis"],new XAttribute("type", "bool")),

                    });

                    docDiagnosis.Root.Add(rd);
                }

                xmlClientRow = docDiagnosis.ToString();
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return xmlClientRow;
        }

        public static string GenerateTreatmentPlanXML(DataSet dataset, string xmlClientRow)
        {
            DataTable dataTableTreatmentPlanItem = new DataTable();
            DataTable dataTableGoal = new DataTable();
            DataTable dataTableObjective = new DataTable();
            DataTable dataTableService = new DataTable();
            int treatmentPlanItemSortKey = 0;
            string goalSortKey = string.Empty;
            string objectivesortkey = string.Empty;
            string servicesortkey = string.Empty;
            int treatmentPlanItemId = -1;
            int goalId = -1;
            int objectiveid = -1;
            int i = 1;
            try
            {
                dataTableTreatmentPlanItem = (dataset.Tables[29].DefaultView).ToTable();
                XDocument docTreatmentPlan = XDocument.Parse(xmlClientRow);
                foreach (DataRow row in dataTableTreatmentPlanItem.AsEnumerable())
                {
                    treatmentPlanItemSortKey =  i++;
                    treatmentPlanItemId = Convert.ToInt32(row["CansTreatmentPlanItemID"]);
                    XElement rd = new XElement("treatmentplan", new object[] {
                     new XElement("planrefnum", row["planrefnum"],new XAttribute("type", "str")),
                     new XElement("planpart", row["planpart"],new XAttribute("type", "list")),
                    new XElement("plansortkey", 0.ToString()+treatmentPlanItemSortKey,new XAttribute("type", "str")),
                    new XElement("plandetail", row["plandetail"],new XAttribute("type", "str")),
                    });
                    docTreatmentPlan.Root.Add(rd);
                    var goalData = dataset.Tables[30].Select("CansTreatmentPlanItemID=" + treatmentPlanItemId + "");
                    if (goalData.Any())
                    {
                        dataTableGoal = goalData.CopyToDataTable();
                    }
                    else
                    {
                        dataTableGoal.Clear();
                    }
                    foreach (DataRow goalrow in dataTableGoal.AsEnumerable())
                    {

                         goalSortKey = XMLCommonFunctions.GetTreatmentPlanSortKey("Goal", treatmentPlanItemSortKey, "", "", "");
                        goalId = Convert.ToInt32(goalrow["CansTreatmentPlanGoalID"]);
                        XElement goalNode = new XElement("treatmentplan", new object[] {
                            new XElement("planrefnum",goalrow["planrefnum"],new XAttribute("type", "str")),
                            new XElement("planpart", goalrow["planpart"],new XAttribute("type", "list")),
                            new XElement("plansortkey",goalSortKey,new XAttribute("type", "str")),
                            new XElement("plandetail", goalrow["plandetail"],new XAttribute("type", "str")),
                            new XElement("goalstatus",goalrow["GoalStatus"],new XAttribute("type", "list")),
                            new XElement("goaldate",goalrow["CompletedDate"],new XAttribute("type", "date"))
                        });
                        docTreatmentPlan.Root.Add(goalNode);
                        var objectiveData = dataset.Tables[31].Select("CansTreatmentPlanGoalID=" + goalId + "");
                        if (objectiveData.Any())
                        {
                            dataTableObjective = objectiveData.CopyToDataTable();
                        }
                        else
                        {
                            dataTableObjective.Clear();
                        }
                        foreach (DataRow objrow in dataTableObjective.AsEnumerable())
                        {

                            objectivesortkey = XMLCommonFunctions.GetTreatmentPlanSortKey("Objective", treatmentPlanItemSortKey, goalSortKey, "", "");
                            objectiveid = Convert.ToInt32(objrow["CansTreatmentPlanObjctiveID"]);
                            XElement objNode = new XElement("treatmentplan", new object[] {
                            new XElement("planrefnum",objrow["planrefnum"],new XAttribute("type", "str")),
                            new XElement("planpart", objrow["planpart"],new XAttribute("type", "list")),
                            new XElement("plansortkey",objectivesortkey,new XAttribute("type", "str")),
                            new XElement("plandetail", objrow["plandetail"],new XAttribute("type", "str")),
                            });
                            docTreatmentPlan.Root.Add(objNode);
                            var rowdata = dataset.Tables[32].Select("CansTreatmentPlanObjctiveID=" + objectiveid + "");
                            if (rowdata.Any())
                            {
                                dataTableService = rowdata.CopyToDataTable();
                            }
                            else
                            {
                                dataTableService.Clear();
                            }
                            foreach (DataRow serviceRow in dataTableService.AsEnumerable())
                            {

                                servicesortkey = XMLCommonFunctions.GetTreatmentPlanSortKey("Service", treatmentPlanItemSortKey,goalSortKey, objectivesortkey, "");
                                XElement serviceNode = new XElement("treatmentplan", new object[] {
                                new XElement("planrefnum",serviceRow["planrefnum"],new XAttribute("type", "str")),
                                new XElement("planpart", serviceRow["planpart"],new XAttribute("type", "list")),
                                new XElement("plandetail", serviceRow["plandetail"],new XAttribute("type", "str")),
                                new XElement("plansortkey",servicesortkey,new XAttribute("type", "str")),
                                new XElement("servicetype",serviceRow["ServiceType"],new XAttribute("type", "list")),
                                new XElement("servicemode",serviceRow["ServiceMode"],new XAttribute("type", "list")),
                                new XElement("serviceplace",serviceRow["ServicePlace"],new XAttribute("type", "list")),
                                new XElement("servicefrequency",serviceRow["ServiceFrequency"],new XAttribute("type", "list")),
                                 new XElement("serviceagcystaff",serviceRow["serviceagcystaff"],new XAttribute("type", "str")),
                                 });
                                docTreatmentPlan.Root.Add(serviceNode);
                                
                               
                            }
                            XMLCommonFunctions.serviceSortKey = 0;

                        }
                        XMLCommonFunctions.objectiveSortKey = 0;

                    }
                    XMLCommonFunctions.goalSortKey = 0;

                    dataTableGoal.Clear();
                      
                }
                xmlClientRow = docTreatmentPlan.ToString();
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return xmlClientRow;
        }
    }
}
