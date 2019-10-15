using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Sql.TestCommon.Interface
{
    public interface IUpdateTest
    {
        void Update_With_Inline_Sql();
        void Update_With_StoredProcedure();
    }
}
