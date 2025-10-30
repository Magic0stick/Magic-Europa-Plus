using Content.Shared._Europa.Copier;
using JetBrains.Annotations;
using Robust.Shared.Prototypes;

namespace Content.Client._Europa.Copier;

[UsedImplicitly]
public sealed class CopierBoundUi : BoundUserInterface
{
    [ViewVariables]
    private CopierWindow? _window;

    public CopierBoundUi(EntityUid owner, Enum uiKey) : base(owner, uiKey)
    {
    }

    protected override void Open()
    {
        base.Open();

        _window = new CopierWindow(IoCManager.Resolve<IPrototypeManager>());
        _window.OpenCentered();

        _window.OnClose += Close;
        _window.FormButtonPressed += OnFormButtonPressed;
        _window.StartButtonPressed += OnStartButtonPressed;
        _window.StopButtonPressed += OnStopButtonPressed;
        _window.CopyModeButtonPressed += OnCopyModeButtonPressed;
        _window.PrintModeButtonPressed += OnPrintModeButtonPressed;
        _window.AmountChanged += OnAmountChanged;
    }

    private void OnFormButtonPressed(DocumentFormPrototype formPrototype)
    {
        SendMessage(new CopierSelectDocumentMessage(formPrototype));
    }

    private void OnAmountChanged(int amount)
    {
        SendMessage(new CopierSelectAmountMessage(amount));
    }

    private void OnStartButtonPressed()
    {
        SendMessage(new CopierPrintMessage());
    }

    private void OnStopButtonPressed()
    {
        SendMessage(new CopierStopMessage());
    }

    private void OnCopyModeButtonPressed()
    {
        SendMessage(new CopierSelectModeMessage(CopierMode.Copy));
    }

    private void OnPrintModeButtonPressed()
    {
        SendMessage(new CopierSelectModeMessage(CopierMode.Print));
    }

    protected override void UpdateState(BoundUserInterfaceState state)
    {
        base.UpdateState(state);

        if (_window == null || state is not CopierUiState cast)
            return;

        _window.UpdateState(cast);
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (!disposing || _window == null)
            return;

        _window.OnClose -= Close;
        _window.FormButtonPressed -= OnFormButtonPressed;
        _window.StartButtonPressed -= OnStartButtonPressed;
        _window.CopyModeButtonPressed -= OnCopyModeButtonPressed;
        _window.PrintModeButtonPressed -= OnPrintModeButtonPressed;

        _window?.Close();
        _window = null;
    }
}
