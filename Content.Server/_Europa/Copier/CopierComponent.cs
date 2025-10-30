using Content.Shared._Europa.Copier;
using Content.Shared.Containers.ItemSlots;
using Content.Shared.Paper;
using Robust.Shared.Audio;
using Robust.Shared.Containers;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Server._Europa.Copier;

[RegisterComponent, Access( typeof(CopierSystem))]
public sealed partial class CopierComponent : Component
{
    /// <summary>
    /// Sprite to use when inserting an object.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField, AutoNetworkedField]
    public string InsertingState = "inserting";

    /// <summary>
    /// Contains the item to be copied, assumes it's paper...
    /// </summary>
    [DataField(required: true), ViewVariables(VVAccess.ReadOnly)]
    public ItemSlot PaperSlot = new();

    [ViewVariables(VVAccess.ReadOnly)]
    public readonly string PaperSlotId = "paper_slot";

    /// <summary>
    /// Contains the items to be used, assumes it's papers...
    /// </summary>
    [ViewVariables(VVAccess.ReadOnly)]
    public Container PaperTray = default!;

    [ViewVariables(VVAccess.ReadOnly)]
    public readonly string PaperTrayId = "entity_storage";

    /// <summary>
    /// Sound to play when copier printing new message
    /// </summary>
    [DataField]
    public SoundSpecifier PrintSound = new SoundPathSpecifier("/Audio/Machines/printer.ogg");

    /// <summary>
    /// Sound to play when copier copying new message
    /// </summary>
    [DataField]
    public SoundSpecifier CopySound = new SoundPathSpecifier("/Audio/Machines/printer.ogg");

    /// <summary>
    /// Print queue of the incoming message
    /// </summary>
    [ViewVariables]
    [DataField]
    public Queue<CopierPrintout> PrintingQueue { get; private set; } = new();

    /// <summary>
    /// Remaining time of printing animation
    /// </summary>
    [ViewVariables]
    [DataField]
    public float PrintingTimeRemaining;

    /// <summary>
    /// How long the printing animation will play
    /// </summary>
    [ViewVariables]
    [DataField]
    public float PrintingTime = 2f;

    /// <summary>
    /// Remaining time of inserting animation
    /// </summary>
    [DataField]
    public float InsertingTimeRemaining;

    /// <summary>
    /// How long the inserting animation will play
    /// </summary>
    [ViewVariables]
    public float InsertionTime = 0.9f;

    /// <summary>
    /// Selected document
    /// </summary>
    [DataField]
    public DocumentFormPrototype? SelectedDocument;

    /// <summary>
    /// Selected mode
    /// </summary>
    [ViewVariables(VVAccess.ReadOnly)]
    public CopierMode SelectedMode = CopierMode.Print;

    /// <summary>
    /// Amount of printing/copying papers per click
    /// </summary>
    [ViewVariables]
    public int Amount = 1;

    /// <summary>
    ///     The prototype ID to use if we can't get one from
    ///     the paper entity for whatever reason.
    /// </summary>
    [DataField]
    public EntProtoId FallbackPaperId = "Paper";

    /// <summary>
    /// Pause due to not enough paper
    /// </summary>
    [DataField]
    public bool Paused;
}

[DataDefinition]
public sealed partial class CopierPrintout
{
    [DataField(required: true)]
    public string Name { get; private set; } = default!;

    [DataField(required: true)]
    public string Content { get; private set; } = default!;

    [DataField(customTypeSerializer: typeof(PrototypeIdSerializer<EntityPrototype>), required: true)]
    public string PrototypeId { get; private set; } = default!;

    [DataField("stampState")]
    public string? StampState { get; private set; }

    [DataField("stampedBy")]
    public List<StampDisplayInfo> StampedBy { get; private set; } = new();

    private CopierPrintout()
    {
    }

    public CopierPrintout(string content, string name, string? prototypeId = null, string? stampState = null, List<StampDisplayInfo>? stampedBy = null)
    {
        Content = content;
        Name = name;
        PrototypeId = prototypeId ?? "";
        StampState = stampState;
        StampedBy = stampedBy ?? new List<StampDisplayInfo>();
    }
}
