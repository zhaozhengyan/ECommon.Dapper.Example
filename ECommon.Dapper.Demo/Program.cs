using Dapper;
using System;
using System.Configuration;
using System.Data.SqlClient;

namespace ECommon.Dapper.Demo
{
    class Program
    {
        public static string connectionString = ConfigurationManager.ConnectionStrings["default"].ConnectionString;
        public static string tableName = "[User]";
        static void Main(string[] args)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                #region 查询
                ////Dapper原生写法
                //var sql = string.Format($"SELECT * FROM {tableName} WHERE Id=@Id");
                //var user = connection.QueryFirstOrDefault<User>(sql, new { Id = 2 });
                ////ECommon的扩展写法
                //var userECommon = connection.QueryList<User>(new { Id = 2 }, tableName).FirstOrDefault();


                ////Dapper原生写法
                //var sql = string.Format($"SELECT Id,UserName FROM {tableName} WHERE Id=@Id");
                //var user = connection.QueryFirstOrDefault<User>(sql, new { Id = 2 });
                ////ECommon的扩展写法
                //var userECommon = connection.QueryList<User>(new { Id = 2 }, tableName, "Id,UserName").FirstOrDefault();

                //Console.WriteLine(user.UserName);
                //Console.WriteLine(userECommon.UserName);
                #endregion

                #region 创建
                ////原生写法
                //var sql = $"INSERT INTO {tableName} VALUES (@UserName, @Password, @CreateTime,@CategoryId)";
                ////返回值为受影响的行数
                //var result = connection.Execute(sql, new
                //{ UserName = "jack", Password = "123456", CreateTime = DateTime.Now, CategoryId = 1 });

                ////更新部分字段
                //var partialSql = $"INSERT INTO {tableName}(UserName,Password,CreateTime) VALUES (@UserName, @Password, @CreateTime)";
                //var resultPartial = connection.Execute(partialSql, new
                //{ UserName = "jack", Password = "123456", CreateTime = DateTime.Now });

                ////ECommon的扩展写法
                ////返回值为当前主键的值
                //var resultECommon = connection.Insert(new
                //{ UserName = "jack", Password = "123456", CreateTime = DateTime.Now }, tableName);
                //Console.WriteLine(result);
                //Console.WriteLine(resultECommon); 
                #endregion

                #region 更新

                ////原生写法
                //var sql = $"UPDATE {tableName} SET UserName=@UserName WHERE Id=@Id";
                //var result = connection.Execute(sql, new { UserName = "mark", Id = 2 });

                ////ECommon的扩展写法
                //var resultECommon = connection.Update(new { UserName = "mark" }, new { Id = 3 }, tableName);

                //Console.WriteLine(result);
                //Console.WriteLine(resultECommon); 
                #endregion

                #region 删除
                //原生写法
                var sql = $"DELETE FROM {tableName} WHERE Id=@Id";
                var result = connection.Execute(sql, new { Id = 2 });

                //ECommon的扩展写法
                var resultECommon = connection.Delete(new { Id = 3 }, tableName);

                Console.WriteLine(result);
                Console.WriteLine(resultECommon);
                #endregion
            }


            Console.ReadKey();
        }


    }
}
