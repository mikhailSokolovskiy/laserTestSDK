using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;
using System.Threading;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using laserTestSDK.Views;
using MarkAPI;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using ReactiveUI;

namespace laserTestSDK.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public ReactiveCommand<Unit, Unit> RedLightCommand { get; set; }
    public ReactiveCommand<Unit, Unit> MarkingCommand { get; set; }
    public ReactiveCommand<Unit, Unit> StopMarkingCommand { get; set; }
    public ReactiveCommand<Unit, Unit> InsertTextCommand { get; set; }
    public ReactiveCommand<Unit, Unit> LoadPictureCommand { get; set; }
    public ReactiveCommand<Unit, Unit> RefreshCommand { get; set; }


    private DllInvoke m_hMarkDll; //markdll对象

    private List<string> _devList;
    private int _currentDevId;
    private int _markSpeed = 1000;
    private int _markPower = 70;
    private string _textLines = "";
    private Bitmap _image;

    public int MarkSpeed
    {
        get => _markSpeed;
        set => this.RaiseAndSetIfChanged(ref _markSpeed, value);
    }

    public int MarkPower
    {
        get => _markPower;
        set => this.RaiseAndSetIfChanged(ref _markPower, value);
    }

    public string TextLines
    {
        get => _textLines;
        set => this.RaiseAndSetIfChanged(ref _textLines, value);
    }

    public Avalonia.Media.Imaging.Bitmap Image
    {
        get => _image;
        set => this.RaiseAndSetIfChanged(ref _image, value);
    }

    public List<string> DevList
    {
        get => _devList;
        set => this.RaiseAndSetIfChanged(ref _devList, value);
    }

    public int CurrentDevId
    {
        get => _currentDevId;
        set => this.RaiseAndSetIfChanged(ref _currentDevId, value);
    }

    public MainWindowViewModel()
    {
        DevList = new List<string>();
        CheckSdk();
        GetDeviceList();
        LoadPictureCommand = ReactiveCommand.Create(() => { LoadImage(); });

        InsertTextCommand = ReactiveCommand.Create(() =>
        {
            
        });

        RedLightCommand = ReactiveCommand.Create(() =>
        {
            if (CurrentDevId >= 0)
            {
                // if (!m_bStop)
                // {
                //     MessageBox.Show("Вам необходимо создать изображение");
                //     return;
                // }
                if (m_hMarkDll.hLib != IntPtr.Zero)
                {
                    // m_bStop = false;

                    //灰掉红光按钮，避免重复点击
                    // button19.Enabled = false;

                    object[] param = new object[3];
                    param[0] = m_hMarkDll;
                    param[1] = DevList[0];
                    Thread threadC = new Thread(ThreadRedLight);
                    threadC.Start(param);
                }
            }
        });

        MarkingCommand = ReactiveCommand.Create(() =>
        {
            if (CurrentDevId >= 0)
            {
                if (m_hMarkDll.hLib != IntPtr.Zero)
                {
                    // if (!m_bStop)
                    // {
                    //     OnBnClickedBtnStop(this, e);
                    //     Thread.Sleep(500);
                    // }
                    // m_bStop = false;

                    //动态偏移、旋转  tbOffsetX
                    BSL_SetOffsetValues func =
                        (BSL_SetOffsetValues)m_hMarkDll.GetFunctionAddress("SetOffsetValues",
                            typeof(BSL_SetOffsetValues));
                    if (func != null)
                    {
                        //double dCenterX = Convert.ToDouble(tbCenterX.Text);
                        //double dCenterY = Convert.ToDouble(tbCenterY.Text);
                        //double dOffsetX = Convert.ToDouble(tbOffsetX.Text);
                        //double dOffsetY = Convert.ToDouble(tbOffsetY.Text);
                        //double dAngle = Convert.ToDouble(tbRotAngle.Text);
                        func(0.0, 0.0, 0.0, 0.0, 0.0);
                    }

                    //灰掉标刻按钮，避免重复点击
                    // button17.Enabled = false;
                    // Threads = 1;
                    object[] param = new object[2];
                    param[0] = m_hMarkDll;
                    param[1] = DevList[0];
                    Thread threadC = new Thread(ThreadMarkCard);
                    threadC.Start(param);
                }
            }
        });

        StopMarkingCommand = ReactiveCommand.Create(ThreadStopDev);

        RefreshCommand = ReactiveCommand.Create(GetDeviceList);
    }


    //фильтр для изображений
    private static FilePickerFileType Img { get; } = new("All image")
    {
        Patterns = new[]
        {
            "*.jpg",
            "*.png"
        },
        MimeTypes = null,
        AppleUniformTypeIdentifiers = null,
    };

    private async void LoadImage()
    {
        string strFileName = "text.orzx";
        if (m_hMarkDll.hLib != IntPtr.Zero)
        {
            BSL_LoadDataFile func =
                (BSL_LoadDataFile)m_hMarkDll.GetFunctionAddress("LoadDataFile", typeof(BSL_LoadDataFile));
            {
                BslErrCode iRes = func(strFileName);
                if (iRes == BslErrCode.BSL_ERR_SUCCESS)
                {
                    Console.WriteLine("load empty file");
                }
            }
            BSL_AppendFileToDevice func1 =
                (BSL_AppendFileToDevice)m_hMarkDll.GetFunctionAddress("AppendFileToDevice",
                    typeof(BSL_AppendFileToDevice));
            {
                BslErrCode iRes = func1(strFileName, DevList[0]);
                if (iRes == BslErrCode.BSL_ERR_SUCCESS)
                {
                    Console.WriteLine("load file to device");
                }
            }
        }
        
        var topLevel = TopLevel.GetTopLevel(new MainWindow());
        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Открыть изображение",
            AllowMultiple = false,
            FileTypeFilter = new[] { Img },
            SuggestedStartLocation =
                await topLevel.StorageProvider.TryGetFolderFromPathAsync(
                    Environment.GetFolderPath(Environment.SpecialFolder.Desktop))
        });
        if (files.Count >= 1 && m_hMarkDll.hLib != IntPtr.Zero)
        {
            var file = files[0];
            await using var stream = await file.OpenReadAsync();
            Image = new Bitmap(stream);
            BSL_AddBmpToFile func = (BSL_AddBmpToFile)m_hMarkDll.GetFunctionAddress("AddBmpToFile", typeof(BSL_AddBmpToFile));
            if (func != null)
            {
                BslErrCode iRes = func(strFileName, file.Name, "Импорт изображения", 100, 100, 0, 0, 0, 0);
                if (iRes == BslErrCode.BSL_ERR_SUCCESS)
                {
                    // ShowShapeList(strFileName);
                    // PreViewFile(strFileName);
                    Console.WriteLine("complete add picture to Preview file");
                }
            }
        }
        
        
    }

    private void CheckSdk()
    {
        if (m_hMarkDll == null)
        {
            m_hMarkDll = new DllInvoke("MarkSDK.dll");
            if (m_hMarkDll.hLib == IntPtr.Zero)
            {
                var box = MessageBoxManager
                    .GetMessageBoxStandard("Ошибка", "Драйвер не установлен", ButtonEnum.Ok, Icon.Error,
                        WindowStartupLocation.Manual);

                box.ShowAsync();
            }
        }
    }

    private void GetDeviceList()
    {
        DevList.Clear();
        if (m_hMarkDll.hLib != IntPtr.Zero)
        {
            BSL_GetAllDevices2 func =
                (BSL_GetAllDevices2)m_hMarkDll.GetFunctionAddress("GetAllDevices2", typeof(BSL_GetAllDevices2));
            if (func != null)
            {
                int iDevCount = 0;
                STU_DEVID[] vDevId = new STU_DEVID[10]; //其实最多支持8张卡
                BslErrCode iRes = func(vDevId, ref iDevCount);
                if (iRes == BslErrCode.BSL_ERR_SUCCESS)
                {
                    int iCount = iDevCount;
                    for (int i = 0; i < iCount; i++)
                    {
                        string str = System.Text.Encoding.Default.GetString(vDevId[i].wszDevName).TrimEnd('\0');
                        DevList.Add(str);
                        Console.WriteLine(str);
                    }

                    if (iCount > 0)
                    {
                        CurrentDevId = 0;
                    }
                }
                else
                {
                    String str;
                    str = "operating fail errorcode = " + iRes;
                    Console.WriteLine(str);
                }

                vDevId = null;
                GC.Collect();
            }
        }
    }

    private void ThreadRedLight(object DevId)
    {
        object[] param = (object[])DevId;
        DllInvoke hMarkDll = (DllInvoke)param[0];
        String strDevId = param[1].ToString();
        BSL_RedLightMark func = (BSL_RedLightMark)hMarkDll.GetFunctionAddress("RedLightMark", typeof(BSL_RedLightMark));
        if (func != null)
        {
            BslErrCode iRes = func(strDevId, true);

            //让红光按钮可用
            // this.Invoke((EventHandler)delegate { this.button19.Enabled = true; });

            if (iRes != BslErrCode.BSL_ERR_SUCCESS)
            {
                Console.WriteLine("Error redlight");
            }
        }
    }

    private void ThreadStopDev()
    {
        if (CurrentDevId >= 0)
        {
            if (m_hMarkDll.hLib != IntPtr.Zero)
            {
                BSL_GetAllDevices2 func_GetAllDevices =
                    (BSL_GetAllDevices2)m_hMarkDll.GetFunctionAddress("GetAllDevices2", typeof(BSL_GetAllDevices2));
                if (func_GetAllDevices != null)
                {
                    int iDevCount = 0;
                    STU_DEVID[] vDevId = new STU_DEVID[10]; //其实最多支持8张卡
                    BslErrCode iRes = func_GetAllDevices(vDevId, ref iDevCount);
                    if (iRes == BslErrCode.BSL_ERR_SUCCESS)
                    {
                        if (iDevCount > 0)
                        {
                            BSL_StopMark func_StopMark =
                                (BSL_StopMark)m_hMarkDll.GetFunctionAddress("StopMark", typeof(BSL_StopMark));
                            if (func_StopMark != null)
                            {
                                for (int i = 0; i < iDevCount; i++)
                                {
                                    iRes = func_StopMark((vDevId[i]).ToString());
                                }

                                if (iRes == BslErrCode.BSL_ERR_SUCCESS)
                                {
                                    // m_bStop = true;
                                    //MessageBox.Show("stopmark OK");
                                }
                                else
                                {
                                    Console.WriteLine("Не удалось остановить выполнение");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Функция остановки не выполнена");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Нет подключенных устройств");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Не удалось получить подключенные устройства");
                    }
                }
                else
                {
                    Console.WriteLine("Не удалось вызвать функцию получения списка устройств");
                }
            }
            else
            {
                Console.WriteLine("Драйвер не установлен");
            }
        }
        else
        {
            Console.WriteLine("Пожалуйста, сначала выберите устройство");
        }
    }

    private void ThreadMarkCard(object? obj)
    {
        object[] param = (object[])obj;
        DllInvoke hMarkDll = (DllInvoke)param[0];
        String strDevId = param[1].ToString();

        DateTime beforDT = System.DateTime.Now;
        BSL_MarkByDeviceId func =
            (BSL_MarkByDeviceId)m_hMarkDll.GetFunctionAddress("MarkByDeviceId", typeof(BSL_MarkByDeviceId));
        if (func != null)
        {
            StringBuilder ssID = new StringBuilder(strDevId);
            BslErrCode iRes = func(strDevId);

            //让标刻按钮可用
            // this.Invoke((EventHandler)delegate { this.button17.Enabled = true; this.m_bStop = true; });

            if (iRes == BslErrCode.BSL_ERR_SUCCESS)
            {
                DateTime afterDT = System.DateTime.Now;
                TimeSpan ts = afterDT.Subtract(beforDT);
                Console.WriteLine("success Общее время выполнения {0}ms", ts.TotalMilliseconds);
            }
            else
            {
                Console.WriteLine("Error");
            }
        }
    }
}