namespace CSharpLib
{
    /// <summary>
    /// 泛型键值对
    /// </summary>
    /// <typeparam name="T">类型</typeparam>
    public class ValueItem<T>
    {
        // <summary>
        /// 键
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public T Value { get; set; }

        /// <summary>
        /// 尝试将<see cref="object"/>转化<see cref="ValueItem{T}"/>
        /// </summary>
        /// <param name="ObjectValue">object值</param>
        /// <returns>自定义泛型ValueItem</returns>
        public static ValueItem<T> Cast(object ObjectValue)
        {
            if (ObjectValue is ValueItem<T>)
            {
                return ObjectValue as ValueItem<T>;
            }
            else
            {
                return new ValueItem<T>()
                {
                    Key = ObjectValue + string.Empty,
                    Value = (T)ObjectValue
                };
            }
        }
    }
}
