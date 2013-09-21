using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileWatcherService.Responders
{
    public interface IResponder
    {
        void OnChanged(object source, FileSystemEventArgs e);
        void OnCreated(object source, FileSystemEventArgs e);
        void OnDeleted(object source, FileSystemEventArgs e);
        void OnRenamed(object source, RenamedEventArgs e);
    }
}
