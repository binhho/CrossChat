using System;
using System.Collections.Generic;
using System.Text;
using Abo.Client.Core;
using Abo.Server.Application.DataTransferObjects.Messages;
using Abo.Server.Infrastructure.Protocol;
using Cirrious.CrossCore.IoC;

namespace Abo.Client.Android
{
    public class App : Core.App
    {
        public override void Initialize()
        {
            //var assemblies = new[]
            //    {
            //        typeof(App).Assembly,
            //        typeof(BaseDto).Assembly,
            //        typeof(App).Assembly,
            //        typeof(CommandBuffer).Assembly,
            //    };

            //foreach (var assembly in assemblies)
            //{
            //    CreatableTypes(assembly)
            //        //.EndingWith("Proxy")
            //        .AsInterfaces()
            //        .RegisterAsLazySingleton();
            //}

            base.Initialize();
        }
    }
}