// SPDX-FileCopyrightText: 2025 Aiden <28298836+Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Fishbait <Fishbait@git.ml>
// SPDX-FileCopyrightText: 2025 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 2025 fishbait <gnesse@gmail.com>
// SPDX-FileCopyrightText: 2025 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Shared.Damage;
using Content.Shared.Whitelist;
using Robust.Shared.GameStates;

namespace Content.Shared._Europa.Damage.DamageOnCollide;

/// <summary>
/// When this component is added, we insert to a given container any entity we collide with
/// </summary>
[RegisterComponent, NetworkedComponent]
[Access(typeof(DamageOnCollideSystem))]
public sealed partial class DamageOnCollideComponent : Component
{
    /// <summary>
    /// Дамаг,
    /// </summary>

    [DataField("ignoreResistances")]
    [ViewVariables(VVAccess.ReadWrite)]
    public bool IgnoreResistances;

    [DataField("damage", required: true)]
    [ViewVariables(VVAccess.ReadWrite)]
    public DamageSpecifier Damage = default!;


    [DataField("damageCount")]
    [ViewVariables(VVAccess.ReadWrite)]
    public int DamageCount = 8;
    /// <summary>
    /// The minimum velocity we have to have to be able to insert something in the container.
    /// Represented in meters/tiles per second
    /// </summary>
    [DataField("requiredVelocity")]
    [ViewVariables(VVAccess.ReadWrite)]
    public float RequiredVelocity;

    /// <summary>
    /// Entities which we should never insert on collide
    /// </summary>
    [DataField("blacklistedEntities")]
    public EntityWhitelist? BlacklistedEntities;
}
