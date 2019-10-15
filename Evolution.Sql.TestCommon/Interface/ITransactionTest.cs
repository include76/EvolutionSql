using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Sql.TestCommon.Interface
{
    public interface ITransactionTest
    {
        void Commit_Test();
        void Rollback_Test();
    }
}
