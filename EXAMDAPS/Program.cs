using Dapper;
using EXAMDAPS.Model;
using System.Data;
using System.Data.SqlClient;

namespace EXAMDAPS
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //все 5 процедур !

            //Console.WriteLine(DapperDB.pAddBookCall());
            //Console.WriteLine(DapperDB.pUpdBookCall());
            //Console.WriteLine(DapperDB.pDelBookCall());
            //DapperDB.pSelBookCall();
            //DapperDB.pSelBookCallByID();
            //DapperDB.TransAction();
            DapperDB.pAuthorReturnByID();
        }
    }
    public class DapperDB
    {
        public static string connectionString = @"Server=206-4\SQLEXPRESS;Database=examDB;Trusted_Connection=True";

       public  static string pAddBookCall()
        {
            using (var db = new SqlConnection(connectionString))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("Title", "ahah");
                parameters.Add("AuthorId", 1);
                parameters.Add("CategoryId", 1);
                parameters.Add("Pages", 200);
                parameters.Add("Cost", 500);
                var res = db.Query<Book>("pAddBook", parameters, commandType: CommandType.StoredProcedure);
                if (res != null)
                {
                    return "ok";
                }
                return "not ok";
            }
        }
        public static string pUpdBookCall()
        {
            using (var db = new SqlConnection(connectionString))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("id", 2);
                parameters.Add("Title", "ahah");
                parameters.Add("AuthorId", 1);
                parameters.Add("CategoryId", 1);
                parameters.Add("Pages", 500);
                parameters.Add("Cost", 666);
                var res = db.Query<Book>("pUpdBook", parameters, commandType: CommandType.StoredProcedure);
                if (res != null)
                {
                    return "ok";
                }
                return "not ok";
            }
        }
        public static string pDelBookCall()
        {
            using (var db = new SqlConnection(connectionString))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("id", 2);
                var res = db.Query<Book>("pDelBook", parameters, commandType: CommandType.StoredProcedure);
                if (res != null)
                {
                    return "ok";
                }
                return "not ok";
            }
        }
        public static void pSelBookCall()
        {
            using (var db = new SqlConnection(connectionString))
            {
                var res = db.Query<Book>("pSelBook", commandType: CommandType.StoredProcedure);

                foreach (var item in res)
                {
                    Console.WriteLine($"id = {item.Id} title = {item.Title} catID = {item.CategoryId} autID =  {item.AuthorId} cost = {item.Cost} pages = {item.Pages}\n");
                }
            }
        }
        public static void pSelBookCallByID()
        {
            using (var db = new SqlConnection(connectionString))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("id", 4);
                var res = db.Query<Book>("pSelBookByID",parameters, commandType: CommandType.StoredProcedure);

                foreach (var item in res)
                {
                    Console.WriteLine($"id = {item.Id} title = {item.Title} catID = {item.CategoryId} autID =  {item.AuthorId} cost = {item.Cost} pages = {item.Pages}\n");
                }
            }
        }
        public static void TransAction()
        {
            using (SqlConnection db = new SqlConnection(connectionString))
            {
                db.Open();
                SqlTransaction tran = db.BeginTransaction();
                try
                {
                    using (SqlCommand cmd = new SqlCommand("insert into Author values('Тестовый автор Фамилия', 'Тестовый автор Имя')", db))
                    {
                        cmd.Transaction = tran;
                        cmd.ExecuteNonQuery();
                        Console.WriteLine("ok");
                    }
                    using (SqlCommand cmd = new SqlCommand(" insert into Category  values('Тестовая категория')", db))
                    {
                        cmd.Transaction = tran;
                        cmd.ExecuteNonQuery();
                        Console.WriteLine("ok");
                    }
                    using (SqlCommand cmd = new SqlCommand("insert into Book values('Словарь', (select id from Author where FirstName = 'Тестовый автор Имя'),  (select id from Category where Name = 'Тестовая категория'), 200, 15000.00), ('Книга рецептов',(select id from Author where FirstName = 'Тестовый автор Имя'), (select id from Category where Name = 'Тестовая категория'), 1200, 12000.00), ('Уголовный кодекс РК',(select id from Author where FirstName = 'Тестовый автор Имя'), (select id from Category where Name = 'Тестовая категория'), 500000, 20000.00)", db))
                    {
                        cmd.Transaction = tran;
                        cmd.ExecuteNonQuery();
                        Console.WriteLine("ok");
                    }
                    Console.WriteLine("all ok");
                    tran.Commit();
                }
                catch (Exception)
                {
                    Console.WriteLine("err error ROLLBAKU!");
                    tran.Rollback();
                }
                db.Close();
            }

        }
        public static void pAuthorReturnByID()
        {
            using (var db = new SqlConnection(connectionString))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("id", 1);
                parameters.Add("PagesSum", dbType: DbType.Int32,direction: ParameterDirection.Output);
                parameters.Add("BookCount", dbType: DbType.Int32, direction: ParameterDirection.Output);
                parameters.Add("BookMaxPrice", dbType: DbType.Int32, direction: ParameterDirection.Output);

                var res = db.Query<AuthorJoin>("pAuthorReturnByID", parameters, commandType: CommandType.StoredProcedure);
                foreach (var item in res)
                {
                    Console.WriteLine($"{item.FirstName} {item.Title} {item.Name}");
                }
                Console.WriteLine(parameters.Get<int>("PagesSum"));
                Console.WriteLine(parameters.Get<int>("BookCount"));
                Console.WriteLine(parameters.Get<int>("BookMaxPrice"));

            }
        }

    }
}