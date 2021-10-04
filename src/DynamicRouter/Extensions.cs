namespace DynamicRoutes

{
    public static class Extensions
    {
        public static string ToClean(this object somePath)
        {
            return somePath.ToString().ToLower().Trim().Trim('/');
        }

        //public static string ToClean<T>(this PathString somePath)
        //{
        //    return somePath.ToString().ToLower().Trim().Trim('/');
        //}

    }
}
