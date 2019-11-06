using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Evolution.Sql.SqlServerTest.Modal
{
    public class DataTypeModal
    {
        public long? ColBigInt { get; set; }
        public bool? ColBit { get; set; }
        public decimal? ColDecimal { get; set; }
        public int? ColInt { get; set; }
        public decimal? ColMoney { get; set; }
        public decimal? ColNumeric { get; set; }
        public Int16? ColSmallInt { get; set; }
        public decimal? ColSmallMoney { get; set; }
        public Byte? ColTinyInt { get; set; }

        public double? ColFloat { get; set; }
        public Single? ColReal { get; set; }

        public DateTime? ColDate { get; set; }
        public DateTime? ColDatetime { get; set; }
        public DateTime? ColDatetime2 { get; set; }
        public DateTimeOffset? ColDatetimeOffset { get; set; }
        public DateTime? ColSmallDatetime { get; set; }
        //https://stackoverflow.com/questions/8503825/what-is-the-correct-sql-type-to-store-a-net-timespan-with-values-240000
        //public TimeSpan? ColTime { get; set; } do NOT use TimeSpan, you can never set DbType to Time becaus .NET bug
        public DateTime? ColTime { get; set; }

        //public char[] ColChar { get; set; }
        public string ColChar { get; set; }
        public string ColText { get; set; }
        //public char[] ColVarchar { get; set; }
        public string ColVarchar { get; set; }
        //public char[] ColNChar { get; set; }
        public string ColNChar { get; set; }
        public string ColNText { get; set; }
        //public char[] ColNVarchar { get; set; }
        public string ColNVarchar { get; set; }

        public byte[] ColBinary { get; set; }
        public byte[] ColImage { get; set; }
        public byte[] ColVarBinary { get; set; }
        public string ColXml { get; set; }

        public byte[] ColTimestamp { get; set; }
    }
}
