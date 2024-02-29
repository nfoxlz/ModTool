namespace ModTool.Models
{
    internal sealed class ReadWriteSetting
    {
        public byte SlaveId { get; set; }

        public Function Function { get; set;}

        public ushort Address { get; set; }

        public ushort Quantity { get; set; } = 8;

        public double ScanRate { get; set; } = 500D;

        public int Timeout { get; set; } = 3000;
    }
}
