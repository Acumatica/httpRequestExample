using System;
using System.Collections;
using System.Net.Http;

using PX.Data;

namespace GetValueFromAPIExample
{
    public class ARSetupMaint_Extension : PXGraphExtension<PX.Objects.AR.ARSetupMaint>
    {
        // we need IsActive here to be able to enable/disable the extension conditionally. 
        // for more information see https://help.acumatica.com/Help?ScreenId=ShowWiki&pageid=cd70b408-b389-4bd8-8502-3d9c12b11112
        public static bool IsActive()
        {
            return true;
        }

        [InjectDependency]
        // IHttpClientFactory is used to create HttpClient instances efficiently. 
        // It helps manage the lifetime of HttpClient, preventing common issues like 
        // socket exhaustion caused by instantiating HttpClient manually.
        public IHttpClientFactory HttpClientFactory
        {
            get;
            set;
        }

        // this adds a button to the ARSetup screen
        public PXAction<PX.Objects.AR.ARSetup> GetDataFromExternalAPI;
        [PXButton(CommitChanges = true)]
        [PXUIField(DisplayName = "Get Data From External API")]
        protected IEnumerable getDataFromExternalAPI(PXAdapter adapter)
        {
            string responseBody = "";
            var key = Guid.NewGuid();

            // this is a special way to run async operations in Acumatica
            Base.LongOperationManager.StartAsyncOperation(key, async cancellationToken =>
                {
                    using (var client = HttpClientFactory.CreateClient())
                    {
                        HttpResponseMessage response = await client.GetAsync("https://reqres.in/api/users", cancellationToken);
                        response.EnsureSuccessStatusCode();

                        responseBody = await response.Content.ReadAsStringAsync();
                    }
                }
            );
            // wait for the operation to complete using the key we've assigned to the operation
            Base.LongOperationManager.WaitCompletion(key);

            // since the custom field we want to write the data to is defined in an extension, we need to get the extension object first
            var extension = PXCache<PX.Objects.AR.ARSetup>.GetExtension<ARSetupExt>(Base.ARSetupRecord.Current);
            extension.UsrTestField = responseBody;

            //need to update the record for the changes to be properly applied
            Base.ARSetupRecord.Update(Base.ARSetupRecord.Current);


            return adapter.Get();
        }
    }
}