using Dapper;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ECommon.Dapper.Demo
{
    class Program
    {
        public static string ConnectionString = ConfigurationManager.ConnectionStrings["default"].ConnectionString;
        public static string TableName = "[User]";

        private static void Main(string[] args)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                //Query()；
                Console.WriteLine($"主线程Id:{Thread.CurrentThread.ManagedThreadId}");
                //Task.Run(QueryAsync);
                var result = Task.Run(QueryAsync).Result;
                Console.WriteLine(result);

                #region other

                #region 创建


                #endregion

                #endregion
            }


            Console.ReadKey();
        }

        public static void Query()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                //Dapper原生写法
                var sql = string.Format($"SELECT * FROM {TableName} WHERE Id=@Id");
                var user = connection.QueryFirstOrDefault<User>(sql, new { Id = 2 });
                //ECommon的扩展写法
                var userModel = new User { Id = 2 };
                var userModel2 = new User { Id = 1, UserName = "F3EEFC36-B070-4DD3-99B2-CB3F10745E22" };

                var userECommon = connection.QueryList<User>(userModel, TableName).FirstOrDefault();
                var userECommon2 = connection.QueryList<User>(userModel2, TableName).FirstOrDefault();


                ////Dapper原生写法
                //var sql = string.Format($"SELECT Id,UserName FROM {TableName} WHERE Id=@Id");
                //var user = connection.QueryFirstOrDefault<User>(sql, new { Id = 2 });
                ////ECommon的扩展写法
                //var userECommon = connection.QueryList<User>(new { Id = 2 }, TableName, "Id,UserName").FirstOrDefault();

                Console.WriteLine(userECommon.UserName);
                Console.WriteLine(userECommon2.UserName);
            }
        }
        public static async Task<string> QueryAsync()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var sql = string.Format($"SELECT * FROM {TableName} WHERE Id=@Id");
                var user = await connection.QueryFirstOrDefaultAsync<User>(sql, new { Id = 2 });
                Console.WriteLine($"函数线程ThreadId:{Thread.CurrentThread.ManagedThreadId},UserName:{user.UserName}");
                return user.UserName;
            }

        }
        public static void Create()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                //原生写法
                var sql = $"INSERT INTO {TableName} VALUES (@UserName, @Password, @CreateTime,@CategoryId)";
                //返回值为受影响的行数
                var result = connection.Execute(sql, new
                { UserName = "jack", Password = "123456", CreateTime = DateTime.Now, CategoryId = 1 });

                //更新部分字段
                var partialSql = $"INSERT INTO {TableName}(UserName,Password,CreateTime) VALUES (@UserName, @Password, @CreateTime)";
                var resultPartial = connection.Execute(partialSql, new
                { UserName = "jack", Password = "123456", CreateTime = DateTime.Now });

                //ECommon的扩展写法
                //返回值为当前主键的值
                var resultECommon = connection.Insert(new
                { UserName = "jack", Password = "123456", CreateTime = DateTime.Now }, TableName);
                Console.WriteLine(result);
                Console.WriteLine(resultECommon);
            }

        }
        public static void Update()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                //原生写法
                var sql = $"UPDATE {TableName} SET UserName=@UserName WHERE Id=@Id";
                var result = connection.Execute(sql, new { UserName = "mark", Id = 2 });

                //ECommon的扩展写法
                var resultECommon = connection.Update(new { UserName = "mark" }, new { Id = 3 }, TableName);

                Console.WriteLine(result);
                Console.WriteLine(resultECommon);
            }
        }
        public static void Delete()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                //原生写法
                var sql = $"DELETE FROM {TableName} WHERE Id=@Id";
                var result = connection.Execute(sql, new { Id = 2 });

                //ECommon的扩展写法
                var resultECommon = connection.Delete(new { Id = 3 }, TableName);

                Console.WriteLine(result);
                Console.WriteLine(resultECommon);
            }
        }
    }
}
