
using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Data.SqlClient;
using static System.Net.Mime.MediaTypeNames;
using System.Data;
using System.Text;

class FileData
{
    public int Id { get; set; }
    public string FileName { get; set; }
    public string FilePath { get; set; }
    public DateTime DateTimeCreated { get; set; }
    public DateTime DateTimeModified { get; set; }
    public string Author { get; set; }
    public string FileType { get; set; }

}

class Program
{
    static void Main()
    {
        Console.WriteLine("FILE LISTER");
        Console.WriteLine("===========\n");

        // inits
        int ID_int = 0;
        string json_path = "./file_list.json";
        using (FileStream fs = File.Create(json_path)) ;

        // Time code execution
        var watch = new System.Diagnostics.Stopwatch();
        watch.Start();

        // search path and put into array
        string[] files_list = Directory.GetFiles("C:\\Users\\David\\Documents\\Projects\\dotnetcore\\FileShareLocation", "*.*", SearchOption.AllDirectories);

        // loop array to get file info of files
        foreach (string file_name in files_list)
        {
            ID_int += 1;
            FileInfo file_info = new FileInfo(file_name);


            // file_name
            //Console.WriteLine(file_name);

            // date created
            DateTime dt_created = file_info.CreationTime;

            // date created
            DateTime dt_modified = file_info.LastWriteTime;

            //Console.WriteLine(dt_created);


            // author name
            string author_name = file_info.GetAccessControl().GetOwner(typeof(System.Security.Principal.NTAccount)).ToString();
            //Console.WriteLine(author_name);

            // title
            string file_title =  Path.GetFileName(file_name);
            //Console.WriteLine(file_title);


            // path
            string file_path = Path.GetFullPath(file_name);

            // file type
            string file_type = Path.GetExtension(file_name);
            //Console.WriteLine(file_type);



            FileData file_data = new FileData()
            {
                Id = ID_int,
                FileName = file_title,
                FilePath = file_path,
                DateTimeCreated = dt_created,
                DateTimeModified = dt_modified,
                Author = author_name,
                FileType = file_type
            };


            //Console.WriteLine("Exporting data ...");
            string stringjson = JsonConvert.SerializeObject(file_data);
            //Console.WriteLine(stringjson);
            File.AppendAllText(json_path, stringjson);


            // SqlConnection conn = new SqlConnection("Server=localhost\\SQLEXPRESS;Database=SharedFilesDB;Trusted_Connection=True;");
            //SqlConnection conn = new("Server=localhost\\SQLEXPRESS;Database=SharedFilesDB;Trusted_Connection=True;");

            SqlConnection conn = new SqlConnection();
            conn.ConnectionString =
                "Data Source = localhost\\SQLEXPRESS;" +
                "Initial Catalog=SharedFilesDB;" +
                "User ID=David;" +
                "Password=Lucian0661;";
         





            try
            {
                conn.Open();
                Console.WriteLine("Connection Established!");
                string sql = "insert into SharedFiles (FileName, FilePath, DateTimeCreated, , DateTimeModified, Author, FileType) values (@file_title, @file_path, @dt_created, @dt_modified, @author_name, @file_type)";

                SqlCommand cmd = new(sql, conn);

                cmd.Parameters.Add("@file_title", SqlDbType.VarChar);
                cmd.Parameters["@file_title"].Value = file_title;

                cmd.Parameters.Add("@file_path", SqlDbType.VarChar);
                cmd.Parameters["@file_path"].Value = file_path;

                cmd.Parameters.Add("@dt_created", SqlDbType.DateTime2);
                cmd.Parameters["@dt_created"].Value = dt_created;

                cmd.Parameters.Add("@dt_modified", SqlDbType.DateTime2);
                cmd.Parameters["@dt_modified"].Value = dt_modified;

                cmd.Parameters.Add("@author_name", SqlDbType.VarChar);
                cmd.Parameters["@author_name"].Value = author_name;

                cmd.Parameters.Add("@file_type", SqlDbType.VarChar);
                cmd.Parameters["@file_type"].Value = file_type;

                Console.WriteLine(sql);


                string sql1 = "INSERT INTO [SharedFiles] ([FileName], [FilePath], [DateTimeCreated], [DateTimeModified], [Author], [FileType]) VALUES (@file_title, @file_path, @dt_created, @dt_modified, @author_name, @file_type)";
                SqlCommand sqlCmd1 = new(sql1 , conn);
                
                sqlCmd1.Parameters.AddWithValue("@file_title", file_title);
                sqlCmd1.Parameters.AddWithValue("@file_path", file_path);
                sqlCmd1.Parameters.AddWithValue("@dt_created", dt_created);
                sqlCmd1.Parameters.AddWithValue("@dt_modified", dt_modified);
                sqlCmd1.Parameters.AddWithValue("@author_name", author_name);
                sqlCmd1.Parameters.AddWithValue("@file_type", file_type);
                sqlCmd1.ExecuteNonQuery();
                Console.WriteLine("sqlCmd1 executed");



                cmd.ExecuteNonQuery();
            }


            catch (System.Data.SqlClient.SqlException ex)
            {
                string msg = "Insert Error:";
                msg += ex.Message;
            }
            finally
            {
                Console.WriteLine("Connection closed");
                conn.Close();
            }





        }

        // final output
        watch.Stop();
        Console.WriteLine($"Files logged  : {ID_int} ");
        Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");
    }
}