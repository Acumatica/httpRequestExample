using System;
using System.Collections;

using PX.Data;

namespace GetValueFromAPIExample
{
    public class ARSetupMaint_Extension : PXGraphExtension<PX.Objects.AR.ARSetupMaint>
    {
        public static bool IsActive()
        {
            return true;
        }

        [InjectDependency]
        public IExternalAPIService ExternalAPIService
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
                    responseBody = await ExternalAPIService.GetDataFromApi(cancellationToken);
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