using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Text.RegularExpressions;

namespace Evolution.Sql.SqlAdapter
{
    internal abstract class SqlAdapter
    {

        #region Properties

        protected virtual string sqlParameters
        {
            get { return @"select PARAMETER_NAME, PARAMETER_MODE, DATA_TYPE from INFORMATION_SCHEMA.PARAMETERS where SPECIFIC_SCHEMA= @schema and SPECIFIC_NAME = @name"; }
        }

        protected virtual string DefaultSchema { get; }

        protected virtual Dictionary<string, DbType> DbEgineTypeToDbTypeMap { get; set; }

        #endregion

        public SqlAdapter()
        {
            

        }

        
    }
}
