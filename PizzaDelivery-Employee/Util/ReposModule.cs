﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces.Repository;
using Interfaces.Service;
using BLL.Service;
using DAL.Repository;

namespace PD_Employee.Util
{
    public class ReposModule : Ninject.Modules.NinjectModule
    {
        string connectionString;

        public ReposModule(string theConnectionString)
        {
            connectionString = theConnectionString;
        }

        public override void Load()
        {
            Bind<IDbRepos>().To<DbRepos>().InSingletonScope();
        }
    }
}