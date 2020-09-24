using System;
using System.Collections.Generic;
using System.Reflection;

namespace KJ1012.Core.Infrastructure
{

    /// <summary>
    /// 通过接口查找具体实现的类
    /// </summary>
    public interface ITypeFinder
    {
        /// <summary>
        /// 更新类型Type查找指定的类
        /// </summary>
        /// <typeparam name="T">要查找的类</typeparam>
        /// <param name="onlyConcreteClasses">是否只查找具体的实现类</param>
        /// <returns>Result</returns>
        IEnumerable<Type> FindClassesOfType<T>(bool onlyConcreteClasses = true);

        /// <summary>
        /// 更新类型Type查找指定的类
        /// </summary>
        /// <param name="assignTypeFrom">查找类的类型</param>
        /// <param name="onlyConcreteClasses">是否只查找具体的实现类</param>
        /// <returns>Result</returns>
        /// <returns></returns>
        IEnumerable<Type> FindClassesOfType(Type assignTypeFrom, bool onlyConcreteClasses = true);

        /// <summary>
        /// 更新类型Type查找指定的类
        /// </summary>
        /// <typeparam name="T">要查找的类</typeparam>
        /// <param name="assemblies">从指定的程序集查找</param>
        /// <param name="onlyConcreteClasses">是否只查找具体的实现类</param>
        /// <returns>Result</returns>
        IEnumerable<Type> FindClassesOfType<T>(IEnumerable<Assembly> assemblies, bool onlyConcreteClasses = true);

        /// <summary>
        /// 更新类型Type查找指定的类
        /// </summary>
        /// <param name="assignTypeFrom">查找类的类型</param>
        /// <param name="assemblies">从指定的程序集查找</param>
        /// <param name="onlyConcreteClasses">是否只查找具体的实现类</param>
        /// <returns>Result</returns>
        IEnumerable<Type> FindClassesOfType(Type assignTypeFrom, IEnumerable<Assembly> assemblies, bool onlyConcreteClasses = true);

        /// <summary>
        /// 获取与当前操作有关的程序集
        /// </summary>
        /// <returns>程序集</returns>
        IList<Assembly> GetAssemblies();

    }
}
