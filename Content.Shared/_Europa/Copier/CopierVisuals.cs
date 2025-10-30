using Robust.Shared.Serialization;

namespace Content.Shared._Europa.Copier;

[Serializable, NetSerializable]
public enum CopierVisuals : byte
{
    VisualState,
}

[Serializable, NetSerializable]
public enum CopierVisualState : byte
{
    Normal,
    Inserting,
    Printing
}
