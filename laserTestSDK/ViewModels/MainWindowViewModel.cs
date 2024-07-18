﻿using System;
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
    public ReactiveCommand<Unit, Unit> CfgCommand { get; set; }
    public ReactiveCommand<Unit, Unit> ClearAllCommand { get; set; }


    private DllInvoke m_hMarkDll; //markdll对象

    private List<string> _devList;
    private int _currentDevId;
    private int _markSpeed = 1000;
    private int _markPower = 70;
    private string _gravingText = "";
    private string _posX = "0";
    private string _posY = "0";
    private string _fontSize = "5";
    private bool _fill;
    private Bitmap _image;
    private double _fillLineSpacing = 0.01;

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

    public string GravingText
    {
        get => _gravingText;
        set => this.RaiseAndSetIfChanged(ref _gravingText, value);
    }

    public string PosX
    {
        get => _posX;
        set => this.RaiseAndSetIfChanged(ref _posX, value);
    }

    public string PosY
    {
        get => _posY;
        set => this.RaiseAndSetIfChanged(ref _posY, value);
    }

    public string FontSize
    {
        get => _fontSize;
        set => this.RaiseAndSetIfChanged(ref _fontSize, value);
    }

    public bool Fill
    {
        get => _fill;
        set => this.RaiseAndSetIfChanged(ref _fill, value);
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

    public double FillLineSpacing
    {
        get => _fillLineSpacing;
        set => this.RaiseAndSetIfChanged(ref _fillLineSpacing, value);
    }

    string strFileName = "empty.orzx";

    public MainWindowViewModel()
    {
        DevList = new List<string>();
        CheckSdk();
        GetDeviceList();
        LoadImage();
        LoadPictureCommand = ReactiveCommand.Create(() => { LoadImage(); });

        InsertTextCommand = ReactiveCommand.Create(() =>
        {
            if (m_hMarkDll.hLib != IntPtr.Zero)
            {
                BSL_AddTextToFileEx func =
                    (BSL_AddTextToFileEx)m_hMarkDll.GetFunctionAddress("AddTextToFileEx", typeof(BSL_AddTextToFileEx));
                BSL_SetFillParam func_setFillParam =
                    (BSL_SetFillParam)m_hMarkDll.GetFunctionAddress("SetFillParam", typeof(BSL_SetFillParam));
                // *******参数设置********
                BSL_FillPara para = new BSL_FillPara();
                para.init();
                //使能
                para.m_arrPar[0].m_bEnable = true;
                //弓形填充
                para.m_arrPar[0].m_nFillType = BSL_FILLTYPE.BSL_FT_SINGLE_LINE;
                //取异或集
                para.m_arrPar[0].m_nExecuteType = 0;
                //整体计算,此时m_nExecuteType才有效
                para.m_arrPar[0].m_bWholeConsider = true;
                //绕边走一次 始终在填充线后标刻 , FALSE=不绕边走
                para.m_arrPar[0].m_bAlongBorder = false;
                // 交叉填充
                para.m_arrPar[0].m_bCrossFill = true;
                //先打填充线,后打外部轮廓
                para.m_bOutLineFirst = false;
                //填充线间距
                para.m_arrPar[0].m_fLineSpacing = FillLineSpacing;
                // *******参数设置********
                
                
                if (func_setFillParam != null)
                {
                    func_setFillParam(ref para);
                }

                if (func != null)
                {
                    String szEntName = ("Вставленный текст");
                    BslErrCode iRes = func(strFileName, GravingText, szEntName, Convert.ToDouble(PosX),
                        Convert.ToDouble(PosY), 0, 0, 0, 0, Fill, Convert.ToDouble(FontSize), "Arial");
                    if (iRes == BslErrCode.BSL_ERR_SUCCESS)
                    {
                        // ShowShapeList(strFileName);
                        // PreViewFile(strFileName);
                        Console.WriteLine("success add text");
                    }
                    else
                    {
                        Console.WriteLine("faild insert text");
                    }
                }
                else
                {
                    Console.WriteLine("Эта функция не была найдена в SDK");
                }
            }
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
        CfgCommand = ReactiveCommand.Create(() =>
        {
            if (m_hMarkDll.hLib != IntPtr.Zero)
            {
                BSL_DisplayDevCfgDlg func =
                    (BSL_DisplayDevCfgDlg)m_hMarkDll.GetFunctionAddress("DisplayDevCfgDlg",
                        typeof(BSL_DisplayDevCfgDlg));
                if (func != null)
                {
                    BslErrCode iRes = func(DevList[0]);
                }
            }
        });
        ClearAllCommand = ReactiveCommand.Create(() =>
        {
            if (m_hMarkDll.hLib != IntPtr.Zero)
            {
                BSL_DeleteAllEntityByName func =
                    (BSL_DeleteAllEntityByName)m_hMarkDll.GetFunctionAddress("DeleteAllEntityByName",
                        typeof(BSL_DeleteAllEntityByName));
                if (func != null)
                {
                    BslErrCode iRes = func(strFileName);
                    if (iRes == BslErrCode.BSL_ERR_SUCCESS)
                    {
                        // ShowShapeList(strFileName);
                        // PreViewFile(strFileName);
                        Console.WriteLine("clear success");
                    }
                    else
                    {
                        Console.WriteLine("Не удалось очистить поле");
                    }
                }
                else
                {
                    Console.WriteLine("Функция не найдена");
                }
            }
            else
            {
                Console.WriteLine("SDK not found");
            }
        });
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

        // var topLevel = TopLevel.GetTopLevel(new MainWindow());
        // var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        // {
        //     Title = "Открыть изображение",
        //     AllowMultiple = false,
        //     FileTypeFilter = new[] { Img },
        //     SuggestedStartLocation =
        //         await topLevel.StorageProvider.TryGetFolderFromPathAsync(
        //             Environment.GetFolderPath(Environment.SpecialFolder.Desktop))
        // });
        // if (files.Count >= 1 && m_hMarkDll.hLib != IntPtr.Zero)
        // {
        //     var file = files[0];
        //     await using var stream = await file.OpenReadAsync();
        //     Image = new Bitmap(stream);
        //     BSL_AddBmpToFile func = (BSL_AddBmpToFile)m_hMarkDll.GetFunctionAddress("AddBmpToFile", typeof(BSL_AddBmpToFile));
        //     if (func != null)
        //     {
        //         BslErrCode iRes = func(file.Name, strFileName, "Импорт изображения", 100, 100, 0, 0, 0, 0);
        //         if (iRes == BslErrCode.BSL_ERR_SUCCESS)
        //         {
        //             // ShowShapeList(strFileName);
        //             // PreViewFile(strFileName);
        //             Console.WriteLine("complete add picture to Preview file");
        //         }
        //     }
        // }
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
        ThreadStopDev();
        object[] param = (object[])obj;
        DllInvoke hMarkDll = (DllInvoke)param[0];
        String strDevId = param[1].ToString();
        //параметры маркировки (скорость мощность и тд)
        BSL_SetPenParam2 func_param =
            (BSL_SetPenParam2)m_hMarkDll.GetFunctionAddress("SetPenParam", typeof(BSL_SetPenParam2));
        if (func_param != null)
        {
            BslErrCode iRes = func_param(strFileName, 0, 1, MarkSpeed, MarkPower, 1, 30, 1,10, 0, 100, 50, 80, 4000, 500, 100,
                0.5, true, 0, 1);
            if (iRes == BslErrCode.BSL_ERR_SUCCESS)
            {
                Console.WriteLine("params are set");   
            }
        }
        
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