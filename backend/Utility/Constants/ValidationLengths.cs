namespace Utility.Constants;
public static class ValidationLengths
{
    // General constraints
    public const int MaxLengthDefault = 255;

    // Specific field constraints
    public static class Fields
    {
        public const int Name = 50;
        public const int LongName = 50;
        public const int Content = 1000;
        public const int ShortDescription = 200;
        public const int Address = 100;
        public const int Phone = 15;
        public const int OperatingHours = 30;
        public const int Characteristic = 20;
    }
}
