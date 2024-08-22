using DuckSurvivor.Scripts.Configs.DataClass;
using Imba.Utils;
using System.Diagnostics;

namespace DuckSurvivor.Scripts.Configs
{
    public class ConfigManager : Singleton<ConfigManager>
    {
        private const string ConfigSharePath = "Configs/";

        #region GAME_CONFIG
        
        public ConfigSkills configSkills;
        public LevelConfig configLevels;
        public ConfigWaveDetail configWaveDetails;
        public ConfigDucks configDucks;
        public ConfigDuckLevels configDuckLevels;
        public ConfigWaves configWaves;

        #endregion

        //======================================================

        public void LoadAllConfigLocal()
        {
            if (isLoadedConfigLocal)
                return;

            configSkills = new ConfigSkills();
            configSkills.LoadFromAssetPath(ConfigSharePath + "ConfigSkills");

            configLevels = new LevelConfig();
            configLevels.LoadFromAssetPath(ConfigSharePath + "ConfigLevels");
            
            configWaveDetails = new ConfigWaveDetail();
            configWaveDetails.LoadFromAssetPath(ConfigSharePath + "ConfigWaveDetails");

            configDucks = new ConfigDucks();
            configDucks.LoadFromAssetPath(ConfigSharePath + "ConfigDucks");

            configDuckLevels = new ConfigDuckLevels();
            configDuckLevels.LoadFromAssetPath(ConfigSharePath + "ConfigDuckLevels");

            configWaves = new ConfigWaves();
            configWaves.LoadFromAssetPath(ConfigSharePath + "ConfigWaves");

            isLoadedConfigLocal = true;
        }

        private static bool isLoadedConfigLocal = false;
        public static bool IsLoadedConfigLocal
        {
            set { isLoadedConfigLocal = value; }
            get { return isLoadedConfigLocal; }
        }

    }
}