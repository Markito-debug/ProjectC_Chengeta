using Newtonsoft.Json;

namespace Mqttlistener
{
    class AddListenerInfoToDB
    {
        public static string ListenerDataPath = Path.GetFullPath(@"ListenerData.json");

        /// <summary>
        /// This function loads everything that is in the JSON into a Chengeta Database, filling it
        /// Keeping this for no db created purposes
        /// </summary>
        public static void AddJsonToDB()
        {
            var dbContext = new ListenerDb();
            var jsonData = File.ReadAllText(ListenerDataPath);
            var listened = JsonConvert.DeserializeObject<List<Listener>>(jsonData);
            foreach(Listener notif in listened)
            {
                var epoch = new DateTime(1970, 1, 1, 0, 0, 0);
                double time = double.Parse(notif.Time);
                DateTime date = epoch.AddSeconds(time).ToUniversalTime();
                dbContext.Notifs.Add(new Notification(Guid.NewGuid(), date, notif.NodeID, notif.Latitude,notif.Longitude, notif.Sound_type, notif.Probability,notif.Sound ));
            }
            dbContext.SaveChanges();
            Console.WriteLine("Done loading former data");
        }
        

        /// <summary>
        /// This Function adds the notification message to the Chengeta Database
        /// </summary>
        /// <param name="message"></param>
        public static void AddToDB(string message)
        {
            var dbContext = new ListenerDb();

            string[] parsedMsg = Conversion.ParseData(message);

            var epoch = new DateTime(1970, 1, 1, 0, 0, 0);
            double time = double.Parse(parsedMsg[0]);
            DateTime date = epoch.AddSeconds(time).ToUniversalTime();
            Guid id = Guid.NewGuid();
            dbContext.Notifs.Add(new Notification(id, date, Int32.Parse(parsedMsg[1]), (parsedMsg[2]), (parsedMsg[3]), parsedMsg[4], Int32.Parse(parsedMsg[5]), parsedMsg[6]));
            dbContext.SaveChanges();
            var notifStatus = dbContext.Notifs.Where(x => x.ID == id).First();
            notifStatus.Status = "Open";
            notifStatus.Notes = "";
            dbContext.SaveChanges();
            Console.WriteLine("Done adding new notif");
        }
    }


    class Conversion
    {
        public static string[] ParseData(string notif)
        {
            string[][] dataSplit = notif.Split(',').Select(x => x.Split(':')).ToArray();
            string[] time = DataSplit(dataSplit[0],'"');
            string[] nodeid = DataSplit(dataSplit[1], ' ');
            string[] latitude = DataSplit(dataSplit[2], ' ');
            string[] longitude = DataSplit(dataSplit[3], ' ');
            string[] soundtype = DataSplit(dataSplit[4], '"');
            string[] probability = DataSplit(dataSplit[5], ' ');
            string[] soundStart = DataSplit(dataSplit[6], '"');
            string[] soundEnd = dataSplit[6][2].Split('"');

            string Sound = soundStart[1] +":"+soundEnd[0];

            string[] parsedData = {time[1], nodeid[1], latitude[1], longitude[1], soundtype[1], probability[1], Sound};
            return parsedData;
        }

        public static string[] DataSplit(string[] arr, char x)
        {
            string[] split = arr[1].Split(x);
            return split;
        }
    }
}