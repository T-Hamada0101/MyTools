using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HDD_Info
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (!IsAdministrator())
            {
                Console.WriteLine("このプログラムは管理者権限で実行する必要があります。");
                //管理者権限に昇格
                System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo();
                psi.FileName = System.Reflection.Assembly.GetExecutingAssembly().Location;
                psi.Verb = "runas";
                System.Diagnostics.Process.Start(psi);

                return;
            }

            GetHDDStatus();
            GetSmartInfo();
            Console.ReadLine();
        }

        static bool IsAdministrator()
        {
            var identity = System.Security.Principal.WindowsIdentity.GetCurrent();
            var principal = new System.Security.Principal.WindowsPrincipal(identity);
            return principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator);
        }

        static void GetHDDStatus()
        {
            try
            {
                // WMIクエリを作成
                string query = "SELECT * FROM Win32_DiskDrive";

                // ManagementObjectSearcherを使用してWMIクエリを実行
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);

                // 結果を取得
                foreach (ManagementObject disk in searcher.Get())
                {
                    //全てを表示
                    Console.WriteLine(disk.ToString());

                    // ディスクの情報を表示
                    Console.WriteLine("Name: " + disk["Name"]);
                    Console.WriteLine("Model: " + disk["Model"]);
                    Console.WriteLine("Status: " + disk["Status"]);
                    Console.WriteLine("Availability: " + disk["Availability"]);
                    Console.WriteLine("PowerManagementSupported: " + disk["PowerManagementSupported"]);
                    Console.WriteLine("PowerManagementCapabilities: " + disk["PowerManagementCapabilities"]);
                    Console.WriteLine("Partitions: " + disk["Partitions"]);
                    Console.WriteLine("SerialNumber: " + disk["SerialNumber"]);
                    Console.WriteLine("InterfaceType: " + disk["InterfaceType"]);
                    Console.WriteLine("MediaType: " + disk["MediaType"]);
                    Console.WriteLine("Manufacturer: " + disk["Manufacturer"]);
                    Console.WriteLine("DeviceID: " + disk["DeviceID"]);
                    Console.WriteLine("Caption: " + disk["Caption"]);
                    Console.WriteLine("Description: " + disk["Description"]);
                    Console.WriteLine("Size: " + disk["Size"]);
                    Console.WriteLine("TotalHeads: " + disk["TotalHeads"]);
                    Console.WriteLine("TotalCylinders: " + disk["TotalCylinders"]);
                    Console.WriteLine("TotalTracks: " + disk["TotalTracks"]);
                    Console.WriteLine("TotalSectors: " + disk["TotalSectors"]);
                    Console.WriteLine("TracksPerCylinder: " + disk["TracksPerCylinder"]);
                    Console.WriteLine("SectorsPerTrack: " + disk["SectorsPerTrack"]);
                    Console.WriteLine("BytesPerSector: " + disk["BytesPerSector"]);
                    foreach (UInt16 cap in (UInt16[])disk["Capabilities"])
                    {
                        Console.WriteLine("Capabilities: " + cap);
                    }
                    foreach (string cap in (string[])disk["CapabilityDescriptions"])
                    {
                        Console.WriteLine("CapabilityDescriptions: " + cap);
                    }
                    Console.WriteLine("SectorsPerTrack: " + disk["SectorsPerTrack"]);
                    Console.WriteLine("FirmwareRevision: " + disk["FirmwareRevision"]);

                    Console.WriteLine();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("エラー: " + e.Message);
            }
        }

        static void GetSmartInfo()
        {
            try
            {
                // WMIクエリを作成
                string query = "SELECT * FROM MSStorageDriver_FailurePredictStatus";

                // ManagementObjectSearcherを使用してWMIクエリを実行
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);

                // 結果を取得
                foreach (ManagementObject drive in searcher.Get())
                {
                    // S.M.A.R.T.情報を表示
                    Console.WriteLine("InstanceName: " + drive["InstanceName"]);
                    Console.WriteLine("PredictFailure: " + drive["PredictFailure"]);
                    Console.WriteLine();
                }

                // 詳細なS.M.A.R.T.データを取得
                query = "SELECT * FROM MSStorageDriver_FailurePredictData";
                searcher = new ManagementObjectSearcher(query);

                foreach (ManagementObject data in searcher.Get())
                {
                    // S.M.A.R.T.データを表示
                    Console.WriteLine("InstanceName: " + data["InstanceName"]);
                    byte[] vendorSpecific = (byte[])data["VendorSpecific"];
                    for (int i = 0; i < vendorSpecific.Length; i += 12)
                    {
                        if (vendorSpecific[i] != 0)
                        {
                            Console.WriteLine("ID: " + vendorSpecific[i]);
                            Console.WriteLine("Status: " + vendorSpecific[i + 1]);
                            Console.WriteLine("Value: " + vendorSpecific[i + 3]);
                            Console.WriteLine("Worst: " + vendorSpecific[i + 4]);
                            Console.WriteLine("VendorData: " + BitConverter.ToString(vendorSpecific, i + 5, 6));
                            Console.WriteLine();
                        }
                    }
                }
            }
            catch (ManagementException me)
            {
                Console.WriteLine("WMIエラー: " + me.Message);
            }
            catch (UnauthorizedAccessException uae)
            {
                Console.WriteLine("アクセスエラー: " + uae.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("一般的なエラー: " + e.Message);
            }
        }
    }
}
