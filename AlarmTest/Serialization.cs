using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AlarmTest
{
    public class Serialization
    {
        public static void Serialize(ObservableCollection<Alarm> alarms)
        {
            XmlSerializer xs = new XmlSerializer(typeof(ObservableCollection<Alarm>));

            TextWriter txtWriter = new StreamWriter(ConfigurationManager.AppSettings["XmlPath"]);

            xs.Serialize(txtWriter, alarms);

            txtWriter.Close();
        }
    }
}
