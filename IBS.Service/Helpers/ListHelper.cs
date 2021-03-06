﻿using IBS.Service.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBS.Service.Helpers
{
    public static class ListHelper
    {
        public static List<SelectListCommon> GetDateTypes()
        {
            return new List<SelectListCommon>()
            {
             
                new SelectListCommon()
                {
                    Id=1,
                    Name="Statement"
                },
                new SelectListCommon()
                {
                    Id=2,
                    Name="Applied"
                }
            };
        }
        public static List<SelectListCommon> RType()
        {
            return new List<SelectListCommon>()
            {
                new SelectListCommon()
                {
                    Id=1,
                    Name="Open"
                },
                new SelectListCommon()
                {
                    Id=2,
                    Name="Paid"
                }

            };
        }
        public static List<SelectListCommon> GetDivisions()
        {
            return new List<SelectListCommon>()
            {
                new SelectListCommon()
                {
                    Id=1,
                    Name="HBS"
                },
                new SelectListCommon()
                {
                    Id=2,
                    Name="SBS"
                }
            };
        }
    }
}
