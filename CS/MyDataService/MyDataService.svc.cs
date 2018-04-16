using System;
using System.Collections.Generic;
using System.Data.Services;
using System.Data.Services.Common;
using System.Data.Services.Providers;
using System.Linq;
using System.ServiceModel.Web;
using System.Web;

namespace MyDataService {
    [System.ServiceModel.ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    [JSONPSupportBehavior]
    public class DataService : DataService<TestDataEntities>, IServiceProvider {
        public static void InitializeService(DataServiceConfiguration config) {
            config.SetEntitySetAccessRule("*", EntitySetRights.AllRead);
            config.DataServiceBehavior.MaxProtocolVersion = DataServiceProtocolVersion.V3;
        }
        public object GetService(Type serviceType) {
            if (serviceType == typeof(IDataServiceStreamProvider)) {
                return new ImageStreamProvider();
            }
            return null;
        }
        protected override void OnStartProcessingRequest(ProcessRequestArgs args) {
            CustomBasicAuth.Authenticate(HttpContext.Current);
            if (HttpContext.Current.User == null)
                throw new DataServiceException(401, "Invalid login or password");
            base.OnStartProcessingRequest(args);
        }
    }
}
