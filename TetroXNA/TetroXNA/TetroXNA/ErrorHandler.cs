using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Management;
using System.Diagnostics;

namespace TetroXNA
{
    public class ErrorHandler
    {
        private FileStream fs;
        private StreamWriter sw;
        private StreamReader sr;
        private DateTime currentDateTime = DateTime.Now;
        private string operatingSystem, computerName, computerManufacturer, 
            numOfCPU, numOfLogicCPU, totalRAM, nameCPU, nameGPU, serialNum, computerModel, userName, recordName;
        private bool existingRecordFiles = true, collectPersonalPCInfo;
        private int numOfRecordFiles = 0, identifyRecordNum = 0;

        public ErrorHandler()
        {
            DialogResult dialogResult = MessageBox.Show(
                "For Devloping Perposes\nMay FlashBlock Studio Collect Data in the following:\n-Computer Name\n-User Name\n-Computer Cerial Number", 
                "Computer Sesitive Info Premission", 
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                collectPersonalPCInfo = true;
            }
            else if (dialogResult == DialogResult.No)
            {
                collectPersonalPCInfo = false;
            }

            gatherSystemInfo();
        }

        public void recordError(int errorLevel, int errorCode, String helpfulInfo, String errorDetails)
        {
            try 
            {
                //checks to see how many record files
                while (existingRecordFiles)
                {
                    if (File.Exists("Error_Record_" + (numOfRecordFiles + 1) + ".txt"))
                    {
                        numOfRecordFiles++;
                        Console.WriteLine("Found Record File");
                    }
                    else 
                    {
                        existingRecordFiles = false;
                    }
                }

                //Check to see if there is an exsisting record for current computer
                for (int i = 1; i <= numOfRecordFiles; i++ )
                {
                    if (checkFileForComputerID("Error_Record_" + i + ".txt"))
                    {
                        identifyRecordNum = i;
                        continue;
                    }
                }

                //If no file was found or match
                if (identifyRecordNum == 0)
                {
                    Console.WriteLine("No Matching record");
                    Console.WriteLine("Current session will be recoreded in Error_Record_" + (numOfRecordFiles + 1) + ".txt");
                    createNewRecord("Error_Record_" + (numOfRecordFiles + 1) + ".txt");
                    recordName = "Error_Record_" + (numOfRecordFiles + 1) + ".txt";
                }
                else
                {
                    Console.WriteLine("Found Matching Record");
                    Console.WriteLine("Current session will be recoreded in Error_Record_" + identifyRecordNum + ".txt");
                    recordName = "Error_Record_" + identifyRecordNum + ".txt";
                }

                //APPEND FILE
                fs = new FileStream(recordName,
                            FileMode.Append,
                            FileAccess.Write);
                sw = new StreamWriter(fs);

                if (errorLevel == 1)
                {
                    sw.WriteLine(currentDateTime.ToString() + "\tINFORMATION");
                    sw.WriteLine("   Error Code:\t\t" + errorCode);
                    sw.WriteLine("   Helpful Info:\t" + helpfulInfo);
                    sw.WriteLine("");
                }
                if (errorLevel == 2)
                {
                    sw.WriteLine(currentDateTime.ToString() + "\tWarning");
                    sw.WriteLine("   Error Code:\t\t" + errorCode);
                    sw.WriteLine("   Helpful Info:\t" + helpfulInfo);
                    sw.WriteLine("   Details:");
                    sw.WriteLine(errorDetails);
                    sw.WriteLine("");
                }
                if (errorLevel == 3)
                {
                    sw.WriteLine(currentDateTime.ToString() + "\tError");
                    sw.WriteLine("   Error Code:\t\t" + errorCode);
                    sw.WriteLine("   Helpful Info:\t" + helpfulInfo);
                    sw.WriteLine("   Details:");
                    sw.WriteLine(errorDetails);
                    sw.WriteLine("");
                }
                sw.Close();
                fs.Close();
                if (errorLevel == 3)
                {
                    MessageBox.Show("Ciritical Error!\nError code: " + errorCode + "\nRefer to " + recordName +
                        "\nLocation: " + Path.GetFullPath(recordName).ToString(), "TetroXNA Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Process.GetCurrentProcess().Kill();
                }
            }
            catch (Exception d)
            {
                MessageBox.Show("WARNING:\nThis session of Tetro is unable to record error data.\n" + d.ToString(),
                    "Error Record Warning",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                
            }
        }

        private void createNewRecord(string filename)
        {
            try
            {
                fs = new FileStream(filename,
                            FileMode.Create,
                            FileAccess.Write);
                sw = new StreamWriter(fs);
                
                sw.WriteLine("----------TETROXNA ERROR RECORD---------");
                sw.WriteLine("File Created " + currentDateTime.ToString());
                sw.WriteLine("");
                sw.WriteLine("---------------SYSTEM INFO--------------");
                sw.WriteLine("Operateing System:\t" + operatingSystem);
                sw.WriteLine("Computer Name:\t\t" + computerName);
                sw.WriteLine("Current User:\t\t" + userName);
                sw.WriteLine("Computer Manufacturer:\t" + computerManufacturer);
                sw.WriteLine("Computer Model:\t\t" + computerModel);
                sw.WriteLine("Computer Serial Number:\t" + serialNum);
                sw.WriteLine("Computer GPU:\t\t" + nameGPU);
                sw.WriteLine("Computer CPU:\t\t" + nameCPU);
                sw.WriteLine("Number of CPUs:\t\t" + numOfCPU);
                sw.WriteLine("Number of Logical CPUs:\t" + numOfLogicCPU);
                sw.WriteLine("Total amount of RAM:\t" + totalRAM);
                sw.WriteLine("");
                sw.WriteLine("---------------ERROR INFO---------------");

                sw.Close();
                fs.Close();
                Console.WriteLine("Created Error Record Sucsessfully");
            }
            catch (Exception d)
            {
                MessageBox.Show("WARNING:\nTetro is unable to create an error record for this system!/n" + d.ToString(),
                    "Warning TetroXNA",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        private bool checkFileForComputerID(string filename)
        {
            string data;
            fs = new FileStream(filename,
                        FileMode.Open,
                        FileAccess.Read);
            sr = new StreamReader(fs);

            sr.ReadLine();
            sr.ReadLine();
            sr.ReadLine();
            sr.ReadLine();
            data = sr.ReadLine().Substring(19);  //operating system from file
            if (operatingSystem.Equals(data))
            {
                data = sr.ReadLine().Substring(16);
                if (computerName.Equals(data))
                {
                    data = sr.ReadLine().Substring(15);
                    if (userName.Equals(data))
                    {
                        data = sr.ReadLine().Substring(23);
                        if (computerManufacturer.Equals(data))
                        {
                            data = sr.ReadLine().Substring(17);
                            if (computerModel.Equals(data))
                            {
                                data = sr.ReadLine().Substring(24);
                                if (serialNum.Equals(data))
                                {
                                    data = sr.ReadLine().Substring(15);
                                    if (nameGPU.Equals(data))
                                    {
                                        data = sr.ReadLine().Substring(15);
                                        if (nameCPU.Equals(data))
                                        {
                                            data = sr.ReadLine().Substring(17);
                                            if (numOfCPU.Equals(data))
                                            {
                                                data = sr.ReadLine().Substring(24);
                                                if (numOfLogicCPU.Equals(data))
                                                {
                                                    data = sr.ReadLine().Substring(21);
                                                    if (totalRAM.Equals(data))
                                                    {
                                                        sr.Close();
                                                        fs.Close();
                                                        return true;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            sr.Close();
            fs.Close();
            return false;
        }

        private void gatherSystemInfo()
        {
            ManagementObjectSearcher searcher;
            try
            {
                searcher = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem");
                foreach (ManagementObject os in searcher.Get())
                {
                    operatingSystem = os["Caption"].ToString();
                    break;
                }
                Console.WriteLine("OS data retrieved");
            }
            catch
            {
                Console.WriteLine("OS data retrieval FAILED");
            }
            try
            {
                searcher = new ManagementObjectSearcher("\\root\\CIMV2", "SELECT * FROM Win32_ComputerSystem");
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    computerName = queryObj["Name"].ToString();
                    computerManufacturer = queryObj["Manufacturer"].ToString();
                    computerModel = queryObj["Model"].ToString();
                    numOfCPU = queryObj["NumberOfProcessors"].ToString();
                    numOfLogicCPU = queryObj["NumberOfLogicalProcessors"].ToString();
                    userName = queryObj["UserName"].ToString();
                    totalRAM = (Convert.ToDouble(queryObj["TotalPhysicalMemory"]) / 1000000000).ToString() + " GB";    //divid by billion to convert byte to gigibyte
                }
                Console.WriteLine("Computer System Data retrieved");
            }
            catch
            {
                Console.WriteLine("Computer System Data retrieval FAILED");
            }
            try
            {
                searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Processor");
                foreach (ManagementObject sysProcessor in searcher.Get())
                {
                    nameCPU = sysProcessor["Name"].ToString();
                }
                Console.WriteLine("Processor Data retrieved");
            }
            catch
            {
                Console.WriteLine("Processor Data retrieval FAILED");
            }
            try
            {
                searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_VideoController");
                foreach (ManagementObject sysVC in searcher.Get())
                {
                    nameGPU = sysVC["Name"].ToString();
                }
                Console.WriteLine("GPU Data retrived");
            }
            catch
            {
                Console.WriteLine("GPU Data retrieval FAILED");
            }
            try
            {
                searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_ComputerSystemProduct");
                foreach (ManagementObject sysProduct in searcher.Get())
                {
                    serialNum = sysProduct["IdentifyingNumber"].ToString();
                }
                Console.WriteLine("Cerial Number retrieved");
            }
            catch 
            {
                Console.WriteLine("Cerial Number retrieval FAILED");
            }
            if (!collectPersonalPCInfo)
            {
                computerName = "NA";
                userName = "NA";
                serialNum = "NA";
            }
        }
    }
}
