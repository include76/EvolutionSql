using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text;

namespace Evolution.Sql.SQLiteTest
{
    public class InitDatabase
    {
        static string connStr = "Data Source=blog.sqlite;Version=3;";
        public static void Init()
        {
            var conn = new SQLiteConnection(connStr);
            // create user table
            var sql = "DROP TABLE IF EXISTS user";
            conn.Sql(sql).Execute();
            sql = @"CREATE TABLE IF NOT EXISTS user(
                user_id         TEXT,
                first_name      TEXT,
                last_name       TEXT,
                created_by      TEXT,
                created_on      TEXT,
                updated_by      TEXT,
                updated_on      TEXT 
            )";
            conn.Sql(sql).Execute();
            sql = "DELETE FROM user;";
            conn.Sql(sql).Execute();
            // create table blog
            sql = "DROP TABLE IF EXISTS blog";
            conn.Sql(sql).Execute();
            sql = @"CREATE TABLE blog(
                    blog_id     INTEGER PRIMARY KEY AUTOINCREMENT,
                    title       TEXT,
                    content     TEXT,
                    created_by  TEXT,
                    created_on  REAL,
                    updated_by  TEXT,
                    updated_on  INTEGER
            )";
            conn.Sql(sql).Execute();
            conn.Close();
        }
    }
}
