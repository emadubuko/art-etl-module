using Common.CommonEntities;
using Common.DBFasade;
using Common.Utility;
using NHibernate;
using NHibernate.Criterion;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Common.CommonRepo
{
    public class LabRepo : BaseDAO<Laboratory, int>
    {
        public Laboratory RetrievebyName(string name)
        {
            Laboratory lab = null;
            ISession session = BuildSession();

            ICriteria criteria = session.CreateCriteria<Laboratory>()
                .Add(Restrictions.Eq("LaboratoryName", name));
            lab = criteria.UniqueResult<Laboratory>();

            return lab;
        }

        public void BulkInsertWithStatelessSession(List<Laboratory> labs)
        {
            using (var session = BuildSession().SessionFactory.OpenStatelessSession())
            using (var tx = session.BeginTransaction())
            {
                foreach (var fac in labs)
                {
                    session.Insert(fac);
                }
                tx.Commit();
            }
        }


        public void UpdateWithStateLessSession(List<Laboratory> labs)
        {
            using (var session = BuildSession().SessionFactory.OpenStatelessSession())
            using (var tx = session.BeginTransaction())
            {
                foreach (var fac in labs)
                {
                    session.Update(fac);
                }
                tx.Commit();
            }
        }


        public string OnBoardlab(Stream fileStream)
        {
            List<string> err = new List<string>();
            try
            {

                var existinglabs = RetrieveAllLazily().ToDictionary(x => x.LabUniqueID);
                var lgas = new LGADAO().RetrieveAllLazily().ToList();

                var IpRepo = new IPRepo();

                List<Laboratory> labs = new List<Laboratory>();
                List<Laboratory> previously = new List<Laboratory>();

                using (ExcelPackage package = new ExcelPackage(fileStream))
                {
                    var mainWorksheet = package.Workbook.Worksheets.FirstOrDefault();

                    int row = 2;
                    while (true)
                    {
                        string ipname = mainWorksheet.Cells["B" + row].Text;

                        if (string.IsNullOrEmpty(ipname))
                            break;
                         
                        string state = mainWorksheet.Cells["C" + row].Text;
                        string lga_name = mainWorksheet.Cells["D" + row].Text;

                        string LabName = mainWorksheet.Cells["E" + row].Text;
                        string Latitude = mainWorksheet.Cells["F" + row].Text;
                        string Longitude = mainWorksheet.Cells["G" + row].Text;                        
                        string UniqueID = mainWorksheet.Cells["H" + row].Text;  
                       
                        string servicesOffered = mainWorksheet.Cells["I" + row].Text;
                        string funder = mainWorksheet.Cells["J" + row].Text;
                        
                        if (string.IsNullOrEmpty(UniqueID))
                        {
                            err.Add("no UniqueId supplied for s/n " + mainWorksheet.Cells["A" + row].Text);
                            row++;
                            continue;
                        }

                        if (string.IsNullOrEmpty(LabName))
                        {
                            err.Add("No Lab Name for s/n " + mainWorksheet.Cells["A" + row].Text);
                            row++;
                            continue;
                        }

                        LGA lga = LGADAO.FindLGA(lgas, lga_name, state);

                        if (lga == null)
                        {
                            err.Add("Error reading the State/LGA, check spelling and re-upload " + state + "/" + lga_name);
                        }

                        var ip = IpRepo.SearchByShortName(ipname);
                        if(ip == null)
                        {
                            err.Add("Unknown Ip, check spelling and re-upload - |" + ipname);
                        }

                        Laboratory lab = null;
                        if (!existinglabs.TryGetValue(UniqueID, out lab))
                        {
                            lab = new Laboratory
                            {
                                Organization = ip,
                                LabUniqueID = UniqueID, 
                                LaboratoryName = LabName,
                                LGA = lga,
                                Longitude = Longitude,
                                Latitutde = Latitude,
                                Funder = funder,
                                ServicesOffered = servicesOffered,
                            };
                            labs.Add(lab);
                        }
                        else
                        {
                            lab.LGA = lga;
                            lab.Organization = ip;
                            if (!string.IsNullOrEmpty(UniqueID))
                                lab.LabUniqueID = UniqueID;

                            if (!string.IsNullOrEmpty(LabName))
                                lab.LaboratoryName = LabName;
                            if (!string.IsNullOrEmpty(Longitude))
                                lab.Longitude = Longitude;
                            if (!string.IsNullOrEmpty(Latitude))
                                lab.Latitutde = Latitude;

                            if (!string.IsNullOrEmpty(funder))
                                lab.Funder = funder;

                            if (!string.IsNullOrEmpty(servicesOffered))
                                lab.ServicesOffered = servicesOffered;

                            previously.Add(lab);
                        }
                        row++;
                    }
                }

                try
                {
                    var merge = new List<Laboratory>();
                    merge.AddRange(labs);
                    merge.AddRange(previously);
                    merge.ToDictionary(x => x.LabUniqueID); //check for duplicate code. It will throw error if not unique
                }
                catch
                {
                    throw new ApplicationException("Duplicate Ids found. Ensure that there is no duplicates on the Unique Id column");
                }

                BulkInsertWithStatelessSession(labs);

                UpdateWithStateLessSession(previously);

            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return ex.Message;
            }
            err.ForEach(x => Logger.LogInfo("", x));

            return string.Join("<br />", err);
        }

    }

}
