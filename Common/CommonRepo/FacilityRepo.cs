using Common;
using Common.CommonEntities;
using Common.CommonRepo;
using Common.DBFasade;
using Common.Utility;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Common.ViewModels;
using OfficeOpenXml;

namespace Common.CommonRepo
{
    public class FacilityRepo : BaseDAO<OnboardedFacility, int>
    {
        public Dictionary<string, string> RetriveStateCoding()
        {
            //var cmd = new SqlCommand("select state_name, map_code from states");
            return RetrieveAsDataTable("select state_name, map_code from admin_states")
                .AsEnumerable()
                .ToDictionary(row => row.Field<string>(0), row => row.Field<string>(1));
        }

        public OnboardedFacility RetrievebyName(string name)
        {
            OnboardedFacility sdf = null;
            ISession session = BuildSession();

            ICriteria criteria = session.CreateCriteria<OnboardedFacility>().Add(Restrictions.Eq("FacilityName", name));
            sdf = criteria.UniqueResult<OnboardedFacility>();

            return sdf;
        }

        public List<IPLGAFacility> GetDashBoardFilter(string IP)
        {
            string sql = "select ip.ShortName as IP, obf.FacilityName, obf.DatimFacilityCode, l.lga_code from admin_IPFacility ipf, admin_ImplementingPartners ip, admin_OnboardedFacility obf, admin_lga l where ipf.IP = ip.Id and ipf.FacilityId = obf.Id and obf.LGA = l.lga_code";
            if (!string.IsNullOrEmpty(IP))
            {
                sql = string.Format("{0} and ip.ShortName='{1}'", sql, IP);
            }

            //var cmd = new SqlCommand(sql);
            var data = RetrieveAsDataTable(sql);

            List<IPLGAFacility> IPLocation = new List<IPLGAFacility>();

            var lgas = new LGADAO().RetrieveAllLazily().ToDictionary(x => x.lga_code);

            foreach (DataRow dr in data.Rows)
            {
                string lga = Convert.ToString(dr[3]);
                IPLocation.Add(new IPLGAFacility
                {
                    IP = Convert.ToString(dr[0]),
                    FacilityName = Convert.ToString(dr[1]),
                    DatimFacilityCode = Convert.ToString(dr[2]),
                    LGA = lgas[lga]
                });
            }
            return IPLocation.Distinct().ToList();
        }

        public void BulkInsertWithStatelessSession(List<OnboardedFacility> facilities, List<IPFacility> IPFacility)
        {
            using (var session = BuildSession().SessionFactory.OpenStatelessSession())
            using (var tx = session.BeginTransaction())
            {
                foreach (var fac in facilities)
                {
                    session.Insert(fac);
                }
                foreach (var o in IPFacility)
                {
                    session.Insert(o);
                }
                tx.Commit();
            }
        }


        public void UpdateWithStateLessSession(List<OnboardedFacility> facilities)
        {
            using (var session = BuildSession().SessionFactory.OpenStatelessSession())
            using (var tx = session.BeginTransaction())
            {
                foreach (var fac in facilities)
                {
                    session.Update(fac);
                }
                tx.Commit();
            }
        }


