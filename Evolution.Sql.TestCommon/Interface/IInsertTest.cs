using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Sql.TestCommon.Interface
{
    public interface IInsertTest
    {
        void Insert_With_Inline_Sql();

        void Insert_With_StoredProcedure();
    }
}
