using IBS.Core.Models;
using IBS.Service.Helpers;
using IBS.Service.Services;
using IBS.Service.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace IBS.Controllers
{
    public class SearchFiltrs
    {
        public List<int> DateTypes { get; set; }
        public List<int> Partners { get; set; }
        public List<int> Clients { get; set; }
        public List<int> Coverages { get; set; }
        public List<int> Products { get; set; }
        public List<int> Divisions { get; set; }
        public List<int> Rtype { get; set; }
        public List<int> Git { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
    }
    public class ClientRevenue
    {
        public List<CommisionModel> Result { get; set; }
    }
    public class ClientCoverageRevenueByMonth
    {
        public string ClientName { get; set; }
        public string CoverageName { get; set; }
        public string CarrierName { get; set; }
        public string Division { get; set; }
        public string ProductName { get; set; }
        public decimal Jan { get; set; }
        public decimal Feb { get; set; }
        public decimal Mar { get; set; }
        public decimal Apr { get; set; }
        public decimal May { get; set; }
        public decimal Jun { get; set; }
        public decimal July { get; set; }
        public decimal Aug { get; set; }
        public decimal Sep { get; set; }
        public decimal Oct { get; set; }
        public decimal Nov { get; set; }
        public decimal Dec { get; set; }
        public decimal Total
        {
            get
            {
                return Jan + Feb + Mar + Apr + May + Jun + July + Aug + Sep + Oct + Nov + Dec;
            }
        }
    }
    public class GroupResult
    {
        public List<CommisionModel> Result { get; set; }
        public List<ClientRevenue> ClientRevenueResult { get; set; }
        public List<ClientCoverageRevenueByMonth> ClientRevenueResultMonth { get; set; }
    }
    public class SearchFiltrsResult
    {
        public List<GroupResult> Result { get; set; }
        public List<GroupResult> GITclients { get; set; }
        public List<MonthSummary> MonthSummaryResult { get; set; }
        public int DisplayTillMonth { get; set; }
    }
    public class ReportsController : Controller
    {
        private readonly ICarrierService _carrierService;
        private readonly ICommisionService _commisionService;
        private readonly IClientService _clientServive;
        private readonly ICommonService _commonService;
        public ReportsController(ICarrierService carrierService, ICommisionService commisionService, IClientService clientService, ICommonService commonService)
        {
            _carrierService = carrierService;
            _commisionService = commisionService;
            _clientServive = clientService;
            _commonService = commonService;
        }

        // GET: Reports
        public ActionResult Index()
        {
            var commisions = new SearchFiltrsResult();
            return View(commisions);
        }
        [HttpPost]
        public ActionResult Index(SearchFiltrs filters)
        {
            var commisions = new SearchFiltrsResult();
            var data = _commisionService.GetAllSavedCommissionsForCarrier(Convert.ToInt32(0)).OrderBy(c => c.CarrierId).ToList();

            filters.DateTypes.ForEach(df =>
            {
                if (df == 1)
                {
                    data = data.Where(dd => dd.StatementDate >= Convert.ToDateTime(filters.StartDate) && dd.StatementDate <= Convert.ToDateTime(filters.EndDate)).ToList();
                }
                if (df == 2)
                {
                    data = data.Where(dd => dd.AppliedDate >= Convert.ToDateTime(filters.StartDate) && dd.AppliedDate <= Convert.ToDateTime(filters.EndDate)).ToList();
                }
                if (df == 3)
                {
                    data = data.Where(dd => dd.ReconciliationPaymentDate >= Convert.ToDateTime(filters.StartDate) && dd.ReconciliationPaymentDate <= Convert.ToDateTime(filters.EndDate)).ToList();
                }
            });

            if (filters.Partners != null && filters.Partners.Count > 0)
            {
                data = data.Where(dd => filters.Partners.Contains(dd.CarrierId)).ToList();
            }

            if (filters.Rtype != null && filters.Rtype.Count > 0)
            {
                data = data.Where(dd => filters.Rtype.Contains(dd.ReconciliationStatusId)).ToList();
            }

            if (filters.Divisions != null && filters.Divisions.Count > 0)
            {
                data = data.Where(dd => filters.Divisions.Contains(dd.DivisionId)).ToList();
            }
            if (filters.Git != null && filters.Git.Count == 1)
            {
                filters.Git.ForEach(g =>
                {
                    if (g == 1)
                    {
                        data = data.Where(dd => dd.Git == true).ToList();
                    }
                    if (g == 2)
                    {
                        data = data.Where(dd => dd.Git == false).ToList();
                    }
                });
            }

            string cId = "";
            var tempRs = new List<GroupResult>();
            var tempR = new GroupResult();
            var groups = data.GroupBy(x => new { x.CarrierName, x.StatementDateAsString })
                        .Select(x => new CommisionModel
                        {
                            CarrierName = x.Key.CarrierName,
                            StatementDateAsString = x.Key.StatementDateAsString,
                            CommissionValue = x.Sum(y => y.CommissionValue),
                            ReconciliationPaymentDateAsString = x.ToList().Select(y => y.ReconciliationPaymentDateAsString).FirstOrDefault(),
                            AppliedDateAsFullString = x.ToList().Select(y => y.AppliedDateAsFullString).FirstOrDefault(),
                        }).OrderBy(x => x.CarrierName).ToList();

            groups.ForEach(d =>
            {
                if (cId != d.CarrierName)
                {
                    tempR = new GroupResult();
                    tempR.Result = new List<CommisionModel>();
                    tempR.Result.Add(d);
                    tempRs.Add(tempR);
                }
                else
                {
                    tempR.Result.Add(d);
                }
                cId = d.CarrierName;
            });

            commisions.Result = tempRs;
            return Json(commisions, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ActivitySummarybyClient()
        {
            var commissions = new SearchFiltrsResult();
            return View(commissions);
        }
        [HttpPost]
        public ActionResult ActivitySummarybyClient(SearchFiltrs filters)
        {
            SearchFiltrsResult commissions;
            GetCommissions(filters, out commissions, out List<CommisionModel> data);

            string cId = "";
            var tempRs = new List<GroupResult>();
            var tempR = new GroupResult();
            var groups = data.GroupBy(x => new { x.ClientName, x.StatementDateAsString })
                        .Select(x => new CommisionModel
                        {
                            ClientName = x.Key.ClientName,
                            StatementDateAsString = x.Key.StatementDateAsString,
                            CommissionValue = x.Sum(y => y.CommissionValue),
                            ReconciliationPaymentDateAsString = x.ToList().Select(y => y.ReconciliationPaymentDateAsString).FirstOrDefault(),
                            AppliedDateAsFullString = x.ToList().Select(y => y.AppliedDateAsFullString).FirstOrDefault(),
                        }).OrderBy(x => x.ClientName).ToList();

            groups.ForEach(d =>
            {
                if (cId != d.ClientName)
                {
                    tempR = new GroupResult();
                    tempR.Result = new List<CommisionModel>();
                    tempR.Result.Add(d);
                    tempRs.Add(tempR);
                }
                else
                {
                    tempR.Result.Add(d);
                }
                cId = d.ClientName;
            });

            commissions.Result = tempRs;
            return Json(commissions, JsonRequestBehavior.AllowGet);
        }
        public ActionResult RevenueByPartner()
        {
            var commissions = new SearchFiltrsResult();
            return View(commissions);
        }
        [HttpPost]
        public ActionResult RevenueByPartner(SearchFiltrs filters)
        {
            var commissions = new SearchFiltrsResult();
            var data = _commisionService.GetAllSavedCommissionsForCarrier(Convert.ToInt32(0)).OrderBy(c => c.CarrierId).ToList();

            filters.DateTypes.ForEach(df =>
            {
                if (df == 1)
                {
                    data = data.Where(dd => dd.StatementDate >= Convert.ToDateTime(filters.StartDate) && dd.StatementDate <= Convert.ToDateTime(filters.EndDate)).ToList();
                }
                if (df == 2)
                {
                    data = data.Where(dd => dd.AppliedDate >= Convert.ToDateTime(filters.StartDate) && dd.AppliedDate <= Convert.ToDateTime(filters.EndDate)).ToList();
                }
                if (df == 3)
                {
                    data = data.Where(dd => dd.ReconciliationPaymentDate >= Convert.ToDateTime(filters.StartDate) && dd.ReconciliationPaymentDate <= Convert.ToDateTime(filters.EndDate)).ToList();
                }
            });

            if (filters.Partners != null && filters.Partners.Count > 0)
            {
                data = data.Where(dd => filters.Partners.Contains(dd.CarrierId)).ToList();
            }
            if (filters.Products != null && filters.Products.Count > 0)
            {
                data = data.Where(dd => filters.Products.Contains(dd.SelectedCorporateProduct.Id)).ToList();
            }
            if (filters.Coverages != null && filters.Coverages.Count > 0)
            {
                data = data.Where(dd => filters.Coverages.Contains(dd.CoverageId)).ToList();
            }
            if (filters.Rtype != null && filters.Rtype.Count > 0)
            {
                data = data.Where(dd => filters.Rtype.Contains(dd.ReconciliationStatusId)).ToList();
            }

            if (filters.Divisions != null && filters.Divisions.Count > 0)
            {
                data = data.Where(dd => filters.Divisions.Contains(dd.DivisionId)).ToList();
            }
            if (filters.Git != null && filters.Git.Count == 1)
            {
                filters.Git.ForEach(g =>
                {
                    if (g == 1)
                    {
                        data = data.Where(dd => dd.Git == true).ToList();
                    }
                    if (g == 2)
                    {
                        data = data.Where(dd => dd.Git == false).ToList();
                    }
                });
            }

            string cId = "";
            var tempRs = new List<GroupResult>();
            var tempR = new GroupResult();
            data.ToList().ForEach(d =>
            {
                if (cId != d.CarrierName)
                {
                    tempR = new GroupResult();
                    tempR.Result = new List<CommisionModel>();
                    tempR.Result.Add(d);
                    tempRs.Add(tempR);
                }
                else
                {
                    tempR.Result.Add(d);
                }
                cId = d.CarrierName;
            });
            commissions.Result = tempRs;

            //return View(commisions);
            return Json(commissions, JsonRequestBehavior.AllowGet);
        }
        public ActionResult RevenueByClient()
        {
            var commisions = new SearchFiltrsResult();
            return View(commisions);
        }
        [HttpPost]
        public ActionResult RevenueByClient(SearchFiltrs filters)
        {
            SearchFiltrsResult commissions;
            GetCommissions(filters, out commissions, out List<CommisionModel> data);

            string cId = "";
            var tempRs = new List<GroupResult>();
            var tempR = new GroupResult();
            data.ToList().ForEach(d =>
            {
                if (cId != d.ClientName)
                {
                    tempR = new GroupResult();
                    tempR.Result = new List<CommisionModel>();
                    tempR.Result.Add(d);
                    tempRs.Add(tempR);
                }
                else
                {
                    tempR.Result.Add(d);
                }
                cId = d.ClientName;
            });


            tempRs.ForEach(c =>
            {
                var CM = c.Result.OrderBy(cm => cm.CarrierId).ToList();

                var ClientRevenueS = new List<ClientRevenue>();
                var ClientRevenue = new ClientRevenue();

                int cRId = 0;
                CM.ForEach(cm =>
                {
                    if (cm.CarrierId != cRId)
                    {
                        ClientRevenue = new ClientRevenue();
                        ClientRevenue.Result = new List<CommisionModel>();
                        ClientRevenue.Result.Add(cm);
                        ClientRevenueS.Add(ClientRevenue);
                    }
                    else
                    {
                        ClientRevenue.Result.Add(cm);
                    }
                    cRId = cm.CarrierId;
                });
                c.ClientRevenueResult = ClientRevenueS;
                c.Result = null;
            });


            commissions.Result = tempRs;
            return Json(commissions, JsonRequestBehavior.AllowGet);
        }

        private void GetCommissions(SearchFiltrs filters, out SearchFiltrsResult commisions, out List<CommisionModel> data)
        {
            commisions = new SearchFiltrsResult();
            var commissions = _commisionService.GetAllSavedCommissionsForCarrier(Convert.ToInt32(0)).OrderBy(c => c.ClientId).ToList();
            data = commissions;

            if (filters.StartDate != null)
            {
                filters.DateTypes.ForEach(df =>
                {
                    if (df == 1)
                    {
                        commissions = commissions.Where(dd => dd.StatementDate >= Convert.ToDateTime(filters.StartDate) && dd.StatementDate <= Convert.ToDateTime(filters.EndDate)).ToList();
                    }
                    if (df == 2)
                    {
                        commissions = commissions.Where(dd => dd.AppliedDate >= Convert.ToDateTime(filters.StartDate) && dd.AppliedDate <= Convert.ToDateTime(filters.EndDate)).ToList();
                    }
                    if (df == 3)
                    {
                        commissions = commissions.Where(dd => dd.ReconciliationPaymentDate >= Convert.ToDateTime(filters.StartDate) && dd.ReconciliationPaymentDate <= Convert.ToDateTime(filters.EndDate)).ToList();
                    }
                });
            }

            if (filters.Partners != null && filters.Partners.Count > 0)
            {
                commissions = commissions.Where(dd => filters.Partners.Contains(dd.CarrierId)).ToList();
            }

            if (filters.Clients != null && filters.Clients.Count > 0)
            {
                commissions = commissions.Where(dd => filters.Clients.Contains(dd.ClientId)).ToList();
            }

            if (filters.Products != null && filters.Products.Count > 0)
            {
                commissions = commissions.Where(dd => filters.Products.Contains(dd.SelectedCorporateProduct.Id)).ToList();
            }

            if (filters.Coverages != null && filters.Coverages.Count > 0)
            {
                commissions = commissions.Where(dd => filters.Coverages.Contains(dd.CoverageId)).ToList();
            }

            if (filters.Rtype != null && filters.Rtype.Count > 0)
            {
                commissions = commissions.Where(dd => filters.Rtype.Contains(dd.ReconciliationStatusId)).ToList();
            }

            if (filters.Divisions != null && filters.Divisions.Count > 0)
            {
                commissions = commissions.Where(dd => filters.Divisions.Contains(dd.DivisionId)).ToList();
            }


            if (filters.Git != null && filters.Git.Count == 1)
            {
                filters.Git.ForEach(g =>
                {
                    if (g == 1)
                    {
                        commissions = commissions.Where(dd => dd.Git == true).ToList();
                    }
                    if (g == 2)
                    {
                        commissions = commissions.Where(dd => dd.Git == false).ToList();
                    }
                });
            }
            data = commissions;
        }

        public ActionResult LastPaymentDate()
        {
            var commissions = new SearchFiltrsResult();
            return View(commissions);
        }
        [HttpPost]
        public ActionResult LastPaymentDate(SearchFiltrs filters)
        {
            var commissions = new SearchFiltrsResult();
            var data = _commisionService.GetAllSavedCommissionsForCarrier(Convert.ToInt32(0)).OrderBy(c => c.ClientId).ToList();

            if (filters.StartDate != null)
            {
                data = data.Where(dd => Convert.ToDateTime(filters.StartDate) >= dd.ReconciliationPaymentDate).ToList();
            }

            if (filters.Clients != null && filters.Clients.Count > 0)
            {
                data = data.Where(dd => filters.Clients.Contains(dd.ClientId)).ToList();
            }

            string cId = "";
            var tempRs = new List<GroupResult>();
            var tempR = new GroupResult();
            data.ToList().ForEach(d =>
            {
                if (cId != d.ClientName)
                {
                    tempR = new GroupResult();
                    tempR.Result = new List<CommisionModel>();
                    tempR.Result.Add(d);
                    tempRs.Add(tempR);
                }
                else
                {
                    tempR.Result.Add(d);
                }
                cId = d.ClientName;
            });
            commissions.Result = tempRs;

            //return View(commisions);
            return Json(commissions, JsonRequestBehavior.AllowGet);
        }
        public ActionResult RevenueByCoverageByMonth()
        {
            var commissions = new SearchFiltrsResult();
            return View(commissions);
        }
        [HttpPost]
        public ActionResult RevenueByCoverageByMonth(SearchFiltrs filters)
        {
            var commisions = new SearchFiltrsResult();
            var data = _commisionService.GetAllSavedCommissionsForCarrier(Convert.ToInt32(0)).OrderBy(c => c.SelectedCorporateProduct.Id).ToList();

            filters.DateTypes.ForEach(df =>
            {
                if (df == 1)
                {
                    data = data.Where(dd => dd.StatementDate >= Convert.ToDateTime(filters.StartDate) && dd.StatementDate <= Convert.ToDateTime(filters.EndDate)).ToList();
                }
                if (df == 2)
                {
                    data = data.Where(dd => dd.AppliedDate >= Convert.ToDateTime(filters.StartDate) && dd.AppliedDate <= Convert.ToDateTime(filters.EndDate)).ToList();
                }
                if (df == 3)
                {
                    data = data.Where(dd => dd.ReconciliationPaymentDate >= Convert.ToDateTime(filters.StartDate) && dd.ReconciliationPaymentDate <= Convert.ToDateTime(filters.EndDate)).ToList();
                }
            });

            if (filters.Partners != null && filters.Partners.Count > 0)
            {
                data = data.Where(dd => filters.Partners.Contains(dd.CarrierId)).ToList();
            }
            if (filters.Coverages != null && filters.Coverages.Count > 0)
            {
                data = data.Where(dd => filters.Coverages.Contains(dd.CoverageId)).ToList();
            }
            if (filters.Products != null && filters.Products.Count > 0)
            {
                data = data.Where(dd => filters.Products.Contains(dd.SelectedCorporateProduct.Id)).ToList();
            }
            if (filters.Rtype != null && filters.Rtype.Count > 0)
            {
                data = data.Where(dd => filters.Rtype.Contains(dd.ReconciliationStatusId)).ToList();
            }
            if (filters.Divisions != null && filters.Divisions.Count > 0)
            {
                data = data.Where(dd => filters.Divisions.Contains(dd.DivisionId)).ToList();
            }
            if (filters.Git != null && filters.Git.Count == 1)
            {
                filters.Git.ForEach(g =>
                {
                    if (g == 1)
                    {
                        data = data.Where(dd => dd.Git == true).ToList();
                    }
                    if (g == 2)
                    {
                        data = data.Where(dd => dd.Git == false).ToList();
                    }
                });
            }

            string cId = "";
            var tempRs = new List<GroupResult>();
            var tempR = new GroupResult();
            data.ToList().ForEach(d =>
            {
                if (cId != d.SelectedCorporateProduct.Name)
                {
                    tempR = new GroupResult();
                    tempR.Result = new List<CommisionModel>();
                    tempR.Result.Add(d);
                    tempRs.Add(tempR);
                }
                else
                {
                    tempR.Result.Add(d);
                }
                cId = d.SelectedCorporateProduct.Name;
            });

            tempRs.ForEach(c =>
            {
                var CM = c.Result.OrderBy(cm => cm.CarrierName).ToList();

                var ClientRevenueM = new List<ClientCoverageRevenueByMonth>();
                var ClientRevenue = new ClientCoverageRevenueByMonth();

                //int cRId = 0;
                CM.ForEach(cm =>
                {
                    //if (cm.ClientId != cRId)
                    //{
                    //    ClientRevenue = new ClientCoverageRevenueByMonth();
                    //    ClientRevenue.ClientName = cm.ClientName;
                    //    ClientRevenue.CoverageName = cm.CoverageName;
                    //    ClientRevenue.CarrierName = cm.CarrierName;
                    //    ClientRevenue.Division = cm.DivisionName;
                    //    ClientRevenue.ProductName = cm.SelectedCorporateProduct.Name;
                    //    MapMonthRevenue(ClientRevenue, cm, filters);
                    //    ClientRevenueM.Add(ClientRevenue);
                    //}
                    //else
                    //{
                    //    MapMonthRevenue(ClientRevenue, cm, filters);
                    //}
                    //cRId = cm.ClientId;

                    ClientRevenue = new ClientCoverageRevenueByMonth();
                    ClientRevenue.ClientName = cm.ClientName;
                    ClientRevenue.CoverageName = cm.CoverageName;
                    ClientRevenue.CarrierName = cm.CarrierName;
                    ClientRevenue.Division = cm.DivisionName;
                    ClientRevenue.ProductName = cm.SelectedCorporateProduct.Name;
                    MapMonthRevenue(ClientRevenue, cm, filters);
                    ClientRevenueM.Add(ClientRevenue);
                });
                c.ClientRevenueResultMonth = ClientRevenueM;
                c.Result = null;
            });


            commisions.Result = tempRs;
            return Json(commisions, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MonthlyDetails()
        {
            var commisions = new SearchFiltrsResult();
            return View(commisions);
        }

        public void MapMonthlySummaryActual(MonthSummary summary, CommisionModel model)
        {

        }
        [HttpPost]
        public ActionResult MonthlyDetails(SearchFiltrs filters)
        {
            SearchFiltrsResult commisions = new SearchFiltrsResult()
            {
                DisplayTillMonth = filters.Month == 0 ? 12 : filters.Month,
                MonthSummaryResult = new List<MonthSummary>()
            };

            GetCommissions(filters, out commisions, out List<CommisionModel> data);
            commisions.MonthSummaryResult = new List<MonthSummary>();
            commisions.DisplayTillMonth = filters.Month == 0 ? 12 : filters.Month;
            data = data.Where(y => Convert.ToDateTime(y.ReconciliationPaymentDate).Year == filters.Year && Convert.ToDateTime(y.ReconciliationPaymentDate).Month <= filters.Month).ToList();
            data.ToList().ForEach(d =>
            {
                //var isExist = commisions.MonthSummaryResult?.FirstOrDefault(cm => cm.PolicyNumber == d.PolicyNumber);
                //if (isExist == null)
                //{
                    var monthlySumaryFinal = new MonthSummary()
                    {
                        PolicyNumber = d.PolicyNumber,
                        CoverageName = d.CoverageName,
                        ClientName = d.ClientName,
                        CarrierName = d.CarrierName,
                        ProductType = d.SelectedCorporateProduct.Name
                    };
                    commisions.MonthSummaryResult.Add(monthlySumaryFinal);
                //}
                //else
                //{
                //    commisions.MonthSummaryResult.Add(isExist);
                //}
                d.MapMonthlySummaryActual(monthlySumaryFinal, _commonService);
                d.MapMonthlySummaryBudget(monthlySumaryFinal, _commonService);
            });

            var result = commisions.MonthSummaryResult.GroupBy(x => new {
                x.ProductType,
                x.CoverageName,
                x.CarrierName,
                x.ClientName
            })
                  .Select(x => new MonthSummary()
                  {
                      ProductType = x.Key.ProductType,
                      CoverageName = x.Key.CoverageName,
                      CarrierName = x.Key.CarrierName,
                      ClientName = x.Key.ClientName,
                      PolicyNumber = x.FirstOrDefault()?.PolicyNumber,

                      JanA = x.Sum(y => y.JanA),
                      JanB = x.Sum(y => y.JanB),

                      FebA = x.Sum(y => y.FebA),
                      FebB = x.Sum(y => y.FebB),

                      MarA = x.Sum(y => y.MarA),
                      MarB = x.Sum(y => y.MarB),

                      AprA = x.Sum(y => y.AprA),
                      AprB = x.Sum(y => y.AprB),

                      MayA = x.Sum(y => y.MayA),
                      MayB = x.Sum(y => y.MayB),

                      JunA = x.Sum(y => y.JunA),
                      JunB = x.Sum(y => y.JunB),

                      JulA = x.Sum(y => y.JulA),
                      JulB = x.Sum(y => y.JulB),

                      AugA = x.Sum(y => y.AugA),
                      AugB = x.Sum(y => y.AugB),

                      SepA = x.Sum(y => y.SepA),
                      SepB = x.Sum(y => y.SepB),

                      OctA = x.Sum(y => y.OctA),
                      OctB = x.Sum(y => y.OctB),

                      NovA = x.Sum(y => y.NovA),
                      NovB = x.Sum(y => y.NovB),

                      DecA = x.Sum(y => y.DecA),
                      DecB = x.Sum(y => y.DecB),

                  }).ToList();

            var result1 = result.OrderBy(x => x.ClientName).ToList();
            var result2 = result1.OrderBy(y => y.CarrierName).ToList();
            commisions.MonthSummaryResult = result2.OrderBy(m => m.ProductType).ToList();
            commisions.MonthSummaryResult = commisions.MonthSummaryResult.OrderBy(x => x.CoverageName).ToList();
            return Json(commisions, JsonRequestBehavior.AllowGet);
        }
        public ActionResult MonthlyDetailsForNewYear()
        {
            var commisions = new SearchFiltrsResult();
            return View(commisions);
        }
        [HttpPost]
        public ActionResult MonthlyDetailsForNewYear(SearchFiltrs filters)
        {
            SearchFiltrsResult commisions = new SearchFiltrsResult()
            {
                DisplayTillMonth = filters.Month == 0 ? 12 : filters.Month,
                MonthSummaryResult = new List<MonthSummary>()
            };

            GetCommissions(filters, out commisions, out List<CommisionModel> data);
            commisions.MonthSummaryResult = new List<MonthSummary>();
            commisions.DisplayTillMonth = filters.Month == 0 ? 12 : filters.Month;
            data = data.Where(y => Convert.ToDateTime(y.AppliedDate).Year == filters.Year && Convert.ToDateTime(y.AppliedDate).Month <= filters.Month).ToList();
            data.ToList().ForEach(d =>
            {
                //var isExist = commisions.MonthSummaryResult?.FirstOrDefault(cm => cm.PolicyNumber == d.PolicyNumber);
                //if (isExist == null)
                //{
                var monthlySumaryFinal = new MonthSummary()
                {
                    PolicyNumber = d.PolicyNumber,
                    CoverageName = d.CoverageName,
                    ClientName = d.ClientName,
                    CarrierName = d.CarrierName,
                    ProductType = d.SelectedCorporateProduct.Name
                };
                commisions.MonthSummaryResult.Add(monthlySumaryFinal);
                //}
                //else
                //{
                //    commisions.MonthSummaryResult.Add(isExist);
                //}
                d.MapMonthlySummaryActual(monthlySumaryFinal, _commonService,true);
                d.MapMonthlySummaryBudget(monthlySumaryFinal, _commonService,true);
            });

            var result = commisions.MonthSummaryResult.GroupBy(x => new {
                x.ProductType,
                x.CoverageName,
                x.CarrierName,
                x.ClientName
            })
                  .Select(x => new MonthSummary()
                  {
                      ProductType = x.Key.ProductType,
                      CoverageName = x.Key.CoverageName,
                      CarrierName = x.Key.CarrierName,
                      ClientName = x.Key.ClientName,
                      PolicyNumber = x.FirstOrDefault()?.PolicyNumber,

                      JanA = x.Sum(y => y.JanA),
                      JanB = x.Sum(y => y.JanB),

                      FebA = x.Sum(y => y.FebA),
                      FebB = x.Sum(y => y.FebB),

                      MarA = x.Sum(y => y.MarA),
                      MarB = x.Sum(y => y.MarB),

                      AprA = x.Sum(y => y.AprA),
                      AprB = x.Sum(y => y.AprB),

                      MayA = x.Sum(y => y.MayA),
                      MayB = x.Sum(y => y.MayB),

                      JunA = x.Sum(y => y.JunA),
                      JunB = x.Sum(y => y.JunB),

                      JulA = x.Sum(y => y.JulA),
                      JulB = x.Sum(y => y.JulB),

                      AugA = x.Sum(y => y.AugA),
                      AugB = x.Sum(y => y.AugB),

                      SepA = x.Sum(y => y.SepA),
                      SepB = x.Sum(y => y.SepB),

                      OctA = x.Sum(y => y.OctA),
                      OctB = x.Sum(y => y.OctB),

                      NovA = x.Sum(y => y.NovA),
                      NovB = x.Sum(y => y.NovB),

                      DecA = x.Sum(y => y.DecA),
                      DecB = x.Sum(y => y.DecB),

                  }).ToList();

            var result1 = result.OrderBy(x => x.ClientName).ToList();
            var result2 = result1.OrderBy(y => y.CarrierName).ToList();
            commisions.MonthSummaryResult = result2.OrderBy(m => m.ProductType).ToList();
            commisions.MonthSummaryResult = commisions.MonthSummaryResult.OrderBy(x => x.CoverageName).ToList();
            return Json(commisions, JsonRequestBehavior.AllowGet);
        }
        public ActionResult MonthlySummaryForNewYear()
        {
            var commisions = new SearchFiltrsResult();
            return View(commisions);
        }
        [HttpPost]
        public ActionResult MonthlySummaryForNewYear(SearchFiltrs filters)
        {
            SearchFiltrsResult commisions = new SearchFiltrsResult()
            {
                DisplayTillMonth = filters.Month == 0 ? 12 : filters.Month,
                MonthSummaryResult = new List<MonthSummary>()
            };

            GetCommissions(filters, out commisions, out List<CommisionModel> data);
            commisions.MonthSummaryResult = new List<MonthSummary>();
            commisions.DisplayTillMonth = filters.Month == 0 ? 12 : filters.Month;
            data = data.Where(y => Convert.ToDateTime(y.AppliedDate).Year == filters.Year && Convert.ToDateTime(y.AppliedDate).Month <= filters.Month).ToList();
            data.ToList().ForEach(d =>
            {
                //var isExist = commisions.MonthSummaryResult?.FirstOrDefault(cm => cm.PolicyNumber == d.PolicyNumber);
                //if (isExist == null)
                //{
                //    isExist = new MonthSummary()
                //    {
                //        PolicyNumber = d.PolicyNumber,
                //        CoverageName = d.CoverageName,
                //        CarrierName = d.CarrierName,
                //        ProductType = d.SelectedCorporateProduct.Name
                //    };
                //    commisions.MonthSummaryResult.Add(isExist);
                //}
                //else
                //{
                //    commisions.MonthSummaryResult.Add(isExist);
                //}
                var monthSummaryfinal = new MonthSummary()
                {
                    PolicyNumber = d.PolicyNumber,
                    CoverageName = d.CoverageName,
                    CarrierName = d.CarrierName,
                    ProductType = d.SelectedCorporateProduct.Name
                };
                commisions.MonthSummaryResult.Add(monthSummaryfinal);

                d.MapMonthlySummaryActual(monthSummaryfinal, _commonService,true);
                d.MapMonthlySummaryBudget(monthSummaryfinal, _commonService, true);
            });


            var result = commisions.MonthSummaryResult.GroupBy(x => new { x.ProductType, x.CoverageName, x.CarrierName })
                  .Select(x => new MonthSummary()
                  {
                      ProductType = x.Key.ProductType,
                      CoverageName = x.Key.CoverageName,
                      CarrierName = x.Key.CarrierName,

                      JanA = x.Sum(y => y.JanA),
                      JanB = x.Sum(y => y.JanB),

                      FebA = x.Sum(y => y.FebA),
                      FebB = x.Sum(y => y.FebB),

                      MarA = x.Sum(y => y.MarA),
                      MarB = x.Sum(y => y.MarB),

                      AprA = x.Sum(y => y.AprA),
                      AprB = x.Sum(y => y.AprB),

                      MayA = x.Sum(y => y.MayA),
                      MayB = x.Sum(y => y.MayB),

                      JunA = x.Sum(y => y.JunA),
                      JunB = x.Sum(y => y.JunB),

                      JulA = x.Sum(y => y.JulA),
                      JulB = x.Sum(y => y.JulB),

                      AugA = x.Sum(y => y.AugA),
                      AugB = x.Sum(y => y.AugB),

                      SepA = x.Sum(y => y.SepA),
                      SepB = x.Sum(y => y.SepB),

                      OctA = x.Sum(y => y.OctA),
                      OctB = x.Sum(y => y.OctB),

                      NovA = x.Sum(y => y.NovA),
                      NovB = x.Sum(y => y.NovB),

                      DecA = x.Sum(y => y.DecA),
                      DecB = x.Sum(y => y.DecB),

                  }).ToList();

            var result1 = result.OrderBy(x => x.CarrierName).ToList();
            var result2 = result1.OrderBy(c => c.ProductType).ToList();
            commisions.MonthSummaryResult = result2;

            commisions.MonthSummaryResult = commisions.MonthSummaryResult.OrderBy(x => x.CoverageName).ToList();
            return Json(commisions, JsonRequestBehavior.AllowGet);
        }


        public ActionResult MonthlySummary()
        {
            var commisions = new SearchFiltrsResult();
            return View(commisions);
        }
        [HttpPost]
        public ActionResult MonthlySummary(SearchFiltrs filters)
        {
            SearchFiltrsResult commisions = new SearchFiltrsResult()
            {
                DisplayTillMonth = filters.Month == 0 ? 12 : filters.Month,
                MonthSummaryResult = new List<MonthSummary>()
            };

            GetCommissions(filters, out commisions, out List<CommisionModel> data);
            commisions.MonthSummaryResult = new List<MonthSummary>();
            commisions.DisplayTillMonth = filters.Month == 0 ? 12 : filters.Month;
            data = data.Where(y => Convert.ToDateTime(y.ReconciliationPaymentDate).Year == filters.Year && Convert.ToDateTime(y.ReconciliationPaymentDate).Month <= filters.Month).ToList();
            data.ToList().ForEach(d =>
            {
                //var isExist = commisions.MonthSummaryResult?.FirstOrDefault(cm => cm.PolicyNumber == d.PolicyNumber);
                //if (isExist == null)
                //{
                //    isExist = new MonthSummary()
                //    {
                //        PolicyNumber = d.PolicyNumber,
                //        CoverageName = d.CoverageName,
                //        CarrierName = d.CarrierName,
                //        ProductType = d.SelectedCorporateProduct.Name
                //    };
                //    commisions.MonthSummaryResult.Add(isExist);
                //}
                //else
                //{
                //    commisions.MonthSummaryResult.Add(isExist);
                //}
                var monthSummaryfinal = new MonthSummary()
                {
                    PolicyNumber = d.PolicyNumber,
                    CoverageName = d.CoverageName,
                    CarrierName = d.CarrierName,
                    ProductType = d.SelectedCorporateProduct.Name
                };
                commisions.MonthSummaryResult.Add(monthSummaryfinal);

                d.MapMonthlySummaryActual(monthSummaryfinal, _commonService);
                d.MapMonthlySummaryBudget(monthSummaryfinal, _commonService);
            });


            var result = commisions.MonthSummaryResult.GroupBy(x => new { x.ProductType, x.CoverageName, x.CarrierName })
                  .Select(x => new MonthSummary()
                  {
                      ProductType = x.Key.ProductType,
                      CoverageName = x.Key.CoverageName,
                      CarrierName = x.Key.CarrierName,

                      JanA = x.Sum(y => y.JanA),
                      JanB = x.Sum(y => y.JanB),

                      FebA = x.Sum(y => y.FebA),
                      FebB = x.Sum(y => y.FebB),

                      MarA = x.Sum(y => y.MarA),
                      MarB = x.Sum(y => y.MarB),

                      AprA = x.Sum(y => y.AprA),
                      AprB = x.Sum(y => y.AprB),

                      MayA = x.Sum(y => y.MayA),
                      MayB = x.Sum(y => y.MayB),

                      JunA = x.Sum(y => y.JunA),
                      JunB = x.Sum(y => y.JunB),

                      JulA = x.Sum(y => y.JulA),
                      JulB = x.Sum(y => y.JulB),

                      AugA = x.Sum(y => y.AugA),
                      AugB = x.Sum(y => y.AugB),

                      SepA = x.Sum(y => y.SepA),
                      SepB = x.Sum(y => y.SepB),

                      OctA = x.Sum(y => y.OctA),
                      OctB = x.Sum(y => y.OctB),

                      NovA = x.Sum(y => y.NovA),
                      NovB = x.Sum(y => y.NovB),

                      DecA = x.Sum(y => y.DecA),
                      DecB = x.Sum(y => y.DecB),

                  }).ToList();

            var result1 = result.OrderBy(x => x.CarrierName).ToList();
            var result2 = result1.OrderBy(c => c.ProductType).ToList();
            commisions.MonthSummaryResult = result2;

            commisions.MonthSummaryResult = commisions.MonthSummaryResult.OrderBy(x => x.CoverageName).ToList();
            return Json(commisions, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GITClientsReport()
        {
            var commissions = new SearchFiltrsResult();
            return View(commissions);
        }
        [HttpPost]
        public ActionResult GITClientsReport(SearchFiltrs filters)
        {
            var commissions = new SearchFiltrsResult();
            var data = _commisionService.GetAllSavedCommissionsForCarrier(Convert.ToInt32(0)).OrderBy(c => c.ClientId).ToList();

            if (filters.Git != null && filters.Git.Count == 1)
            {
                filters.Git.ForEach(g =>
                {
                    if (g == 1)
                    {
                        data = data.Where(dd => dd.Git == true).ToList();
                    }
                    if (g == 2)
                    {
                        data = data.Where(dd => dd.Git == false).ToList();
                    }
                });
            }

            string cId = "";
            var tempRs = new List<GroupResult>();
            var tempR = new GroupResult();
            data.ToList().ForEach(d =>
            {
                if (cId != d.ClientName)
                {
                    tempR = new GroupResult();
                    tempR.Result = new List<CommisionModel>();
                    tempR.Result.Add(d);
                    tempRs.Add(tempR);
                }
                else
                {
                    tempR.Result.Add(d);
                }
                cId = d.ClientName;
            });
            commissions.Result = tempRs;

            //return View(commisions);
            return Json(commissions, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ClientsByDivision()
        {
            var commissions = new SearchFiltrsResult();
            return View(commissions);
        }
        [HttpPost]
        public ActionResult ClientsByDivision(SearchFiltrs filters)
        {
            var commissions = new SearchFiltrsResult();
            var data = _commisionService.GetAllSavedCommissionsForCarrier(Convert.ToInt32(0)).OrderBy(c => c.ClientId).ToList();

            if (filters.Divisions != null && filters.Divisions.Count > 0)
            {
                data = data.Where(dd => filters.Divisions.Contains(dd.DivisionId)).ToList();
            }

            string cId = "";
            var tempRs = new List<GroupResult>();
            var tempR = new GroupResult();
            data.ToList().ForEach(d =>
            {
                if (cId != d.ClientName)
                {
                    tempR = new GroupResult();
                    tempR.Result = new List<CommisionModel>();
                    tempR.Result.Add(d);
                    tempRs.Add(tempR);
                }
                else
                {
                    tempR.Result.Add(d);
                }
                cId = d.ClientName;
            });
            commissions.Result = tempRs;

            //return View(commisions);
            return Json(commissions, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetPatrners()
        {
            var Partners = _carrierService.GetAllCarriers().OrderBy(x => x.Name).ToList();
            return Json(Partners, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetClients()
        {
            var clients = _clientServive.GetAllClients().OrderBy(x => x.Name).ToList();
            return Json(clients, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetCoverages()
        {
            var coverages = _commonService.GetAllCoverages().OrderBy(x => x.Name).ToList();
            return Json(coverages, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetProducts()
        {
            var products = _commonService.GetAllProducts().OrderBy(x => x.Name).ToList();
            return Json(products, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetCFProducts()
        {
            var cfproducts = _commisionService.GetAllCorporateProducts().OrderBy(x => x.Name).ToList();
            return Json(cfproducts, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetDateTypes()
        {
            return Json(ListHelper.GetDateTypes(), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetDivisions()
        {
            return Json(ListHelper.GetDivisions(), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetRType()
        {

            return Json(ListHelper.RType(), JsonRequestBehavior.AllowGet);
        }
        private void MapMonthRevenue(ClientCoverageRevenueByMonth rm, CommisionModel cm, SearchFiltrs filters)
        {
            filters.DateTypes.ForEach(df =>
            {
                if (df == 1)
                {
                    if (cm.StatementDate == null)
                        return;

                    switch (cm.StatementDate.Value.Month)
                    {
                        case 1:
                            rm.Jan = cm.CommissionValue ?? 0;
                            break;
                        case 2:
                            rm.Feb = cm.CommissionValue ?? 0;
                            break;
                        case 3:
                            rm.Mar = cm.CommissionValue ?? 0;
                            break;
                        case 4:
                            rm.Apr = cm.CommissionValue ?? 0;
                            break;
                        case 5:
                            rm.May = cm.CommissionValue ?? 0;
                            break;
                        case 6:
                            rm.Jun = cm.CommissionValue ?? 0;
                            break;
                        case 7:
                            rm.July = cm.CommissionValue ?? 0;
                            break;
                        case 8:
                            rm.Aug = cm.CommissionValue ?? 0;
                            break;
                        case 9:
                            rm.Sep = cm.CommissionValue ?? 0;
                            break;
                        case 10:
                            rm.Oct = cm.CommissionValue ?? 0;
                            break;
                        case 11:
                            rm.Nov = cm.CommissionValue ?? 0;
                            break;
                        case 12:
                            rm.Dec = cm.CommissionValue ?? 0;
                            break;
                    }
                }
                if (df == 2)
                {
                    if (cm.AppliedDate == null)
                        return;

                    switch (cm.AppliedDate.Value.Month)
                    {
                        case 1:
                            rm.Jan = cm.CommissionValue ?? 0;
                            break;
                        case 2:
                            rm.Feb = cm.CommissionValue ?? 0;
                            break;
                        case 3:
                            rm.Mar = cm.CommissionValue ?? 0;
                            break;
                        case 4:
                            rm.Apr = cm.CommissionValue ?? 0;
                            break;
                        case 5:
                            rm.May = cm.CommissionValue ?? 0;
                            break;
                        case 6:
                            rm.Jun = cm.CommissionValue ?? 0;
                            break;
                        case 7:
                            rm.July = cm.CommissionValue ?? 0;
                            break;
                        case 8:
                            rm.Aug = cm.CommissionValue ?? 0;
                            break;
                        case 9:
                            rm.Sep = cm.CommissionValue ?? 0;
                            break;
                        case 10:
                            rm.Oct = cm.CommissionValue ?? 0;
                            break;
                        case 11:
                            rm.Nov = cm.CommissionValue ?? 0;
                            break;
                        case 12:
                            rm.Dec = cm.CommissionValue ?? 0;
                            break;
                    }
                }
                if (df == 3)
                {
                    if (cm.ReconciliationPaymentDate == null)
                        return;

                    switch (cm.ReconciliationPaymentDate.Value.Month)
                    {
                        case 1:
                            rm.Jan = cm.CommissionValue ?? 0;
                            break;
                        case 2:
                            rm.Feb = cm.CommissionValue ?? 0;
                            break;
                        case 3:
                            rm.Mar = cm.CommissionValue ?? 0;
                            break;
                        case 4:
                            rm.Apr = cm.CommissionValue ?? 0;
                            break;
                        case 5:
                            rm.May = cm.CommissionValue ?? 0;
                            break;
                        case 6:
                            rm.Jun = cm.CommissionValue ?? 0;
                            break;
                        case 7:
                            rm.July = cm.CommissionValue ?? 0;
                            break;
                        case 8:
                            rm.Aug = cm.CommissionValue ?? 0;
                            break;
                        case 9:
                            rm.Sep = cm.CommissionValue ?? 0;
                            break;
                        case 10:
                            rm.Oct = cm.CommissionValue ?? 0;
                            break;
                        case 11:
                            rm.Nov = cm.CommissionValue ?? 0;
                            break;
                        case 12:
                            rm.Dec = cm.CommissionValue ?? 0;
                            break;
                    }
                }

            });
        }
    }
}