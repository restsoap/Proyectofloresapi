﻿using System.Web.Mvc;

namespace Proyectofloresapi
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            //filters.Add(new Proyectoflores.Filters.VerificaSession());

        }
    }
}
