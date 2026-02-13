public enum YapFileLeafType : int
{
	Unkown = 0,
	Line = 1,
	Marker = 2,
	Command = 3,
}

public enum CommandType : int
{
	None = 0,
	Jump = 1,
	SetActor = 2,
}