using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection;

namespace HelloData.FWCommon.AOP
{
    public class DynamicProxyGenerator
    {
        const string AssemblyName = "HelloData.FWCommon.AOP.DynamicAssembly";
        const string AssemblyFileName = "HelloData.FWCommon.AOP.DynamicAssembly.dll";
        const string ModuleName = "HelloData.FWCommon.AOP.DynamicModule";
        const string TypeNameFormat = "HelloData.FWCommon.AOP.Dynamic{0}Type";

        private Type _realProxyType;
        private Type _interfaceType;

        private AssemblyBuilder _assemblyBuilder;
        private ModuleBuilder _moduleBuilder;
        private TypeBuilder _typeBuilder;

        private FieldBuilder _realProxyField;

        public DynamicProxyGenerator(Type realProxyType, Type interfaceType)
        {
            _realProxyType = realProxyType;
            _interfaceType = interfaceType;
        }

        public Type GenerateType()
        {
            // 构造程序集
            BuildAssembly();
            // 构造模块
            BuildModule();
            // 构造类型
            BuildType();
            // 构造字段
            BuildField();
            // 构造函数
            BuildConstructor();
            // 构造方法
            BuildMethods();

            Type type = _typeBuilder.CreateType();
            // 将新建的类型保存在硬盘上（如果每次都动态生成，此步骤可省略）
            // _assemblyBuilder.Save(AssemblyFileName);
            return type;
        }

        #region Assembly & Module & Type

        void BuildAssembly()
        {
            // 程序集名字
            AssemblyName assemblyName = new AssemblyName(AssemblyName);

            // 在当前的AppDomain中构造程序集
            _assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName,
                AssemblyBuilderAccess.RunAndSave, System.AppDomain.CurrentDomain.BaseDirectory);
        }

        void BuildModule()
        {
            // 
            _moduleBuilder = _assemblyBuilder.DefineDynamicModule(ModuleName, AssemblyFileName);
        }

        void BuildType()
        {
            _typeBuilder = _moduleBuilder.DefineType(string.Format(TypeNameFormat, _realProxyType.Name),
                TypeAttributes.Public | TypeAttributes.Sealed);
            _typeBuilder.AddInterfaceImplementation(_interfaceType);
        }

        #endregion

        #region Coustructor

        void BuildConstructor()
        {
            ConstructorBuilder constructorBuilder = _typeBuilder.DefineConstructor(MethodAttributes.Public,
                CallingConventions.HasThis, null);
            ILGenerator generator = constructorBuilder.GetILGenerator();

            // _realProxy = new RealProxy();
            generator.Emit(OpCodes.Ldarg_0);
            ConstructorInfo defaultConstructorInfo = _realProxyType.GetConstructor(Type.EmptyTypes);
            generator.Emit(OpCodes.Newobj, defaultConstructorInfo);
            generator.Emit(OpCodes.Stfld, _realProxyField);

            generator.Emit(OpCodes.Ret);
        }

        #endregion

        #region Field

        void BuildField()
        {
            _realProxyField = _typeBuilder.DefineField("_realProxy", _realProxyType, FieldAttributes.Private);
            _realProxyField.SetConstant(null);
        }

        #endregion

        #region Method

        void BuildMethods()
        {
            MethodInfo[] methodInfos = _realProxyType.GetMethods(BindingFlags.Public | BindingFlags.Instance);
            foreach (MethodInfo methodInfo in methodInfos)
            {
                object[] aopAtts = _realProxyType.GetMethod(methodInfo.Name).GetCustomAttributes(typeof(BaseAttribute), true); ;
                if (aopAtts != null && aopAtts.Length > 0) //检测是否标注了AOP属性
                {
                    BuildMethod(methodInfo, false);
                }
                else
                {
                    BuildMethod(methodInfo, true);
                }

            }
        }

