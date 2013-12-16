using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Abo.Server.Application.DataTransferObjects.Requests;
using Abo.Server.Application.Seedwork;
using Abo.Server.Application.Services;
using Abo.Server.Application.Sessions;
using Abo.Server.Infrastructure.Protocol;
using Abo.Utils.Logging;

namespace Abo.Server.Infrastructure.Transport.Commands
{
    public class RequestCommand : AboCommandBase
    {
        protected new readonly ILogger Logger = LogFactory.GetLogger<RequestCommand>();

        protected override bool AllowAnonymousAccess { get { return true; } }

        public override void ExecuteAstralCommand(AboSession session, byte[] data)
        {
            var request = DtoSerializer.Deserialize<RequestBase>(data);
            var methodAndInstancePair = AppServicesMethods[request.GetType()];

            Logger.Trace("{0}  {1}  {2}", methodAndInstancePair.Item1.Name, request.GetType().Name, session.GetSessionName());

            try
            {
                var responseObj = methodAndInstancePair.Item1.Invoke(methodAndInstancePair.Item2,
                    methodAndInstancePair.Item3 == ArgumentsOrder.SessionRequest ?
                    new object[] { session, request } :
                    new object[] { request, session });
                if (methodAndInstancePair.Item1.ReflectedType != typeof (void))
                {
                    session.Send(responseObj);
                }
            }
            catch (Exception exc)
            {
                Logger.Exception(exc, "{0} failed ({1},  {2})", methodAndInstancePair.Item1.Name, request.GetType().Name, session.GetSessionName());
            }
        }

        protected override CommandNames CommandName
        {
            get { return CommandNames.Request; }
        }

        private static readonly Dictionary<Type, Tuple<MethodInfo, AppService, ArgumentsOrder>> AppServicesMethods = 
            new Dictionary<Type, Tuple<MethodInfo, AppService, ArgumentsOrder>>();

        static RequestCommand()
        {
            var servicesTypes = typeof(ChatService).Assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(AppService)));
            var servicesInstances = new List<AppService>();
            var methods = new List<Tuple<MethodInfo, AppService>>();

            foreach (var serviceType in servicesTypes)
            {
                var controller = ServiceLocator.Resolve(serviceType) as AppService;
                servicesInstances.Add(controller);
                methods.AddRange(serviceType.GetMethods().Select(i => new Tuple<MethodInfo, AppService>(i, controller)));
            }

            foreach (var method in methods)
            {
                var parameters = method.Item1.GetParameters();
                if (parameters.Length != 2)
                    continue;
                if (parameters[0].ParameterType == typeof(ISession) && parameters[1].ParameterType.IsSubclassOf(typeof(RequestBase)))
                {
                    AppServicesMethods[parameters[1].ParameterType] = new Tuple<MethodInfo, AppService, ArgumentsOrder>(method.Item1, method.Item2, ArgumentsOrder.SessionRequest);
                }
                else if (parameters[1].ParameterType == typeof(ISession) && parameters[0].ParameterType.IsSubclassOf(typeof(RequestBase)))
                {
                    AppServicesMethods[parameters[0].ParameterType] = new Tuple<MethodInfo, AppService, ArgumentsOrder>(method.Item1, method.Item2, ArgumentsOrder.RequestSession);
                }
            }
        }

        enum ArgumentsOrder
        {
            SessionRequest,
            RequestSession
        }
    }
}
