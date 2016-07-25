﻿using PokemonGo.RocketAPI.Enums;
using PokemonGo.RocketAPI.GeneratedCode;
using PokemonGo.RocketAPI.Logic.Event;
using PokemonGo.RocketAPI.Logic.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonGo.RocketAPI.Console
{
    public class ConsoleEventListener
    {
        public void Listen(IEvent evt, Context ctx)
        {
            dynamic eve = evt;

            HandleEvent(eve, ctx);
        }

        public void HandleEvent(ProfileEvent evt, Context ctx)
        {
            Logger.Write($"Playing as {evt.Profile.Profile.Username ?? ""}");
        }

        public void HandleEvent(ErrorEvent evt, Context ctx)
        {
            Logger.Write(evt.ToString(), LogLevel.Error);
        }

        public void HandleEvent(NoticeEvent evt, Context ctx)
        {
            Logger.Write(evt.ToString());
        }

        public void HandleEvent(WarnEvent evt, Context ctx)
        {
            Logger.Write(evt.ToString(), LogLevel.Warning);
        }

        public void HandleEvent(UseLuckyEggEvent evt, Context ctx)
        {
            Logger.Write($"Used Lucky Egg, remaining: {evt.Count}", LogLevel.Egg);
        }

        public void HandleEvent(PokemonEvolveEvent evt, Context ctx)
        {
            Logger.Write(evt.Result == EvolvePokemonOut.Types.EvolvePokemonStatus.PokemonEvolvedSuccess
                        ? $"{evt.Id} successfully for {evt.Exp}xp"
                        : $"Failed {evt.Id}. EvolvePokemonOutProto.Result was {evt.Result}, stopping evolving {evt.Id}",
                    LogLevel.Evolve);
        }

        public void HandleEvent(TransferPokemonEvent evt, Context ctx)
        {
            Logger.Write($"{evt.Id} with {evt.Cp} ({evt.Perfection.ToString("0.00")} % perfect) CP (Best: {evt.BestCp} | ({evt.BestPerfection.ToString("0.00")} % perfect))", LogLevel.Transfer);
        }

        public void HandleEvent(ItemRecycledEvent evt, Context ctx)
        {
            Logger.Write($"{evt.Count}x {(ItemId)evt.Id}", LogLevel.Recycling);
        }

        public void HandleEvent(FortUsedEvent evt, Context ctx)
        {
            Logger.Write($"XP: {evt.Exp}, Gems: {evt.Gems}, Items: {evt.Items}", LogLevel.Pokestop);
        }

        public void HandleEvent(FortTargetEvent evt, Context ctx)
        {
            Logger.Write($"{evt.Name} in ({Math.Round(evt.Distance)}m)", LogLevel.Info, ConsoleColor.DarkRed);
        }

        public void HandleEvent(PokemonCaptureEvent evt, Context ctx)
        {
            Func<MiscEnums.Item, string> returnRealBallName = a =>
            {
                switch (a)
                {
                    case MiscEnums.Item.ITEM_POKE_BALL:
                        return "Poke";
                    case MiscEnums.Item.ITEM_GREAT_BALL:
                        return "Great";
                    case MiscEnums.Item.ITEM_ULTRA_BALL:
                        return "Ultra";
                    case MiscEnums.Item.ITEM_MASTER_BALL:
                        return "Master";
                    default:
                        return "Unknown";
                }
            };

            var catchStatus = evt.Attempt > 1
                        ? $"{evt.Status} Attempt #{evt.Attempt}"
                        : $"{evt.Status}";

            Logger.Write($"({catchStatus}) | {evt.Id} Lvl {evt.Level} ({evt.Cp}/{evt.MaxCp} CP) ({evt.Perfection.ToString("0.00")}% perfect) | Chance: {evt.Probability}% | {Math.Round(evt.Distance)}m dist | with a {returnRealBallName(evt.Pokeball)}Ball.",
                LogLevel.Caught);
        }

        public void HandleEvent(NoPokeballEvent evt, Context ctx)
        {
            Logger.Write($"No Pokeballs - We missed a {evt.Id} with CP {evt.Cp}", LogLevel.Caught);
        }
    }
}
