namespace KJ1012.Core.Infrastructure
{
    public static class EngineContext
    {
        private static IEngine _currentEngine;
        /// <summary>
        /// 替换当前操作引擎
        /// </summary>
        /// <param name="engine"></param>
        public static void Replace(IEngine engine)
        {
            _currentEngine = engine;
        }
        /// <summary>
        /// 获取当前操作引擎
        /// </summary>
        public static IEngine Current = _currentEngine ?? (_currentEngine = new Engine());
    }
}
