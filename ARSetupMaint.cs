using System;
using System.Collections;
using System.Net.Http;

using PX.Data;

namespace GetValueFromAPIExample
{
    public class ARSetupMaint_Extension : PXGraphExtension<PX.Objects.AR.ARSetupMaint>
    {
        //we need IsActive here to be able to enable/disable the extension conditionally. 
        //for more information see https://help.acumatica.com/Help?ScreenId=ShowWiki&pageid=cd70b408-b389-4bd8-8502-3d9c12b11112
        public static bool IsActive()
        {
            return true;
        }

        [InjectDependency]
        public IHttpClientFactory HttpClientFactory
        {
            get;
            set;
        }

        public PXAction<PX.Objects.AR.ARSetup> GetDataFromExternalAPI;
        [PXButton(CommitChanges = true)]
        [PXUIField(DisplayName = "Get Data From External API")]
        protected IEnumerable getDataFromExternalAPI(PXAdapter adapter)
        {
            string responseBody = "";
            var key = Guid.NewGuid();
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
            Base.LongOperationManager.WaitCompletion(key);

            var extension = PXCache<PX.Objects.AR.ARSetup>.GetExtension<ARSetupExt>(Base.ARSetupRecord.Current);
            extension.UsrTestField = responseBody;
            Base.ARSetupRecord.Update(Base.ARSetupRecord.Current);
            return adapter.Get();
        }
    }
}