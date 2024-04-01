using System.Diagnostics;
using System;
using Avalonia.Threading;
using System.ComponentModel;
using System.Xml.Linq;
using System.Diagnostics.Metrics;
using Avalonia.Controls;

using System.IO.Ports;
using appTest.Views;

namespace appTest.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    SerialPort serialPort = new SerialPort(
                portName: "COM8",
                baudRate: 115200,
                parity: Parity.None,
                dataBits: 8,
                stopBits: StopBits.One
            );

    public void buttonConnect(object msg)
    {
        if (!serialPort.IsOpen)
        {
            try
            {
                serialPort.Open();
                ConsoleBuffer += "opened\n";
                serialPort.DataReceived += (sender, e) =>
                {
                    ConsoleBuffer += serialPort.ReadExisting();
                };
            }
            catch
            {
                ConsoleBuffer += "error open\n";
            }
        }
    }

    public void buttonDisconnect(object msg)
    {
        if (serialPort.IsOpen)
        {
            serialPort.Close();
            ConsoleBuffer += "port closed\n";
        } 
        else
        {
            ConsoleBuffer += "port is not open\n";
        }
    }

    public void buttonClear(object msg)
    {
        ConsoleBuffer = "";
    }

    bool buttonDownPressed = false;
    public void buttonDown(object msg)
    {
        buttonDownPressed = true;
    }

    private string _ConsoleBuffer = "start\nvery long test string >>--<< >>--<< >>--<< >>--<< >>--<< >>--<< >>--<< >>--<< >>--<<";
    public string ConsoleBuffer
    {
        get { return _ConsoleBuffer; }
        set 
        { 
            _ConsoleBuffer = value;
            OnPropertyChanged(nameof(ConsoleBuffer));
            Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
            {
                double current_offset = MainConcoleScroll.Offset.Y;
                double current_extent = MainConcoleScroll.ScrollBarMaximum.Y - 20;
                if (buttonDownPressed)
                {
                    current_extent = 0;
                    buttonDownPressed = false;
                }
                if (current_offset > current_extent)
                {
                    MainConcoleScroll?.ScrollToEnd();
                }
            });

            
        }
    }



    private ScrollViewer mainConcoleScroll;
    public ScrollViewer MainConcoleScroll
    {
        get { return mainConcoleScroll; }
        set
        {
            if (mainConcoleScroll != value)
            {
                mainConcoleScroll = value;
                OnPropertyChanged(nameof(MainConcoleScroll));
            }
        }
    }

    public void textChangedTest(object msg)
    {
        Celsius++;
    }

    private int _Celsius = 10;
    public int Celsius
    {
        get { return _Celsius; }
        set
        {
            _Celsius = value;
            _Fahrenheit = _Celsius * 2;
            OnPropertyChanged(nameof(Celsius));
            OnPropertyChanged(nameof(Fahrenheit));
        }
    }

    private int _Fahrenheit = 80;
    public int Fahrenheit
    {
        get { return _Fahrenheit; }
        set
        {
            _Fahrenheit = value;
            _Celsius = _Fahrenheit / 2;
            OnPropertyChanged(nameof(Fahrenheit));
            OnPropertyChanged(nameof(Celsius));
        }
    }

    public void PerformAction(object msg)
    {
        Debug.WriteLine("The action was called. Celsius {0}, Fahrenheit {1}", this.Celsius, this.Fahrenheit);
    }

    public void buttonPlus(object msg)
    {
        Fahrenheit++;
    }

    public void buttonMinus(object msg)
    {
        Fahrenheit--;
    }

    public string _sendString { get; set; } = "";
    public string sendString
    {
        get { return _sendString; }
        set
        {
            _sendString = value;
            OnPropertyChanged(nameof(sendString));
        }
    }
    public void buttonSend(object msg)
    {
        if ("" != sendString)
        {
            ConsoleBuffer += sendString + '\n';
            sendString = "";
        } 
        //else
        //{
        //    ConsoleBuffer += "test\ntest\ntest\ntest\ntest\n";
        //    sendString = "";
        //}
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
