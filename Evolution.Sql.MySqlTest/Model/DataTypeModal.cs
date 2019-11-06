using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Sql.MySqlTest.Model
{
    public class DataTypeModel
    {
        //int
        public byte ColTinyInt { get; set; }
        public Int16 ColSmallInt { get; set; }
        public Int32 ColMediumInt { get; set; }
        public Int32 ColInt { get; set; }
        public Int64 ColBigInt { get; set; }
        //
        public decimal ColDecimal { get; set; }
        public decimal ColNumeric { get; set; }
        public float ColFloat { get; set; }
        public double ColDouble { get; set; }
        public double ColReal { get; set; }
        //
        public bool ColBit { get; set; }
        public DateTime ColDateTime { get; set; }
        public DateTime ColDate { get; set; }
        public TimeSpan ColTime { get; set; }
        /// <summary>
        /// error
        /// </summary>
        public DateTime ColTimeStamp { get; set; }
        public int ColYear { get; set; }

        /// <summary>
        /// MySql char[] error, use string instead
        /// </summary>
        //public char[] ColChar { get; set; }
        public string ColChar { get; set; }

        public string ColVarchar { get; set; }
        public string ColTinyText { get; set; }
        public string ColText { get; set; }
        public string ColMediumText { get; set; }
        public string ColLongText { get; set; }
        //
        public byte[] ColBinary { get; set; }
        public byte[] ColVarBinary { get; set; }
        public byte[] ColTinyBlob { get; set; }
        public byte[] ColBlob { get; set; }
        public byte[] ColMediumBlob { get; set; }
        public byte[] ColLongBlob { get; set; }
        //
        public string ColJson { get; set; }
        //
        public bool ColBool { get; set; }
    }
}
