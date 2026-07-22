namespace School.Utilities.Enums
{
    public static class BloodGroupExtensions
    {
        public static string ToDisplay(this BloodGroup bloodGroup)
        {
            return bloodGroup switch
            {
                BloodGroup.A_Positive => "A+",
                BloodGroup.A_Negative => "A-",
                BloodGroup.B_Positive => "B+",
                BloodGroup.B_Negative => "B-",
                BloodGroup.AB_Positive => "AB+",
                BloodGroup.AB_Negative => "AB-",
                BloodGroup.O_Positive => "O+",
                BloodGroup.O_Negative => "O-",
                _ => string.Empty
            };
        }
    }

}
