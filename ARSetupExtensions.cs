using System;
using PX.Data;

namespace GetValueFromAPIExample
{
    public class ARSetupExt : PXCacheExtension<PX.Objects.AR.ARSetup>
    {
        // we need IsActive here to be able to enable/disable the extension conditionally. 
        // for more information see https://help.acumatica.com/Help?ScreenId=ShowWiki&pageid=9ca4cca5-a46c-4dda-af09-8cb8b0793c34
        public static bool IsActive()
        {
            return true;
        }
        #region UsrTestField
        [PXString(50)] // since it is PXString and not PXDBString the field is not going to be read from the database or written to it
        [PXUIField(DisplayName = "Test Field")]

        public virtual string UsrTestField { get; set; }
        public abstract class usrTestField : PX.Data.BQL.BqlString.Field<usrTestField> { }
        #endregion
    }
}