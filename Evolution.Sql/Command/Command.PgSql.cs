﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Evolution.Sql
{
    internal class PgSqlCommand : AbstractCommand
    {
        static Dictionary<string, DbType> portableType = new Dictionary<string, DbType>()
        {
            //# Numeric
            {"smallint", DbType.Int16  },
            {"integer",  DbType.Int32 },
            {"bigint",  DbType.Int64 },
            {"decimal", DbType.Decimal },
            {"numeric", DbType.Decimal },
            {"real", DbType.Single  },
            {"double precision", DbType.Double },
            {"smallserial", DbType.UInt16 },
            {"serial", DbType.UInt32 },
            {"bigserial", DbType.UInt64 },
            //# Money
            {"money", DbType.Currency },
            //# Character
            {"character varying", DbType.String },
            {"varchar",  DbType.String},
            {"character",  DbType.StringFixedLength},
            {"char",  DbType.StringFixedLength },
            {"text", DbType.String  },
            //# Binary
            {"bytea",  DbType.Binary},
            //# Date/Time
            {"timestamp", DbType.DateTime2},
            {"timestamp with timezone", DbType.DateTime2 },
            {"date", DbType.Date },
            {"time", DbType.Time },
            {"time with timezone", DbType.DateTimeOffset },
            //{"interval" , DbType.},
            //# Boolean
            {"boolean", DbType.Boolean  },
            //# Enuerated Type: TO BE DECIDED
            //# Geometric Types: TO BE DECIDED
            //# Network Address Types: TO BE DECIDED
            //# bit string

            //# UUID
            {"uuid", DbType.Guid },
            //# xml
            {"xml", DbType.Xml },
            //# json
            {"json", DbType.String },
            {"jsonb", DbType.String },
            {"jsonpath", DbType.String }
        };
        protected override string DefaultSchema => "public";
        protected override string ParameterSymbol => "@";
        protected override Dictionary<string, DbType> DbDataTypeDbTypeMap => portableType;
        public PgSqlCommand() : base()
        {
        }
    }
}
