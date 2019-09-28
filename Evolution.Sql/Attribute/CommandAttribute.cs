using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Evolution.Sql.Attribute
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class CommandAttribute: System.Attribute
    {
        private CommandType _commandType = CommandType.StoredProcedure;

        public CommandType CommandType
        {
            get { return _commandType; }
            set { _commandType = value; }
        }

        /// <summary>
        /// name of command
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// stored procedure or inline sql for Insert/Update/Delete
        /// </summary>
        public string Text { get; set; }
    }
}
