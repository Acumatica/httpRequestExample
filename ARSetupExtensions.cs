using System;
using PX.Data;

namespace GetValueFromAPIExample
{
    public class ARSetupExt : PXCacheExtension<PX.Objects.AR.ARSetup>
    {
        public static bool IsActive()
        {
            return true;
        }
        #region UsrTestField
        [PXString(50)]
        [PXUIField(DisplayName = "TestField")]

        public virtual string UsrTestField { get; set; }
        public abstract class usrTestField : PX.Data.BQL.BqlString.Field<usrTestField> { }
        #endregion
    }
}