using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;
using Robust.Shared.Utility;

namespace Content.Shared._Europa.Copier;

[Serializable, NetSerializable]
[Prototype("documentForm")]
public sealed partial class DocumentFormPrototype : IPrototype
{
    [IdDataField]
    public string ID { get; private set; } = default!;

    [DataField("category")]
    public string Category { get; private set; } = default!;

    [DataField("name", required: true)]
    public string Name = default!;

    [DataField("document", required: true)]
    public ResPath Document = default!;
}
