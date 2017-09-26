﻿using BusinessLogic.Interfaces;
using BusinessLogic.Services;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using Ninject;
using System.Web.Mvc;

namespace ClientWeb
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;

        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }
        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            kernel.Bind<ILobbyService>().To<LobbyService>();
            kernel.Bind<IUserService>().To<UserService>();
        }
    }
}