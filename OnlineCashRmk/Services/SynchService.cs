using Microsoft.Extensions.Logging;
using OnlineCashRmk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCashRmk.Services
{
    public class SynchService : ISynchService
    {
        DataContext db_;
        ILogger<SynchService> logger_;
        private static List<DocSynch> DocSynches = new List<DocSynch>();

        public SynchService(DataContext db, ILogger<SynchService> logger)
        {
            db_ = db;
            logger_ = logger;
        }
        public void AppendDoc(DocSynch docSynch)
        {
            db_.DocSynches.Add(docSynch);
            db_.SaveChanges();
            DocSynches.Add(docSynch);
        }
    }
}
