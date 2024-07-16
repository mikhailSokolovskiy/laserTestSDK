using System;
using System.Collections.Generic;
using System.Reactive;
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

    
    private DllInvoke m_hMarkDll;         //markdll对象
    
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
        LoadPictureCommand = ReactiveCommand.Create(() =>
        {
            LoadImage();
        });

        InsertTextCommand = ReactiveCommand.Create(() =>
        {

        });

        RedLightCommand = ReactiveCommand.Create(() =>
        {

        });

        MarkingCommand = ReactiveCommand.Create(() =>
        {

        });

        StopMarkingCommand = ReactiveCommand.Create(() =>
        {

        });

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
        if (files.Count >= 1)
        {
            var file = files[0];
            await using var stream = await file.OpenReadAsync();
            Image = new Bitmap(stream);
        }

        if (m_hMarkDll.hLib != IntPtr.Zero)
        {
            BSL_LoadDataFile func = (BSL_LoadDataFile)m_hMarkDll.GetFunctionAddress("LoadDataFile", typeof(BSL_LoadDataFile));
            if (func != null)
            {
                BslErrCode iRes = func("empty.orzx");
                if (iRes == BslErrCode.BSL_ERR_SUCCESS)
                {
                    Console.WriteLine("load empty file");
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
                    .GetMessageBoxStandard("Ошибка", "Драйвер не установлен", ButtonEnum.Ok, Icon.Error, WindowStartupLocation.Manual);
                
                box.ShowAsync();
            }
        }
    }
    
    private void GetDeviceList()
    {
        DevList.Clear();
        if (m_hMarkDll.hLib != IntPtr.Zero)
        {
            BSL_GetAllDevices2 func = (BSL_GetAllDevices2)m_hMarkDll.GetFunctionAddress("GetAllDevices2", typeof(BSL_GetAllDevices2));
            
                int iDevCount = 0;
                STU_DEVID[] vDevId = new STU_DEVID[10];   //其实最多支持8张卡
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