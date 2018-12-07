using Common.DBFasade;
using FileService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileService.Repository
{
    public class FileZipUploadRepo : BaseDAO<FileZipUpload, int>
    {

        public FileZipUpload RetrieveByStatus(FileBatchStatus status)
        {
            var session = BuildSession();
            var aBatch = session.Query<FileZipUpload>()
                .Where(x => x.Status == status)
                .OrderByDescending(r => r.Id).FirstOrDefault();
            return aBatch;
        }

    }
}
