using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ServiceModel;
using Microsoft.Xrm.Sdk;

namespace NinjaBank.ContactMethod
{        
    

    public class ContactMethodPreUpdate : IPlugin
    {
        
        public void Execute(IServiceProvider serviceProvider)
        {        
            Dictionary<int, string> opts = new Dictionary<int, string>();
            opts.Add(100000000, "טלפון");
            opts.Add(100000001, "מייל");


            OptionSetValue optionSetValue = null;
            string name = null;
            Entity target = null;


            IPluginExecutionContext context = (IPluginExecutionContext)
                serviceProvider.GetService(typeof(IPluginExecutionContext));



            if (context.InputParameters.Contains("Target") &&
                context.InputParameters["Target"] is Entity)
            {
                target = (Entity)context.InputParameters["Target"];

                name = target.GetAttributeValue<String>("new_value");

                optionSetValue = target.GetAttributeValue<OptionSetValue>("new_type");

            }

            if (context.PreEntityImages.Contains("preUpdateImage") &&
                context.PreEntityImages["preUpdateImage"] is Entity)
            {
                Entity preImage = context.PreEntityImages["preUpdateImage"];
                
                if(optionSetValue == null)
                {
                    optionSetValue = preImage.GetAttributeValue<OptionSetValue>("new_type");
                }

                if(name == null)
                {
                    name = preImage.GetAttributeValue<String>("new_value");
                }
            }

            int cmTypeValue = optionSetValue.Value;

            target["new_name"] = ($"{name} - {opts[cmTypeValue]}");


        }
    }
}
