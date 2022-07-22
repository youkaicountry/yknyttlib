using System.Collections.Generic;

namespace YKnyttLib.Parser
{
    public class CommandSet
    {
        public List<CommandDeclaration> Commands { get; }
        public Dictionary<string, CommandDeclaration> Name2Command { get; }
        public int Revision { get; private set; }

        public CommandSet()
        {
            Commands = new List<CommandDeclaration>();
            Name2Command = new Dictionary<string, CommandDeclaration>();
        }

        public void AddCommand(CommandDeclaration decl)
        {
            Commands.Add(decl);
            Name2Command.Add(decl.Name, decl);
            Revision++;
        }
    }
}
