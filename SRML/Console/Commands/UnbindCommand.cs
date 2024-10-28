﻿using InControl;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SRML.Console.Commands
{
    class UnbindCommand : ConsoleCommand
    {
        public override string ID => "unbind";

        public override string Usage => "unbind [key]";

        public override string Description => "U    nbinds a key";

        public override bool Execute(string[] args)
        {
            if ((args?.Length ?? 0) == 0)
            {
                Console.Instance.LogError("Please supply a key!");
                return false;
            }
            Key key;
            try
            {
                key = (Key)Enum.Parse(typeof(Key), args[0], true);
            }
            catch
            {
                Console.Instance.LogError("Please supply valid key!");
                return false;
            }


            KeyBindManager.RemoveBinding(key);
            KeyBindManager.SaveBinds();

            return true;
        }

        public override List<string> GetAutoComplete(int argIndex, string argText)
        {
            if (argIndex == 0) return Enum.GetNames(typeof(Key)).ToList();
            return base.GetAutoComplete(argIndex, argText);
        }
    }
}
