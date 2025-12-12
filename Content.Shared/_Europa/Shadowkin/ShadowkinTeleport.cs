using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Numerics;
using Content.Shared.Actions;
using Content.Shared.Charges.Systems;
using Content.Shared.Examine;
using Content.Shared.Hands.EntitySystems;
using Content.Shared.Interaction;
using Content.Shared.Movement.Pulling.Components;
using Content.Shared.Movement.Pulling.Systems;
using Content.Shared.Popups;
using Robust.Shared.GameObjects;
using Robust.Shared.IoC;
using Robust.Shared.Localization;
using Robust.Shared.Map;
using Robust.Shared.Map.Components;
using Robust.Shared.Physics.Components;
using Robust.Shared.Player;
using Robust.Shared.Timing;
using Robust.Shared.Utility;
using Robust.Shared.Log;

namespace Content.Shared._Europa.Shadowkin;

/// <summary>
/// This handles...
/// </summary>
public sealed class ShadowkinTeleportSystem : EntitySystem
{
    /// <inheritdoc/>
    [Dependency] private readonly ActionContainerSystem _actionContainer = default!;
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly SharedChargesSystem _sharedCharges = default!;
    [Dependency] private readonly SharedHandsSystem _hands = default!;
    [Dependency] private readonly ExamineSystemShared _examine = default!;
    [Dependency] private readonly SharedPopupSystem _popup = default!;
    [Dependency] private readonly PullingSystem _pullingSystem = default!;
    [Dependency] private readonly SharedTransformSystem _transform = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<ShadowkinAbilitiesComponent, ShadowkinTeleportEvent>(OnShadowkinTeleport);
    }

    private void OnShadowkinTeleport(Entity<ShadowkinAbilitiesComponent> ent, ref ShadowkinTeleportEvent args)
    {
        if (!_timing.IsFirstTimePredicted)
            return;

        var (uid, comp) = ent;
        var user = args.Performer;

        var origin = _transform.GetMapCoordinates(user);
        var target = _transform.ToMapCoordinates(args.Target);
        if (!_examine.InRangeUnOccluded(origin, target, SharedInteractionSystem.MaxRaycastRange, null))
        {
            // can only dash if the destination is visible on screen
            _popup.PopupClient(Loc.GetString("dash-ability-cant-see", ("item", uid)), user, user);
            return;
        }

        // Check if the user is BEING pulled, and escape if so
        if (TryComp<PullableComponent>(user, out var pull) && _pullingSystem.IsPulled(user, pull))
            _pullingSystem.TryStopPull(user, pull);

        // Check if the user is pulling anything, and drop it if so
        if (TryComp<PullerComponent>(user, out var puller) && TryComp<PullableComponent>(puller.Pulling, out var pullable))
            _pullingSystem.TryStopPull(puller.Pulling.Value, pullable);

        var xform = Transform(user);
        _transform.SetCoordinates(user, xform, args.Target);
        _transform.AttachToGridOrMap(user, xform);
        args.Handled = true;
    }
}
