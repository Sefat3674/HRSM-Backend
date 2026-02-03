using System;
using System.Collections.Generic;
using System.Deployment.Application;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms; 
using RestSharp;
using S7.Net;
using S7.Net.Types;

namespace SmartManufacturingLocal_PLC
{
    public partial class Form1 : Form
    {
        SmartManufacturingV2Entities db_SMV2 = new SmartManufacturingV2Entities();
        string fileName = string.Empty;
        CancellationTokenSource tokenSource = null;

        public Form1()
        {
            InitializeComponent();
        }

        private async void StartingMethod()
        {
            try
            {
                await Task.Delay(TimeSpan.FromMinutes(1)); // UI stays responsive
                notify.Text = "Starting...";
                StartTask(true);
            }
            catch { }
        }

        private void Start_Click(object sender, EventArgs e)
        {
            notify.Text = "Starting...";
            StartTask(true);
        }

        private void Stop_Click(object sender, EventArgs e)
        {
            tokenSource.Cancel();
        }

        private async void StartTask(bool cmd)
        {
            tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;

            var progress = new Progress<int>(value =>
            {
                progressBar.Value = value;
                label2.Text = $"{value} %";
            });
            if (Directory.Exists(@"D:\"))
            {
                fileName = @"D:\" + "PAL_NAL_LogFile.txt";
            }
            else if (Directory.Exists(@"E:\"))
            {
                fileName = @"E:\" + "PAL_NAL_LogFile.txt";
            }
            else
            {
                fileName = Path.Combine(Directory.GetCurrentDirectory(), "PAL_NAL_LogFile.txt");
            }
            if (!File.Exists(fileName))
            {
                using (FileStream fs = File.Create(fileName)) { }
            }

            try
            {
                while (cmd)
                {
                    try
                    {
                        File.AppendAllText(fileName, System.DateTime.Now.ToString() + "\n");
                        if (!token.IsCancellationRequested)
                        {
                            System.DateTime Start_Time = System.DateTime.Now;
                            await Task.Run(() => LongRunningTask(progress, token));

                            System.DateTime End_Time = System.DateTime.Now;
                            if ((End_Time - Start_Time).TotalSeconds < 120)
                            {
                                double Delay_Time = 120 - (End_Time - Start_Time).TotalSeconds;
                                await Task.Delay((int)Delay_Time * 1000);
                            }
                            notify.Text = "Updated On: " + System.DateTime.Now.ToString();
                        }
                        else
                        {
                            notify.Text = "Stopped";
                            cmd = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        File.AppendAllText(
                            fileName,
                            "PAL NAL Listener error in cmd: \n" + System.DateTime.Now.ToString()
                        );
                    }
                }
            }
            catch (OperationCanceledException ex)
            {
                File.AppendAllText(
                    fileName,
                    "PAL NAL Listener error in cancel: \n" + System.DateTime.Now.ToString()
                );
            }
            catch (Exception ex)
            {
                File.AppendAllText(
                    fileName,
                    "PAL NAL Listener Start in error: \n" + System.DateTime.Now.ToString()
                );
            }
        }

        //private async Task<string> LongRunningTask(IProgress<int> progress, CancellationToken token)
        private string LongRunningTask(IProgress<int> progress, CancellationToken token)
        {
            List<ReadMachineData> ts = new List<ReadMachineData>();

            #region Local_PLC

            #region 192.168.242.46 PFG
            try
            {
                using (var plc = new Plc(CpuType.S71200, "192.168.242.46", 0, 1))
                {
                    try
                    {
                        if (!plc.IsConnected)
                        {
                            var openTask = Task.Run(() =>
                            {
                                try
                                {
                                    plc.Open();
                                }
                                catch (Exception ex) { }
                            });
                            openTask.Wait(2000);
                            if (!plc.IsConnected)
                            {
                                throw new Exception();
                            }
                        }

                        try
                        {
                            byte[] byte1 = plc.ReadBytes(DataType.DataBlock, 1, 0, 232);
                            byte[] byte2 = plc.ReadBytes(DataType.DataBlock, 7, 0, 232);
                            uint[] result1 = DWord.ToArray(byte1);
                            uint[] result2 = DWord.ToArray(byte2);

                            for (int i = 1; i < 58; i++)
                            {
                                var totalCountList = new List<long>();
                                var tempId = "242461" + i;
                                if ((i > 9 && i < 12) || (i > 26 && i < 33))
                                {
                                    totalCountList.Add((long)result1[i]);
                                    totalCountList.Add((long)result2[i]);
                                }
                                else
                                {
                                    totalCountList.Add((long)result1[i]);
                                }
                                ReadMachineData data = new ReadMachineData
                                {
                                    MachineID = Convert.ToInt32(tempId),
                                    TotalCount = totalCountList,
                                    Break = false,
                                    Sched = false,
                                };

                                ts.Add(data);
                            }
                        }
                        catch (Exception) { }

                        plc.Close();
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("192.168.242.46");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            #endregion
            #region FetchDataForRRF_Vulta_Regal_Sheet_Matal_1
            try
            {
                using (var plc = new Plc(CpuType.S71200, "172.22.216.71", 0, 1))
                {
                    try
                    {
                        if (!plc.IsConnected)
                        {
                            var openTask = Task.Run(() =>
                            {
                                try
                                {
                                    plc.Open();
                                }
                                catch (Exception ex) { }
                            });
                            openTask.Wait(2000);
                            if (!plc.IsConnected)
                            {
                                throw new Exception();
                            }
                        }
                        try
                        {
                            byte[] byte1 = plc.ReadBytes(DataType.DataBlock, 1, 0, 516);
                            uint[] result1 = DWord.ToArray(byte1);

                            for (int i = 0; i < 129; i++)
                            {
                                if (i != 0)
                                {
                                    var totalCountList = new List<long>();
                                    string newId = "216711" + i;

                                    totalCountList = new List<long>() { (long)result1[i] };

                                    ReadMachineData data = new ReadMachineData
                                    {
                                        MachineID = Convert.ToInt64(newId),
                                        TotalCount = totalCountList,
                                        Break = false,
                                        Sched = false,
                                    };

                                    ts.Add(data);
                                }
                            }
                        }
                        catch (Exception ex) { }
                        plc.Close();
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("172.22.216.71");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            #region oldone
            //try
            //{
            //    using (var plc = new Plc(CpuType.S71200, "172.22.115.141", 0, 1))
            //    {
            //        try
            //        {
            //            if (!plc.IsConnected)
            //            {
            //                var openTask = Task.Run(() =>
            //                {
            //                    try
            //                    {
            //                        plc.Open();
            //                    }
            //                    catch (Exception ex)
            //                    {

            //                    }
            //                });
            //                openTask.Wait(2000);
            //                if (!plc.IsConnected)
            //                {
            //                    throw new Exception();
            //                }
            //            }

            //            try
            //            {
            //                byte[] byte1 = plc.ReadBytes(DataType.DataBlock, 1, 0, 516);
            //                uint[] result1 = DWord.ToArray(byte1);
            //                byte[] byte2 = plc.ReadBytes(DataType.DataBlock, 11, 0, 516);
            //                uint[] result2 = DWord.ToArray(byte2);

            //                for (int i = 1; i < 129; i++)
            //                {
            //                    if (i < 117 || i > 122)
            //                    {
            //                        var totalCountList = new List<long>() { (long)result1[i] };
            //                        string newId = "1151411" + i;
            //                        ReadMachineData data = new ReadMachineData
            //                        {
            //                            MachineID = Convert.ToInt64(newId),
            //                            TotalCount = totalCountList,
            //                            Break = false,
            //                            Sched = false
            //                        };
            //                        ts.Add(data);
            //                    }
            //                    else
            //                    {
            //                        var totalCountList = new List<long>() { (long)result1[i], (long)result2[i] };
            //                        string newId = "1151411" + i;
            //                        ReadMachineData data = new ReadMachineData
            //                        {
            //                            MachineID = Convert.ToInt64(newId),
            //                            TotalCount = totalCountList,
            //                            Break = false,
            //                            Sched = false
            //                        };
            //                        ts.Add(data);
            //                    }
            //                }
            //            }
            //            catch (Exception)
            //            {

            //            }

            //            plc.Close();

            //        }
            //        catch (Exception)
            //        {
            //            Console.WriteLine("172.22.115.141");
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
            #endregion

            #endregion
            #region PFG=>AMCL=>Drinks Line
            try
            {
                using (var plc = new Plc(CpuType.S71200, "192.168.242.41", 0, 1))
                {
                    try
                    {
                        if (!plc.IsConnected)
                        {
                            var openTask = Task.Run(() =>
                            {
                                try
                                {
                                    plc.Open();
                                }
                                catch (Exception ex) { }
                            });
                            openTask.Wait(2000);
                            if (!plc.IsConnected)
                            {
                                throw new Exception();
                            }
                        }
                        try
                        {
                            byte[] byte1 = plc.ReadBytes(DataType.DataBlock, 17, 0, 20);
                            uint[] result1 = DWord.ToArray(byte1);

                            for (int i = 0; i < 5; i++)
                            {
                                if (i != 0)
                                {
                                    var totalCountList = new List<long>();
                                    string newId = "2424117" + i;

                                    totalCountList = new List<long>() { (long)result1[i] };

                                    ReadMachineData data = new ReadMachineData
                                    {
                                        MachineID = Convert.ToInt64(newId),
                                        TotalCount = totalCountList,
                                        Break = false,
                                        Sched = false,
                                    };

                                    ts.Add(data);
                                }
                            }
                        }
                        catch (Exception ex) { }
                        plc.Close();
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("192.168.242.41");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            #endregion
            #region PFG_AMCL_CSDLine
            try
            {
                using (var plc = new Plc(CpuType.S7200Smart, "192.168.242.49", 0, 1))
                {
                    try
                    {
                        if (!plc.IsConnected)
                        {
                            var openTask = Task.Run(() =>
                            {
                                try
                                {
                                    plc.Open();
                                }
                                catch (Exception ex) { }
                            });
                            openTask.Wait(2000);
                            if (!plc.IsConnected)
                            {
                                throw new Exception();
                            }
                        }

                        try
                        {
                            byte[] byte1 = plc.ReadBytes(DataType.DataBlock, 1, 0, 32);
                            uint[] result1 = DWord.ToArray(byte1);

                            for (int i = 1; i < 8; i++)
                            {
                                var totalCountList = new List<long>() { (long)result1[i] }; //2424916
                                string newId = "242491" + i;
                                ReadMachineData data = new ReadMachineData
                                {
                                    MachineID = Convert.ToInt64(newId),
                                    TotalCount = totalCountList,
                                    Break = false,
                                    Sched = false,
                                };
                                ts.Add(data);
                            }
                        }
                        catch (Exception) { }

                        plc.Close();
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("192.168.242.49");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            #endregion
            #region 192.168.120.30 KEZ
            try
            {
                using (var plc = new Plc(CpuType.S71200, "192.168.120.30", 0, 1))
                {
                    try
                    {
                        if (!plc.IsConnected)
                        {
                            var openTask = Task.Run(() =>
                            {
                                try
                                {
                                    plc.Open();
                                }
                                catch (Exception ex) { }
                            });
                            openTask.Wait(2000);
                            if (!plc.IsConnected)
                            {
                                throw new Exception();
                            }
                        }

                        try
                        {
                            byte[] byte2 = plc.ReadBytes(DataType.DataBlock, 13, 0, 8);
                            uint[] result2 = DWord.ToArray(byte2);
                            byte[] byte3 = plc.ReadBytes(DataType.DataBlock, 12, 0, 8);
                            uint[] result3 = DWord.ToArray(byte3);
                            byte[] byte1 = plc.ReadBytes(DataType.DataBlock, 1, 0, 12);
                            uint[] result1 = DWord.ToArray(byte1);

                            for (int i = 1; i < 2; i++)
                            {
                                string tempId = i.ToString();
                                tempId = "1203013" + i;

                                var totalCountList = new List<long>()
                                {
                                    (long)result2[i],
                                    (long)result3[i],
                                };
                                ReadMachineData data = new ReadMachineData
                                {
                                    MachineID = Convert.ToInt64(tempId),
                                    TotalCount = totalCountList,
                                    Break = false,
                                    Sched = false,
                                };

                                ts.Add(data);
                            }

                            for (int i = 1; i < 3; i++)
                            {
                                var totalCountList = new List<long>() { (long)result1[i] };
                                string newId = "120301" + i;
                                ReadMachineData data = new ReadMachineData
                                {
                                    MachineID = Convert.ToInt64(newId),
                                    TotalCount = totalCountList,
                                    Break = false,
                                    Sched = false,
                                };

                                ts.Add(data);
                            }
                        }
                        catch (Exception) { }

                        plc.Close();
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("192.168.120.30");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            #endregion
            #region FetchDataForKEZ120311
            try
            {
                using (var plc = new Plc(CpuType.S71200, "192.168.120.31", 0, 1))
                {
                    try
                    {
                        if (!plc.IsConnected)
                        {
                            var openTask = Task.Run(() =>
                            {
                                try
                                {
                                    plc.Open();
                                }
                                catch (Exception ex) { }
                            });
                            openTask.Wait(2000);
                            if (!plc.IsConnected)
                            {
                                throw new Exception();
                            }
                        }

                        try
                        {
                            byte[] byte1 = plc.ReadBytes(DataType.DataBlock, 1, 0, 84);
                            uint[] result1 = DWord.ToArray(byte1);

                            for (int i = 1; i < 21; i++)
                            {
                                var totalCountList = new List<long>() { (long)result1[i] };
                                string newId = "120311" + i;
                                ReadMachineData data = new ReadMachineData
                                {
                                    MachineID = Convert.ToInt64(newId),
                                    TotalCount = totalCountList,
                                    Break = false,
                                    Sched = false,
                                };

                                ts.Add(data);
                            }
                        }
                        catch (Exception) { }

                        plc.Close();
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("192.168.120.31");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            #endregion
            #region FetchDataForKEZ120321
            try
            {
                using (var plc = new Plc(CpuType.S71200, "192.168.120.32", 0, 1))
                {
                    try
                    {
                        if (!plc.IsConnected)
                        {
                            var openTask = Task.Run(() =>
                            {
                                try
                                {
                                    plc.Open();
                                }
                                catch (Exception ex) { }
                            });
                            openTask.Wait(2000);
                            if (!plc.IsConnected)
                            {
                                throw new Exception();
                            }
                        }

                        try
                        {
                            byte[] byte1 = plc.ReadBytes(DataType.DataBlock, 1, 0, 68);
                            uint[] result1 = DWord.ToArray(byte1);

                            for (int i = 1; i < 17; i++)
                            {
                                var totalCountList = new List<long>() { (long)result1[i] };
                                string newId = "120321" + i;
                                ReadMachineData data = new ReadMachineData
                                {
                                    MachineID = Convert.ToInt64(newId),
                                    TotalCount = totalCountList,
                                    Break = false,
                                    Sched = false,
                                };

                                ts.Add(data);
                            }
                        }
                        catch (Exception) { }

                        plc.Close();
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("192.168.120.32");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            #endregion
            #region FetchDataForCLF_BBMT
            try
            {
                using (var plc = new Plc(CpuType.S71200, "192.168.194.5", 0, 1))
                {
                    try
                    {
                        if (!plc.IsConnected)
                        {
                            var openTask = Task.Run(() =>
                            {
                                try
                                {
                                    plc.Open();
                                }
                                catch (Exception ex) { }
                            });
                            openTask.Wait(2000);
                            if (!plc.IsConnected)
                            {
                                throw new Exception();
                            }
                        }

                        try
                        {
                            byte[] byte1 = plc.ReadBytes(DataType.DataBlock, 1, 0, 268);
                            uint[] result1 = DWord.ToArray(byte1);

                            for (int i = 1; i < 67; i++)
                            {
                                var totalCountList = new List<long>() { (long)result1[i] }; //1945148
                                string newId = "19451" + i;
                                ReadMachineData data = new ReadMachineData
                                {
                                    MachineID = Convert.ToInt64(newId),
                                    TotalCount = totalCountList,
                                    Break = false,
                                    Sched = false,
                                };
                                ts.Add(data);
                            }
                        }
                        catch (Exception) { }

                        plc.Close();
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("192.168.194.5");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            #endregion
            #region KEZ=>PML=>Foil
            try
            {
                using (var plc = new Plc(CpuType.S71200, "192.168.120.34", 0, 1))
                {
                    try
                    {
                        if (!plc.IsConnected)
                        {
                            var openTask = Task.Run(() =>
                            {
                                try
                                {
                                    plc.Open();
                                }
                                catch (Exception ex) { }
                            });
                            openTask.Wait(2000);
                            if (!plc.IsConnected)
                            {
                                throw new Exception();
                            }
                        }

                        try
                        {
                            byte[] byte1 = plc.ReadBytes(DataType.DataBlock, 3, 0, 80);
                            uint[] result1 = DWord.ToArray(byte1);

                            for (int i = 1; i < 20; i++)
                            {
                                var totalCountList = new List<long>() { (long)result1[i] };
                                string newId = "120343" + i;
                                ReadMachineData data = new ReadMachineData
                                {
                                    MachineID = Convert.ToInt64(newId),
                                    TotalCount = totalCountList,
                                    Break = false,
                                    Sched = false,
                                };
                                ts.Add(data);
                            }
                        }
                        catch (Exception) { }

                        plc.Close();
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("192.168.120.34");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            #endregion
            #region BIP
            try
            {
                using (var plc = new Plc(CpuType.S7200Smart, "192.168.32.185", 0, 1)) //oldip192.168.34.185
                {
                    try
                    {
                        if (!plc.IsConnected)
                        {
                            var openTask = Task.Run(() =>
                            {
                                try
                                {
                                    plc.Open();
                                }
                                catch (Exception ex) { }
                            });
                            openTask.Wait(2000);
                            if (!plc.IsConnected)
                            {
                                throw new Exception();
                            }
                        }

                        try
                        {
                            byte[] byte1 = plc.ReadBytes(DataType.DataBlock, 1, 0, 20);
                            uint[] result1 = DWord.ToArray(byte1);

                            for (int i = 1; i < 5; i++)
                            {
                                var totalCountList = new List<long>() { (long)result1[i] };
                                string newId = "321851" + i;
                                ReadMachineData data = new ReadMachineData
                                {
                                    MachineID = Convert.ToInt64(newId),
                                    TotalCount = totalCountList,
                                    Break = false,
                                    Sched = false,
                                };
                                ts.Add(data);
                            }
                        }
                        catch (Exception) { }

                        plc.Close();
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("192.168.32.185");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            #endregion
            #region FetchDataForPAL_BIP_APCL
            try
            {
                using (var plc = new Plc(CpuType.S7200Smart, "192.168.32.186", 0, 1))
                {
                    try
                    {
                        if (!plc.IsConnected)
                        {
                            var openTask = Task.Run(() =>
                            {
                                try
                                {
                                    plc.Open();
                                }
                                catch (Exception ex) { }
                            });
                            openTask.Wait(2000);
                            if (!plc.IsConnected)
                            {
                                throw new Exception();
                            }
                        }

                        try
                        {
                            byte[] byte1 = plc.ReadBytes(DataType.DataBlock, 1, 0, 16); //salehmir vai godabai factoy
                            uint[] result1 = DWord.ToArray(byte1);

                            for (int i = 1; i < 4; i++)
                            {
                                var totalCountList = new List<long>() { (long)result1[i] };
                                string newId = "321861" + i;
                                ReadMachineData data = new ReadMachineData
                                {
                                    MachineID = Convert.ToInt64(newId),
                                    TotalCount = totalCountList,
                                    Break = false,
                                    Sched = false,
                                };
                                ts.Add(data);
                            }
                        }
                        catch (Exception) { }

                        plc.Close();
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("192.168.32.186");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            #endregion
            #region FetchDataForMDF_DPL
            try
            {
                using (var plc = new Plc(CpuType.S7200Smart, "192.168.90.240", 0, 1))
                {
                    try
                    {
                        if (!plc.IsConnected)
                        {
                            var openTask = Task.Run(() =>
                            {
                                try
                                {
                                    plc.Open();
                                }
                                catch (Exception ex) { }
                            });
                            openTask.Wait(2000);
                            if (!plc.IsConnected)
                            {
                                throw new Exception();
                            }
                        }

                        try
                        {
                            byte[] byte1 = plc.ReadBytes(DataType.DataBlock, 1, 0, 100);
                            uint[] result1 = DWord.ToArray(byte1);

                            for (int i = 1; i < 25; i++)
                            {
                                var totalCountList = new List<long>() { (long)result1[i] };
                                string newId = "902401" + i;
                                ReadMachineData data = new ReadMachineData
                                {
                                    MachineID = Convert.ToInt64(newId),
                                    TotalCount = totalCountList,
                                    Break = false,
                                    Sched = false,
                                };
                                ts.Add(data);
                            }
                        }
                        catch (Exception) { }

                        plc.Close();
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("192.168.90.240");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            #endregion
            #region 10.7.238.19
            try
            {
                using (var plc = new Plc(CpuType.S71200, "10.7.238.19", 0, 1))
                {
                    try
                    {
                        if (!plc.IsConnected)
                        {
                            var openTask = Task.Run(() =>
                            {
                                try
                                {
                                    plc.Open();
                                }
                                catch (Exception ex) { }
                            });
                            openTask.Wait(2000);
                            if (!plc.IsConnected)
                            {
                                throw new Exception();
                            }
                        }
                        try
                        {
                            byte[] byte1 = plc.ReadBytes(DataType.DataBlock, 1, 0, 48);
                            uint[] result1 = DWord.ToArray(byte1);
                            byte[] byte2 = plc.ReadBytes(DataType.DataBlock, 10, 0, 36);
                            uint[] result2 = DWord.ToArray(byte2);
                            for (int i = 1; i < 9; i++)
                            {
                                var totalCountList = new List<long>();
                                string newId = "107238191" + i;

                                totalCountList = new List<long>()
                                {
                                    (long)result1[i],
                                    (long)result2[i],
                                }; //

                                ReadMachineData data = new ReadMachineData
                                {
                                    MachineID = Convert.ToInt64(newId),
                                    TotalCount = totalCountList,
                                    Break = false,
                                    Sched = false,
                                };

                                ts.Add(data);
                            }

                            for (int i = 9; i < 12; i++)
                            {
                                var totalCountList = new List<long>();
                                string newId = "107238191" + i;

                                totalCountList = new List<long>() { (long)result1[i] }; //

                                ReadMachineData data = new ReadMachineData
                                {
                                    MachineID = Convert.ToInt64(newId),
                                    TotalCount = totalCountList,
                                    Break = false,
                                    Sched = false,
                                };

                                ts.Add(data);
                            }
                        }
                        catch (Exception) { }
                        plc.Close();
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("10.7.238.19");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            #endregion
            #region fatchDataFor_PFG_AMCL_Tetra_Pak_Line
            try
            {
                using (var plc = new Plc(CpuType.S71200, "192.168.242.43", 0, 1))
                {
                    try
                    {
                        if (!plc.IsConnected)
                        {
                            var openTask = Task.Run(() =>
                            {
                                try
                                {
                                    plc.Open();
                                }
                                catch (Exception ex) { }
                            });
                            openTask.Wait(2000);
                            if (!plc.IsConnected)
                            {
                                throw new Exception();
                            }
                        }
                        try
                        {
                            byte[] byte1 = plc.ReadBytes(DataType.DataBlock, 1, 0, 64);
                            uint[] result1 = DWord.ToArray(byte1);

                            for (int i = 1; i < 16; i++)
                            {
                                var totalCountList = new List<long>();
                                string newId = "242431" + i;

                                totalCountList = new List<long>() { (long)result1[i] };

                                ReadMachineData data = new ReadMachineData
                                {
                                    MachineID = Convert.ToInt64(newId),
                                    TotalCount = totalCountList,
                                    Break = false,
                                    Sched = false,
                                };

                                ts.Add(data);
                            }
                        }
                        catch (Exception) { }
                        plc.Close();
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("192.168.242.43");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            #endregion
            #region PAL_RAJ_1_PAL_Misty_Boroi
            try
            {
                using (var plc = new Plc(CpuType.S71200, "192.168.123.210", 0, 1))
                {
                    try
                    {
                        if (!plc.IsConnected)
                        {
                            var openTask = Task.Run(() =>
                            {
                                try
                                {
                                    plc.Open();
                                }
                                catch (Exception ex) { }
                            });
                            openTask.Wait(2000);
                            if (!plc.IsConnected)
                            {
                                throw new Exception();
                            }
                        }
                        try
                        {
                            byte[] byte1 = plc.ReadBytes(DataType.DataBlock, 1, 0, 56);
                            uint[] result1 = DWord.ToArray(byte1);

                            for (int i = 0; i < 14; i++)
                            {
                                if (i != 0)
                                {
                                    var totalCountList = new List<long>();
                                    string newId = "1232101" + i;

                                    totalCountList = new List<long>() { (long)result1[i] };

                                    ReadMachineData data = new ReadMachineData
                                    {
                                        MachineID = Convert.ToInt64(newId),
                                        TotalCount = totalCountList,
                                        Break = false,
                                        Sched = false,
                                    };

                                    ts.Add(data);
                                }
                            }
                        }
                        catch (Exception) { }
                        plc.Close();
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("192.168.123.210");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            #endregion
            #region PFG_PELine
            try
            {
                using (var plc = new Plc(CpuType.S71200, "192.168.242.48", 0, 1))
                {
                    try
                    {
                        if (!plc.IsConnected)
                        {
                            var openTask = Task.Run(() =>
                            {
                                try
                                {
                                    plc.Open();
                                }
                                catch (Exception ex) { }
                            });
                            openTask.Wait(2000);
                            if (!plc.IsConnected)
                            {
                                throw new Exception();
                            }
                        }
                        try
                        {
                            byte[] byte1 = plc.ReadBytes(DataType.DataBlock, 6, 0, 16);
                            uint[] result1 = DWord.ToArray(byte1);

                            for (int i = 1; i < 4; i++)
                            {
                                var totalCountList = new List<long>();
                                string newId = "242486" + i;

                                totalCountList = new List<long>() { (long)result1[i] }; //2424862

                                ReadMachineData data = new ReadMachineData
                                {
                                    MachineID = Convert.ToInt64(newId),
                                    TotalCount = totalCountList,
                                    Break = false,
                                    Sched = false,
                                };

                                ts.Add(data);
                            }
                        }
                        catch (Exception) { }
                        plc.Close();
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("192.168.242.48");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            #endregion
            #region AMCL=>Hot Fill=>Duel Filler

            try
            {
                using (var plc = new Plc(CpuType.S71200, "192.168.242.48", 0, 1))
                {
                    try
                    {
                        if (!plc.IsConnected)
                        {
                            var openTask = Task.Run(() =>
                            {
                                try
                                {
                                    plc.Open();
                                }
                                catch (Exception ex) { }
                            });
                            openTask.Wait(2000);
                            if (!plc.IsConnected)
                            {
                                throw new Exception();
                            }
                        }
                        try
                        {
                            byte[] byte1 = plc.ReadBytes(DataType.DataBlock, 8, 0, 36);
                            uint[] result1 = DWord.ToArray(byte1);

                            for (int i = 1; i < 9; i++)
                            {
                                var totalCountList = new List<long>();
                                string newId = "242488" + i;

                                totalCountList = new List<long>() { (long)result1[i] }; //2424887

                                ReadMachineData data = new ReadMachineData
                                {
                                    MachineID = Convert.ToInt64(newId),
                                    TotalCount = totalCountList,
                                    Break = false,
                                    Sched = false,
                                };

                                ts.Add(data);
                            }
                        }
                        catch (Exception) { }
                        plc.Close();
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("192.168.242.48");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            #endregion
            #region PFG=>PFL=> Hot Fill=>Duel Filler
            try
            {
                using (var plc = new Plc(CpuType.S7200Smart, "192.168.242.42", 0, 1))
                {
                    try
                    {
                        if (!plc.IsConnected)
                        {
                            var openTask = Task.Run(() =>
                            {
                                try
                                {
                                    plc.Open();
                                }
                                catch (Exception ex) { }
                            });
                            openTask.Wait(2000);
                            if (!plc.IsConnected)
                            {
                                throw new Exception();
                            }
                        }
                        try
                        {
                            byte[] byte1 = plc.ReadBytes(DataType.DataBlock, 1, 0, 124);
                            uint[] result1 = DWord.ToArray(byte1);

                            for (int i = 1; i < 31; i++)
                            {
                                var totalCountList = new List<long>();
                                string newId = "242421" + i;

                                totalCountList = new List<long>() { (long)result1[i] };

                                ReadMachineData data = new ReadMachineData
                                {
                                    MachineID = Convert.ToInt64(newId),
                                    TotalCount = totalCountList,
                                    Break = false,
                                    Sched = false,
                                };

                                ts.Add(data);
                            }
                        }
                        catch (Exception) { }
                        plc.Close();
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("192.168.242.42");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            #endregion
            #region DIP_REL_THA
            try
            {
                using (var plc = new Plc(CpuType.S71200, "10.7.238.21", 0, 1))
                {
                    try
                    {
                        if (!plc.IsConnected)
                        {
                            var openTask = Task.Run(() =>
                            {
                                try
                                {
                                    plc.Open();
                                }
                                catch (Exception ex) { }
                            });
                            openTask.Wait(2000);
                            if (!plc.IsConnected)
                            {
                                throw new Exception();
                            }
                        }
                        try
                        {
                            byte[] byte1 = plc.ReadBytes(DataType.DataBlock, 1, 0, 60);
                            uint[] result1 = DWord.ToArray(byte1);

                            for (int i = 1; i < 15; i++)
                            {
                                var totalCountList = new List<long>();
                                string newId = "107238211" + i;

                                totalCountList = new List<long>() { (long)result1[i] };

                                ReadMachineData data = new ReadMachineData
                                {
                                    MachineID = Convert.ToInt64(newId),
                                    TotalCount = totalCountList,
                                    Break = false,
                                    Sched = false,
                                };

                                ts.Add(data);
                            }
                        }
                        catch (Exception) { }
                        plc.Close();
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("10.7.238.21");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            #endregion
            #region PFG PFL Ex_Snacks
            try
            {
                using (var plc = new Plc(CpuType.S71200, "192.168.242.50", 0, 1))
                {
                    try
                    {
                        if (!plc.IsConnected)
                        {
                            var openTask = Task.Run(() =>
                            {
                                try
                                {
                                    plc.Open();
                                }
                                catch (Exception ex) { }
                            });
                            openTask.Wait(2000);
                            if (!plc.IsConnected)
                            {
                                throw new Exception();
                            }
                        }
                        try
                        {
                            byte[] byte1 = plc.ReadBytes(DataType.DataBlock, 1, 0, 52);
                            uint[] result1 = DWord.ToArray(byte1);

                            for (int i = 1; i < 13; i++)
                            {
                                var totalCountList = new List<long>();
                                string newId = "242501" + i;

                                totalCountList = new List<long>() { (long)result1[i] }; //24250112

                                ReadMachineData data = new ReadMachineData
                                {
                                    MachineID = Convert.ToInt64(newId),
                                    TotalCount = totalCountList,
                                    Break = false,
                                    Sched = false,
                                };

                                ts.Add(data);
                            }
                        }
                        catch (Exception) { }
                        plc.Close();
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("192.168.242.50");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            #endregion
            #region RNG DPL
            try
            {
                using (var plc = new Plc(CpuType.S71200, "192.168.40.60", 0, 1))
                {
                    try
                    {
                        if (!plc.IsConnected)
                        {
                            var openTask = Task.Run(() =>
                            {
                                try
                                {
                                    plc.Open();
                                }
                                catch (Exception ex) { }
                            });
                            openTask.Wait(2000);
                            if (!plc.IsConnected)
                            {
                                throw new Exception();
                            }
                        }
                        try
                        {
                            byte[] byte1 = plc.ReadBytes(DataType.DataBlock, 1, 0, 1200);
                            uint[] result1 = DWord.ToArray(byte1);

                            for (int i = 1; i < 300; i++)
                            {
                                var totalCountList = new List<long>();
                                string newId = "40601" + i;

                                totalCountList = new List<long>() { (long)result1[i] };

                                ReadMachineData data = new ReadMachineData
                                {
                                    MachineID = Convert.ToInt64(newId),
                                    TotalCount = totalCountList,
                                    Break = false,
                                    Sched = false,
                                };

                                ts.Add(data);
                            }
                        }
                        catch (Exception) { }
                        plc.Close();
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("192.168.40.60");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            #endregion
            #region BBMTD Wooden Door
            try
            {
                using (var plc = new Plc(CpuType.S71200, "10.7.238.24", 0, 1))
                {
                    try
                    {
                        if (!plc.IsConnected)
                        {
                            var openTask = Task.Run(() =>
                            {
                                try
                                {
                                    plc.Open();
                                }
                                catch (Exception ex) { }
                            });
                            openTask.Wait(2000);
                            if (!plc.IsConnected)
                            {
                                throw new Exception();
                            }
                        }

                        try
                        {
                            byte[] byte1 = plc.ReadBytes(DataType.DataBlock, 1, 0, 52);
                            uint[] result1 = DWord.ToArray(byte1);
                            byte[] byte2 = plc.ReadBytes(DataType.DataBlock, 2, 0, 52);
                            uint[] result2 = DWord.ToArray(byte2);

                            for (int i = 1; i < 13; i++)
                            {
                                string tempId = i.ToString();
                                tempId = "107238241" + i;

                                var totalCountList = new List<long>()
                                {
                                    (long)result1[i],
                                    (long)result2[i],
                                };
                                ReadMachineData data = new ReadMachineData
                                {
                                    MachineID = Convert.ToInt64(tempId),
                                    TotalCount = totalCountList,
                                    Break = false,
                                    Sched = false,
                                };

                                ts.Add(data);
                            }
                        }
                        catch (Exception) { }

                        plc.Close();
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("10.7.238.24");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            #endregion
            #region PIP BBL
            try
            {
                using (var plc = new Plc(CpuType.S7200Smart, "10.7.0.28", 0, 1))
                {
                    try
                    {
                        if (!plc.IsConnected)
                        {
                            var openTask = Task.Run(() =>
                            {
                                try
                                {
                                    plc.Open();
                                }
                                catch (Exception ex) { }
                            });
                            openTask.Wait(2000);
                            if (!plc.IsConnected)
                            {
                                throw new Exception();
                            }
                        }

                        try
                        {
                            byte[] byte1 = plc.ReadBytes(DataType.DataBlock, 1, 0, 72);
                            uint[] result1 = DWord.ToArray(byte1);

                            for (int i = 1; i < 18; i++)
                            {
                                var totalCountList = new List<long>();
                                string newId = "1070281" + i;

                                totalCountList = new List<long>() { (long)result1[i] };

                                ReadMachineData data = new ReadMachineData
                                {
                                    MachineID = Convert.ToInt64(newId),
                                    TotalCount = totalCountList,
                                    Break = false,
                                    Sched = false,
                                };

                                ts.Add(data);
                            }
                        }
                        catch (Exception) { }

                        plc.Close();
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("10.7.0.28");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            #endregion
            #region PIP BPIL
            try
            {
                using (var plc = new Plc(CpuType.S71200, "10.7.0.60", 0, 1))
                {
                    try
                    {
                        if (!plc.IsConnected)
                        {
                            var openTask = Task.Run(() =>
                            {
                                try
                                {
                                    plc.Open();
                                }
                                catch (Exception ex) { }
                            });
                            openTask.Wait(2000);
                            if (!plc.IsConnected)
                            {
                                throw new Exception();
                            }
                        }

                        try
                        {
                            byte[] byte1 = plc.ReadBytes(DataType.DataBlock, 1, 0, 48);
                            uint[] result1 = DWord.ToArray(byte1);

                            for (int i = 1; i < 12; i++)
                            {
                                var totalCountList = new List<long>();
                                string newId = "1070601" + i;

                                totalCountList = new List<long>() { (long)result1[i] };

                                ReadMachineData data = new ReadMachineData
                                {
                                    MachineID = Convert.ToInt64(newId),
                                    TotalCount = totalCountList,
                                    Break = false,
                                    Sched = false,
                                };

                                ts.Add(data);
                            }
                        }
                        catch (Exception) { }

                        plc.Close();
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("10.7.0.60");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            #endregion
            #region PFG PCL Choco choco
            try
            {
                using (var plc = new Plc(CpuType.S7200Smart, "192.168.242.51", 0, 1))
                {
                    try
                    {
                        if (!plc.IsConnected)
                        {
                            var openTask = Task.Run(() =>
                            {
                                try
                                {
                                    plc.Open();
                                }
                                catch (Exception ex) { }
                            });
                            openTask.Wait(2000);
                            if (!plc.IsConnected)
                            {
                                throw new Exception();
                            }
                        }

                        try
                        {
                            byte[] byte1 = plc.ReadBytes(DataType.DataBlock, 1, 0, 216);
                            uint[] result1 = DWord.ToArray(byte1);

                            for (int i = 1; i < 54; i++)
                            {
                                var totalCountList = new List<long>();
                                string newId = "242511" + i;

                                totalCountList = new List<long>() { (long)result1[i] }; //24251144

                                ReadMachineData data = new ReadMachineData
                                {
                                    MachineID = Convert.ToInt64(newId),
                                    TotalCount = totalCountList,
                                    Break = false,
                                    Sched = false,
                                };

                                ts.Add(data);
                            }
                        }
                        catch (Exception) { }

                        plc.Close();
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("192.168.242.51");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            #endregion
            #region BMD-BML
            try
            {
                using (var plc = new Plc(CpuType.S7200Smart, "192.168.100.11", 0, 1))
                {
                    try
                    {
                        if (!plc.IsConnected)
                        {
                            var openTask = Task.Run(() =>
                            {
                                try
                                {
                                    plc.Open();
                                }
                                catch (Exception ex) { }
                            });
                            openTask.Wait(2000);
                            if (!plc.IsConnected)
                            {
                                throw new Exception();
                            }
                        }

                        try
                        {
                            byte[] byte1 = plc.ReadBytes(DataType.DataBlock, 1, 0, 28);
                            uint[] result1 = DWord.ToArray(byte1);

                            for (int i = 1; i < 7; i++)
                            {
                                var totalCountList = new List<long>();
                                string newId = "100111" + i;

                                totalCountList = new List<long>() { (long)result1[i] };

                                ReadMachineData data = new ReadMachineData
                                {
                                    MachineID = Convert.ToInt64(newId),
                                    TotalCount = totalCountList,
                                    Break = false,
                                    Sched = false,
                                };

                                ts.Add(data);
                            }
                        }
                        catch (Exception) { }

                        plc.Close();
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("192.168.100.11");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            #endregion
            #region BIP PAL
            try
            {
                using (var plc = new Plc(CpuType.S7200Smart, "192.168.32.187", 0, 1))
                {
                    try
                    {
                        if (!plc.IsConnected)
                        {
                            var openTask = Task.Run(() =>
                            {
                                try
                                {
                                    plc.Open();
                                }
                                catch (Exception ex) { }
                            });
                            openTask.Wait(2000);
                            if (!plc.IsConnected)
                            {
                                throw new Exception();
                            }
                        }

                        try
                        {
                            byte[] byte1 = plc.ReadBytes(DataType.DataBlock, 1, 0, 100); //3218715
                            uint[] result1 = DWord.ToArray(byte1);

                            for (int i = 1; i < 25; i++)
                            {
                                var totalCountList = new List<long>();
                                string newId = "321871" + i;

                                totalCountList = new List<long>() { (long)result1[i] };

                                ReadMachineData data = new ReadMachineData
                                {
                                    MachineID = Convert.ToInt64(newId),
                                    TotalCount = totalCountList,
                                    Break = false,
                                    Sched = false,
                                };

                                ts.Add(data);
                            }
                        }
                        catch (Exception) { }

                        plc.Close();
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("192.168.32.187");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            #endregion
            #region 192.168.228.40
            try
            {
                using (var plc = new Plc(CpuType.S71200, "192.168.228.40", 0, 1))
                {
                    try
                    {
                        if (!plc.IsConnected)
                        {
                            var openTask = Task.Run(() =>
                            {
                                try
                                {
                                    plc.Open();
                                }
                                catch (Exception ex) { }
                            });
                            openTask.Wait(2000);
                            if (!plc.IsConnected)
                            {
                                throw new Exception();
                            }
                        }

                        try
                        {
                            byte[] byte1 = plc.ReadBytes(DataType.DataBlock, 1, 0, 16);
                            uint[] result1 = DWord.ToArray(byte1);
                            byte[] byte2 = plc.ReadBytes(DataType.DataBlock, 2, 0, 376);
                            uint[] result2 = DWord.ToArray(byte2);

                            for (int i = 1; i < 4; i++)
                            {
                                var totalCountList = new List<long>();
                                string newId = "228401" + i;

                                totalCountList = new List<long>() { (long)result1[i] };

                                ReadMachineData data = new ReadMachineData
                                {
                                    MachineID = Convert.ToInt64(newId),
                                    TotalCount = totalCountList,
                                    Break = false,
                                    Sched = false,
                                };

                                ts.Add(data);
                            }
                            for (int i = 1; i < 94; i++)
                            {
                                var totalCountList = new List<long>();
                                string newId = "228402" + i;

                                totalCountList = new List<long>() { (long)result2[i] };

                                ReadMachineData data = new ReadMachineData
                                {
                                    MachineID = Convert.ToInt64(newId),
                                    TotalCount = totalCountList,
                                    Break = false,
                                    Sched = false,
                                };

                                ts.Add(data);
                            }
                        }
                        catch (Exception) { }

                        plc.Close();
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("192.168.228.40");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            #endregion 
            #region 10.7.0.29 BBL PIP
            try
            {
                using (var plc = new Plc(CpuType.S7200Smart, "10.7.0.29", 0, 1))
                {
                    try
                    {
                        if (!plc.IsConnected)
                        {
                            var openTask = Task.Run(() =>
                            {
                                try
                                {
                                    plc.Open();
                                }
                                catch (Exception ex) { }
                            });
                            openTask.Wait(2000);
                            if (!plc.IsConnected)
                            {
                                throw new Exception();
                            }
                        }

                        try
                        {
                            byte[] byte1 = plc.ReadBytes(DataType.DataBlock, 1, 0, 76); //3218715
                            uint[] result1 = DWord.ToArray(byte1);

                            for (int i = 1; i < 19; i++)
                            {
                                var totalCountList = new List<long>();
                                string newId = "1070291" + i;

                                totalCountList = new List<long>() { (long)result1[i] };

                                ReadMachineData data = new ReadMachineData
                                {
                                    MachineID = Convert.ToInt64(newId),
                                    TotalCount = totalCountList,
                                    Break = false,
                                    Sched = false,
                                };

                                ts.Add(data);
                            }
                        }
                        catch (Exception) { }

                        plc.Close();
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("10.7.0.29");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            #endregion

            #region 10.7.0.30 BBL PIP Wafer Line (Old)
            try
            {
                using (var plc = new Plc(CpuType.S7200Smart, "10.7.0.30", 0, 1))
                {
                    try
                    {
                        if (!plc.IsConnected)
                        {
                            var openTask = Task.Run(() =>
                            {
                                try
                                {
                                    plc.Open();
                                }
                                catch (Exception ex) { }
                            });
                            openTask.Wait(2000);
                            if (!plc.IsConnected)
                            {
                                throw new Exception();
                            }
                        }

                        try
                        {
                            byte[] byte1 = plc.ReadBytes(DataType.DataBlock, 1, 0, 76); //3218715
                            uint[] result1 = DWord.ToArray(byte1);

                            for (int i = 1; i < 19; i++)
                            {
                                var totalCountList = new List<long>();
                                string newId = "1070301" + i;

                                totalCountList = new List<long>() { (long)result1[i] };

                                ReadMachineData data = new ReadMachineData
                                {
                                    MachineID = Convert.ToInt64(newId),
                                    TotalCount = totalCountList,
                                    Break = false,
                                    Sched = false,
                                };

                                ts.Add(data);
                            }
                        }
                        catch (Exception) { }

                        plc.Close();
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("10.7.0.30");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            #endregion
            #region 10.7.0.45 PIP-2	Marketing	Digital Printing House
            try
            {
                using (var plc = new Plc(CpuType.S71200, "10.7.0.45", 0, 1))
                {
                    try
                    {
                        if (!plc.IsConnected)
                        {
                            var openTask = Task.Run(() =>
                            {
                                try
                                {
                                    plc.Open();
                                }
                                catch (Exception ex) { }
                            });
                            openTask.Wait(2000);
                            if (!plc.IsConnected)
                            {
                                throw new Exception();
                            }
                        }

                        try
                        {
                            byte[] byte1 = plc.ReadBytes(DataType.DataBlock, 1, 0, 52); //10704519
                            uint[] result1 = DWord.ToArray(byte1);

                            for (int i = 1; i < 13; i++)
                            {
                                var totalCountList = new List<long>();
                                string newId = "1070451" + i;

                                totalCountList = new List<long>() { (long)result1[i] };

                                ReadMachineData data = new ReadMachineData
                                {
                                    MachineID = Convert.ToInt64(newId),
                                    TotalCount = totalCountList,
                                    Break = false,
                                    Sched = false,
                                };

                                ts.Add(data);
                            }
                        }
                        catch (Exception) { }

                        plc.Close();
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("10.7.0.45");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            #endregion
            #region 172.29.1.115 PDC(Shahjadpur) Dairy Yogurt Line
            try
            {
                using (var plc = new Plc(CpuType.S7200Smart, "172.29.1.115", 0, 1))
                {
                    try
                    {
                        if (!plc.IsConnected)
                        {
                            var openTask = Task.Run(() =>
                            {
                                try
                                {
                                    plc.Open();
                                }
                                catch (Exception ex) { }
                            });
                            openTask.Wait(2000);
                            if (!plc.IsConnected)
                            {
                                throw new Exception();
                            }
                        }

                        try
                        {
                            byte[] byte1 = plc.ReadBytes(DataType.DataBlock, 1, 0, 56); //3218715
                            uint[] result1 = DWord.ToArray(byte1);

                            for (int i = 1; i < 14; i++)
                            {
                                var totalCountList = new List<long>();
                                string newId = "1722911151" + i;

                                totalCountList = new List<long>() { (long)result1[i] };

                                ReadMachineData data = new ReadMachineData
                                {
                                    MachineID = Convert.ToInt64(newId),
                                    TotalCount = totalCountList,
                                    Break = false,
                                    Sched = false,
                                };

                                ts.Add(data);
                            }
                        }
                        catch (Exception) { }

                        plc.Close();
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("172.29.1.115");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            #endregion
            #region 10.7.0.31 PIP	BBL	Honeycomb
            try
            {
                using (var plc = new Plc(CpuType.S7200Smart, "10.7.0.31", 0, 1))
                {
                    try
                    {
                        if (!plc.IsConnected)
                        {
                            var openTask = Task.Run(() =>
                            {
                                try
                                {
                                    plc.Open();
                                }
                                catch (Exception ex) { }
                            });
                            openTask.Wait(2000);
                            if (!plc.IsConnected)
                            {
                                throw new Exception();
                            }
                        }

                        try
                        {
                            byte[] byte1 = plc.ReadBytes(DataType.DataBlock, 2, 0, 136);
                            uint[] result1 = DWord.ToArray(byte1);
                            byte[] byte2 = plc.ReadBytes(DataType.DataBlock, 1, 0, 28);
                            uint[] result2 = DWord.ToArray(byte2);
                            for (int i = 1; i < 34; i++)
                            {
                                var totalCountList = new List<long>();
                                string newId = "1070312" + i;

                                totalCountList = new List<long>() { (long)result1[i] };

                                ReadMachineData data = new ReadMachineData
                                {
                                    MachineID = Convert.ToInt64(newId),
                                    TotalCount = totalCountList,
                                    Break = false,
                                    Sched = false,
                                };

                                ts.Add(data);
                            }

                            for (int i = 1; i < 7; i++)
                            {
                                var totalCountList = new List<long>();
                                string newId = "1070311" + i;

                                totalCountList = new List<long>() { (long)result2[i] };

                                ReadMachineData data = new ReadMachineData
                                {
                                    MachineID = Convert.ToInt64(newId),
                                    TotalCount = totalCountList,
                                    Break = false,
                                    Sched = false,
                                };

                                ts.Add(data);
                            }
                        }
                        catch (Exception) { }

                        plc.Close();
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("10.7.0.31");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            #endregion
            #region 10.7.0.40  PIP pcl Molded Chocolate Line
            try
            {
                using (var plc = new Plc(CpuType.S71200, "10.7.0.40", 0, 1))
                {
                    try
                    {
                        if (!plc.IsConnected)
                        {
                            var openTask = Task.Run(() =>
                            {
                                try
                                {
                                    plc.Open();
                                }
                                catch (Exception ex) { }
                            });
                            openTask.Wait(2000);
                            if (!plc.IsConnected)
                            {
                                throw new Exception();
                            }
                        }

                        try
                        {
                            byte[] byte1 = plc.ReadBytes(DataType.DataBlock, 1, 0, 328); //3218715
                            uint[] result1 = DWord.ToArray(byte1);

                            for (int i = 1; i < 82; i++)
                            {
                                var totalCountList = new List<long>();
                                string newId = "1070401" + i;

                                totalCountList = new List<long>() { (long)result1[i] };

                                ReadMachineData data = new ReadMachineData
                                {
                                    MachineID = Convert.ToInt64(newId),
                                    TotalCount = totalCountList,
                                    Break = false,
                                    Sched = false,
                                };

                                ts.Add(data);
                            }
                        }
                        catch (Exception) { }

                        plc.Close();
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("10.7.0.40");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            #endregion
            #region 10.7.0.46  PIP-2 BBL Bun Line
            try
            {
                using (var plc = new Plc(CpuType.S7200Smart, "10.7.0.46", 0, 1))
                {
                    try
                    {
                        if (!plc.IsConnected)
                        {
                            var openTask = Task.Run(() =>
                            {
                                try
                                {
                                    plc.Open();
                                }
                                catch (Exception ex) { }
                            });
                            openTask.Wait(2000);
                            if (!plc.IsConnected)
                            {
                                throw new Exception();
                            }
                        }

                        try
                        {
                            byte[] byte1 = plc.ReadBytes(DataType.DataBlock, 1, 0, 56); //3218715
                            uint[] result1 = DWord.ToArray(byte1);

                            for (int i = 1; i < 14; i++)
                            {
                                var totalCountList = new List<long>();
                                string newId = "1070461" + i;

                                totalCountList = new List<long>() { (long)result1[i] };

                                ReadMachineData data = new ReadMachineData
                                {
                                    MachineID = Convert.ToInt64(newId),
                                    TotalCount = totalCountList,
                                    Break = false,
                                    Sched = false,
                                };

                                ts.Add(data);
                            }
                        }
                        catch (Exception) { }

                        plc.Close();
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("10.7.0.46");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            #endregion

            #region 10.7.0.38 PIP-2	PFF	Paratha
            try
            {
                using (var plc = new Plc(CpuType.S7200Smart, "10.7.0.38", 0, 1))
                {
                    try
                    {
                        if (!plc.IsConnected)
                        {
                            var openTask = Task.Run(() =>
                            {
                                try
                                {
                                    plc.Open();
                                }
                                catch (Exception ex) { }
                            });
                            openTask.Wait(2000);
                            if (!plc.IsConnected)
                            {
                                throw new Exception();
                            }
                        }

                        try
                        {
                            byte[] byte1 = plc.ReadBytes(DataType.DataBlock, 1, 0, 40); //3218715
                            uint[] result1 = DWord.ToArray(byte1);

                            for (int i = 1; i < 10; i++)
                            {
                                var totalCountList = new List<long>();
                                string newId = "1070381" + i;

                                totalCountList = new List<long>() { (long)result1[i] };

                                ReadMachineData data = new ReadMachineData
                                {
                                    MachineID = Convert.ToInt64(newId),
                                    TotalCount = totalCountList,
                                    Break = false,
                                    Sched = false,
                                };

                                ts.Add(data);
                            }
                        }
                        catch (Exception) { }

                        plc.Close();
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("10.7.0.38");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            #endregion

            #region 192.168.242.52 PFG PCL	Chocolate_Line
            try
            {
                using (var plc = new Plc(CpuType.S7200Smart, "192.168.242.52", 0, 1))
                {
                    try
                    {
                        if (!plc.IsConnected)
                        {
                            var openTask = Task.Run(() =>
                            {
                                try
                                {
                                    plc.Open();
                                }
                                catch (Exception ex) { }
                            });
                            openTask.Wait(2000);
                            if (!plc.IsConnected)
                            {
                                throw new Exception();
                            }
                        }

                        try
                        {
                            byte[] byte1 = plc.ReadBytes(DataType.DataBlock, 1, 0, 24); //3218715
                            uint[] result1 = DWord.ToArray(byte1);

                            for (int i = 1; i < 6; i++)
                            {
                                var totalCountList = new List<long>();
                                string newId = "242521" + i;

                                totalCountList = new List<long>() { (long)result1[i] };

                                ReadMachineData data = new ReadMachineData
                                {
                                    MachineID = Convert.ToInt64(newId),
                                    TotalCount = totalCountList,
                                    Break = false,
                                    Sched = false,
                                };

                                ts.Add(data);
                            }
                        }
                        catch (Exception) { }

                        plc.Close();
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("192.168.242.52");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            #endregion

            #region 10.7.0.36 PIP2  BBL
            try
            {
                using (var plc = new Plc(CpuType.S71200, "10.7.0.36", 0, 1))
                {
                    try
                    {
                        if (!plc.IsConnected)
                        {
                            var openTask = Task.Run(() =>
                            {
                                try
                                {
                                    plc.Open();
                                }
                                catch (Exception ex) { }
                            });
                            openTask.Wait(2000);
                            if (!plc.IsConnected)
                            {
                                throw new Exception();
                            }
                        }

                        try
                        {
                            byte[] byte1 = plc.ReadBytes(DataType.DataBlock, 1, 0, 76);
                            uint[] result1 = DWord.ToArray(byte1);
                            byte[] byte2 = plc.ReadBytes(DataType.DataBlock, 2, 0, 72);
                            uint[] result2 = DWord.ToArray(byte2);

                            for (int i = 1; i < 19; i++)
                            {
                                var totalCountList = new List<long>();
                                string newId = "1070361" + i;

                                totalCountList = new List<long>() { (long)result1[i] };

                                ReadMachineData data = new ReadMachineData
                                {
                                    MachineID = Convert.ToInt64(newId),
                                    TotalCount = totalCountList,
                                    Break = false,
                                    Sched = false,
                                };

                                ts.Add(data);
                            }

                            for (int i = 1; i < 18; i++)
                            {
                                var totalCountList = new List<long>();
                                string newId = "1070362" + i;

                                totalCountList = new List<long>() { (long)result2[i] };

                                ReadMachineData data = new ReadMachineData
                                {
                                    MachineID = Convert.ToInt64(newId),
                                    TotalCount = totalCountList,
                                    Break = false,
                                    Sched = false,
                                };

                                ts.Add(data);
                            }
                        }
                        catch (Exception) { }

                        plc.Close();
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("10.7.0.36");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            #endregion

            #region 10.7.0.50 PIP2  PML
            try
            {
                using (var plc = new Plc(CpuType.S71200, "10.7.0.50", 0, 1))
                {
                    try
                    {
                        if (!plc.IsConnected)
                        {
                            var openTask = Task.Run(() =>
                            {
                                try
                                {
                                    plc.Open();
                                }
                                catch (Exception ex) { }
                            });
                            openTask.Wait(2000);
                            if (!plc.IsConnected)
                            {
                                throw new Exception();
                            }
                        }

                        try
                        {
                            byte[] byte1 = plc.ReadBytes(DataType.DataBlock, 1, 0, 120);
                            uint[] result1 = DWord.ToArray(byte1);
                            byte[] byte2 = plc.ReadBytes(DataType.DataBlock, 2, 0, 12);
                            uint[] result2 = DWord.ToArray(byte2);

                            for (int i = 1; i < 30; i++)
                            {
                                var totalCountList = new List<long>();
                                string newId = "1070501" + i;

                                totalCountList = new List<long>() { (long)result1[i] };

                                ReadMachineData data = new ReadMachineData
                                {
                                    MachineID = Convert.ToInt64(newId),
                                    TotalCount = totalCountList,
                                    Break = false,
                                    Sched = false,
                                };

                                ts.Add(data);
                            }

                            for (int i = 1; i < 3; i++)
                            {
                                var totalCountList = new List<long>();
                                string newId = "1070502" + i;

                                totalCountList = new List<long>() { (long)result2[i] };

                                ReadMachineData data = new ReadMachineData
                                {
                                    MachineID = Convert.ToInt64(newId),
                                    TotalCount = totalCountList,
                                    Break = false,
                                    Sched = false,
                                };

                                ts.Add(data);
                            }
                        }
                        catch (Exception) { }

                        plc.Close();
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("10.7.0.50");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            #endregion

            #region 10.7.0.47 PIP2  BBL
            try
            {
                using (var plc = new Plc(CpuType.S7200Smart, "10.7.0.47", 0, 1))
                {
                    try
                    {
                        if (!plc.IsConnected)
                        {
                            var openTask = Task.Run(() =>
                            {
                                try
                                {
                                    plc.Open();
                                }
                                catch (Exception ex) { }
                            });
                            bool connected = openTask.Wait(2000);
                            if (!connected)
                            {
                                throw new Exception();
                            }
                        }

                        try
                        {
                            byte[] byte1 = plc.ReadBytes(DataType.DataBlock, 1, 0, 20);
                            uint[] result1 = DWord.ToArray(byte1);
                            byte[] byte2 = plc.ReadBytes(DataType.DataBlock, 2, 0, 24);
                            uint[] result2 = DWord.ToArray(byte2);

                            for (int i = 1; i < 5; i++)
                            {
                                var totalCountList = new List<long>();
                                string newId = "1070471" + i;

                                totalCountList = new List<long>() { (long)result1[i] };

                                ReadMachineData data = new ReadMachineData
                                {
                                    MachineID = Convert.ToInt64(newId),
                                    TotalCount = totalCountList,
                                    Break = false,
                                    Sched = false,
                                };

                                ts.Add(data);
                            }

                            for (int i = 1; i < 6; i++)
                            {
                                var totalCountList = new List<long>();
                                string newId = "1070472" + i;

                                totalCountList = new List<long>() { (long)result2[i] };

                                ReadMachineData data = new ReadMachineData
                                {
                                    MachineID = Convert.ToInt64(newId),
                                    TotalCount = totalCountList,
                                    Break = false,
                                    Sched = false,
                                };

                                ts.Add(data);
                            }
                        }
                        catch (Exception) { }

                        plc.Close();
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("10.7.0.47");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            #endregion

            #region 10.7.0.48  PIP-2 BBL Bun Line
            try
            {
                using (var plc = new Plc(CpuType.S7200Smart, "10.7.0.48", 0, 1))
                {
                    try
                    {
                        if (!plc.IsConnected)
                        {
                            var openTask = Task.Run(() =>
                            {
                                try
                                {
                                    plc.Open();
                                }
                                catch (Exception ex) { }
                            });
                            openTask.Wait(2000);
                            if (!plc.IsConnected)
                            {
                                throw new Exception();
                            }
                        }

                        try
                        {
                            byte[] byte1 = plc.ReadBytes(DataType.DataBlock, 1, 0, 52); //3218715
                            uint[] result1 = DWord.ToArray(byte1);

                            for (int i = 1; i < 13; i++)
                            {
                                var totalCountList = new List<long>();
                                string newId = "1070481" + i;

                                totalCountList = new List<long>() { (long)result1[i] };

                                ReadMachineData data = new ReadMachineData
                                {
                                    MachineID = Convert.ToInt64(newId),
                                    TotalCount = totalCountList,
                                    Break = false,
                                    Sched = false,
                                };

                                ts.Add(data);
                            }
                        }
                        catch (Exception) { }

                        plc.Close();
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("10.7.0.48");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            #endregion

            #region 10.7.0.49  PIP-2 BBL Bun Line
            try
            {
                using (var plc = new Plc(CpuType.S7200Smart, "10.7.0.49", 0, 1))
                {
                    try
                    {
                        if (!plc.IsConnected)
                        {
                            var openTask = Task.Run(() =>
                            {
                                try
                                {
                                    plc.Open();
                                }
                                catch (Exception ex) { }
                            });
                            bool connected = openTask.Wait(2000);
                            if (!plc.IsConnected)
                            {
                                throw new Exception();
                            }
                        }

                        try
                        {
                            byte[] byte1 = plc.ReadBytes(DataType.DataBlock, 1, 0, 60); //3218715
                            uint[] result1 = DWord.ToArray(byte1);

                            for (int i = 1; i < 15; i++)
                            {
                                var totalCountList = new List<long>();
                                string newId = "1070491" + i;

                                totalCountList = new List<long>() { (long)result1[i] };

                                ReadMachineData data = new ReadMachineData
                                {
                                    MachineID = Convert.ToInt64(newId),
                                    TotalCount = totalCountList,
                                    Break = false,
                                    Sched = false,
                                };

                                ts.Add(data);
                            }
                        }
                        catch (Exception) { }

                        plc.Close();
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("10.7.0.49");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            #endregion

            #region 192.168.194.7  CLF BBML Sandwich Panel And Corksheet
            try
            {
                using (var plc = new Plc(CpuType.S7200Smart, "192.168.194.7", 0, 1))
                {
                    try
                    {
                        if (!plc.IsConnected)
                        {
                            var openTask = Task.Run(() =>
                            {
                                try
                                {
                                    plc.Open();
                                }
                                catch (Exception ex) { }
                            });
                            openTask.Wait(2000);
                            if (!plc.IsConnected)
                            {
                                throw new Exception();
                            }
                        }

                        try
                        {
                            byte[] byte1 = plc.ReadBytes(DataType.DataBlock, 1, 0, 76); //3218715
                            uint[] result1 = DWord.ToArray(byte1);

                            for (int i = 1; i < 19; i++)
                            {
                                var totalCountList = new List<long>();
                                string newId = "19471" + i;

                                totalCountList = new List<long>() { (long)result1[i] };

                                ReadMachineData data = new ReadMachineData
                                {
                                    MachineID = Convert.ToInt64(newId),
                                    TotalCount = totalCountList,
                                    Break = false,
                                    Sched = false,
                                };

                                ts.Add(data);
                            }
                        }
                        catch (Exception) { }

                        plc.Close();
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("192.168.194.7");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            #endregion

            #region 192.168.194.8  CLF BBML Sandwich Panel And Corksheet
            try
            {
                using (var plc = new Plc(CpuType.S7200Smart, "192.168.194.8", 0, 1))
                {
                    try
                    {
                        if (!plc.IsConnected)
                        {
                            var openTask = Task.Run(() =>
                            {
                                try
                                {
                                    plc.Open();
                                }
                                catch (Exception ex) { }
                            });
                            bool connected = openTask.Wait(2000);
                            if (!plc.IsConnected)
                            {
                                throw new Exception();
                            }
                        }

                        try
                        {
                            byte[] byte1 = plc.ReadBytes(DataType.DataBlock, 1, 0, 36); //3218715
                            uint[] result1 = DWord.ToArray(byte1);

                            for (int i = 1; i < 9; i++)
                            {
                                var totalCountList = new List<long>();
                                string newId = "19481" + i;

                                totalCountList = new List<long>() { (long)result1[i] };

                                ReadMachineData data = new ReadMachineData
                                {
                                    MachineID = Convert.ToInt64(newId),
                                    TotalCount = totalCountList,
                                    Break = false,
                                    Sched = false,
                                };

                                ts.Add(data);
                            }
                        }
                        catch (Exception) { }

                        plc.Close();
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("192.168.194.8");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            #endregion

            #region 10.7.160.8 NAL Packaging	Spice
            try
            {
                using (var plc = new Plc(CpuType.S71200, "10.7.160.8", 0, 1))
                {
                    try
                    {
                        if (!plc.IsConnected)
                        {
                            var openTask = Task.Run(() =>
                            {
                                try
                                {
                                    plc.Open();
                                }
                                catch (Exception ex) { }
                            });
                            openTask.Wait(2000);
                            if (!plc.IsConnected)
                            {
                                throw new Exception();
                            }
                        }

                        try
                        {
                            byte[] byte1 = plc.ReadBytes(DataType.DataBlock, 10, 0, 84); //3218715
                            uint[] result1 = DWord.ToArray(byte1);

                            for (int i = 1; i < 21; i++)
                            {
                                var totalCountList = new List<long>();
                                string newId = "107160810" + i;

                                totalCountList = new List<long>() { (long)result1[i] };

                                ReadMachineData data = new ReadMachineData
                                {
                                    MachineID = Convert.ToInt64(newId),
                                    TotalCount = totalCountList,
                                    Break = false,
                                    Sched = false,
                                };

                                ts.Add(data);
                            }
                        }
                        catch (Exception) { }

                        plc.Close();
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("10.7.160.8");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            #endregion

            #region 192.168.242.53 PFG	PCL	Lolipop_Line
            try
            {
                using (var plc = new Plc(CpuType.S71200, "192.168.242.53", 0, 1))
                {
                    try
                    {
                        if (!plc.IsConnected)
                        {
                            var openTask = Task.Run(() =>
                            {
                                try
                                {
                                    plc.Open();
                                }
                                catch (Exception ex) { }
                            });
                            openTask.Wait(2000);
                            if (!plc.IsConnected)
                            {
                                throw new Exception();
                            }
                        }

                        try
                        {
                            byte[] byte1 = plc.ReadBytes(DataType.DataBlock, 1, 0, 32); //3218715
                            uint[] result1 = DWord.ToArray(byte1);

                            for (int i = 1; i < 8; i++)
                            {
                                var totalCountList = new List<long>();
                                string newId = "242531" + i;

                                totalCountList = new List<long>() { (long)result1[i] };

                                ReadMachineData data = new ReadMachineData
                                {
                                    MachineID = Convert.ToInt64(newId),
                                    TotalCount = totalCountList,
                                    Break = false,
                                    Sched = false,
                                };

                                ts.Add(data);
                            }
                        }
                        catch (Exception) { }

                        plc.Close();
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("192.168.242.53");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            #endregion

            #region 192.168.242.52 PFG PCL	Chocolate_Line
            try
            {
                using (var plc = new Plc(CpuType.S71200, "192.168.242.52", 0, 1))
                {
                    try
                    {
                        if (!plc.IsConnected)
                        {
                            var openTask = Task.Run(() =>
                            {
                                try
                                {
                                    plc.Open();
                                }
                                catch (Exception ex) { }
                            });
                            openTask.Wait(2000);
                            if (!plc.IsConnected)
                            {
                                throw new Exception();
                            }
                        }

                        try
                        {
                            byte[] byte1 = plc.ReadBytes(DataType.DataBlock, 1, 0, 44); //3218715
                            uint[] result1 = DWord.ToArray(byte1);

                            for (int i = 1; i < 11; i++)
                            {
                                var totalCountList = new List<long>();
                                string newId = "242521" + i;

                                totalCountList = new List<long>() { (long)result1[i] };

                                ReadMachineData data = new ReadMachineData
                                {
                                    MachineID = Convert.ToInt64(newId),
                                    TotalCount = totalCountList,
                                    Break = false,
                                    Sched = false,
                                };

                                ts.Add(data);
                            }
                        }
                        catch (Exception) { }

                        plc.Close();
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("192.168.242.52");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            #endregion
            #region 192.168.80.87
            try
            {
                using (var plc = new Plc(CpuType.S71200, "192.168.80.87", 0, 1))
                {
                    try
                    {
                        if (!plc.IsConnected)
                        {
                            var openTask = Task.Run(() =>
                            {
                                try
                                {
                                    plc.Open();
                                }
                                catch (Exception ex) { }
                            });
                            openTask.Wait(2000);
                            if (!plc.IsConnected)
                            {
                                throw new Exception();
                            }
                        }
                        try
                        {
                            byte[] byte1 = plc.ReadBytes(DataType.DataBlock, 1, 0, 236);
                            uint[] result1 = DWord.ToArray(byte1);
                            byte[] byte2 = plc.ReadBytes(DataType.DataBlock, 2, 0, 148);
                            uint[] result2 = DWord.ToArray(byte2);

                            for (int i = 1; i <= 33; i++)
                            {
                                var totalCountList = new List<long>();
                                string newId = "19216880871" + i;
                                totalCountList = new List<long>() { (long)result1[i] };
                                ReadMachineData data = new ReadMachineData
                                {
                                    MachineID = Convert.ToInt64(newId),
                                    TotalCount = totalCountList,
                                    Break = false,
                                    Sched = false,
                                };
                                ts.Add(data);
                            }

                            for (int i = 34; i <= 36; i++)
                            {
                                var totalCountList = new List<long>();
                                string newId = "19216880871" + i;
                                //int runtimeIndex = 16 + (i - 34); //  34->16, 35->17, 36->18
                                totalCountList = new List<long>()
                                {
                                    (long)result1[i],
                                    (long)result2[i],
                                };
                                ReadMachineData data = new ReadMachineData
                                {
                                    MachineID = Convert.ToInt64(newId),
                                    TotalCount = totalCountList,
                                    Break = false,
                                    Sched = false,
                                };
                                ts.Add(data);
                            }

                            for (int i = 37; i <= 58; i++)
                            {
                                var totalCountList = new List<long>();
                                string newId = "19216880871" + i;
                                totalCountList = new List<long>() { (long)result1[i] };
                                ReadMachineData data = new ReadMachineData
                                {
                                    MachineID = Convert.ToInt64(newId),
                                    TotalCount = totalCountList,
                                    Break = false,
                                    Sched = false,
                                };
                                ts.Add(data);
                            }

                            for (int i = 1; i < 13; i++)
                            {
                                var totalCountList = new List<long>();
                                string newId = "19216880872" + i;

                                totalCountList = new List<long>() { (long)result2[i] };

                                ReadMachineData data = new ReadMachineData
                                {
                                    MachineID = Convert.ToInt64(newId),
                                    TotalCount = totalCountList,
                                    Break = false,
                                    Sched = false,
                                };

                                ts.Add(data);
                            }
                        }
                        catch (Exception) { }
                        plc.Close();
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("192.168.80.87");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            //try
            //{
            //    using (var plc = new Plc(CpuType.S71200, "192.168.80.87", 0, 1))
            //    {
            //        try
            //        {
            //            if (!plc.IsConnected)
            //            {
            //                var openTask = Task.Run(() =>
            //                {
            //                    try
            //                    {
            //                        plc.Open();
            //                    }
            //                    catch (Exception ex)
            //                    {

            //                    }
            //                });
            //                openTask.Wait(2000);
            //                if (!plc.IsConnected)
            //                {
            //                    throw new Exception();
            //                }
            //            }

            //            try
            //            {
            //                byte[] byte1 = plc.ReadBytes(DataType.DataBlock, 1, 0, 232);
            //                uint[] result1 = DWord.ToArray(byte1);
            //                byte[] byte2 = plc.ReadBytes(DataType.DataBlock, 2, 0, 76);
            //                uint[] result2 = DWord.ToArray(byte2);

            //                for (int i = 1; i < 58; i++)
            //                {
            //                    var totalCountList = new List<long>();
            //                    string newId = "19216880871" + i;

            //                    totalCountList = new List<long>() { (long)result1[i] };//1921688087135

            //                    ReadMachineData data = new ReadMachineData
            //                    {
            //                        MachineID = Convert.ToInt64(newId),
            //                        TotalCount = totalCountList,
            //                        Break = false,
            //                        Sched = false
            //                    };

            //                    ts.Add(data);
            //                }

            //for (int i = 1; i < 13; i++)
            //{
            //    var totalCountList = new List<long>();
            //    string newId = "19216880872" + i;

            //    totalCountList = new List<long>() { (long)result2[i] };

            //    ReadMachineData data = new ReadMachineData
            //    {
            //        MachineID = Convert.ToInt64(newId),
            //        TotalCount = totalCountList,
            //        Break = false,
            //        Sched = false
            //    };

            //    ts.Add(data);
            //}
            //            }
            //            catch (Exception)
            //            {

            //            }

            //            plc.Close();

            //        }
            //        catch (Exception)
            //        {
            //            Console.WriteLine("192.168.80.87");
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
            #endregion
            #region 10.7.0.15
            try
            {
                using (var plc = new Plc(CpuType.S71200, "10.7.0.15", 0, 1))
                {
                    try
                    {
                        if (!plc.IsConnected)
                        {
                            var openTask = Task.Run(() =>
                            {
                                try
                                {
                                    plc.Open();
                                }
                                catch (Exception ex) { }
                            });
                            openTask.Wait(2000);
                            if (!plc.IsConnected)
                            {
                                throw new Exception();
                            }
                        }

                        try
                        {
                            byte[] byte1 = plc.ReadBytes(DataType.DataBlock, 27, 0, 12);
                            uint[] result1 = DWord.ToArray(byte1);
                            byte[] byte2 = plc.ReadBytes(DataType.DataBlock, 28, 0, 112);
                            uint[] result2 = DWord.ToArray(byte2);

                            for (int i = 1; i < 3; i++)
                            {
                                var totalCountList = new List<long>();
                                string newId = "10701527" + i;

                                totalCountList = new List<long>() { (long)result1[i] };

                                ReadMachineData data = new ReadMachineData
                                {
                                    MachineID = Convert.ToInt64(newId),
                                    TotalCount = totalCountList,
                                    Break = false,
                                    Sched = false,
                                };

                                ts.Add(data);
                            }

                            for (int i = 1; i < 28; i++)
                            {
                                var totalCountList = new List<long>();
                                string newId = "10701528" + i;

                                totalCountList = new List<long>() { (long)result2[i] };

                                ReadMachineData data = new ReadMachineData
                                {
                                    MachineID = Convert.ToInt64(newId),
                                    TotalCount = totalCountList,
                                    Break = false,
                                    Sched = false,
                                };

                                ts.Add(data);
                            }
                        }
                        catch (Exception) { }

                        plc.Close();
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("192.168.80.87");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            #endregion

            #region CLF	DPL	PCP
            try
            {
                using (var plc = new Plc(CpuType.S71200, "192.168.194.9", 0, 1))
                {
                    try
                    {
                        if (!plc.IsConnected)
                        {
                            var openTask = Task.Run(() =>
                            {
                                try
                                {
                                    plc.Open();
                                }
                                catch (Exception ex) { }
                            });
                            openTask.Wait(2000);
                            if (!plc.IsConnected)
                            {
                                throw new Exception();
                            }
                        }

                        try
                        {
                            byte[] byte1 = plc.ReadBytes(DataType.DataBlock, 1, 0, 44);
                            uint[] result1 = DWord.ToArray(byte1);

                            for (int i = 1; i < 11; i++)
                            {
                                var totalCountList = new List<long>();
                                string newId = "19491" + i;

                                totalCountList = new List<long>() { (long)result1[i] };

                                ReadMachineData data = new ReadMachineData
                                {
                                    MachineID = Convert.ToInt64(newId),
                                    TotalCount = totalCountList,
                                    Break = false,
                                    Sched = false,
                                };

                                ts.Add(data);
                            }
                        }
                        catch (Exception) { }

                        plc.Close();
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("192.168.194.9");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            #endregion
            #region DIP REL-THA
            try
            {
                using (var plc = new Plc(CpuType.S71200, "10.7.238.32", 0, 1))
                {
                    try
                    {
                        if (!plc.IsConnected)
                        {
                            var openTask = Task.Run(() =>
                            {
                                try
                                {
                                    plc.Open();
                                }
                                catch (Exception ex) { }
                            });
                            openTask.Wait(2000);
                            if (!plc.IsConnected)
                            {
                                throw new Exception();
                            }
                        }

                        try
                        {
                            byte[] byte1 = plc.ReadBytes(DataType.DataBlock, 1, 0, 168);
                            uint[] result1 = DWord.ToArray(byte1);

                            for (int i = 1; i < 42; i++)
                            {
                                var totalCountList = new List<long>();
                                string newId = "107238321" + i;

                                totalCountList = new List<long>() { (long)result1[i] };

                                ReadMachineData data = new ReadMachineData
                                {
                                    MachineID = Convert.ToInt64(newId),
                                    TotalCount = totalCountList,
                                    Break = false,
                                    Sched = false,
                                };

                                ts.Add(data);
                            }
                        }
                        catch (Exception) { }

                        plc.Close();
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("10.7.238.32");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            #endregion

            #region RAF	BBMT	FRP
            try
            {
                using (var plc = new Plc(CpuType.S71200, "172.22.114.7", 0, 1))
                {
                    try
                    {
                        if (!plc.IsConnected)
                        {
                            var openTask = Task.Run(() =>
                            {
                                try
                                {
                                    plc.Open();
                                }
                                catch (Exception ex) { }
                            });
                            openTask.Wait(2000);
                            if (!plc.IsConnected)
                            {
                                throw new Exception();
                            }
                        }

                        try
                        {
                            byte[] byte1 = plc.ReadBytes(DataType.DataBlock, 1, 0, 64);
                            uint[] result1 = DWord.ToArray(byte1);
                            byte[] byte2 = plc.ReadBytes(DataType.DataBlock, 2, 0, 16);
                            uint[] result2 = DWord.ToArray(byte2);
                            for (int i = 1; i < 16; i++)
                            {
                                var totalCountList = new List<long>();
                                string newId = "1722211471" + i;

                                totalCountList = new List<long>() { (long)result1[i] };

                                ReadMachineData data = new ReadMachineData
                                {
                                    MachineID = Convert.ToInt64(newId),
                                    TotalCount = totalCountList,
                                    Break = false,
                                    Sched = false,
                                };

                                ts.Add(data);
                            }

                            for (int i = 1; i < 4; i++)
                            {
                                var totalCountList = new List<long>();
                                string newId = "1722211472" + i;

                                totalCountList = new List<long>() { (long)result2[i] };

                                ReadMachineData data = new ReadMachineData
                                {
                                    MachineID = Convert.ToInt64(newId),
                                    TotalCount = totalCountList,
                                    Break = false,
                                    Sched = false,
                                };

                                ts.Add(data);
                            }
                        }
                        catch (Exception) { }

                        plc.Close();
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("172.22.114.7");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            #endregion
            #region DIP RAC
            try
            {
                using (var plc = new Plc(CpuType.S71200, "10.7.238.25", 0, 1))
                {
                    try
                    {
                        if (!plc.IsConnected)
                        {
                            var openTask = Task.Run(() =>
                            {
                                try
                                {
                                    plc.Open();
                                }
                                catch (Exception ex) { }
                            });
                            openTask.Wait(2000);
                            if (!plc.IsConnected)
                            {
                                throw new Exception();
                            }
                        }

                        try
                        {
                            byte[] byte1 = plc.ReadBytes(DataType.DataBlock, 1, 0, 68);
                            uint[] result1 = DWord.ToArray(byte1);

                            for (int i = 1; i < 17; i++)
                            {
                                var totalCountList = new List<long>();
                                string newId = "107238251" + i;

                                totalCountList = new List<long>() { (long)result1[i] };

                                ReadMachineData data = new ReadMachineData
                                {
                                    MachineID = Convert.ToInt64(newId),
                                    TotalCount = totalCountList,
                                    Break = false,
                                    Sched = false,
                                };

                                ts.Add(data);
                            }
                        }
                        catch (Exception) { }

                        plc.Close();
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("10.7.238.25");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            try
            {
                using (var plc = new Plc(CpuType.S71200, "10.7.238.33", 0, 1))
                {
                    try
                    {
                        if (!plc.IsConnected)
                        {
                            var openTask = Task.Run(() =>
                            {
                                try
                                {
                                    plc.Open();
                                }
                                catch (Exception ex) { }
                            });
                            openTask.Wait(2000);
                            if (!plc.IsConnected)
                            {
                                throw new Exception();
                            }
                        }

                        try
                        {
                            byte[] byte1 = plc.ReadBytes(DataType.DataBlock, 1, 0, 164);
                            uint[] result1 = DWord.ToArray(byte1);

                            //for (int i = 1; i < 41; i++)
                            for (int i = 1; i < 45; i++) // for PRG MIS AKM Kalam 24 Nov-2025
                            {
                                var totalCountList = new List<long>();
                                string newId = "107238331" + i;

                                totalCountList = new List<long>() { (long)result1[i] };

                                ReadMachineData data = new ReadMachineData
                                {
                                    MachineID = Convert.ToInt64(newId),
                                    TotalCount = totalCountList,
                                    Break = false,
                                    Sched = false,
                                };

                                ts.Add(data);
                            }
                        }
                        catch (Exception) { }

                        plc.Close();
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("10.7.238.33");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            #endregion
            #region RIP DPL
            try
            {
                using (var plc = new Plc(CpuType.S71500, "10.7.148.3", 0, 1))
                {
                    try
                    {
                        if (!plc.IsConnected)
                        {
                            var openTask = Task.Run(() =>
                            {
                                try
                                {
                                    plc.Open();
                                }
                                catch (Exception ex) { }
                            });
                            openTask.Wait(2000);
                            if (!plc.IsConnected)
                            {
                                throw new Exception();
                            }
                        }
                        try
                        {
                            byte[] byte1 = plc.ReadBytes(DataType.DataBlock, 1, 0, 1156);
                            uint[] result1 = DWord.ToArray(byte1);
                            byte[] byte2 = plc.ReadBytes(DataType.DataBlock, 2, 0, 84);
                            uint[] result2 = DWord.ToArray(byte2);

                            for (int i = 1; i < 289; i++)
                            {
                                var totalCountList = new List<long>();
                                string newId = "10714831" + i;

                                totalCountList = new List<long>() { (long)result1[i] };

                                ReadMachineData data = new ReadMachineData
                                {
                                    MachineID = Convert.ToInt64(newId),
                                    TotalCount = totalCountList,
                                    Break = false,
                                    Sched = false,
                                };

                                ts.Add(data);
                            }

                            for (int i = 1; i < 21; i++)
                            {
                                var totalCountList = new List<long>();
                                string newId = "10714832" + i;

                                totalCountList = new List<long>() { (long)result1[i] };

                                ReadMachineData data = new ReadMachineData
                                {
                                    MachineID = Convert.ToInt64(newId),
                                    TotalCount = totalCountList,
                                    Break = false,
                                    Sched = false,
                                };

                                ts.Add(data);
                            }
                        }
                        catch (Exception) { }
                        plc.Close();
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("10.7.148.3");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            #endregion
            #endregion


            #region Send Data
            try
            {
                var count = 0;
                foreach (var item in ts) //855
                {
                    if (item.MachineID > 0)
                    {
                        //if (item.MachineID == 1921688087144)
                        //if (item.MachineID == 136111135 || item.MachineID == 136111131)
                        //{
                        try
                        {
                            string uri = "";
                            if (item.TotalCount.Count() == 3)
                            {
                                string value1 = item.TotalCount[0].ToString();
                                string value2 = item.TotalCount[1].ToString();
                                string value3 = item.TotalCount[2].ToString();
                                //uri = @"https://localhost:44344/api/ProductionAPI/machinedatawithweight?api-version=1&" + "MachineID=" + item.MachineID.ToString() + "&TotalCountList[0]=" + value1 + "&TotalCountList[1]=" + value2 + "&TotalCountList[2]=" + value3 + "&Break=false&Sched=false";
                                uri =
                                    @"http://172.17.2.117:5005/api/ProductionAPI/machinedatawithweight?api-version=1&"
                                    + "MachineID="
                                    + item.MachineID.ToString()
                                    + "&TotalCountList[0]="
                                    + value1
                                    + "&TotalCountList[1]="
                                    + value2
                                    + "&TotalCountList[2]="
                                    + value3
                                    + "&Break=false&Sched=false";
                            }
                            else if (item.TotalCount.Count() == 2)
                            {
                                string value1 = item.TotalCount[0].ToString();
                                string value2 = item.TotalCount[1].ToString();
                                //uri = @"https://localhost:44344/api/ProductionAPI/machinedatawithruntime?api-version=1&" + "MachineID=" + item.MachineID.ToString() + "&TotalCountList[0]=" + value1 + "&TotalCountList[1]=" + value2 + "&Break=false&Sched=false";
                                uri =
                                    @"http://172.17.2.117:5005/api/ProductionAPI/machinedatawithruntime_test?api-version=1&"
                                    + "MachineID="
                                    + item.MachineID.ToString()
                                    + "&TotalCountList[0]="
                                    + value1
                                    + "&TotalCountList[1]="
                                    + value2
                                    + "&Break=false&Sched=false";
                            }
                            else
                            {
                                //uri = @"https://localhost:44344/api/ProductionAPI/machinedata?api-version=1&" + "MachineID=" + item.MachineID.ToString() + "&TotalCountList[0]=" + item.TotalCount[0].ToString() + "&Break=false&Sched=false";
                                uri =
                                    @"http://172.17.2.117:5005/api/ProductionAPI/machinedata_test?api-version=1&"
                                    + "MachineID="
                                    + item.MachineID.ToString()
                                    + "&TotalCountList[0]="
                                    + item.TotalCount[0].ToString()
                                    + "&Break=false&Sched=false";
                            }
                            var client = new RestClient(uri);
                            client.Timeout = -1;
                            var request = new RestRequest(Method.GET);
                            //var request = new RestRequest(Method.POST);
                            //IRestResponse response = await client.ExecuteAsync(request);
                            IRestResponse response = client.Execute(request);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        //}
                    }

                    count += 1;
                    int percent = ((count * 100) / ts.Count);
                    progress.Report(percent);

                    if (token.IsCancellationRequested)
                    {
                        token.ThrowIfCancellationRequested();
                    }

                    Thread.Sleep(1);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            #endregion

            return "ok";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string version = "Development Env";

            try
            {
                // If ClickOnce deployed, use ClickOnce version
                if (ApplicationDeployment.IsNetworkDeployed)
                {
                    version = ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString();
                }
            }
            catch
            {
                // keep "unknown"
            }

            label1.Text = $"{label1.Text}  — Version {version}";
            StartingMethod();
        }
    }
}