        private void BuildMethod(MethodInfo methodInfo, bool IsGloab)
        {
            string methodName = methodInfo.Name;
            ParameterInfo[] parameterInfos = methodInfo.GetParameters();
            Type returnType = methodInfo.ReturnType;

            MethodBuilder methodBuilder = null;

            methodBuilder = _typeBuilder.DefineMethod(methodName,
                                                      MethodAttributes.Public | MethodAttributes.Virtual,
                                                      returnType,
                                                      parameterInfos.Select(pi => pi.ParameterType).ToArray());

            var il = methodBuilder.GetILGenerator();
            Label castPerSuccess = il.DefineLabel();
            Label castArroundSuccess = il.DefineLabel();
            Label castAfterArroundSuccess = il.DefineLabel();
            Label castPostSuccess = il.DefineLabel();

            Label castExSuccess = il.DefineLabel();

            #region Decalre InvokeContext

            Type contextType = typeof(InvokeContext);
            var contextLocal = il.DeclareLocal(contextType);
            il.Emit(OpCodes.Newobj, contextType.GetConstructor(Type.EmptyTypes));
            il.Emit(OpCodes.Stloc, contextLocal);

            // set method name
            il.Emit(OpCodes.Ldloc, contextLocal);
            il.Emit(OpCodes.Ldstr, methodName);
            il.Emit(OpCodes.Call, contextType.GetMethod("SetMethod", BindingFlags.Public | BindingFlags.Instance));

            #endregion

            #region Decalre result

            LocalBuilder resultLocal = null;

            if (returnType != typeof(void))
            {
                resultLocal = il.DeclareLocal(returnType);
                if (returnType.IsValueType)
                {
                    il.Emit(OpCodes.Ldstr, returnType.FullName);
                    il.Emit(OpCodes.Call, typeof(Type).GetMethod("GetType", new Type[] { typeof(string) }));
                    il.Emit(OpCodes.Call, typeof(Activator).GetMethod("CreateInstance", new Type[] { typeof(Type) }));
                }
                else
                {
                    il.Emit(OpCodes.Ldnull);
                }

                il.Emit(OpCodes.Stloc, resultLocal);
            }

            #endregion

            #region Declare Exception

            var exceptionLocal = il.DeclareLocal(typeof(Exception));
            il.Emit(OpCodes.Ldnull);
            il.Emit(OpCodes.Stloc, exceptionLocal);

            #endregion

            #region Invoke PreInvoke

            #region Set parameter to InvkeContext

            for (int i = 1; i <= parameterInfos.Length; i++)
            {
                il.Emit(OpCodes.Ldloc, contextLocal);
                il.Emit(OpCodes.Ldarg, i);
                if (parameterInfos[i - 1].ParameterType.IsValueType)
                {
                    il.Emit(OpCodes.Box, parameterInfos[i - 1].ParameterType);
                }
                il.Emit(OpCodes.Call, contextType.GetMethod("SetParameter", BindingFlags.Public | BindingFlags.Instance));
            }

            #endregion
            //var classInfoLocal1 = _realProxyType.GetType();
            //ArrountAttribute preAspectLocal1 =
            //    (ArrountAttribute)Attribute.GetCustomAttribute(classInfoLocal1, typeof(ArrountAttribute));
            /*
             * C# 代码
             * MethodInfo classInfoLocal = _realProxyField.GetType().GetMethod("methodName");
             * ArrountAttribute preAspectLocal = 
             *      (ArrountAttribute)Attribute.GetCustomAttribute(classInfoLocal, typeof(ArrountAttribute))
             *      
             * if (preAspectLocal != null)
             * {
             *      preAspectLocal.Action(contextLocal);
             * }
             * 
             */

            var classInfoLocal = il.DeclareLocal(_realProxyField.GetType());
            LocalBuilder arrountAspectLocal = il.DeclareLocal(typeof(ArrountAttribute));
            if (IsGloab)
            {
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldfld, _realProxyField);
                il.Emit(OpCodes.Callvirt,
                        typeof(System.Object).GetMethod("GetType", BindingFlags.Public | BindingFlags.Instance));
                //il.Emit(OpCodes.Ldstr, methodName);
                //il.Emit(OpCodes.Callvirt,
                //        typeof(System.Type).GetMethod("GetMethod", new Type[] { typeof(string) }));
                il.Emit(OpCodes.Stloc, classInfoLocal);

                il.Emit(OpCodes.Ldloc, classInfoLocal);

            }
            else
            {
                classInfoLocal = il.DeclareLocal(typeof(MethodInfo));
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldfld, _realProxyField);
                il.Emit(OpCodes.Callvirt,
                        typeof(System.Object).GetMethod("GetType", BindingFlags.Public | BindingFlags.Instance));
                il.Emit(OpCodes.Ldstr, methodName);
                il.Emit(OpCodes.Callvirt,
                        typeof(System.Type).GetMethod("GetMethod", new Type[] { typeof(string) }));
                il.Emit(OpCodes.Stloc, classInfoLocal);

