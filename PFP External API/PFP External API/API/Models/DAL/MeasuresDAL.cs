namespace API.Models.DAL
{
    using API.Models.EDM;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Data.Entity;

    public class MeasuresDAL
    {
        private readonly PFPEntities db = new PFPEntities();

        internal List<HospitalVM> GetHospitalsData(string email = "")
        {
            var hospitals = new List<Hospital>();
            if (email != null && email.Length > 0)
            {
                var userHosName = db.Users.Where(u => u.USR_Email == email)?.FirstOrDefault()?.USR_OrganizationName?.Replace(" ", "")?.ToLower() ?? string.Empty;
                var Hoss = db.Hospitals.Where(h => h.HOS_Active == true);
                var HosUniqueId = Hoss.Where(h => h.HOS_Active == true && h.HOS_HospitalName.Replace(" ", "").ToLower() == userHosName)?.FirstOrDefault()?.HOS_Unique_Id?.Replace(" ", "")?.ToLower() ?? string.Empty;
                hospitals = Hoss.Where(h => h.HOS_Active == true && (h.HOS_HospitalName.Replace(" ", "").ToLower() == userHosName || h.HOS_Unique_Id.Replace(" ", "").ToLower() == HosUniqueId || h.HOS_Parent_Id.Replace(" ", "").ToLower() == HosUniqueId)).ToList(); //.Select(h => new HospitalVM() { Id = h.HOS_Id, HospitalName = h.HOS_HospitalName }).ToList();
                return Komparer(hospitals);
            }
            else
            {
                hospitals = db.Hospitals.Where(h => h.HOS_Active == true).ToList(); //.Select(h => new HospitalVM() { Id = h.HOS_Id, HospitalName = h.HOS_HospitalName }).OrderBy(h => h.HospitalName).ToList();
                return Komparer(hospitals);
            }            
        }

        public IQueryable<MeasuresEventTypes> GetMeasuresEventTypes(bool DisplayAll = false)
        {
            var emm = db.EventMeasureMasters.Include(e => e.EventTypeMaster).Include(e => e.MeasureMaster);
            emm = DisplayAll ? emm : emm.Where(d => d.EMM_DisplayInApp == true);
            return emm.Select(m => new MeasuresEventTypes() { EMM_Id = m.EMM_Id, Measure = m.MeasureMaster.MEM_Measure, EventType = m.EventTypeMaster.EVM_EventType, TimePeriodType = m.EMM_TimePeriod }).OrderBy(e => e.EventType);
        }

        internal IQueryable<Years> GetYears(int FromYear)
        {
            var Years = db.CalendarMasters.Where(c => c.CAL_Active == true).GroupBy(y => new { y.CAL_Year }).Select(c => c.FirstOrDefault());
            if (FromYear > 0)
                return Years.Where(c => c.CAL_Year >= FromYear).Select(c => new Years { Year = c.CAL_Year }).OrderByDescending(c => c.Year);
            else
            {
                return Years.Select(c => new Years { Year = c.CAL_Year }).OrderByDescending(c => c.Year);
            }
        }

        public List<EventMeasureData> GetMeasuresData(int HosId, int FromYear, string TimePeriod)
        {
            var eventMeasureDatas = new List<EventMeasureData>();
            IEnumerable<MeasurementData> measurementDatas = new List<MeasurementData>() { new MeasurementData() };
            if (HosId > 0 && FromYear > 0)
            {
                var IsQuarterlyOrMonthlyFlag = TimePeriod == "q" ? "q" : "m";
                var etms = db.EventMeasureMasters.Where(etm => etm.EMM_Active == true && etm.EMM_DisplayInApp == true && etm.EMM_TimePeriod.Replace(" ", "").ToLower().StartsWith(IsQuarterlyOrMonthlyFlag))
                    .Include(etm => etm.EventTypeMaster).Include(etm => etm.MeasureMaster).OrderBy(etm => etm.EventTypeMaster.EVM_EventType).ToList();

                foreach (var etm in etms)
                {
                    var eventMeasureData = new EventMeasureData
                    {
                        EventDataId = etm.EMM_Id,
                        EventType = etm.EventTypeMaster.EVM_EventType,
                        MeasureName = etm.MeasureMaster.MEM_Measure,
                        OrderBy = etm.EMM_Id
                    };

                    var cals = new List<CalendarMaster>();
                    switch(TimePeriod)
                    {
                        case "mq1":
                            cals = db.CalendarMasters.Where(y => y.CAL_Year == FromYear && (y.CAL_Month == 1 || y.CAL_Month == 2 || y.CAL_Month == 3)).ToList();
                            break;
                        case "mq2":
                            cals = db.CalendarMasters.Where(y => y.CAL_Year == FromYear && (y.CAL_Month == 4 || y.CAL_Month == 5 || y.CAL_Month == 6)).ToList();
                            break;
                        case "mq3":
                            cals = db.CalendarMasters.Where(y => y.CAL_Year == FromYear && (y.CAL_Month == 7 || y.CAL_Month == 8 || y.CAL_Month == 9)).ToList();
                            break;
                        case "mq4":
                            cals = db.CalendarMasters.Where(y => y.CAL_Year == FromYear && (y.CAL_Month == 10 || y.CAL_Month == 11 || y.CAL_Month == 12)).ToList();
                            break;
                        case "q":
                            cals = db.CalendarMasters.Where(y => y.CAL_Year == FromYear && (y.CAL_Month == 1 || y.CAL_Month == 4 || y.CAL_Month == 7 || y.CAL_Month == 10)).ToList();
                            foreach (var c in cals)
                                c.CAL_MonthText = c.CAL_Month == 1 ? "Q1" : c.CAL_Month == 4 ? "Q2" : c.CAL_Month == 7 ? "Q3" : c.CAL_Month == 10 ? "Q4" : c.CAL_MonthText;
                            break;
                        default:
                            cals = db.CalendarMasters.Where(y => y.CAL_Year == FromYear && (y.CAL_Month == 1 || y.CAL_Month == 2 || y.CAL_Month == 3)).ToList();
                            break;
                    }

                    List<NYSPFPData> pfpDatas = new List<NYSPFPData>();
                    foreach (var cal in cals)
                        pfpDatas.AddRange(db.NYSPFPDatas.Where(e => e.NPD_HOS_Id == HosId && e.NPD_Cal_Id == cal.CAL_Id && e.NPD_EMM_Id == etm.EMM_Id).ToList());

                    measurementDatas = from c1 in cals
                                      join p1 in pfpDatas on c1.CAL_Id equals p1.NPD_Cal_Id into ll
                                      from l1 in ll.DefaultIfEmpty()
                                      select new MeasurementData()
                                      {
                                          MeasurementDataId = string.Format("{0}_{1}_{2}", HosId, c1.CAL_Id, etm.EMM_Id),
                                          HosId = HosId,
                                          CalId = c1.CAL_Id,
                                          EmmId = etm.EMM_Id,
                                          MonthYear = string.Format("{0}-{1}", c1?.CAL_MonthText ?? string.Empty, c1?.CAL_Year.ToString() ?? string.Empty),
                                          Numerator = l1?.NPD_Numerator ?? 0.0m,
                                          Denominator = l1?.NPD_Denominator ?? 0.0m,
                                          Multiplier = etm.MeasureMaster.MEM_Multiplier,
                                          Measurement = l1?.NPD_Measurement ?? 0,
                                          OrderBy = c1?.CAL_OrderBy ?? 1
                                      };
                    eventMeasureData.MeasurementDatas = measurementDatas.ToList();
                    eventMeasureDatas.Add(eventMeasureData);
                }
            }

            return eventMeasureDatas;
        }
        public MeasuresData GetMeasuresData(int id, int HosId, int FromYear)
        {
            var md = new MeasuresData();
            var emm = db.EventMeasureMasters.Where(e => e.EMM_Id == id).Include(m => m.MeasureMaster).Include(e => e.EventTypeMaster).FirstOrDefault();
            if (emm != null && emm.EMM_DisplayInApp)
            {
                md = new MeasuresData
                {
                    MeasuresDataId = string.Format("{1}_{0}", id, HosId),
                    Measure = emm?.MeasureMaster.MEM_Measure ?? string.Empty,
                    EventType = emm?.EventTypeMaster.EVM_EventType ?? string.Empty,
                    TimePeriodType = emm?.EMM_TimePeriod ?? string.Empty,
                    OrgId = HosId.ToString()
                };

                if(HosId > 0 && FromYear > 0)
                {
                    var cals = db.CalendarMasters.Where(y => y.CAL_Year == FromYear).ToList();
                    if (cals.Count() > 0)
                    {
                        var pfps = db.NYSPFPDatas.Where(e => e.NPD_HOS_Id == HosId
                        && e.NPD_EMM_Id == id
                        && e.CalendarMaster.CAL_Year == FromYear)
                        .Include(e => e.EventMeasureMaster)
                        .Include(e => e.EventMeasureMaster.MeasureMaster)
                        .Include(e => e.AnalysisPeriodMaster)
                        .ToList();

                        if ((emm?.EMM_TimePeriod ?? string.Empty).Replace(" ", "").ToLower().StartsWith("q"))
                        {
                            cals = db.CalendarMasters.Where(y => y.CAL_Year == FromYear && (y.CAL_Month == 1 || y.CAL_Month == 4 || y.CAL_Month == 7 || y.CAL_Month == 10)).ToList();
                            foreach (var c in cals)
                                c.CAL_MonthText = c.CAL_Month == 1 ? "Q1" : c.CAL_Month == 4 ? "Q2" : c.CAL_Month == 7 ? "Q3" : c.CAL_Month == 10 ? "Q4" : c.CAL_MonthText;
                        }
                        IEnumerable<Measure> measures = new List<Measure>();
                        List<AnalysisPeriod> apms = db.AnalysisPeriodMasters.Where(a => a.APM_Active == true).Select(p => new AnalysisPeriod { AnalysisPeriodId = p.APM_Id, AnalysisPeriodName = p.APM_Period }).ToList();
                        measures = from c1 in cals
                                   join p1 in pfps on c1.CAL_Id equals p1.NPD_Cal_Id into ll
                                   from l1 in ll.DefaultIfEmpty()
                                   select new Measure()
                                   {
                                       MeasureId = string.Format("{1}_{0}_{2}", id, HosId, c1.CAL_Id),
                                       HosId = HosId,
                                       CalId = c1.CAL_Id,
                                       EmmId = id,
                                       MonthYear = string.Format("{0}-{1}", c1?.CAL_MonthText ?? string.Empty, c1?.CAL_Year.ToString() ?? string.Empty),
                                       OrderBy = c1?.CAL_OrderBy ?? 1,
                                       AnalysisPeriodId = l1?.NPD_APM_Id ?? null,
                                       Numerator = l1?.NPD_Numerator ?? 0.0m,
                                       Denominator = l1?.NPD_Denominator ?? 0.0m,
                                       Multiplier = emm?.MeasureMaster.MEM_Multiplier ?? 0,
                                       Measurement = l1?.NPD_Measurement ?? 0,
                                       SourceType = l1?.NPD_SourceType ?? string.Empty,
                                       AnalysisPeriod = apms
                                   };
                        md.Measures = measures;
                    }
                }
                md.FromYear = FromYear;
            }
            else
            {
                md = new MeasuresData
                {
                    MeasuresDataId = string.Format("{1}_{0}", id, HosId),
                    Measure = string.Empty,
                    EventType = string.Empty,
                    TimePeriodType = string.Empty,
                    OrgId = HosId.ToString(),
                    FromYear = FromYear
                };
            }
            return md;
        }

        internal void SaveMeasuresData(List<MeasurementData> measurementDatas)
        {
            try
            {
                foreach (var m in measurementDatas)
                {
                    var pfp = db.NYSPFPDatas.Where(k => k.NPD_HOS_Id == m.HosId && k.NPD_Cal_Id == m.CalId && k.NPD_EMM_Id == m.EmmId).FirstOrDefault();
                    if (pfp != null && pfp.NPD_Id > 0)
                    {
                        pfp.NPD_Numerator = m.Numerator;
                        pfp.NPD_Denominator = m.Denominator;
                        pfp.NPD_Measurement = m.Measurement;
                        pfp.NPD_SourceType = "NYSPFP";
                        pfp.NPD_CreatedBy = "HANYSNT\\Ksuriyak";
                        pfp.NPD_CreatedOn = DateTime.Now;
                        pfp.NPD_UpdatedBy = m.UpdatedBy;
                        pfp.NPD_UpdatedOn = DateTime.Now;

                        db.Entry(pfp).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        pfp = new NYSPFPData
                        {
                            NPD_Id = 0,
                            NPD_HOS_Id = m.HosId,
                            NPD_Cal_Id = m.CalId,
                            NPD_EMM_Id = m.EmmId,
                            NPD_Numerator = m.Numerator,
                            NPD_Denominator = m.Denominator,
                            NPD_Measurement = m.Measurement,
                            NPD_SourceType = "NYSPFP",
                            NPD_CreatedBy = "HANYSNT\\Ksuriyak",
                            NPD_CreatedOn = DateTime.Now,
                            NPD_UpdatedBy = m.UpdatedBy,
                            NPD_UpdatedOn = DateTime.Now
                        };
                        db.NYSPFPDatas.Add(pfp);
                        db.SaveChanges();
                    }
                }
            }
            catch(Exception)
            {
                throw;
            }
        }

        internal List<HospitalVM> Komparer(List<Hospital> hospitals)
        {
            var hoss = hospitals.GroupBy(k => new { k.HOS_HospitalName, k.HOS_Unique_Id }).Select(k => k.FirstOrDefault());
            return hoss.Select(h => new HospitalVM() { Id = h.HOS_Id, HospitalName = h.HOS_HospitalName }).OrderBy(h => h.HospitalName).ToList();
        }
    }
}
