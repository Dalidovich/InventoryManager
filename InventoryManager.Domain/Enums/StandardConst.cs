namespace InventoryManager.Domain.Enums
{
    public static class StandardConst
    {
        public const string NameConnection = "NpgConnectionString";
        public const string CorsPolicyPort = "5173";
        public const string CorsPolicyName = $"AllowLocalhost{CorsPolicyPort}";
        public const double StartJWTTokenLifeTime = 15;
    }
}
