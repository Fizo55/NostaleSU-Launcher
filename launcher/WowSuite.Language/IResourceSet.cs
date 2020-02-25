namespace WowSuite.Language
{
    /// <summary>
    /// Интерфейс для заполнения ресурсам соответствующим ключу.
    /// </summary>
    /// <typeparam name="TKey">Ключ, по которому будет сохраняться и получаться набор ресурсов тип <see cref="TSet"/>.</typeparam>
    /// <typeparam name="TSet">Тип набора ресурсов.</typeparam>
    public interface IResourceSet<in TKey, TSet> 
        where TKey : struct
    {
        TSet Get(TKey key);

        void Add(TKey key, TSet set);

        bool Validate();
    }
}