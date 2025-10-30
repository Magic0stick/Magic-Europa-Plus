using Robust.Shared.Serialization;

namespace Content.Shared._Europa.Copier;

[Serializable, NetSerializable]
public enum CopierUiKey : byte
{
    Key
}

[Serializable, NetSerializable]
public sealed class CopierUiState : BoundUserInterfaceState
{
    public bool CanPrint { get; }
    public bool CanCopy { get; }
    public bool CanStop { get; }
    public DocumentFormPrototype? SelectedForm { get; }
    public CopierMode Mode { get; }
    public int Amount { get; }

    public CopierUiState(
        bool canPrint,
        bool canCopy,
        bool canStop,
        DocumentFormPrototype? selectedForm,
        CopierMode mode,
        int amount)
    {
        CanPrint = canPrint;
        CanCopy = canCopy;
        CanStop = canStop;
        SelectedForm = selectedForm;
        Mode = mode;
        Amount = amount;
    }
}

[Serializable, NetSerializable]
public sealed class CopierSelectDocumentMessage : BoundUserInterfaceMessage
{
    public readonly DocumentFormPrototype DocumentForm;

    public CopierSelectDocumentMessage(DocumentFormPrototype documentForm)
    {
        DocumentForm = documentForm;
    }
}

[Serializable, NetSerializable]
public sealed class CopierSelectAmountMessage : BoundUserInterfaceMessage
{
    public readonly int Amount;

    public CopierSelectAmountMessage(int amount)
    {
        Amount = amount;
    }
}

[Serializable, NetSerializable]
public sealed class CopierPrintMessage : BoundUserInterfaceMessage
{

}

[Serializable, NetSerializable]
public sealed class CopierStopMessage : BoundUserInterfaceMessage
{

}

[Serializable, NetSerializable]
public sealed class CopierSelectModeMessage : BoundUserInterfaceMessage
{
    public readonly CopierMode Mode;

    public CopierSelectModeMessage(CopierMode mode)
    {
        Mode = mode;
    }
}

public enum CopierMode
{
    Print,
    Copy
}
