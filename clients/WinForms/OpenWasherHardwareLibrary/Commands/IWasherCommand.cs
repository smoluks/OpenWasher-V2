namespace OpenWasherHardwareLibrary.Commands
{
    public interface IWasherCommand<TRESULT>
    {
        byte[] GetRequest();
        
        TRESULT ParceResponse(byte[] data);
    }

    public interface IWasherCommand
    {
        byte[] GetRequest();
    }
}
