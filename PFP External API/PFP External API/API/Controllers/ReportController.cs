namespace API.Controllers
{
    using API.Models.DAL;
    using System.Collections.Generic;
    using System.Web.Http;

    public class ReportController : ApiController
    {
        // GET: api/Home
        public string GetPFP(int Id, string from, string to, string type)
        {
            return new SQLHelper().GetDataFromSQL("[pfp].[spPressureInjuryRates]", new List<SQLHelperParams>()
            { new SQLHelperParams() {  Param = "@fromyear", Value = from},
             new SQLHelperParams() {  Param = "@toyear", Value = to},
             new SQLHelperParams() {  Param = "@type", Value = type},
             new SQLHelperParams() {  Param = "@orgid", Value = Id}});
        }
    }
}
