using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiSpectrometerView.Classes
{
    class Parameters
    {
        public static double aNmArguman = 0;
        public static double bNmArguman = 0;
        public static double cNmArguman = 0;
        public static double dNmArguman = 0;
        public static double eNmArguman = 0;

        public static int dataLenght = 2049;
        public static int dataCount = 61;
        public static int firstNm = 400;

        public static string selectDevicePort1 = "";
        public static string selectDevicePort2 = "";
        public static string selectDevicePort3 = "";
        public static string integrationTime = "";
        public static string averageScan = "";
        public static string digitalGain = "";
        public static string analogGain = "";
        public static string testLoopCount = "";
        public static string otherLoopCount = "";
        public static string filter = "";
        public static string cmcType = "";

        public static SQLiteConnection connection;
        public static SQLiteCommand command;
        public static string nameDatabase = "Database";
        public static string nameTable = "Data";
        public static int saveTime = 15000;
        public static int id = 1;
    }
}
