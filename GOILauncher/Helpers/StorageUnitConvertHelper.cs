namespace GOILauncher.Helpers
{
    public static class StorageUnitConvertHelper
    {
        public static string ByteTo(double bytes)
        {
            return bytes switch
            {
                0 => "0.00B",
                < 1024 => $"{bytes:0.00}B",
                < 1048576 => $"{bytes / 1024D:0.00}KB",
                _ => $"{bytes / 1048576D:0.00}MB"
            };
        }
    }
}
