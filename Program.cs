﻿
using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
class FileData
{
    public int Id { get; set; }
    public DateTime DateCreated { get; set; }

    public string Author { get; set; }
    public string FileTitle { get; set; }
    public string FileType { get; set; }

}

class Program
{
    static void Main()
    {
        // inits
        int ID_int = 0;
        string json_path = "./file_list.json";
        using (FileStream fs = File.Create(json_path)) ;


            Console.WriteLine("FILE LISTER");
        Console.WriteLine("===========");

        // search path and put into array
        string[] files_list = Directory.GetFiles("./", "*.*", SearchOption.AllDirectories);




        // loop array to get file info of files
        foreach (string file_name in files_list)
        {
            ID_int += 1;


            // file_name
            Console.WriteLine(file_name);
            FileInfo file_info = new FileInfo(file_name);

            // date created
            DateTime dt_created = file_info.CreationTime;
            Console.WriteLine(dt_created);


            // author name
            string author_name = file_info.GetAccessControl().GetOwner(typeof(System.Security.Principal.NTAccount)).ToString();
            Console.WriteLine(author_name);

            // title
            string file_title =  Path.GetFileName(file_name);
            Console.WriteLine(file_title);

            // file type
            string file_type = Path.GetExtension(file_name);
            Console.WriteLine(file_type);



            FileData file_data = new FileData()
            {
                Id = ID_int,
                DateCreated = dt_created,
                Author = author_name,
                FileTitle = file_title,
                FileType = file_type
            };


            Console.WriteLine("Exporting data ...");
            string stringjson = JsonConvert.SerializeObject(file_data);
            Console.WriteLine(stringjson);
            File.AppendAllText(json_path, stringjson);
        }
    }
}