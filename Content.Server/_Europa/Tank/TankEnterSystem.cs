using Content.Shared._Europa.Tank;
using Content.Shared._Shitmed.Targeting;
using Content.Shared.Actions;
using Content.Shared.MedicalScanner;
using Content.Shared.Popups;
using Content.Shared.StatusEffectNew;
using Content.Shared.Coordinates;
using Content.Shared.Damage;
using Content.Shared.Damage.Prototypes;
using Content.Shared.Verbs;
using Robust.Shared.Containers;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;
using Robust.Shared.Serialization.Manager;
using Content.Shared.Administration.Logs;
using Content.Shared.Database;

namespace Content.Server._Europa.Tank;


/// <summary>
/// УУУУ, щит код, по идее делаю максимально унимверсально для захода в тушку
/// </summary>
public sealed class TankEnterSystem : EntitySystem
{
    /// <inheritdoc/>

    [Dependency] protected readonly SharedContainerSystem Container = default!;
    [Dependency] protected readonly SharedActionsSystem Actions = default!;
    [Dependency] private readonly ISharedAdminLogManager _adminlogger = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<TankEnterComponent, GetVerbsEvent<AlternativeVerb>>(AddTankVerb);
        SubscribeLocalEvent<TankEnterComponent, ComponentInit>(OnComponentInit);
        SubscribeLocalEvent(TankEnterComponent, TankTakeControlEvent>(GetOutTank));
    }

    private void OnComponentInit(EntityUid uid, TankEnterComponent component, ComponentInit args)
    {
        component.SeatContainer = Container.EnsureContainer<Container>(uid, $"seat-container");
    }

    private void AddTankVerb(EntityUid uid, TankEnterComponent component, GetVerbsEvent<AlternativeVerb> args)
    {
        if (!args.CanAccess || !args.CanInteract)
            return;

        var verbName = Loc.GetString("tank-enter-verb");

        AlternativeVerb verb = new()
        {
            Act = () => EnterToTank(args.User, uid),
            Text = verbName,
            Priority = 1
        };
        args.Verbs.Add(verb);
    }


    public void EnterToTank(EntityUid uid, EntityUid target)
    {

        if (!TryComp<TankEnterComponent>(target, out var chairComp))
            return;


        // пытаемся запинуть
        try
        {
            Container.Insert(uid, chairComp.SeatContainer);

        }

        catch (Exception error)
        {
            // То же самое: сериализуй безопасные данные
            var safeData = new
            {
                ErrorMessage = error.Message ?? "Unknown error",
                StackTrace = error.StackTrace ?? "No stack trace available",
                ErrorType = error.GetType().Name
            };


            // Логируй напрямую через интерполяцию
            _adminlogger.Add(LogType.Action, LogImpact.Extreme, $"{ToPrettyString(uid)} не смог запинуть в {ToPrettyString(target)} потому что  {safeData.ErrorMessage}");
            return;
        }
        Actions.AddAction(uid, chairComp.ControlAction);
    }

    public void GetOutTank(EntityUid owner, EntityUid getoutuid)
    {
        if (!TryComp<TankEnterComponent>(getoutuid, out var component))
            return;

        if (Container.RemoveEntity(owner, getoutuid))
            Actions.RemoveAction(owner, component.ControlAction);

    }
}
