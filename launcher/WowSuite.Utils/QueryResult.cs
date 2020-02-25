namespace WowSuite.Utils
{
    /// <summary>
    /// ѕредставл€ет из себ€ результат запроса (например, к удалЄнному серверу за какими-то данными)
    /// </summary>
    /// <typeparam name="TData">ƒанные, возвращаемые в случае успешного выполнени€ запроса</typeparam>
    public class QueryResult<TData>
    {
        public QueryResult(bool success, TData data)
        {
            Success = success;
            Data = data;
        }

        /// <summary>
        /// ƒанные, возвращаемые в случае успеха выполнени€ операции
        /// </summary>
        public TData Data { get; set; }

        /// <summary>
        /// ќпераци€ успешно выполнена
        /// </summary>
        public bool Success { get; set; }
    }
}