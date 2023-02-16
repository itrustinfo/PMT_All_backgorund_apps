using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtoSyncDateUpdate
{
    class Program
    {
        static void Main(string[] args)
        {
            Program pg = new Program();
            pg.StartPrg();
        }

        private void StartPrg()
        {
            try
            {
                DBActions db_transact = new DBActions();
                List<string> lstPackages = new List<string> { "CP-04", "CP-07", "CP-08", "CP-10", "CP-12", "CP-25", "CP-27"};
                foreach(string str in lstPackages)
                {
                    string package = str;
                    string sDate1 = db_transact.getdatefromLogs(package);
                    string sDate2 = db_transact.GetDashboardContractotDocsSubmitted_Exto(package);
                    string sDate3 = db_transact.getdatefromLogsforDocStatus_Exto(package);
                    string finalDate = string.Empty;
                    if (!string.IsNullOrEmpty(sDate2) && !string.IsNullOrEmpty(sDate1))
                    {
                        if(DateTime.Parse(sDate1) > DateTime.Parse(sDate2))
                        {
                            finalDate = sDate1;
                        }
                        else
                        {
                            finalDate = sDate2;
                        }
                       
                    }
                     else if (!string.IsNullOrEmpty(sDate1))
                    {
                        finalDate = sDate1;
                    }
                    else if (!string.IsNullOrEmpty(sDate2))
                    {
                        finalDate = sDate2;
                       
                    }
                    //
                    if (!string.IsNullOrEmpty(sDate3) && !string.IsNullOrEmpty(finalDate))
                    {
                        if (DateTime.Parse(sDate3) > DateTime.Parse(finalDate))
                        {
                            finalDate = sDate3;
                        }


                    }
                    else if (!string.IsNullOrEmpty(sDate3))
                    {
                        finalDate = sDate3;
                    }


                    if (!string.IsNullOrEmpty(finalDate))
                    {
                        int result = db_transact.InsertOrUpdateExtoSync(str, DateTime.Parse(finalDate));
                        Console.WriteLine(str + " " + finalDate + " Done !");
                    }
                }
                Console.WriteLine("Done !");
                System.Threading.Thread.Sleep(5000);
                //Console.ReadKey();
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error :" + ex.Message);
                Console.ReadKey();
            }
        }
    }
}