                il.Emit(OpCodes.Ldloc, classInfoLocal);

            }
            il.Emit(OpCodes.Ldtoken, typeof(ArrountAttribute));
            //generator.Emit(OpCodes.Ldloca, )
            il.Emit(OpCodes.Call,
                    typeof(System.Type).GetMethod("GetTypeFromHandle", new Type[] { typeof(System.RuntimeTypeHandle) }));
            il.Emit(OpCodes.Call,
                    typeof(System.Attribute).GetMethod("GetCustomAttribute",
                                                        new Type[]
                                                            {
                                                                typeof (System.Reflection.MemberInfo), typeof (System.Type)
                                                            }));
            il.Emit(OpCodes.Castclass, typeof(ArrountAttribute));
            il.Emit(OpCodes.Stloc, arrountAspectLocal);

            il.Emit(OpCodes.Ldloc, arrountAspectLocal);

            il.Emit(OpCodes.Ldnull);
            il.Emit(OpCodes.Ceq);
            il.Emit(OpCodes.Brtrue_S, castArroundSuccess);
            il.Emit(OpCodes.Ldloc, arrountAspectLocal);
            il.Emit(OpCodes.Ldloc, contextLocal);
            il.Emit(OpCodes.Callvirt,
                typeof(ArrountAttribute).GetMethod("BeginAction", new Type[] { typeof(InvokeContext) }));

            il.MarkLabel(castArroundSuccess);

            #endregion

            #region  pre
            var preAspectLocal = il.DeclareLocal(typeof(PostAspectAttribute));

            /*
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldfld, _realProxyField);
            generator.Emit(OpCodes.Callvirt, typeof(System.Object).GetMethod("GetType", BindingFlags.Public | BindingFlags.Instance));
            generator.Emit(OpCodes.Ldstr, methodName);
            generator.Emit(OpCodes.Callvirt,
                typeof(System.Type).GetMethod("GetMethod", new Type[] { typeof(string) }));
            generator.Emit(OpCodes.Stloc, classInfoLocal);
             */

            il.Emit(OpCodes.Ldloc, classInfoLocal);
            il.Emit(OpCodes.Ldtoken, typeof(PreAspectAttribute));
            //generator.Emit(OpCodes.Ldloca, )
            il.Emit(OpCodes.Call,
                typeof(System.Type).GetMethod("GetTypeFromHandle", new Type[] { typeof(System.RuntimeTypeHandle) }));
            il.Emit(OpCodes.Call,
                typeof(System.Attribute).GetMethod("GetCustomAttribute",
                new Type[] { typeof(System.Reflection.MethodInfo), typeof(System.Type) }));
            il.Emit(OpCodes.Castclass, typeof(PreAspectAttribute));
            il.Emit(OpCodes.Stloc, preAspectLocal);

            il.Emit(OpCodes.Ldloc, preAspectLocal);
            il.Emit(OpCodes.Ldnull);
            il.Emit(OpCodes.Ceq);
            il.Emit(OpCodes.Brtrue_S, castPerSuccess);
            il.Emit(OpCodes.Ldloc, preAspectLocal);
            il.Emit(OpCodes.Ldloc, contextLocal);
            il.Emit(OpCodes.Callvirt,
                typeof(AspectAttribute).GetMethod("Action", new Type[] { typeof(InvokeContext) }));

            il.MarkLabel(castPerSuccess);
            #endregion

            #region Begin Exception Block

            Label exLbl = il.BeginExceptionBlock();

            #endregion

            #region Invoke

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, _realProxyField);
            for (int i = 1; i <= parameterInfos.Length; i++)
            {
                il.Emit(OpCodes.Ldarg, i);
            }

            il.Emit(OpCodes.Call, _realProxyType.GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance));
            if (typeof(void) != returnType)
            {
                il.Emit(OpCodes.Stloc, resultLocal);
            }

            #endregion

            #region Invoke PostInovke

            #region Set result to InvkeContext

            il.Emit(OpCodes.Ldloc, contextLocal);
            // load parameter
            if (typeof(void) != returnType)
            {
                il.Emit(OpCodes.Ldloc, resultLocal);
                if (returnType.IsValueType)
                {
                    il.Emit(OpCodes.Box, returnType);
                }
            }
            else
            {
                il.Emit(OpCodes.Ldnull);
            }
            il.Emit(OpCodes.Call, contextType.GetMethod("SetResult", BindingFlags.Public | BindingFlags.Instance));

            #endregion

            #region Invoke PostInovke

            #region Set result to InvkeContext

            il.Emit(OpCodes.Ldloc, contextLocal);
            // load parameter
            if (typeof(void) != returnType)
            {
                il.Emit(OpCodes.Ldloc, resultLocal);
                if (returnType.IsValueType)
                {
                    il.Emit(OpCodes.Box, returnType);
                }
            }
            else
            {
                il.Emit(OpCodes.Ldnull);
            }
            il.Emit(OpCodes.Call, contextType.GetMethod("SetResult", BindingFlags.Public | BindingFlags.Instance));

            #endregion

            /*
             * C# 代码
             * MethodInfo classInfoLocal = _realProxyField.GetType().GetMethod("methodName");
             * PostAspectAttribute postAspectLocal = 
             *      (PostAspectAttribute)Attribute.GetCustomAttribute(classInfoLocal, typeof(PostAspectAttribute))
             *      
             * if (postAspectLocal != null)
             * {
             *      postAspectLocal.Action(contextLocal);
             * }
             * 
             */
            // get post aspect if has
            //var classInfoLocal = generator.DeclareLocal(typeof(System.Reflection.MethodInfo));
            var postAspectLocal = il.DeclareLocal(typeof(PostAspectAttribute));

            /*
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldfld, _realProxyField);
            generator.Emit(OpCodes.Callvirt, typeof(System.Object).GetMethod("GetType", BindingFlags.Public | BindingFlags.Instance));
            generator.Emit(OpCodes.Ldstr, methodName);
            generator.Emit(OpCodes.Callvirt,
                typeof(System.Type).GetMethod("GetMethod", new Type[] { typeof(string) }));
            generator.Emit(OpCodes.Stloc, classInfoLocal);
             */

            il.Emit(OpCodes.Ldloc, classInfoLocal);
            il.Emit(OpCodes.Ldtoken, typeof(PostAspectAttribute));
            //generator.Emit(OpCodes.Ldloca, )
            il.Emit(OpCodes.Call,
                typeof(System.Type).GetMethod("GetTypeFromHandle", new Type[] { typeof(System.RuntimeTypeHandle) }));
            il.Emit(OpCodes.Call,
                typeof(System.Attribute).GetMethod("GetCustomAttribute",
                new Type[] { typeof(System.Reflection.MethodInfo), typeof(System.Type) }));
            il.Emit(OpCodes.Castclass, typeof(PostAspectAttribute));
            il.Emit(OpCodes.Stloc, postAspectLocal);

            il.Emit(OpCodes.Ldloc, postAspectLocal);
            il.Emit(OpCodes.Ldnull);
            il.Emit(OpCodes.Ceq);
            il.Emit(OpCodes.Brtrue_S, castPostSuccess);
            il.Emit(OpCodes.Ldloc, postAspectLocal);
            il.Emit(OpCodes.Ldloc, contextLocal);
            il.Emit(OpCodes.Callvirt,
                typeof(AspectAttribute).GetMethod("Action", new Type[] { typeof(InvokeContext) }));

            il.MarkLabel(castPostSuccess);

            #endregion

            /*
             * C# 代码
             * MethodInfo classInfoLocal = _realProxyField.GetType().GetMethod("methodName");
             * ArrountAttribute postAspectLocal = 
             *      (ArrountAttribute)Attribute.GetCustomAttribute(classInfoLocal, typeof(ArrountAttribute))
             *      
             * if (postAspectLocal != null)
             * {
             *      postAspectLocal.Action(contextLocal);
             * }
             * 
             */


            il.Emit(OpCodes.Ldloc, arrountAspectLocal);
            il.Emit(OpCodes.Ldnull);
            il.Emit(OpCodes.Ceq);
            il.Emit(OpCodes.Brtrue_S, castAfterArroundSuccess);
            il.Emit(OpCodes.Ldloc, arrountAspectLocal);
            il.Emit(OpCodes.Ldloc, contextLocal);
            il.Emit(OpCodes.Callvirt,
                typeof(ArrountAttribute).GetMethod("EndAction", new Type[] { typeof(InvokeContext) }));

            il.MarkLabel(castAfterArroundSuccess);

            #endregion

            #region Catch Block

            il.BeginCatchBlock(typeof(Exception));

            il.Emit(OpCodes.Stloc, exceptionLocal);
            il.Emit(OpCodes.Ldloc, contextLocal);
            il.Emit(OpCodes.Ldloc, exceptionLocal);
            il.Emit(OpCodes.Call, contextType.GetMethod("SetError", BindingFlags.Public | BindingFlags.Instance));

            /*
              * C# 代码
              * MethodInfo classInfoLocal = _realProxyField.GetType().GetMethod("methodName");
              * ExceptionAspectAttribute exAspectLocal = 
              *      (ExceptionAspectAttribute)Attribute.GetCustomAttribute(classInfoLocal, typeof(ExceptionAspectAttribute))
              *      
              * if (exAspectLocal != null)
              * {
              *      exAspectLocal.Action(contextLocal);
              * }
              * 
              */
            // get exception aspect if has
            //var classInfoLocal = generator.DeclareLocal(typeof(System.Reflection.MethodInfo));
            var exAspectLocal = il.DeclareLocal(typeof(ArrountAttribute));

            /*   
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, _realProxyField);
            il.Emit(OpCodes.Callvirt, typeof(System.Object).GetMethod("GetType", BindingFlags.Public | BindingFlags.Instance));
            il.Emit(OpCodes.Ldstr, methodName);
            il.Emit(OpCodes.Callvirt,
                typeof(System.Type).GetMethod("GetMethod", new Type[] { typeof(string) }));
            il.Emit(OpCodes.Stloc, classInfoLocal);
            */
            il.Emit(OpCodes.Ldloc, classInfoLocal);
            il.Emit(OpCodes.Ldtoken, typeof(ExceptionAspectAttribute));
            //generator.Emit(OpCodes.Ldloca, )
            il.Emit(OpCodes.Call,
                typeof(System.Type).GetMethod("GetTypeFromHandle", new Type[] { typeof(System.RuntimeTypeHandle) }));
            il.Emit(OpCodes.Call,
                typeof(System.Attribute).GetMethod("GetCustomAttribute",
                new Type[] { typeof(System.Reflection.MethodInfo), typeof(System.Type) }));
            il.Emit(OpCodes.Castclass, typeof(ExceptionAspectAttribute));
            il.Emit(OpCodes.Stloc, exAspectLocal);

            il.Emit(OpCodes.Ldloc, exAspectLocal);
            il.Emit(OpCodes.Ldnull);
            il.Emit(OpCodes.Ceq);
            il.Emit(OpCodes.Brtrue_S, castExSuccess);
            il.Emit(OpCodes.Ldloc, exAspectLocal);
            il.Emit(OpCodes.Ldloc, contextLocal);
            il.Emit(OpCodes.Callvirt,
                typeof(AspectAttribute).GetMethod("Action", new Type[] { typeof(InvokeContext) }));

            il.MarkLabel(castExSuccess);

            #endregion

            #region End Exception Block

            il.EndExceptionBlock();

            #endregion

            if (typeof(void) != returnType)
            {
                il.Emit(OpCodes.Ldloc, resultLocal);
            }

            il.Emit(OpCodes.Ret);
        }
        #endregion
    }
}
