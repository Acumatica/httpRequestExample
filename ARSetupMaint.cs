using System;
using System.Collections;
using System.Net.Http;

using PX.Data;

namespace GetValueFromAPIExample
{
    public class ARSetupMaint_Extension : PXGraphExtension<PX.Objects.AR.ARSetupMaint>
    {
        public static bool IsActive()
        {
            return true;
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
                    HttpClient client = new HttpClient();
                    HttpResponseMessage response = await client.GetAsync("https://reqres.in/api/users", cancellationToken);
                    response.EnsureSuccessStatusCode();

                    responseBody = await response.Content.ReadAsStringAsync();
                }
            );
            PXLongOperation.WaitCompletion(key);

            var extension = PXCache<PX.Objects.AR.ARSetup>.GetExtension<ARSetupExt>(Base.ARSetupRecord.Current);
            extension.UsrTestField = responseBody;
            Base.ARSetupRecord.Update(Base.ARSetupRecord.Current);
            return adapter.Get();
        }
    }
}