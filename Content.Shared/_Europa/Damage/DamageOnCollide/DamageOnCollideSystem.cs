// SPDX-FileCopyrightText: 2025 Aiden <28298836+Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Fishbait <Fishbait@git.ml>
// SPDX-FileCopyrightText: 2025 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 2025 fishbait <gnesse@gmail.com>
// SPDX-FileCopyrightText: 2025 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Shared.Damage;
using Content.Shared.Whitelist;
using Robust.Shared.Physics.Events;
using Content.Shared.Damage;

namespace Content.Shared._Europa.Damage.DamageOnCollide;

public sealed class DamageOnCollideSystem : EntitySystem
{
    [Dependency] private readonly EntityWhitelistSystem _whitelistSystem = default!;
    [Dependency] private readonly DamageableSystem _damageableSystem = default!;


    /// <inheritdoc/>
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<DamageOnCollideComponent, StartCollideEvent>(OnStartCollide);
    }

    private void OnStartCollide(EntityUid uid, DamageOnCollideComponent component, ref StartCollideEvent args)
    {
        var currentVelocity = args.OurBody.LinearVelocity.Length();
        if (currentVelocity < component.RequiredVelocity)
            return;

        if (component.BlacklistedEntities != null && _whitelistSystem.IsValid(component.BlacklistedEntities, args.OtherEntity))
            return;
        if (!TryComp<DamageableComponent>(args.OtherEntity, out var otherDamageable))
            return;

        for (var i = 0; i < component.DamageCount; i++)
        {
            _damageableSystem.TryChangeDamage(args.OtherEntity, component.Damage, true, origin: uid);
        }
    }

}
