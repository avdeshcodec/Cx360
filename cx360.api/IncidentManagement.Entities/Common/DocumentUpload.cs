using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace IncidentManagement.Entities.Common
{
    public class DocumentUpload
    {
        public string saveDocumentInFolder(string doc, string type)
        {
            string basePath = "C:\\Project\\Cx360\\cx360.api\\IncidentManagement.API\\Upload";
        
            byte[] docBytes = Convert.FromBase64String(doc);
            MemoryStream ms = new MemoryStream(docBytes, 0, docBytes.Length);


            string newFile = "";

            if (type.ToUpper() == "PDF")
            {
                newFile = Guid.NewGuid().ToString() + ".pdf";
            }
            
            var FilePath = Path.Combine(basePath);

            var path = Path.Combine(FilePath, newFile);

            bool exists = System.IO.Directory.Exists(FilePath);

            if (!exists)
            {
                System.IO.Directory.CreateDirectory(FilePath);
            }


            if (docBytes.Length > 0)
            {
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    stream.Write(docBytes, 0, docBytes.Length);
                    stream.Flush();
                }
            }

            path = path.Replace(basePath, "").Replace("\\", "/");
            return path;

        }
    }
}
