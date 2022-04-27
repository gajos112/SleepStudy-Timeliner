using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SleepStudy_Timeliner
{
    public partial class Form1 : Form
    {
        static string pathToSleepStudy;
        static string pathToSleepStudyScreenOn;
        static string pathToOutput;
        static string pathToETL = Path.GetTempPath() + "ETLParser.exe";

        FolderBrowserDialog openFolderBrowserDialog = new FolderBrowserDialog();

        public Form1()
        {
            InitializeComponent();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
        static int DropETL()
        {
            int result = 0;

            try
            {
                if (File.Exists(pathToETL))
                {
                    result = 2;
                }
                else
                {
                    File.WriteAllBytes(pathToETL, Properties.Resources.ETLParser);
                    if (File.Exists(pathToETL))
                    {
                        result = 1;
                    }
                }
            }
            catch (InvalidCastException e)
            {
                result = 0;
            }
            return result;
        }

        static void CheckDropETLResults(int result)
        {
            if (result == 1)
            {
                Console.WriteLine("ETLParser.exe was dropped sucesfully.");
            }
            else if (result == 2)
            {
                Console.WriteLine("ETLParser.exe already exixsts.");
            }

            else if (result == 0)
            {
                Console.WriteLine("ETLParser.exe was NOT dropped successfully.");
            }
        }
        private void buttonParse_Click(object sender, EventArgs e)
        {
            try
            {

                LogBox.Text = "";

                int result = DropETL();
                CheckDropETLResults(result);
                string argument;

                int j = 0;
                int i = 0;

                if (result != 0)
                {
                    string timeline = pathToOutput + @"\Timeline_SleepStudy.csv";

                    string outputTMP = pathToOutput + @"\Timeline_Parsed_ETL.csv";
                    string outputTMP2 = pathToOutput + @"\Timeline_ETL.sqlite";


                    Console.WriteLine("Path to the TIMELINE: " + timeline);
                    LogBox.AppendText("Path to the TIMELINE: " + timeline + "\r\n\r\n");


                    ///////////////////////////////////// FIRST PROCESS /////////////////////////////////////
                    try
                    {

                        LogBox.AppendText("Trying to parse 'StudySleep'\r\n");


                        argument = "-c Timeline -s " + pathToSleepStudy + " -o " + pathToOutput;


                        Console.WriteLine("");
                        Console.WriteLine("Executable: " + pathToETL);
                        Console.WriteLine("Argument: " + argument);


                        LogBox.AppendText("   Executable: " + pathToETL + "\r\n");
                        LogBox.AppendText("   Argument: " + argument + "\r\n");

                        Process Process1 = new Process();
                        Process1.StartInfo.FileName = pathToETL;
                        Process1.StartInfo.Arguments = argument;
                        //Process1.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
                        Process1.Start();
                        Process1.WaitForExit();

                        Console.WriteLine("");
                        Console.WriteLine("Parsing " + pathToSleepStudy);
                        LogBox.AppendText("   Parsing " + pathToSleepStudy + "\r\n");

                        using (TextFieldParser csvParser = new TextFieldParser(outputTMP))
                        {

                            csvParser.SetDelimiters(new string[] { "," });
                            csvParser.HasFieldsEnclosedInQuotes = true;
                            // Skip the row with the column names
                            csvParser.ReadLine();

                            while (!csvParser.EndOfData)
                            {
                                i++;

                                string[] fields = csvParser.ReadFields();
                                string Index = fields[0];
                                string Payload = fields[1];
                                string Timestamp = fields[2];
                                string EventName = fields[3];
                                string ProviderName = fields[4];

                                Payload = Payload.Replace(",", " ->");
                                Timestamp = Timestamp.Replace(" UTC", "");

                                if ((String.Equals(EventName, "AcDcStateRundown/Info")))
                                {
                                    string row = Timestamp + ",SleepStudy - AcDcStateRundown,,," + Payload;
                                    File.AppendAllText(timeline, row + Environment.NewLine);
                                }

                                if ((String.Equals(EventName, "AcDcStateChange/Info")))
                                {
                                    string row = Timestamp + ",SleepStudy - AcDcStateChange/Info,,," + Payload;
                                    File.AppendAllText(timeline, row + Environment.NewLine);
                                }

                                if ((String.Equals(EventName, "SessionDisplayOn/Info")))
                                {
                                    string row = Timestamp + ",SleepStudy - SessionDisplayOn/Info,,," + Payload;
                                    File.AppendAllText(timeline, row + Environment.NewLine);
                                }

                                if ((String.Equals(EventName, "SessionUnlocked/Info")))
                                {
                                    string row = Timestamp + ",SleepStudy - SessionUnlocked/Info,,," + Payload;
                                    File.AppendAllText(timeline, row + Environment.NewLine);
                                }

                                if ((String.Equals(EventName, "SessionLocked/Info")))
                                {
                                    string row = Timestamp + ",SleepStudy - SessionLocked/Info,,," + Payload;
                                    File.AppendAllText(timeline, row + Environment.NewLine);
                                }

                                if ((String.Equals(EventName, "SessionDisplayOff/Info")))
                                {
                                    string row = Timestamp + ",SleepStudy - SessionDisplayOff/Info,,," + Payload;
                                    File.AppendAllText(timeline, row + Environment.NewLine);
                                }

                                if ((String.Equals(EventName, "WakeDevicesPhaseStart/PhaseStart ")))
                                {
                                    string row = Timestamp + ",SleepStudy - WakeDevicesPhaseStart/PhaseStart ,,," + Payload;
                                    File.AppendAllText(timeline, row + Environment.NewLine);
                                }

                                if ((String.Equals(EventName, "WakeDevicesPhaseStop/PhaseStop")))
                                {
                                    string row = Timestamp + ",SleepStudy - WakeDevicesPhaseStop/PhaseStop,,," + Payload;
                                    File.AppendAllText(timeline, row + Environment.NewLine);
                                }

                                if ((String.Equals(EventName, "SetSystemState/Info")))
                                {
                                    string row = Timestamp + ",SleepStudy - SetSystemState/Info,,," + Payload;
                                    File.AppendAllText(timeline, row + Environment.NewLine);
                                }

                                if ((String.Equals(EventName, "SuspendServicesStart/Start")) || (String.Equals(EventName, "SuspendServicesStop/Stop")))
                                {
                                    string row = Timestamp + ",SleepStudy - SuspendServicesStart,,," + Payload;
                                    File.AppendAllText(timeline, row + Environment.NewLine);
                                }

                                if ((String.Equals(EventName, "SuspendAppsStart/Start")) || (String.Equals(EventName, "SuspendAppsStop/Stop")))
                                {
                                    string row = Timestamp + ",SleepStudy - SuspendAppsStart,,," + Payload;
                                    File.AppendAllText(timeline, row + Environment.NewLine);
                                }

                                if (String.Equals(EventName, "PreSleepNotification_V3/Info"))
                                {
                                    string row = Timestamp + ",SleepStudy - PreSleepNotification_V3/Info,,," + Payload;
                                    File.AppendAllText(timeline, row + Environment.NewLine);
                                }

                                if (String.Equals(EventName, "PreSleepCallbacksPhaseStop/PhaseStop"))
                                {
                                    string row = Timestamp + ",SleepStudy - PreSleepCallbacksPhaseStop/PhaseStop,,," + Payload;
                                    File.AppendAllText(timeline, row + Environment.NewLine);
                                }

                                if (String.Equals(EventName, "PreSleepCallbacksPhaseStart/PhaseStart"))
                                {
                                    string row = Timestamp + ",SleepStudy - PreSleepCallbacksPhaseStart/PhaseStart,,," + Payload;
                                    File.AppendAllText(timeline, row + Environment.NewLine);
                                }
                            }
                        }

                        Console.WriteLine("Parsed " + i + " entires");
                        LogBox.AppendText("   Parsed " + i + " entires\r\n");

                        File.Delete(outputTMP);
                        File.Delete(outputTMP2);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Something went wrong parsing with 'StudySleep'!\r\n\r\nRrror: " + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    ///////////////////////////////////// SECOND PROCESS ///////////////////////////////////// 

                    try
                    {

                        LogBox.AppendText("\r\nTrying to parse 'StudySleep\\ScreenOn'\r\n");

                        if (Directory.Exists(pathToSleepStudyScreenOn))
                        {

                            argument = " -c Timeline -s " + pathToSleepStudyScreenOn + " -o " + pathToOutput;

                            Console.WriteLine("");
                            Console.WriteLine("Executable: " + pathToETL);
                            Console.WriteLine("Argument: " + argument);

                            LogBox.AppendText("   Executable: " + pathToETL + "\r\n");
                            LogBox.AppendText("   Argument: " + argument + "\r\n");

                            Process Process2 = new Process();
                            Process2.StartInfo.FileName = pathToETL;
                            Process2.StartInfo.Arguments = argument;
                            //Process2.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
                            Process2.Start();
                            Process2.WaitForExit();

                            Console.WriteLine("");
                            Console.WriteLine("Parsing " + pathToSleepStudy + @"\ScreenOn");
                            LogBox.AppendText("   Parsing " + pathToSleepStudy + @"\ScreenOn" + "\r\n");

                            using (TextFieldParser csvParser = new TextFieldParser(outputTMP))
                            {

                                csvParser.SetDelimiters(new string[] { "," });
                                csvParser.HasFieldsEnclosedInQuotes = true;
                                // Skip the row with the column names
                                csvParser.ReadLine();

                                while (!csvParser.EndOfData)
                                {
                                    j++;

                                    string[] fields = csvParser.ReadFields();
                                    string Index = fields[0];
                                    string Payload = fields[1];
                                    string Timestamp = fields[2];
                                    string EventName = fields[3];
                                    string ProviderName = fields[4];

                                    Payload = Payload.Replace(",", " ->");
                                    Timestamp = Timestamp.Replace(" UTC", "");

                                    if ((String.Equals(EventName, "DisplayStateChanged/Info")))
                                    {
                                        string row = Timestamp + @",SleepStudy\ScreenOn - DisplayStateChanged,,," + Payload;
                                        File.AppendAllText(timeline, row + Environment.NewLine);
                                    }

                                    if ((String.Equals(EventName, "UpdateAcDcPowerSource/Info")))
                                    {
                                        string row = Timestamp + @",SleepStudy\ScreenOn - UpdateAcDcPowerSource,,," + Payload;
                                        File.AppendAllText(timeline, row + Environment.NewLine);
                                    }

                                    if ((String.Equals(EventName, "PowerSavingBrightnessChanged/Info")))
                                    {
                                        string row = Timestamp + @",SleepStudy\ScreenOn - PowerSavingBrightnessChanged,,," + Payload;
                                        File.AppendAllText(timeline, row + Environment.NewLine);
                                    }

                                    if ((String.Equals(EventName, "DisplayBrightnessAndTransitionsUpdated/Info")))
                                    {
                                        string row = Timestamp + @",SleepStudy\ScreenOn - DisplayBrightnessAndTransitionsUpdated,,," + Payload;
                                        File.AppendAllText(timeline, row + Environment.NewLine);
                                    }

                                    if ((String.Equals(EventName, "IlluminanceLuxHistogram/Info")))
                                    {
                                        string row = Timestamp + @",SleepStudy\ScreenOn - IlluminanceLuxHistogram,,," + Payload;
                                        File.AppendAllText(timeline, row + Environment.NewLine);
                                    }
                                }
                            }

                            Console.WriteLine("Parsed " + j + " entires");
                            LogBox.AppendText("   Parsed " + j + " entires\r\n");

                        }
                        else
                        {
                            LogBox.AppendText("   Location: " + pathToSleepStudy + @"\ScreenOn" + " -> was not found!\r\n");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(@"Something went wrong parsing with 'StudySleep\ScreenOn'!\r\n\r\nRrror: " + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    ///////////////////////////////////// CLEAINING /////////////////////////////////////
                    File.Delete(outputTMP);
                    File.Delete(outputTMP2);
                    MessageBox.Show("Timeline has been completed.", "SleepStudy - Timeliner",MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Console.WriteLine("");
                    int totalCount = i + j;

                    Console.WriteLine("In total parsed " + totalCount + " entries");

                    LogBox.AppendText("\r\nTimeline has been completed.\r\n");
                    LogBox.AppendText("In total parsed " + totalCount + " entries \r\n");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong!\r\n\r\nRrror: " + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFolderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBoxSource.Text = openFolderBrowserDialog.SelectedPath;
                pathToSleepStudy = textBoxSource.Text;
                pathToSleepStudyScreenOn = textBoxSource.Text + @"\ScreenOn";
            }
        }
        private void saveToToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFolderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBoxDestintion.Text = openFolderBrowserDialog.SelectedPath;
                pathToOutput = textBoxDestintion.Text;
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            {
                string message = "It is the fully free software created by Krzysztof Gajewski.\r\n\r\nIcons downloaded from https://www.kindpng.com";
                string title = "SRUM - Timeliner";
                MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
