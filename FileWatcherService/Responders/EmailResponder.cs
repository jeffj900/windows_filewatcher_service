using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace FileWatcherService.Responders
{
    class EmailResponder : IResponder
    {
        SmtpClient _mailClient = null;
        String location = "unknown";
        String receiver;
        EventLog _log;
        public EmailResponder(ServiceBase parent)
        {
            _mailClient = new SmtpClient();
            try
            {
                location = ConfigurationManager.AppSettings["location"];
            }
            catch (Exception)
            {
                parent.EventLog.WriteEntry("Could not find 'location' configuration setting.", EventLogEntryType.Warning, 201);
            }
            try
            {
                String myClassname = this.GetType().FullName;
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                ConfigurationSectionGroup myConfigGroup = config.GetSectionGroup(myClassname);
                AppSettingsSection configuration = (AppSettingsSection)myConfigGroup.Sections["appSettings"];
                receiver = configuration.Settings["receiver"].Value;
            }
            catch (Exception e)
            {
                parent.EventLog.WriteEntry(String.Format("Could not determine 'receiver' configuration setting. {0}", e), EventLogEntryType.Error, 202);
            }
            _log = parent.EventLog;
        }
        void IResponder.OnChanged(object source, System.IO.FileSystemEventArgs e)
        {
            MailMessage message = new MailMessage();
            message.Subject = String.Format("[FileWatcherService] Change at {0} - {1}", location, e.Name);
            message.Priority = MailPriority.High;
            message.Body = String.Format("'{0}' was changed at {1}", e.FullPath, DateTime.Now);
            message.To.Add(receiver);
            try
            {
                _mailClient.Send(message);
            } catch (SmtpException err) {
                _log.WriteEntry(String.Format("Unable to send email notification. {0}", err), EventLogEntryType.Error, 203);
            }
        }

        void IResponder.OnCreated(object source, System.IO.FileSystemEventArgs e)
        {
            MailMessage message = new MailMessage();
            message.Subject = String.Format("[FileWatcherService] Created at {0} - {1}", location, e.Name);
            message.Priority = MailPriority.High;
            message.Body = String.Format("'{0}' was created at {1}", e.FullPath, DateTime.Now);
            message.To.Add(receiver);
            try
            {
                _mailClient.Send(message);
            }
            catch (SmtpException err)
            {
                _log.WriteEntry(String.Format("Unable to send email notification. {0}", err), EventLogEntryType.Error, 203);
            }
        }

        void IResponder.OnDeleted(object source, System.IO.FileSystemEventArgs e)
        {
            MailMessage message = new MailMessage();
            message.Subject = String.Format("[FileWatcherService] Deleted at {0} - {1}", location, e.Name);
            message.Priority = MailPriority.High;
            message.Body = String.Format("'{0}' was deleted at {1}", e.FullPath, DateTime.Now);
            message.To.Add(receiver);
            try
            {
                _mailClient.Send(message);
            }
            catch (SmtpException err)
            {
                _log.WriteEntry(String.Format("Unable to send email notification. {0}", err), EventLogEntryType.Error, 203);
            }
        }

        void IResponder.OnRenamed(object source, System.IO.RenamedEventArgs e)
        {
            MailMessage message = new MailMessage();
            message.Subject = String.Format("[FileWatcherService] Renamed at {0} - {1}", location, e.Name);
            message.Priority = MailPriority.High;
            message.Body = String.Format("'{0}' was renamed to '{1}' at {2}", e.OldFullPath, e.FullPath, DateTime.Now);
            message.To.Add(receiver);
            try
            {
                _mailClient.Send(message);
            }
            catch (SmtpException err)
            {
                _log.WriteEntry(String.Format("Unable to send email notification. {0}", err), EventLogEntryType.Error, 203);
            }
        }
    }
}
