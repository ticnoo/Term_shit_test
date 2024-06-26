﻿using System.Diagnostics;
using System;
using System.ComponentModel;
using System.Xml.Linq;
using System.Diagnostics.Metrics;
using System.IO.Ports;
using appTest.Views;
using System.Collections.Generic;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Net.Mime.MediaTypeNames;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Linq;
using System.Globalization;

using Avalonia.Input;
using Avalonia.Threading;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using Avalonia.Media;
using Avalonia.Controls.ApplicationLifetimes;

using appTest.Core.toolsUI;
using appTest.Core.Settings;

namespace appTest.ViewModels;

public struct BufferLineCounter
{
    public string Text { get; set; }
    public int LineCounter { get; set; }
}

public class MainViewModel : INotifyPropertyChanged
{

    ////////////////// test

    private BufferLineCounter testNewBuffer = new BufferLineCounter
    {
        Text = "test text, so can i do html? <b>asd</b>, no you can not dog",
        LineCounter = 10
    };
    public BufferLineCounter TestNewBuffer
    {
        get { return testNewBuffer; }
        set
        {
            testNewBuffer = value;
            OnPropertyChanged(nameof(TestNewBuffer));
        }
    }

    ////////////////// 
    ////////////////// settings class

    private AzulaSettings Settings = new AzulaSettings();

    public void OpenSettingsWindow(object msg)
    {
        SettingsWindow settingWindow = new SettingsWindow()
        {
            DataContext = new SettingsViewModel()
        };
        settingWindow.Show();
    }

    ////////////////// com port connection

    private object isConnectedColor = Color.Parse("red");
    public object IsConnectedColor
    {
        get { return isConnectedColor; }
        set
        {
            isConnectedColor = value;
            OnPropertyChanged(nameof(IsConnectedColor));    
        }
    }

    private bool isConnected = false;
    public bool IsConnected
    {
        get { return isConnected; }
        set
        {
            isConnected = value;
            if (value)
            {
                IsConnectedColor = Color.Parse("green");
            }
            else
            {
                IsConnectedColor = Color.Parse("red");
            }
        }
    }

    SerialPort serialPort = new SerialPort(
                portName: "COM8",
                baudRate: 115200,
                parity: Parity.None,
                dataBits: 8,
                stopBits: StopBits.One
            );

    //////////////////
    ////////////////// Buttons

    public void buttonConnect(object msg)
    {
        if (!serialPort.IsOpen)
        {
            try
            {
                serialPort.Open();
                AddMainConsole("opened\n");
                IsConnected = true;
                serialPort.DataReceived += (sender, e) =>
                {
                    string data = serialPort.ReadExisting();
                    data = data.Replace("\0", "");
                    AddMainConsole(data);

                    //if (data.Contains("Message"))
                    //{
                    //    AddMessage(data);
                    //    // AddMainConsole(data);
                    //} 
                    //else
                    //{
                    //    AddMainConsole(data);
                    //    // AddHistoryConsole(data);
                    //}
                };
            }
            catch
            {
                AddMainConsole("error open\n");
            }
        }
        else
        {
            AddMainConsole("serial port is open\n");
        }
    }

    public void buttonDisconnect(object msg)
    {
        if (serialPort.IsOpen)
        {
            serialPort.Close();
            
            AddMainConsole("port closed\n");
            // MainConsoleBuffer += "port closed\n";
            IsConnected = false;
        } 
        else
        {
            AddMainConsole("port is not open\n");
            // MainConsoleBuffer += "port is not open\n";
        }
    }

    public void buttonClear(object msg)
    {
        MainConsoleBuffer = "";
        MessengerConsoleCollectionBuffer.Clear();
        HistoryMainConsoleCollectionBuffer.Clear();
    }

    public void buttonDown(object msg)
    {
        // buttonDownPressed = true;
        switch (MainTabs.SelectedIndex)
        {
            case 1:
                toolsUI.MoveScrollToEnd(MainConsoleScroll);
                break;
            case 2:
                toolsUI.MoveScrollToEnd(MessengerConsoleScroll);
                break;
            case 3:
                toolsUI.MoveScrollToEnd(HistoryMainConsoleScroll);
                break;
            default:
                return;
        }
    }

