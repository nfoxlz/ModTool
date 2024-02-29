namespace ModTool.Models
{
    internal enum Function : byte
    {
        None,
        ReadCoils,
        ReadInputs,
        ReadHoldingRegisters,
        ReadInputRegisters,
        WriteSingleCoil,
        WriteSingleRegister,
        WriteMultipleCoils,
        WriteMultipleRegisters,
        ReadWriteMultipleRegisters,
    }
}
