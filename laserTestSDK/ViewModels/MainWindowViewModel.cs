using System.Reactive;
using ReactiveUI;

namespace laserTestSDK.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public ReactiveCommand<Unit, Unit> RedLightCommand { get; set; }
    public ReactiveCommand<Unit, Unit> MarkingCommand { get; set; }
    public ReactiveCommand<Unit, Unit> StopMarkingCommand { get; set; }
    public ReactiveCommand<Unit, Unit> InsertTextCommand { get; set; }
    public ReactiveCommand<Unit, Unit> LoadPictureCommand { get; set; }

    private int _markSpeed = 1000;
    private int _markPower = 70;
    private string _textLines = "";

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

    
    public MainWindowViewModel()
    {
        LoadPictureCommand = ReactiveCommand.Create(() =>
        {

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
    }
}