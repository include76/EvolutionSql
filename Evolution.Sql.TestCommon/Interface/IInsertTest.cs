using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Evolution.Sql.TestCommon.Interface
{
    public interface IInsertTest
    {
        void Insert_With_Inline_Sql();

        Task Insert_With_Inline_Sql_Auto_Generated_Id();

        void Insert_With_StoredProcedure();
    }
}
