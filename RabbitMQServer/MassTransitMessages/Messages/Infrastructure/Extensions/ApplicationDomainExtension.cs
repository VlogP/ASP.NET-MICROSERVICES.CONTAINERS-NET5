using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Microservice.Messages.Infrastructure.Extensions
{
    public static class ApplicationDomainExtension
    {
        public static void LoadAssemblies(this AppDomain appDomain, params string[] assemblyNames)
        {
            foreach(var assemblyName in assemblyNames)
            {               
                appDomain.Load(new AssemblyName(assemblyName));
            }
        }
    }
}
