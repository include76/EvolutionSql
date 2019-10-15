using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Sql.TestCommon.Interface
{
    public interface IQueryTest
    {
        void QueryOne_With_Inline_Sql();
        void QueryOne_With_StoredProcedure();

        void Query_With_Inline_Sql();
        void Query_With_StoredProcedure();

        void Get_Null_Value_From_DB_Property_Should_Set_Default_Value();
    }
}
