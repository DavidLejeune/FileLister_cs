
using System;
using System.Linq;
using System.IO;
class Program
{
    static void Main()
    {

        Console.WriteLine("FILE LISTER");
        Console.WriteLine("===========");

        // search path and put into array
        string[] files_list = Directory.GetFiles("./", "*.*", SearchOption.AllDirectories);


        // loop array to get file info of files
        foreach (string file_name in files_list)
        {
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
        }
    }
}