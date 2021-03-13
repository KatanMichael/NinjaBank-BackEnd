using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;

namespace NinjaBank.Common
{
    public class AutoNumber : IPlugin
    {
        IOrganizationService service;
        public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext context = (IPluginExecutionContext)
                        serviceProvider.GetService(typeof(IPluginExecutionContext));

            Entity target = (Entity) context.InputParameters["Target"]; //new_case (for now)

            IOrganizationServiceFactory serviceFactory =
                    (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));

            service = serviceFactory.CreateOrganizationService(context.UserId);


            string entityName = target.LogicalName; //new_case (for now)

            Entity autoNumber = getAutonumberByEntityName(entityName);


            int counter = autoNumber.GetAttributeValue<int>("new_number");
            counter++;

            string fieldName = autoNumber.GetAttributeValue<string>("new_fieldname");
            target[fieldName] = counter.ToString();

            autoNumber["new_number"] = counter;
            service.Update(autoNumber);
        }

        private Entity getAutonumberByEntityName(string entityName) //new_case
        {

            Entity autoNumber = null;

            QueryExpression query = new QueryExpression("new_autonumber");  //from new_autonumber

            query.ColumnSet = new ColumnSet("new_number", "new_fieldname"); //select new_number, new_fieldname

            query.Criteria.AddCondition("new_name", ConditionOperator.Equal, entityName); // where new_name == new_case

            EntityCollection result = service.RetrieveMultiple(query);

            if(result.Entities.Count > 0)
            {
                autoNumber = result.Entities[0];
            }

            return autoNumber;
        }
    }
}
