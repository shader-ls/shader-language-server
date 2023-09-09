using System.Reflection;

namespace ShaderlabVS.Data
{
    /// <summary>
    /// 
    /// </summary>
    public class ShaderlabDataManager
    {
        #region Constants
        public static string HLSL_CG_DATATYPE_DEFINATIONFILE = Path.Combine("Data", "HLSL_CG_datatype.def");
        public static string HLSL_CG_FUNCTION_DEFINATIONFILE = Path.Combine("Data", "HLSL_CG_functions.def");
        public static string HLSL_CG_KEYWORD_DEFINATIONFILE = Path.Combine("Data", "HLSL_CG_Keywords.def");

        public static string UNITY3D_DATATYPE_DEFINATIONFILE = Path.Combine("Data", "Unity3D_datatype.def");
        public static string UNITY3D_FUNCTION_DEFINATIONFILE = Path.Combine("Data", "Unity3D_functions.def");
        public static string UNITY3D_KEYWORD_DEFINATIONFILE = Path.Combine("Data", "Unity3D_keywords.def");
        public static string UNITY3D_MACROS_DEFINATIONFILE = Path.Combine("Data", "Unity3D_macros.def");
        public static string UNITY3D_VALUES_DEFINATIONFILE = Path.Combine("Data", "Unity3D_values.def");
        #endregion

        #region Properties

        public List<HLSLCGFunction> HLSLCGFunctions { get; private set; }
        public List<string> HLSLCGBlockKeywords { get; private set; }
        public List<string> HLSLCGNonblockKeywords { get; private set; }
        public List<string> HLSLCGSpecialKeywords { get; private set; }
        public List<string> HLSLCGDatatypes { get; private set; }


        public List<UnityBuiltinDatatype> UnityBuiltinDatatypes { get; private set; }
        public List<UnityBuiltinFunction> UnityBuiltinFunctions { get; private set; }
        public List<UnityBuiltinMacros> UnityBuiltinMacros { get; private set; }
        public List<UnityKeywords> UnityKeywords { get; private set; }
        public List<UnityBuiltinValue> UnityBuiltinValues { get; private set; }

        #endregion

        #region Singleton

        private static object lockObj = new object();

        private static ShaderlabDataManager _instance;

        public static ShaderlabDataManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (lockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new ShaderlabDataManager();
                        }
                    }
                }

                return _instance;
            }
        }

        #endregion

        private ShaderlabDataManager()
        {
            string currentAssemblyDir = (new FileInfo(Assembly.GetExecutingAssembly().Location)).DirectoryName;
            HLSLCGFunctions = DefinationDataProvider<HLSLCGFunction>.ProvideFromFile(Path.Combine(currentAssemblyDir, ShaderlabDataManager.HLSL_CG_FUNCTION_DEFINATIONFILE));

            List<HLSLCGKeywords> hlslcgKeywords = DefinationDataProvider<HLSLCGKeywords>.ProvideFromFile(Path.Combine(currentAssemblyDir, ShaderlabDataManager.HLSL_CG_KEYWORD_DEFINATIONFILE));
            HLSLCGBlockKeywords = GetHLSLCGKeywordsByType(hlslcgKeywords, "block");
            HLSLCGNonblockKeywords = GetHLSLCGKeywordsByType(hlslcgKeywords, "nonblock");
            HLSLCGSpecialKeywords = GetHLSLCGKeywordsByType(hlslcgKeywords, "special");

            var dts = DefinationDataProvider<HLSLCGDataTypes>.ProvideFromFile(Path.Combine(currentAssemblyDir, ShaderlabDataManager.HLSL_CG_DATATYPE_DEFINATIONFILE)).First();
            if (dts != null)
            {
                HLSLCGDatatypes = dts.DataTypes;
            }

            UnityBuiltinDatatypes = DefinationDataProvider<UnityBuiltinDatatype>.ProvideFromFile(Path.Combine(currentAssemblyDir, ShaderlabDataManager.UNITY3D_DATATYPE_DEFINATIONFILE));
            UnityBuiltinFunctions = DefinationDataProvider<UnityBuiltinFunction>.ProvideFromFile(Path.Combine(currentAssemblyDir, ShaderlabDataManager.UNITY3D_FUNCTION_DEFINATIONFILE));
            UnityBuiltinMacros = DefinationDataProvider<UnityBuiltinMacros>.ProvideFromFile(Path.Combine(currentAssemblyDir, ShaderlabDataManager.UNITY3D_MACROS_DEFINATIONFILE));
            UnityBuiltinValues = DefinationDataProvider<UnityBuiltinValue>.ProvideFromFile(Path.Combine(currentAssemblyDir, ShaderlabDataManager.UNITY3D_VALUES_DEFINATIONFILE));
            UnityKeywords = DefinationDataProvider<UnityKeywords>.ProvideFromFile(Path.Combine(currentAssemblyDir, ShaderlabDataManager.UNITY3D_KEYWORD_DEFINATIONFILE));

        }

        private List<string> GetHLSLCGKeywordsByType(List<HLSLCGKeywords> alltypes, string type)
        {
            var kw = alltypes.First(k => k.Type.Equals(type, StringComparison.OrdinalIgnoreCase));
            if (kw != null)
            {
                return kw.Keywords;
            }
            else
            {
                return new List<string>();
            }
        }
    }
}
