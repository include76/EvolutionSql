using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Sql.PgSqlTest.Model
{
    public class DataTypeModel
    {
        #region Numeric Types
        public Int16 ColSmallInt { get; set; }
        public Int32 ColInteger { get; set; }
        public Int64 ColBigInt { get; set; }
        public decimal ColDecimal { get; set; }
        public decimal ColNumeric { get; set; }
        public float ColReal { get; set; }
        public double ColDoublePrecision { get; set; }
        public Int16 ColSmallSerial { get; set; }
        public Int32 ColSerial { get; set; }
        public Int64 ColBigSerial { get; set; }
        #endregion

        #region Monetary Types
        public decimal ColMoney { get; set; }
        #endregion

        #region Character Types
        public string ColCharacterVarying { get; set; }
        public string ColVarchar { get; set; }
        public string ColCharacter { get; set; }
        public string ColChar { get; set; }
        public string ColText { get; set; }
        #endregion

        #region Date/Time Types
        public DateTime ColTimestamp { get; set; }
        public DateTime ColDate { get; set; }
        public TimeSpan ColTime { get; set; }
        public TimeSpan ColInterval { get; set; }
        #endregion

        #region Boolean Type
        public bool ColBoolean { get; set; }
        #endregion

        #region Bit String Types
        
        #endregion
    }
}
