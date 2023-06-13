using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace ProjectManagementToolAutomation
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime inputDate = DateTime.Now;
            //Console.WriteLine("Do you want to run this program based on current date and time. If yes then hit 'Y' and enter else hit any key and enter.");
            //string currentDateExecution = Console.ReadLine();
            //if(currentDateExecution != "Y")
            //{
            //    Console.WriteLine("Please enter the date in dd-MMM-yyyy format");
            //    string date = Console.ReadLine();
            //    if(!DateTime.TryParse(date, out inputDate))
            //    {
            //        Console.WriteLine("You need to pass correct date. Automation abborted. Close the program and start it again");
            //        Console.ReadLine();
            //        return;
            //    }
            //}
            

            DocumentFlow documentFlow = new DocumentFlow();
            //documentFlow.GetRequiredDataForStpWorksAStep4And5(inputDate);
            //documentFlow.SaveDocumentStatusAutomatic();
            string FilePath = AppDomain.CurrentDomain.BaseDirectory + "Logs.txt";
            System.IO.StreamWriter testfile = null;
            testfile = new System.IO.StreamWriter(FilePath, true);
            testfile.WriteLine(System.DateTime.Now + "       " + "Client autoshift started...");
            //
            documentFlow.GetRequiredData(inputDate);
            documentFlow.SaveDocumentStatusAutomatic();

            //documentFlow.GetRequiredDataPMC(inputDate);
            //documentFlow.SaveDocumentStatusAutomatic();

            testfile.WriteLine(System.DateTime.Now + "       " + "Client autoshift ended...");
            testfile.Close();
            Console.WriteLine("Completed");
            System.Threading.Thread.Sleep(2000);
        }
    }
}

