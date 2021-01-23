namespace TestASPNet.Constants
{
    public static class Constants
    {
        public static class CommonTemplates
        {
            public const string Elapsed = "Elapsed - {0}";
            public const string Exception = "Message: {0}\n StackTrace: {1}";
        }

        public static class Defaults
        {
            public const string ConnectionString = "ConnectionString";
        }

        public static class QueryTemplates
        {
            public const string GetOrderInfo =
                "SELECT TOP 1 OrderID, CustomerID, TotalMoney, Ordered FROM dbo.Orders where OrderCode='{0}'"; // get first element from table, and TOP 1 maybe will change on LIMIT 1 that depend on SQL provider
        }
    }
}