    //////////////////
    ////////////////// Tab

    private TabControl mainTabs;
    public TabControl MainTabs
    {
        get { return mainTabs; }
        set
        {
            if (mainTabs != value)
            {
                mainTabs = value;
                OnPropertyChanged(nameof(MainTabs));
            }
        }
    }

    ////////////////// 
    ////////////////// Main Console

    //private const int MainConsoleScrollPixelLock = 200;
    private string mainConsoleBuffer = "";
    public string MainConsoleBuffer
    {
        get { return mainConsoleBuffer; }
        set 
        {
            // double test = MainConsoleScroll.ScrollBarMaximum.Y;
            mainConsoleBuffer = value;
            OnPropertyChanged(nameof(MainConsoleBuffer));
            //MoveScrollToEnd(MainConsoleScroll, MainConsoleScrollPixelLock);
            //toolsUI.MoveScrollToEnd(MainConsoleScroll, test);
        }
    }

    private ScrollViewer mainConsoleScroll;
    public ScrollViewer MainConsoleScroll
    {
        get { return mainConsoleScroll; }
        set
        {
            if (mainConsoleScroll != value)
            {
                mainConsoleScroll = value;
                OnPropertyChanged(nameof(MainConsoleScroll));
            }
        }
    }

    private void AddMainConsole(string data)
    {
        //int LineCount = MainConsoleBuffer.Split('\n').Length;
        double prevScrollPosition = MainConsoleScroll.ScrollBarMaximum.Y;
        string[] lines = MainConsoleBuffer.Split('\n');

        //if (MainConsoleBuffer.Length > MaxBufferLenght)
        if (lines.Length > Settings.MainConsole.MaxBufferLines)
        {
            //string dataToHistory = MainConsoleBuffer.Substring(0, HistoryBufferCut);
            //MainConsoleBuffer = MainConsoleBuffer.Remove(0, HistoryBufferCut);
            //string[] parts = MainConsoleBuffer.Split('\n', 2);
            //dataToHistory += parts[0];
            //MainConsoleBuffer = MainConsoleBuffer.Substring(parts[0].Length + 1);
            //MainConsoleBuffer += data;
            //MainConsoleHistoryAdd(dataToHistory);

            int totalCutLength = lines.Take(Settings.MainConsole.HistoryBufferLinesCutCount).Sum(s => s.Length);
            string dataToHistory = MainConsoleBuffer.Substring(0, totalCutLength + Settings.MainConsole.HistoryBufferLinesCutCount);
            MainConsoleBuffer = MainConsoleBuffer.Substring(totalCutLength + Settings.MainConsole.HistoryBufferLinesCutCount);
            MainConsoleBuffer += data;
            // MainConsoleHistoryAdd(dataToHistory);
            AddHistoryConsole(dataToHistory);
        }
        else
        {         
            MainConsoleBuffer += data;
        }
        toolsUI.MoveScrollToEnd(MainConsoleScroll, prevScrollPosition);
    }

    ////////////////// 
    ////////////////// Messenger

    public struct Message
    {
        public string Text { get; set; }
        public DateTime Time { get; set; }
    }

    private ObservableCollection<Message> messengerConsoleCollectionBuffer = new ObservableCollection<Message>();
    public ObservableCollection<Message> MessengerConsoleCollectionBuffer
    {
        get { return messengerConsoleCollectionBuffer; }
        set
        {
            messengerConsoleCollectionBuffer = value;
            OnPropertyChanged(nameof(MessengerConsoleCollectionBuffer));
        }
    }

    private ScrollViewer messengerConsoleScroll;
    public ScrollViewer MessengerConsoleScroll
    {
        get { return messengerConsoleScroll; }
        set
        {
            if (messengerConsoleScroll != value)
            {
                messengerConsoleScroll = value;
                OnPropertyChanged(nameof(MessengerConsoleScroll));
            }
        }
    }

