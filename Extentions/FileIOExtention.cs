using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EmailFetcherDemo.Extentions
{
    public static class FileIOExtention
    {
        public static void CreateFilePathIfNotExist(string filePath)
        {
            if (!Directory.Exists(filePath)) System.IO.Directory.CreateDirectory(filePath);
        }
        public static void UploadFile(byte[] fileContent,string fileName)
        {
            string folderPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? "", "Bora");
            CreateFilePathIfNotExist(folderPath);
            var filePath = Path.Combine(folderPath, fileName);
            File.WriteAllBytes(filePath, fileContent);

        }
    }
}
