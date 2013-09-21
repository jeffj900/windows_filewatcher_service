using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace FileWatcherService.Responders
{
    class EventLogResponder : IResponder
    {
        const string LOG_SOURCE = "FileWatcherService";
        const string LOG_LOCATION = "Application";

        EventLog _log;
        public EventLogResponder(ServiceBase parent)
        {
            this._log = parent.EventLog;
            if (!EventLog.SourceExists(LOG_SOURCE))
            {
                EventLog.CreateEventSource(LOG_SOURCE, LOG_LOCATION);
            }
        }

        void IResponder.OnChanged(object source, FileSystemEventArgs e)
        {
            String eventMessage = String.Format("We changed '{0}', yo", e.FullPath);
            _log.WriteEntry(eventMessage);
        }

        void IResponder.OnCreated(object source, FileSystemEventArgs e)
        {
            String eventMessage = String.Format("We created '{0}', yo", e.FullPath);
            _log.WriteEntry(eventMessage);
        }

        void IResponder.OnDeleted(object source, FileSystemEventArgs e)
        {
            String eventMessage = String.Format("We deleted '{0}', yo", e.FullPath);
            _log.WriteEntry(eventMessage);
        }

        void IResponder.OnRenamed(object source, RenamedEventArgs e)
        {
            String eventMessage = String.Format("We renamed '{0}' to '{1}', yo", e.OldFullPath, e.FullPath);
            _log.WriteEntry(eventMessage);
        }
    }
}
