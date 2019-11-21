namespace OpenWasherHardwareLibrary.Commands
{
    internal interface IWasherCommand<TRESULT>
    {
        byte[] GetRequest();
        
        TRESULT ParceResponse(byte[] data);
    }

    internal interface IWasherCommand
    {
        byte[] GetRequest();
    }
}