    public void AddMessage(string text)
    {
        var message = new Message
        {
            Text = text,
            Time = DateTime.Now
        };
        MessengerConsoleCollectionBuffer.Add(message);
        toolsUI.MoveScrollToEnd(MessengerConsoleScroll, 100);
        //Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
        //{
        //    double current_offset = MessengerConsoleScroll.Offset.Y;
        //    double current_extent = MessengerConsoleScroll.ScrollBarMaximum.Y - 100;
        //    if (buttonDownPressed)
        //    {
        //        current_extent = 0;
        //        buttonDownPressed = false;
        //    }
        //    if (current_offset > current_extent)
        //    {
        //        MessengerConsoleScroll?.ScrollToEnd();
        //    }
        //});
    }

    //////////////////
    ////////////////// History console

    private ObservableCollection<string> historyMainConsoleCollectionBuffer = new ObservableCollection<string>() { "" };
    public ObservableCollection<string> HistoryMainConsoleCollectionBuffer
    {
        get { return historyMainConsoleCollectionBuffer; }
        set
        {
            historyMainConsoleCollectionBuffer = value;
            OnPropertyChanged(nameof(HistoryMainConsoleCollectionBuffer));
        }
    }

    private ScrollViewer historyMainConsoleScroll;
    public ScrollViewer HistoryMainConsoleScroll
    {
        get { return historyMainConsoleScroll; }
        set
        {
            if (historyMainConsoleScroll != value)
            {
                historyMainConsoleScroll = value;
                OnPropertyChanged(nameof(HistoryMainConsoleScroll));
            }
        }
    }

    private void AddHistoryConsole(string data)
    {
        double prevScrollPosition = HistoryMainConsoleScroll.ScrollBarMaximum.Y;
        int lastIndex = HistoryMainConsoleCollectionBuffer.Count;
        if (lastIndex == 0)
        {
            HistoryMainConsoleCollectionBuffer.Add(data);
            return;
        }

        lastIndex--;
        // int lastIndex = HistoryMainConsoleCollectionBuffer.Count - 1;
        int lineCount = HistoryMainConsoleCollectionBuffer[lastIndex].Split('\n').Length;

        if (lineCount > ((Settings.MainConsole.HistoryBufferLinesCutCount *3)-1) && data.Contains('\n')) 
        {
            string[] parts = data.Split('\n', 2);
            HistoryMainConsoleCollectionBuffer[lastIndex] += parts[0]; // + "\nend this block -----------------------------";
            // HistoryMainConsoleCollectionBuffer.Add("starting new block\n");
            // HistoryMainConsoleCollectionBuffer[lastIndex+1] += parts[1];
            HistoryMainConsoleCollectionBuffer.Add(parts[1]);
        }
        else
        {
            HistoryMainConsoleCollectionBuffer[lastIndex] += data;
        }
        toolsUI.MoveScrollToEnd(HistoryMainConsoleScroll, prevScrollPosition);
    }

    //////////////////
    ////////////////// Send button

    // private const string nReplacer = "&&";
    // private const string rReplacer = "";

    private string sendButtonBuffer { get; set; } = "";
    public string SendButtonBuffer
    {
        get { return sendButtonBuffer; }
        set
        {
            // sendButtonBuffer = value.Replace('\n', ' ');
            sendButtonBuffer = value;
            sendButtonBuffer = sendButtonBuffer.Replace("\n", Settings.nReplacer).Replace("\r", Settings.rReplacer);
            OnPropertyChanged(nameof(SendButtonBuffer));
        }
    }

    public void buttonSend(object msg)
    {
        if ("" != SendButtonBuffer)
        {
            AddMainConsole("---> " + SendButtonBuffer + '\n');
            // MainConsoleBuffer += "---> " + SendButtonBuffer + '\n';
            SendButtonBuffer = "";
        } 
        //else
        //{
        //    MainConsoleBuffer += "test\ntest\ntest\ntest\ntest\n";
        //    SendButtonBuffer = "";
        //}
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
