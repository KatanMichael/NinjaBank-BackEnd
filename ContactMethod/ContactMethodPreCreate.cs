using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ServiceModel;
using Microsoft.Xrm.Sdk;



namespace NinjaBank.ContactMethod
{
    public class ContactMethodPreCreate : IPlugin
    {
        Dictionary<int, string> opts = new Dictionary<int, string>();
        public void Execute(IServiceProvider serviceProvider)
        {
            opts.Add(100000000, "טלפון");
            opts.Add(100000001, "מייל");

            // Obtain the tracing service
            ITracingService trace =
            (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            trace.Trace("In CMPreCreate");

           

            // Obtain the execution context from the service provider.  
            IPluginExecutionContext context = (IPluginExecutionContext)
                serviceProvider.GetService(typeof(IPluginExecutionContext));

            // The InputParameters collection contains all the data passed in the message request.  
            if (context.InputParameters.Contains("Target") &&
                context.InputParameters["Target"] is Entity)
            {
                // Obtain the target entity from the input parameters.  
                Entity target = (Entity)context.InputParameters["Target"];

                string name = target.GetAttributeValue<string>("new_value");
                OptionSetValue optionSetValue = target.GetAttributeValue<OptionSetValue>("new_type");


                int cmTypeValue = optionSetValue.Value;
                
                trace.Trace($"status = {cmTypeValue.ToString()} ");

                target["new_name"] = ($"{name} - {opts[optionSetValue.Value]}");

            }
        }
    }
}