        public string OnBoardFacility(Stream fileStream) // string[] theLines)
        {
            List<string> err = new List<string>();
            try
            {
                var facilityDAO = new BaseDAO<OnboardedFacility, int>();
                var existingFacilities = facilityDAO.RetrieveAllLazily().ToDictionary(x => x.DatimFacilityCode);
                var IPs = new IPRepo().RetrieveAllLazily().ToDictionary(x => x.ShortName); //.SearchByShortName(IP);
                var lgas = new BaseDAO<LGA, int>().RetrieveAllLazily().ToList();//.ToDictionary(x => x.lga_code.ToLower());

                var ipfacilities = new BaseDAO<IPFacility, int>().RetrieveAllLazily().Where(x => x.IP.ShortName == "FHI360").ToList();

                List<OnboardedFacility> hfs = new List<OnboardedFacility>();

                List<OnboardedFacility> previously = new List<OnboardedFacility>();

                List<IPFacility> IPfacility = new List<IPFacility>();
                ImplementingPartners ip = null;
                using (ExcelPackage package = new ExcelPackage(fileStream))
                {
                    var mainWorksheet = package.Workbook.Worksheets.FirstOrDefault();

                    int row = 2;
                    while (true)
                    {
                        if (string.IsNullOrEmpty(mainWorksheet.Cells["B" + row].Text)) //if no IP, then assume end of file
                            break;

                        string datimcode = mainWorksheet.Cells["J" + row].Text;
                        string LocalFacilityID = mainWorksheet.Cells["H" + row].Text;
                        string facilityName = mainWorksheet.Cells["E" + row].Text;
                        if (string.IsNullOrEmpty(datimcode))
                        {
                            err.Add("no DATIM code supplied for s/n " + mainWorksheet.Cells["A" + row].Text);
                            row++;
                            continue;
                        }
                        if (string.IsNullOrEmpty(facilityName))
                        {
                            err.Add("No Facility Name for s/n " + mainWorksheet.Cells["A" + row].Text);
                            row++;
                            continue;
                        }

                        ip = null;
                        IPs.TryGetValue(mainWorksheet.Cells["B" + row].Text, out ip);

                        if (ip == null)
                        {
                            throw new ApplicationException("Invalid IP supplied for s/n " + mainWorksheet.Cells["A" + row].Text);
                        }

                        OnboardedFacility facility = null;
                        existingFacilities.TryGetValue(datimcode, out facility);
                        if (facility == null)
                        {
                            string Latitude = mainWorksheet.Cells["F" + row].Text;
                            string Longitude = mainWorksheet.Cells["G" + row].Text;
                            string NationalID = mainWorksheet.Cells["I" + row].Text;

                            string state_lga = mainWorksheet.Cells["C" + row].Text;
                            string lga_name = mainWorksheet.Cells["D" + row].Text;

                            LGA lga = LGADAO.FindLGA(lgas, lga_name, state_lga);
                            //lgas.TryGetValue(lga_code, out lga);

                            if (lga == null)
                            {
                                err.Add("Error reading the State/LGA, check spelling and re-upload " + state_lga + "/" + lga_name);
                            }
                            else
                            {
                                facility = new OnboardedFacility
                                {
                                    FacilityID = LocalFacilityID,
                                    FacilityTypeCode = FacilityTypeFacilityTypeCode.FAC,
                                    FacilityName = facilityName,
                                    LGA = lga,
                                    Longitude = Longitude,
                                    Latitutde = Latitude,
                                    DatimFacilityCode = datimcode,
                                    NationalID = NationalID,
                                };
                                hfs.Add(facility);
                                IPfacility.Add(new IPFacility
                                {
                                    Facility = facility,
                                    IP = ip,
                                    IsActive = true
                                });
                            }
                        }
                        else
                        {
                            string Latitude = mainWorksheet.Cells["F" + row].Text;
                            string Longitude = mainWorksheet.Cells["G" + row].Text;
                            string NationalID = mainWorksheet.Cells["I" + row].Text;

                            string state_lga = mainWorksheet.Cells["C" + row].Text;
                            string lga_name = mainWorksheet.Cells["D" + row].Text;

                            LGA lga = LGADAO.FindLGA(lgas, lga_name, state_lga);

                            if (lga == null)
                            {
                                err.Add("Error reading the State/LGA, check spelling and re-upload " + state_lga + "/" + lga_name);
                            }
                            else
                            {
                                facility.FacilityTypeCode = FacilityTypeFacilityTypeCode.FAC;
                                facility.LGA = lga;

                                if (!string.IsNullOrEmpty(LocalFacilityID))
                                    facility.FacilityID = LocalFacilityID;

                                if (!string.IsNullOrEmpty(facilityName))
                                    facility.FacilityName = facilityName;
                                if (!string.IsNullOrEmpty(Longitude))
                                    facility.Longitude = Longitude;
                                if (!string.IsNullOrEmpty(Latitude))
                                    facility.Latitutde = Latitude;
                                //if (!string.IsNullOrEmpty(datimcode))
                                //    facility.DatimFacilityCode = datimcode;
                                if (!string.IsNullOrEmpty(NationalID))
                                    facility.NationalID = NationalID;

                                previously.Add(facility);
                            }
                        }
                        row++;
                    }
                }


                try
                {
                    var merge = new List<OnboardedFacility>();
                    merge.AddRange(hfs);
                    merge.AddRange(previously);

                    //chinedu mentioned to ignore this column
                    //merge.ToDictionary(x => x.FacilityID); //check for duplicate local facility id
                    merge.ToDictionary(x => x.DatimFacilityCode); //check for duplicate datim code
                }
                catch
                {
                    throw new ApplicationException("Duplicate Ids found. Ensure that there is no duplicate on Local Facility id and Datim code");
                }

                BulkInsertWithStatelessSession(hfs, IPfacility);

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

        public string MarkSiteAsGranular(Stream fileStream)
        {
            List<string> err = new List<string>();
            try
            {
                var existingFacilities = RetrieveAllLazily().ToDictionary(x => x.DatimFacilityCode);

                List<OnboardedFacility> previously = new List<OnboardedFacility>();
                using (ExcelPackage package = new ExcelPackage(fileStream))
                {
                    var mainWorksheet = package.Workbook.Worksheets.FirstOrDefault();

                    int row = 2;
                    while (true)
                    {
                        if (string.IsNullOrEmpty(mainWorksheet.Cells["A" + row].Text)) //if no IP, then assume end of file
                            break;

                        string datimcode = mainWorksheet.Cells["A" + row].Text;
                        string facilityName = mainWorksheet.Cells["B" + row].Text;
                        if (string.IsNullOrEmpty(datimcode))
                        {
                            err.Add("no DATIM code supplied for s/n " + mainWorksheet.Cells["A" + row].Text);
                            row++;
                            continue;
                        }
                        if (string.IsNullOrEmpty(facilityName))
                        {
                            err.Add("No Facility Name for s/n " + mainWorksheet.Cells["A" + row].Text);
                            row++;
                            continue;
                        }
                        if (existingFacilities.TryGetValue(datimcode, out OnboardedFacility facility))
                        {
                            facility.GranularSite = true;
                            previously.Add(facility);
                        }
                        else
                        {
                            err.Add("Invalid Facility DATIM Code | " + datimcode);
                        }
                        row++;
                    }
                }
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