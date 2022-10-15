
using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
class FileData
{
    public int Id { get; set; }
    public string FileName { get; set; }
    public string FilePath { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime DateModified { get; set; }
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
                DateCreated = dt_created,
                DateModified = dt_modified,
                Author = author_name,
                FileType = file_type
            };


            //Console.WriteLine("Exporting data ...");
            string stringjson = JsonConvert.SerializeObject(file_data);
            //Console.WriteLine(stringjson);
            File.AppendAllText(json_path, stringjson);
        }

        // final output
        watch.Stop();
        Console.WriteLine($"Files logged  : {ID_int} ");
        Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");
    }
}