namespace YKnyttLib.Parser
{
    public struct CommandArg
    {
        public enum Type
        {
			StringArg,
			FloatArg,
			CmdNameArg,
			IntArg,
			UIntArg,
			BoolArg,
			HexArg,
			FileArg,
			NoArg,
		}

        public string Name { get; }
		public Type ArgType { get; }
		public bool Optional { get; }

		public CommandArg(string name, Type type, bool optional = false)
        {
			Name = name;
			ArgType = type;
			Optional = optional;
        }
    }
}
